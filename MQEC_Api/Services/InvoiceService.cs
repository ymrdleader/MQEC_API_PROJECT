using MQEC_Api.Models;
using MQEC_Api.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MQEC_Api.Services
{
    public interface IInvoiceService
    {
        InvoiceResponse GetInvoiceData(string[] InvoiceNo);
        InvoiceResponse CreateInvoice(InvoiceViewModel Invoice);
        ApiResultResponse CancelInvoice(InvoiceCancelViewModel Invoice);
    }

    public class InvoiceService : IInvoiceService
    {
        private MQECErpContext _context;
        public InvoiceService(MQECErpContext context)
        {
            _context = context;
        }

        public InvoiceResponse GetInvoiceData(string[] InvoiceNo)
        {
            InvoiceResponse invoiceResult = new InvoiceResponse();
            invoiceResult.Result = new ApiResultResponse();


            return invoiceResult;
        }

        public InvoiceResponse CreateInvoice(InvoiceViewModel Invoice)
        {
            DateTime date = DateTime.Now;
            string strInvoiceNumber = "";
            string strInvoiceTrack = "";
            string strInvoiceNumberMax = "";
            string strTaxID = "";
            InvoiceResponse Result = new InvoiceResponse();
            Result.Result = new ApiResultResponse();
            //if (Invoice.InvoiceCategory == null || string.IsNullOrWhiteSpace(Invoice.InvoiceCategory))
            //{
            //    Result.Result.ResultCode = "9999";
            //    Result.Result.ResultMessage = "發票分類(invoiceCategory)不可空白";
            //    return Result;
            //}
            //if (Invoice.CreateUser == null || string.IsNullOrWhiteSpace(Invoice.CreateUser))
            //{
            //    Result.Result.ResultCode = "9999";
            //    Result.Result.ResultMessage = "建檔人員(createUser)不可空白";
            //    return Result;
            //}

            //表頭檢核
            if (Invoice.Main.Seller.Identifier.Length != 8)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = "賣方統編(Main.Seller.Identifier)須為8碼數字";
                return Result;
            }

            if (Invoice.InvoiceCategory == "B2B" && Invoice.Main.Buyer.Identifier.Length != 8)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = "B2B賣方統編(Main.Seller.Identifier)須為碼8數字";
                return Result;
            }
            var ChkInvoiceType = (from item in _context.ComInvoiceType
                                  where item.InvoiceType == Invoice.Main.InvoiceType
                                  select item).FirstOrDefault();

            if (ChkInvoiceType == null)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = "發票類別(Main.InvoiceType)不正確";
                return Result;
            }
            if (Invoice.Main.DonateMark != "0" && Invoice.Main.DonateMark != "1")
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = "捐贈註記(Main.donateMark)須為 0 或 1";
                return Result;
            }
            if (Invoice.Main.DonateMark == "1" && Invoice.Main.NPOBAN == "")
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = "捐贈發票必須輸入捐贈對象(Main.npoban)";
                return Result;
            }
            if (Invoice.Main.CarrierType != "")
            {
                if (string.IsNullOrWhiteSpace(Invoice.Main.CarrierId1))
                {
                    Result.Result.ResultCode = "9999";
                    Result.Result.ResultMessage = "載具顯碼(Main.CarrierId1)不可空白";
                    return Result;
                }
                if (string.IsNullOrWhiteSpace(Invoice.Main.CarrierId2))
                {
                    Result.Result.ResultCode = "9999";
                    Result.Result.ResultMessage = "載具隱碼(Main.CarrierId2)不可空白";
                    return Result;
                }
            }
            decimal decSaleAmount = (Invoice.Amount.SalesAmount == null ? 0 : Invoice.Amount.SalesAmount);
            decimal decFreeTaxSalesAmount = (Invoice.Amount.FreeTaxSalesAmount == null ? 0 : Invoice.Amount.FreeTaxSalesAmount);
            decimal decZeroTaxSalesAmount = (Invoice.Amount.ZeroTaxSalesAmount == null ? 0 : Invoice.Amount.ZeroTaxSalesAmount);

            if (decSaleAmount + decFreeTaxSalesAmount + decZeroTaxSalesAmount <= 0)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = "應稅, 免稅, 零稅率銷售金額加總不可小於0";
                return Result;
            }
            if (Invoice.Amount.TotalAmount <= 0)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = "總計金額(Main.TotalAmount)不可小於0";
                return Result;
            }

            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                var InvoiceNumberForm = _context.InvoiceNumberDetails.FromSqlRaw(@$"select * from InvoiceNumberDetails with (updlock)");

                var InvoiceNumberList = (from Inv in _context.InvoiceNumberDetails
                                         where Inv.CompanyNo == "MQEC"
                                            && Inv.InvoiceFunction == "Inv_Sys"
                                            && Inv.InvoiceType == Invoice.Main.InvoiceType
                                            && Inv.InvoiceYear == date.Year.ToString().PadLeft(4, '0')
                                            && string.Compare(date.Month.ToString().PadLeft(2, '0'), Inv.InvoiceMonthFrom) >= 0
                                            && string.Compare(date.Month.ToString().PadLeft(2, '0'), Inv.InvoiceMonthTo) <= 0
                                            && string.Compare(Inv.InvoiceNumberLast, Inv.InvoiceNumberTo) < 0
                                            && Inv.BranchTaxId == Invoice.Main.Seller.Identifier
                                            && Inv.Discontinued == false
                                         select Inv).ToList();
                if (InvoiceNumberList.Count == 0)
                {
                    Result.Result.ResultCode = "9999";
                    Result.Result.ResultMessage = "無可開立發票號碼";
                    return Result;
                }
                else
                {
                    //取發票號碼
                    string strINVNo = "";
                    strInvoiceNumberMax = InvoiceNumberList.Max(x => x.InvoiceNumberLast);
                    var InvNumD = InvoiceNumberList.OrderBy(x => x.InvNo + x.InvoiceMonthTo).FirstOrDefault();
                    if (strInvoiceNumberMax == "")
                    {
                        strInvoiceTrack = InvNumD.InvoiceTrack;
                        strInvoiceNumber = InvNumD.InvoiceNumberFrom;
                    }
                    else
                    {
                        InvNumD = (from item in _context.InvoiceNumberDetails
                                   where item.InvoiceNumberLast == strInvoiceNumberMax
                                   select item).FirstOrDefault();

                        strInvoiceTrack = InvNumD.InvoiceTrack;
                        strInvoiceNumber = (int.Parse(InvNumD.InvoiceNumberLast) + 1).ToString().PadLeft(8, '0');
                    }
                    strTaxID = InvNumD.BranchTaxId;
                    //更新可用發票數量
                    InvNumD.AvaQty = InvNumD.AvaQty - 1;
                    strINVNo = InvNumD.InvNo;
                    InvNumD.InvoiceNumberLast = strInvoiceNumber;
                    _context.SaveChanges();
                    var InvNumH = (from InvNum in _context.InvoiceNumber
                                   where InvNum.InvNo == strINVNo
                                   select InvNum).FirstOrDefault();
                    InvNumH.AvaQty = (from InvNumDe in _context.InvoiceNumberDetails
                                      where InvNumDe.InvNo == strINVNo
                                      select InvNumDe).Sum(x => x.AvaQty);
                    _context.SaveChanges();
                    Random rnd = new Random();
                    int num = rnd.Next(10000);//4碼隨機碼
                    //寫入發票
                    Invoice InvH = new Invoice();
                    InvH.CompanyNo = "MQEC";
                    InvH.TaxId = strTaxID;
                    InvH.InvoiceType = Invoice.Main.InvoiceType;
                    InvH.InvoiceCategory = Invoice.InvoiceCategory;
                    InvH.InvoiceKind = "S";
                    InvH.InvoiceNumber = strInvoiceTrack + strInvoiceNumber;
                    InvH.InvoiceDate = date;
                    InvH.SellerId = Invoice.Main.Seller.Identifier;
                    InvH.SellerName = Invoice.Main.Seller.Name;
                    InvH.SellerAddress = (Invoice.Main.Seller.Address == null ? "" : Invoice.Main.Seller.Address);
                    InvH.SellerPersonInCharge = (Invoice.Main.Seller.PersonInCharge == null ? "" : Invoice.Main.Seller.PersonInCharge);
                    InvH.SellerTelephoneNumber = (Invoice.Main.Seller.TelephoneNumber == null ? "" : Invoice.Main.Seller.TelephoneNumber);
                    InvH.SellerFacsimileNumber = (Invoice.Main.Seller.FacsimileNumber == null ? "" : Invoice.Main.Seller.FacsimileNumber);
                    InvH.SellerEmailAddress = (Invoice.Main.Seller.EmailAddress == null ? "" : Invoice.Main.Seller.EmailAddress);
                    InvH.SellerCustomerNumber = (Invoice.Main.Seller.CustomerNumber == null ? "" : Invoice.Main.Seller.CustomerNumber);
                    InvH.SellerRoleRemark = (Invoice.Main.Seller.RoleRemark == null ? "" : Invoice.Main.Seller.RoleRemark);
                    InvH.BuyerId = Invoice.Main.Buyer.Identifier;
                    InvH.BuyerName = Invoice.Main.Buyer.Name;
                    InvH.BuyerAddress = (Invoice.Main.Buyer.Address == null ? "" : Invoice.Main.Buyer.Address);
                    InvH.BuyerPersonInCharge = (Invoice.Main.Buyer.PersonInCharge == null ? "" : Invoice.Main.Buyer.PersonInCharge);
                    InvH.BuyerTelephoneNumber = (Invoice.Main.Buyer.TelephoneNumber == null ? "" : Invoice.Main.Buyer.TelephoneNumber);
                    InvH.BuyerFacsimileNumber = (Invoice.Main.Buyer.FacsimileNumber == null ? "" : Invoice.Main.Seller.Address);
                    InvH.BuyerEmailAddress = (Invoice.Main.Buyer.EmailAddress == null ? "" : Invoice.Main.Buyer.EmailAddress);
                    InvH.BuyerCustomerNumber = (Invoice.Main.Buyer.CustomerNumber == null ? "" : Invoice.Main.Buyer.CustomerNumber);
                    InvH.BuyerRoleRemark = (Invoice.Main.Buyer.RoleRemark == null ? "" : Invoice.Main.Buyer.RoleRemark);
                    InvH.CheckNumber = "";
                    InvH.BuyerRemark = "";
                    InvH.MainRemark = (Invoice.Main.MainRemark == null ? "" : Invoice.Main.MainRemark);
                    InvH.CustomsClearanceMark = (Invoice.Main.CustomsClearanceMark == null ? "" : Invoice.Main.CustomsClearanceMark);
                    InvH.Category = "";
                    InvH.RelateNumber = (Invoice.Main.RelateNumber == null ? "" : Invoice.Main.RelateNumber);
                    InvH.GroupMark = (Invoice.Main.GroupMark == null ? "" : Invoice.Main.GroupMark);
                    InvH.DonateMark = (Invoice.Main.DonateMark == "0" ? false : true);
                    InvH.CarrierType = (Invoice.Main.CarrierType == null ? "" : Invoice.Main.CarrierType);
                    InvH.CarrierId1 = (Invoice.Main.CarrierId1 == null ? "" : Invoice.Main.CarrierId1);
                    InvH.CarrierId2 = (Invoice.Main.CarrierId2 == null ? "" : Invoice.Main.CarrierId2);
                    InvH.Npoban = (Invoice.Main.NPOBAN == null ? "" : Invoice.Main.NPOBAN);
                    InvH.PrintMark = ((Invoice.Main.NPOBAN == null ? "N" : Invoice.Main.NPOBAN) == "Y" ? true : false);
                    InvH.RandomNumber = num.ToString();
                    //InvH.Cancel_Date
                    InvH.CancelFlag = false;
                    InvH.CancelReason = "";
                    InvH.CancelUser = "";
                    //InvH.Delete_Date
                    InvH.DeleteFlag = false;
                    InvH.DeleteReason = "";
                    InvH.DeleteUser = "";
                    InvH.SalesAmount = (Invoice.Amount.SalesAmount == null ? 0 : Invoice.Amount.SalesAmount);
                    InvH.FreeTaxSalesAmount = (Invoice.Amount.FreeTaxSalesAmount == null ? 0 : Invoice.Amount.FreeTaxSalesAmount);
                    InvH.ZeroTaxSalesAmount = (Invoice.Amount.ZeroTaxSalesAmount == null ? 0 : Invoice.Amount.ZeroTaxSalesAmount);
                    InvH.TaxType = (Invoice.Amount.TaxType == null ? "" : Invoice.Amount.TaxType);
                    InvH.TaxRate = (Invoice.Amount.TaxRate == null ? 0 : Invoice.Amount.TaxRate);
                    InvH.TaxAmount = (Invoice.Amount.TaxAmount == null ? 0 : Invoice.Amount.TaxAmount);
                    InvH.TotalAmount = (Invoice.Amount.TotalAmount == null ? 0 : Invoice.Amount.TotalAmount);
                    InvH.DiscountAmount = (Invoice.Amount.DiscountAmount == null ? 0 : Invoice.Amount.DiscountAmount);
                    InvH.OriginalCurrencyAmount = (Invoice.Amount.OriginalCurrencyAmount == null ? 0 : Invoice.Amount.OriginalCurrencyAmount);
                    InvH.ExchangeRate = (Invoice.Amount.ExchangeRate == null ? 0 : Invoice.Amount.ExchangeRate);
                    InvH.Currency = (Invoice.Amount.Currency == null ? "" : Invoice.Main.Seller.Address);
                    InvH.CreateDate = date;
                    InvH.CreareUser = Invoice.CreateUser;
                    //InvH.UpdateDate = ;
                    InvH.UpdateUser = "";
                    _context.Invoice.Add(InvH);
                    int intSerNo = 1;
                    foreach (var Detail in Invoice.Details)
                    {
                        if (Detail.Quantity == null || Detail.Quantity <= 0)
                        {
                            Result.Result.ResultCode = "9999";
                            Result.Result.ResultMessage = $"數量必須大於0 (details.quantity, 序號{Detail.SequenceNumber})";
                            return Result;
                        }
                        InvoiceDetails InvD = new InvoiceDetails();
                        InvD.InvoiceNumber =strInvoiceTrack + strInvoiceNumber;
                        InvD.Serno = intSerNo;
                        InvD.Prodid = "";
                        InvD.Description = Detail.Description;
                        InvD.Quantity = Detail.Quantity;
                        InvD.Unit = Detail.Unit == null ? "" : Detail.Unit;
                        InvD.UnitPrice = Detail.UnitPrice;
                        InvD.Amount = Detail.Amount;
                        InvD.SourceTaxTypeNo = "";
                        InvD.Remark = Detail.Remark;
                        InvD.RelateNumber = Detail.RelateNumber;
                        InvD.SourceNo = Invoice.Main.Buyer.CustomerNumber;
                        InvD.SourceSerno = Detail.SequenceNumber;
                        InvD.ComQty = 0;
                        InvD.AvaQty = Detail.Quantity;
                        _context.InvoiceDetails.Add(InvD);
                        intSerNo++;
                    }
                }
                _context.SaveChanges();
                transaction.Commit();
                Result.InvoiceNumber = strInvoiceTrack + strInvoiceNumber;
                Result.InvoiceDate = date.ToString("yyyy/MM/dd");
                Result.InvoiceTime = date.ToString("HH:mm:ss");
                Result.TotalAmount = Invoice.Amount.TotalAmount;
                Result.Result.ResultCode = "0000";
                Result.Result.ResultMessage = "Success";
            }
            return Result;
        }

        public ApiResultResponse CancelInvoice(InvoiceCancelViewModel Invoice)
        {
            DateTime date = DateTime.Now;
            ApiResultResponse Result = new ApiResultResponse();
            var InvoiceInfo = ( from Inv in _context.Invoice
                               where Inv.InvoiceNumber == Invoice.InvoiceNumber
                              select Inv).FirstOrDefault();
            if (InvoiceInfo == null)
            {
                Result.ResultCode = "9999";
                Result.ResultMessage = "查無發票紀錄，無法作廢";
            }
            else
            {
                if (InvoiceInfo.CancelUser != "")
                {
                    Result.ResultCode = "9999";
                    Result.ResultMessage = "發票已作廢無法再次作廢";
                }
                else
                {
                    InvoiceInfo.CancelUser = Invoice.CancelUser;
                    InvoiceInfo.CancelFlag = true;
                    InvoiceInfo.CancelReason = Invoice.CancelReason;
                    InvoiceInfo.CancelDate = date;
                    Result.ResultCode = "0000";
                    Result.ResultMessage = "Success";
                }
            }
            _context.SaveChanges();
            return Result;
        }

    }
}

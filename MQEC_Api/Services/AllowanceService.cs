using MQEC_Api.Models;
using MQEC_Api.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MQEC_Api.Services
{
    public interface IAllowanceService
    {
        AllowanceResponse CreateAllowance(AllowanceViewModel Allowance);
        ApiResultResponse CancelAllowance(AllowanceCancelViewModel Allowance);
    }

    public class AllowanceService : IAllowanceService
    {
        private MQECErpContext _context;
        public AllowanceService(MQECErpContext context)
        {
            _context = context;
        }


        public AllowanceResponse CreateAllowance(AllowanceViewModel Allowance)
        {
            AllowanceResponse Result = new AllowanceResponse();
            Result.Result = new ApiResultResponse();
            #region 檢核
            if (string.IsNullOrWhiteSpace(Allowance.Main.OriginalAllowanceNo))
            {
                Allowance.Main.OriginalAllowanceNo = "";
            }

            if (Allowance.Main.OriginalAllowanceDate == null)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = "來源折讓日期(Main.OriginalAllowanceDate)不可空白";
                return Result;
            }
            else
            {
                try
                {
                    DateTime.Parse(Allowance.Main.OriginalAllowanceDate);
                }
                catch
                {
                    Result.Result.ResultCode = "9999";
                    Result.Result.ResultMessage = "來源折讓日期(Main.OriginalAllowanceDate)非有效日期格式";
                    return Result;
                }
            }
            if (string.IsNullOrWhiteSpace(Allowance.Main.AllowanceType))
            {
                Allowance.Main.AllowanceType = "2";
            }
            else if (!new string[] { "1", "2" }.Contains(Allowance.Main.AllowanceType))
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = "折讓種類(Main.AllowanceType)應為1:買方開立折讓證明單, 2:賣方折讓證明通知單";
                return Result;
            }
            if (string.IsNullOrWhiteSpace(Allowance.Main.CreateUser))
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = "折讓人員(Main.CreateUser)不可空白";
                return Result;
            }

            var ChkData = (from Ad in Allowance.Details
                           join inv in _context.Invoice on Ad.OriginalInvoiceNumber equals inv.InvoiceNumber into g
                           from gr in g.DefaultIfEmpty()
                           select new
                           {
                               OriginalInvoiceNumber = Ad.OriginalInvoiceNumber,
                               OriginalSequenceNumber = Ad.OriginalSequenceNumber,
                               OriginalDescription = Ad.OriginalDescription,
                               Quantity = Ad.Quantity,
                               Unit = Ad.Unit,
                               UnitPrice = Ad.UnitPrice,
                               Amount = Ad.Amount,
                               Tax = Ad.Tax,
                               TaxType = gr == null ? "" : gr.TaxType,
                               InvoiceNumber = gr == null ? "" : gr.InvoiceNumber,
                               InvoiceDate = gr == null ? "" : gr.InvoiceDate.Value.ToString("yyyyMMdd"),
                               InvoiceCategory = gr == null ? "" : gr.InvoiceCategory,
                               SellerId = gr == null ? "" : gr.SellerId,
                               SellerName = gr == null ? "" : gr.SellerName,
                               SellerAddress = gr == null ? "" : gr.SellerAddress,
                               SellerPersonInCharge = gr == null ? "" : gr.SellerPersonInCharge,
                               SellerTelephoneNumber = gr == null ? "" : gr.SellerTelephoneNumber,
                               SellerFacsimileNumber = gr == null ? "" : gr.SellerFacsimileNumber,
                               SellerEmailAddress = gr == null ? "" : gr.SellerEmailAddress,
                               SellerCustomerNumber = gr == null ? "" : gr.SellerCustomerNumber,
                               SellerRoleRemark = gr == null ? "" : gr.SellerRoleRemark,
                               BuyerId = gr == null ? "" : gr.BuyerId,
                               BuyerName = gr == null ? "" : gr.BuyerName,
                               BuyerAddress = gr == null ? "" : gr.BuyerAddress,
                               BuyerPersonInCharge = gr == null ? "" : gr.BuyerPersonInCharge,
                               BuyerTelephoneNumber = gr == null ? "" : gr.BuyerTelephoneNumber,
                               BuyerFacsimileNumber = gr == null ? "" : gr.BuyerFacsimileNumber,
                               BuyerEmailAddress = gr == null ? "" : gr.BuyerEmailAddress,
                               BuyerCustomerNumber = gr == null ? "" : gr.BuyerCustomerNumber,
                               BuyerRoleRemark = gr == null ? "" : gr.BuyerRoleRemark,
                               CancelDate = gr == null ? null : gr.CancelDate,
                               TotalAmount = gr == null ? 0 : gr.TotalAmount,
                               CurrencyId = gr == null ? "" : gr.Currency,
                               ExchangeRate = gr == null ? 0 : gr.ExchangeRate,
                               ComAmount = gr == null ? 0 : gr.ComAmount,
                               AvaAmount = gr == null ? 0 : gr.AvaAmount,
                           });
            var ChkResult = ChkData.Where(x => x.InvoiceNumber == null || x.InvoiceNumber == "").Select(x => x.OriginalInvoiceNumber);
            if (ChkResult.Count() > 0)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = $"發票號碼{string.Join(',', ChkResult)}不存在, 無法開立折讓";
                return Result;
            }

            ChkResult = ChkData.Where(x => x.CancelDate != null).Select(x => x.OriginalInvoiceNumber);
            if (ChkResult.Count() > 0)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = $"發票號碼{string.Join(',', ChkResult)}已作廢, 無法開立折讓";
                return Result;
            }

            ChkResult = ChkData.Select(x => x.InvoiceCategory).Distinct();
            if (ChkResult.Count() > 1)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = $"明細中發票類別{string.Join(',', ChkResult)}不可同時開立折讓";
                return Result;
            }

            ChkResult = ChkData.Select(x => x.BuyerId).Distinct();
            if (ChkResult.Count() > 1)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = $"明細中發票買方統編不同{string.Join(',', ChkResult)}不可同時開立折讓";
                return Result;
            }

            ChkResult = ChkData.Where(x => x.UnitPrice <= 0).Select(x => x.OriginalInvoiceNumber);
            if (ChkResult.Count() > 1)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = $"明細中發票單價不可低於0, 無法開立折讓";
                return Result;
            }

            if (ChkData.Sum(x => x.Amount) != Allowance.Amount.TotalAmount)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = $"明細加總不含稅金額({ChkData.Sum(x => x.Amount)}) <> 表頭加總不含稅金額({Allowance.Amount.TotalAmount}), 無法開立折讓";
                return Result;
            }

            if (ChkData.Sum(x => x.Tax) != Allowance.Amount.TaxAmount)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = $"明細加總稅額({ChkData.Sum(x => x.Amount)}) <> 表頭加總稅額({Allowance.Amount.TotalAmount}), 無法開立折讓";
                return Result;
            }

            var ChkAvaAmount = (from item in ChkData
                                group item by new
                                {
                                    InvoiceNumber = item.InvoiceNumber,
                                    AvaAmount = item.AvaAmount
                                } into g
                                select new
                                {
                                    InvoiceNumber = g.Key.InvoiceNumber,
                                    AvaAmount = g.Key.AvaAmount,
                                    Amount = g.Sum(x => x.Amount + x.Tax)
                                }).Where(s => s.Amount > s.AvaAmount).Distinct();
            if (ChkAvaAmount.Count() > 0)
            {
                Result.Result.ResultCode = "9999";
                Result.Result.ResultMessage = $"明細中發票{ChkAvaAmount.FirstOrDefault().InvoiceNumber}可折讓金額低於{ChkAvaAmount.FirstOrDefault().Amount}, 無法開立折讓";
                return Result;
            }
            #endregion

            #region 檢核完畢寫入折讓單
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                try
                {
                    DateTime date = DateTime.Now;
                    var Seller = (from Company in _context.CompanyInfo
                                  select Company).FirstOrDefault();
                    var AllowanceForm = _context.AllowanceFormNo
                                       .FromSqlRaw(@$"
                                           SELECT Company_No,CASE WHEN isnull(Max(FormNo), '') = '' THEN 'AL'+convert(varchar, getdate(), 112) +'001' 
                                                                  ELSE Substring(Max(FormNo),0,11) + RIGHT(REPLICATE('0', 3) + CAST(convert(int, Substring(Max(FormNo), 11, 3) + 1) as NVARCHAR), 3) END as FormNo 
                                             FROM ( Select '{Seller.CompanyNo}' as Company_No,Max(FormNo) as FormNo from AllowanceFormNo WITH(UPDLOCK,  HOLDLOCK)
                                                    WHERE Company_No = '{Seller.CompanyNo}' and SUBSTRING(FormNo, 3, 8) = convert(varchar, getdate(), 112)
                                                  ) as AllowanceForm
                                            group by Company_No").FirstOrDefault();

                    AllowanceFormNo AllFormNo = new AllowanceFormNo();
                    AllFormNo.CompanyNo = Seller.CompanyNo;
                    AllFormNo.FormNo = AllowanceForm.FormNo;
                    _context.AllowanceFormNo.Add(AllowanceForm);
                    
                    Allowance Master = new Allowance();
                    Master.CompanyNo = Seller.CompanyNo;
                    Master.AllowanceNo = AllowanceForm.FormNo;
                    Master.OriAllowanceNo = Allowance.Main.OriginalAllowanceNo;
                    Master.AllowanceType = Allowance.Main.AllowanceType;
                    Master.AllowanceDate = DateTime.Parse(Allowance.Main.OriginalAllowanceDate);
                    Master.InvoiceCategory = ChkData.FirstOrDefault().InvoiceCategory;
                    Master.Memo = Allowance.Main.Memo;
                    Master.SellerId = ChkData.FirstOrDefault().SellerId;
                    Master.SellerName = ChkData.FirstOrDefault().SellerName;
                    Master.SellerAddress = ChkData.FirstOrDefault().SellerAddress;
                    Master.SellerPersonInCharge = ChkData.FirstOrDefault().SellerPersonInCharge;
                    Master.SellerTelephoneNumber = ChkData.FirstOrDefault().SellerTelephoneNumber;
                    Master.SellerFacsimileNumber = ChkData.FirstOrDefault().SellerFacsimileNumber;
                    Master.SellerEmailAddress = ChkData.FirstOrDefault().SellerEmailAddress;
                    Master.SellerCustomerNumber = ChkData.FirstOrDefault().SellerCustomerNumber;
                    Master.SellerRoleRemark = ChkData.FirstOrDefault().SellerRoleRemark;
                    Master.BuyerId = ChkData.FirstOrDefault().BuyerId;
                    Master.BuyerName = ChkData.FirstOrDefault().BuyerName;
                    Master.BuyerAddress = ChkData.FirstOrDefault().BuyerAddress;
                    Master.BuyerPersonInCharge = ChkData.FirstOrDefault().BuyerPersonInCharge;
                    Master.BuyerTelephoneNumber = ChkData.FirstOrDefault().BuyerTelephoneNumber;
                    Master.BuyerFacsimileNumber = ChkData.FirstOrDefault().BuyerFacsimileNumber;
                    Master.BuyerEmailAddress = ChkData.FirstOrDefault().BuyerEmailAddress;
                    Master.BuyerCustomerNumber = ChkData.FirstOrDefault().BuyerCustomerNumber;
                    Master.BuyerRoleRemark = ChkData.FirstOrDefault().BuyerRoleRemark;
                    Master.NetAllowance = Allowance.Amount.TotalAmount;
                    Master.AllowanceTax = Allowance.Amount.TaxAmount;
                    Master.AllowanceAmount = Allowance.Amount.TotalAmount + Allowance.Amount.TaxAmount;
                    Master.TaxRate = 0;
                    Master.CurrencyId = ChkData.FirstOrDefault().CurrencyId;
                    Master.ExchangeRate = 0;
                    Master.OpenSummons = false;
                    Master.CreateDate = date;
                    Master.CreateUser = Allowance.Main.CreateUser;
                    Master.UpdateUser = "";
                    Master.CancelFlag = false;
                    Master.CancelReason = "";
                    Master.CancelUser = "";
                    int intSerno = 0;
                    foreach (var item in ChkData)
                    {
                        intSerno++;
                        var Invoice = (from invoice in _context.Invoice
                                       where invoice.InvoiceNumber == item.InvoiceNumber
                                       select invoice).FirstOrDefault();
                        Invoice.AvaAmount = Invoice.AvaAmount - (item.Amount + item.Tax);
                        Invoice.ComAmount = Invoice.ComAmount + (item.Amount + item.Tax);
                        
                        AllowanceDetails details = new AllowanceDetails();
                        details.CompanyNo = Seller.CompanyNo;
                        details.AllowanceNo = AllowanceForm.FormNo;
                        details.Serno = intSerno;
                        details.Prodid = "";
                        details.Description = item.OriginalDescription;
                        details.Unit = item.Unit;
                        details.TaxTypeNo = item.TaxType;
                        details.CurrencyId = item.CurrencyId;
                        details.ExchangeRate = item.ExchangeRate;
                        details.AllowanceQty = item.Quantity;
                        details.AllowancePrice = item.UnitPrice;
                        details.AllowanceAmount = item.Amount;
                        details.TaxAmount = item.Tax;
                        details.GrossAmount = item.Amount + item.Tax;
                        details.InvoiceNumber = item.OriginalInvoiceNumber;
                        details.InvoiceDate = item.InvoiceDate;
                        details.InvoiceSerno = 0;
                        details.Co1D1 = item.OriginalSequenceNumber;
                        _context.AllowanceDetails.Add(details);
                    }
                    _context.Allowance.Add(Master);
                    _context.SaveChanges();
                    transaction.Commit();
                    Result.OriginalAllowanceNo = Allowance.Main.OriginalAllowanceNo;
                    Result.ResponseAllowanceNo = AllowanceForm.FormNo;
                    Result.Result.ResultCode = "0000";
                    Result.Result.ResultMessage = $"Success";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Result.Result.ResultCode = "9999";
                    Result.Result.ResultMessage = $"存檔失敗, 無法開立折讓, 錯誤原因: "+ex.Message;
                    return Result;
                }
            }
            #endregion
            return Result;
        }

        public ApiResultResponse CancelAllowance(AllowanceCancelViewModel Allowance)
        {
            DateTime date = DateTime.Now;
            ApiResultResponse Result = new ApiResultResponse();
            try
            {
                var AllowanceInfo = (from All in _context.Allowance
                                     where All.AllowanceNo == Allowance.ResponseAllowanceNo
                                     select All).FirstOrDefault();
                var AllowanceDetailsInfo = (from AD in _context.AllowanceDetails
                                            where AD.AllowanceNo == Allowance.ResponseAllowanceNo
                                            select AD);
                if (AllowanceInfo == null)
                {
                    Result.ResultCode = "9999";
                    Result.ResultMessage = "查無發票紀錄，無法作廢";
                }
                else
                {
                    if (AllowanceInfo.CancelUser != "")
                    {
                        Result.ResultCode = "9999";
                        Result.ResultMessage = "發票已作廢無法再次作廢";
                    }
                    else
                    {
                        AllowanceInfo.CancelUser = Allowance.CancelUser;
                        AllowanceInfo.CancelFlag = true;
                        AllowanceInfo.CancelReason = Allowance.CancelReason;
                        AllowanceInfo.CancelDate = date;

                        foreach (var item in AllowanceDetailsInfo)
                        {
                            var InvoiceData = (from inv in _context.Invoice
                                               where inv.InvoiceNumber == item.InvoiceNumber
                                               select inv).FirstOrDefault();
                            InvoiceData.AvaAmount += item.GrossAmount;
                            InvoiceData.ComAmount -= item.GrossAmount;
                        }
                        try
                        {
                            _context.SaveChanges();
                            Result.ResultCode = "0000";
                            Result.ResultMessage = "Success";
                        }
                        catch (Exception ex)
                        {
                            Result.ResultCode = "9999";
                            Result.ResultMessage = "無法存檔，錯誤原因: " + ex.Message;
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                Result.ResultCode = "9999";
                Result.ResultMessage = "無法存檔，錯誤原因: " + ex.Message;
            }
            return Result;
        }
    }
}

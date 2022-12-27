using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{
    public class OrderModelView
    {
        public string Order_No { get; set; }
        public string Order_Date { get; set; }
        public string Seller_ID { get; set; }
        public string Seller_Name { get; set; }
        public string Seller_Address { get; set; }
        public string Seller_PersonInCharge { get; set; }
        public string Seller_TelephoneNumber { get; set; }
        public string Buyer_ID { get; set; }
        public string Buyer_Name { get; set; }
        public string Buyer_Address { get; set; }
        public string Buyer_CustomerNumber { get; set; }
        public string MainRemark { get; set; }
        public string InvoiceType { get; set; }
        public string IInvoiceCategory { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal FreeTaxSalesAmount { get; set; }
        public decimal ZeroTaxSalesAmount { get; set; }
        public decimal TaxType { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal OriginalCurrencyAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Currency { get; set; }
    }
}

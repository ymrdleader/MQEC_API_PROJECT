using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{

    public class InvoiceViewModel
    {
        [Required]
        public string InvoiceCategory { get; set; }
        [Required]
        public string CreateUser { get; set; }
        [Required]
        public Main Main { get; set; }
        [Required]
        public List<ProductItem> Details { get; set; }
        [Required]
        public Amount Amount { get; set; }
    }
    public class Main
    {
        public SellerBuyerModel Seller { get; set; }
        public SellerBuyerModel Buyer { get; set; }
        public string MainRemark { get; set; }
        public string CustomsClearanceMark { get; set; }
        public string RelateNumber { get; set; }
        [Required(ErrorMessage = "發票種類必須輸入")]
        [StringLength(2)]
        public string InvoiceType { get; set; }
        public string GroupMark { get; set; } = "";
        [Required(ErrorMessage = "捐贈註記必須輸入")]
        [StringLength(1)]
        public string DonateMark { get; set; }
        public string CarrierType { get; set; }
        public string CarrierId1 { get; set; }
        public string CarrierId2 { get; set; }
        public string PrintMark { get; set; }
        public string NPOBAN { get; set; }
    }

    public class ProductItem
    {
        [Required]
        [StringLength(256)]
        public string Description { get; set; }
        [Required(ErrorMessage = "數量必須輸入")]
        public int Quantity { get; set; }
        public string Unit { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int SequenceNumber { get; set; }
        public string Remark { get; set; }
        public string RelateNumber { get; set; }
    }

    public class Amount
    {
        [Required]
        public decimal SalesAmount { get; set; }
        [Required]
        public decimal FreeTaxSalesAmount { get; set; }
        [Required]
        public decimal ZeroTaxSalesAmount { get; set; }
        [Required]
        public string TaxType { get; set; }
        [Required]
        public decimal TaxRate { get; set; }
        [Required]
        public decimal TaxAmount { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal OriginalCurrencyAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Currency { get; set; }
    }
}

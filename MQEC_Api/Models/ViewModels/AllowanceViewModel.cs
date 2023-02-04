using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{

    public class AllowanceViewModel
    {
        [Required]
        public AllowanceMain Main { get; set; }
        [Required]
        public List<AllowanceProductItem> Details { get; set; }
        [Required]
        public AllowanceAmount Amount { get; set; }
    }
    public class AllowanceMain
    {
        public string OriginalAllowanceNo { get; set; }
        public string OriginalAllowanceDate { get; set; }
        public string AllowanceType { get; set; }
        public string Memo { get; set; }
        public string CreateUser { get; set; }
    }
   
    public class AllowanceProductItem
    {
        public string OriginalInvoiceNumber { get; set; }
        public string OriginalSequenceNumber { get; set; }
        public string OriginalDescription { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
    }

    public class AllowanceAmount
    {
        [Required]
        public decimal TaxAmount { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
    }
}

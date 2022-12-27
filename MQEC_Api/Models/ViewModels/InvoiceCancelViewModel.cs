using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{
    public class InvoiceCancelViewModel
    {
        [Required]
        [StringLength(10)]
        public string InvoiceNumber { get; set; }
        [Required]
        [StringLength(200)]
        public string CancelReason { get; set; }
        [Required]
        [StringLength(20)]
        public string CancelUser { get; set; }
    }
}

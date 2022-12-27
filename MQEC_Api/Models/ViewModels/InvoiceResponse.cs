using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{
    public class InvoiceResponse
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceTime { get; set; }
        public decimal TotalAmount { get; set; }
        public ApiResultResponse Result { get; set; }
    }
}

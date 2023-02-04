using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{
    public class AllowanceResponse
    {
        public string OriginalAllowanceNo { get; set; }
        public string ResponseAllowanceNo { get; set; }
        public ApiResultResponse Result { get; set; }
    }
}

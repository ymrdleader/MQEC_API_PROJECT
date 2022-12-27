using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{
    public class ApiResultResponse
    {
        public string ExceptionMessage { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
    }
}

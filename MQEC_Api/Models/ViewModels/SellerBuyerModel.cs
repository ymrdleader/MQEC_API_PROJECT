using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{
    public class SellerBuyerModel
    {
        [Required(ErrorMessage = "買/賣方統編必須輸入")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "請輸入數字")]
        public string Identifier { get; set; }
        [Required(ErrorMessage = "買/賣方名稱必須輸入")]
        public string Name { get; set; }

        public string Address { get; set; }

        public string PersonInCharge { get; set; }

        public string TelephoneNumber { get; set; }

        public string FacsimileNumber { get; set; }

        public string EmailAddress { get; set; }

        public string CustomerNumber { get; set; }

        public string RoleRemark { get; set; }
    }
}

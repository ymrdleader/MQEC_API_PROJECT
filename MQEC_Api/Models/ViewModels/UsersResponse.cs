using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{
    public class UsersResponse
    {
        [Display(Name = "User ID")]
        [StringLength(30)]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "EncryptPw")]
        public string EncryptPw { get; set; }

        public string AccessToken { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(100)]
        public string UserEmail { get; set; }


        public bool Discontinue { get; set; }

        public string UseCategory { get; set; }
        public ResultObject Result { get; set; }

        public class ResultObject
        {
            public string ReturnCode { get; set; }
            public string ReturnMessage { get; set; }
        }
    }
}

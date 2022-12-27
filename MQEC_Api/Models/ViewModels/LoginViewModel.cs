using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserId.Required")]
        [Display(Name = "UserId")]
        public string UserId { get; set; }


        [Required(ErrorMessage = "EncryptPw.Required")]
        [DataType(DataType.Password)]
        [Display(Name = "EncryptPw")]
        public string EncryptPw { get; set; }
    }
}

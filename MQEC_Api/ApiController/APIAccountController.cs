using MQEC_Api.Models.ViewModels;
using MQEC_Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MQEC_Api.ApiController
{
    [Route("MQEC/api/[controller]/[action]")]
    [ApiController]
    public class APIAccountController : ControllerBase
    {
        private IAccountService _accountService;
        public APIAccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        /// <summary>
        /// App 使用者登入(帳號密碼) (Phase1)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public UsersResponse Login(LoginViewModel model)
        {
            var result = _accountService.GetIdentity(model);

            return result;
        }
    }
}

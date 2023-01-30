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
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        /// <summary>
        /// 使用者登入(帳號密碼)
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

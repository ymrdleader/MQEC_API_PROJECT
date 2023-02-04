using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MQEC_Api.Services;
using MQEC_Api.Models.ViewModels;
using MQEC_Api.Models;

namespace MQEC_Api.ApiController
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("MQEC/api/[controller]/[action]")]
    [ApiController]
    public class AllowanceController : ControllerBase
    {
        private IAllowanceService _allowanceService;
        public AllowanceController(IAllowanceService AllowanceCanceService)
        {
            _allowanceService = AllowanceCanceService;
        }

        /// <summary>
        /// 產生折讓單
        /// </summary>
        /// <param name="Allowance"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AllowanceResponse> CreateAllowance(AllowanceViewModel Allowance)
        {
            var result = _allowanceService.CreateAllowance(Allowance);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        /// <summary>
        /// 折讓單作廢
        /// </summary>
        /// <param name="Allowance"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ApiResultResponse> CancelAllowance(AllowanceCancelViewModel Allowance)
        {
            var result = _allowanceService.CancelAllowance(Allowance);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
    }
}

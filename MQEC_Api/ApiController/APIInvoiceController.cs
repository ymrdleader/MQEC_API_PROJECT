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
    public class APIInvoiceController : ControllerBase
    {
        private IInvoiceService _invoiceService;
        public APIInvoiceController(IInvoiceService InvoiceService)
        {
            _invoiceService = InvoiceService;
        }
        ///// <summary>
        ///// 取得發票資訊
        ///// </summary>
        ///// <param name="InvoiceNo"></param>
        ///// <returns></returns>
        //[HttpGet("{InvoiceNo}")]
        //public ActionResult<InvoiceResponse>  GetInvoiceData(string[] InvoiceNo)
        //{
        //    var result = _invoiceService.GetInvoiceData(InvoiceNo);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    return result;
        //}
        /// <summary>
        /// 產生發票資訊
        /// </summary>
        /// <param name="Invoice"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<InvoiceResponse> CreateInvoice(InvoiceViewModel Invoice)
        {
            var result = _invoiceService.CreateInvoice(Invoice);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
        /// <summary>
        /// 發票作廢
        /// </summary>
        /// <param name="InvoiceNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ApiResultResponse> CancelInvoice(InvoiceCancelViewModel Invoice)
        {
            var result = _invoiceService.CancelInvoice(Invoice);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
    }
}

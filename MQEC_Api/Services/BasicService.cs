using MQEC_Api.Models;
using MQEC_Api.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace MQEC_Api.Services
{
    interface IBasicService
    {
        bool IsNumber(string strWord);
        bool IsNumeric(string strNumber);
        bool IsEnglish(string strWord);
        bool IsEngNumber(string strWord);
    }
    public class BasicService : IBasicService
    {
        private MQECErpContext _context;
        public BasicService(MQECErpContext context)
        {
            _context = context;
        }

        #region 驗證數字型態(無負數)
        /// <summary>
        /// 驗證數字型態-true:數字
        /// </summary>
        /// <param name="strWord"></param>
        /// <returns></returns>
        public bool IsNumber(string strWord)
        {
            Regex rexNum = new Regex("[^0-9]");
            return !rexNum.IsMatch(strWord);
        }
        #endregion

        #region 驗證數字型態(含負數)
        /// <summary>
        /// 驗證數字型態(含. - )-true:數字
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public bool IsNumeric(string strNumber)
        {
            bool isNumeric = true;
            for (int i = 0; i < strNumber.Length; i++)
            {
                string strWord = strNumber.Substring(i, 1);
                isNumeric = Regex.IsMatch(strWord, @"[^0-9.-]");
                if (isNumeric)
                {
                    return !isNumeric;
                }
            }
            return !isNumeric;
        }
        #endregion

        #region 驗證英文型態
        /// <summary>
        /// 驗證英文型態-true:英文
        /// </summary>
        /// <param name="strWord"></param>
        /// <returns></returns>
        public bool IsEnglish(string strWord)
        {
            Regex rexEg = new Regex("[^A-Za-z]");
            return !rexEg.IsMatch(strWord);
        }
        #endregion

        #region 驗證數字型態(無負數)
        /// <summary>
        /// 驗證數字型態-true:數字
        /// </summary>
        /// <param name="strWord"></param>
        /// <returns></returns>
        public bool IsEngNumber(string strWord)
        {
            Regex rexNum = new Regex("[^A-Za-z0-9]");
            return !rexNum.IsMatch(strWord);
        }
        #endregion
    }
}

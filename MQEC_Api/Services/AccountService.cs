using MQEC_Api.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MQEC_Api.Helpers;
using MQEC_Api.Models.ViewModels;
using MQEC_Api.Service;

namespace MQEC_Api.Services
{
    public interface IAccountService
    {
        UsersResponse GetIdentity(LoginViewModel model);
    }
    public class AccountService : IAccountService
    {
        private MQECErpContext _context;
        private readonly JwtHelpers _jwt;
        public AccountService(
           MQECErpContext context,
           JwtHelpers jwt)
        {
            _context = context;
            _jwt = jwt;
        }
        /// <summary>
        /// 登入帳號驗證
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UsersResponse GetIdentity(LoginViewModel model)
        {
            UsersResponse UserInfo = new UsersResponse();
            UserInfo.Result = new UsersResponse.ResultObject();
            try
            {
                UserInfo.Result.ReturnCode = "0000";
                UserInfo.Result.ReturnMessage = "Success";

                var passwordHasher = new PasswordHasher<Fm>();
                var User = _context.Fm.Where(u => u.UserId == model.UserId).FirstOrDefault();
                if (User != null)
                {
                    #region 檢查帳號正確性
                    var Comparestr = model.UserId.CompareTo(User.UserId);//比較大小寫是否一致

                    Service.Encoder encoder = new Service.Encoder();

                    EncoderType type = EncoderType.SHA256;

                    string encryptPassword = encoder.Encrypt(type, "M%" + model.EncryptPw + "$Y");
                    //var result = passwordHasher.VerifyHashedPassword(User, User.EncryptPw, encryptPassword);
                    var result = User.EncryptPw.CompareTo(encryptPassword);
                    if (result != 0 || Comparestr != 0)
                    {
                        UserInfo.Result.ReturnCode = "0002";
                        UserInfo.Result.ReturnMessage = "帳號或密碼輸入不正確!";
                        return UserInfo;
                    }
                    else if (User != null)
                    {
                        if (User.Discontinue == true)
                        {
                            UserInfo.Result.ReturnCode = "0004";
                            UserInfo.Result.ReturnMessage = "此帳號 " + model.UserId + " 已停用 !";
                        }
                        else
                        {
                            UserInfo.UserId = User.UserId;
                            UserInfo.UserName = User.UserName;
                            UserInfo.UserEmail = User.UserEmail;
                            UserInfo.UseCategory = User.UseCategory;
                            UserInfo.AccessToken = _jwt.GenerateToken(model.UserId);
                            UserInfo.Discontinue = User.Discontinue;
                        }
                    }
                }
                else
                {
                    UserInfo.Result.ReturnCode = "0002";
                    UserInfo.Result.ReturnMessage = "帳號或密碼輸入不正確";
                }
            }
            catch (Exception ex)
            {
                UserInfo.Result.ReturnCode = "9999";
                UserInfo.Result.ReturnMessage = ex.Message;
            }
            #endregion

            return UserInfo;
        }
    }
}

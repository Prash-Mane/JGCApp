using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JGC.Models
{
    public class CheckValidLogin : ICheckValidLogin
    {
        readonly IHttpHelper _httpHelper;
        private readonly IUserDialogs _userDialogs;
        public static PageHelper _pageHelper = new PageHelper();
        public static CompletionPageHelper _CompletionpageHelper = new CompletionPageHelper();

        public CheckValidLogin(IUserDialogs _userDialogs,IHttpHelper httpHelper)
        {
            this._httpHelper = httpHelper;
            this._userDialogs = _userDialogs;
        }
        public async Task<bool> GetValidToken(LoginModel loginModel)
        {
            Boolean Result = false;

            if (loginModel.UserName != null && loginModel.Password != null)
            {
                //Check to see if we need a new token (tokens expire every 3 hours on server, but on handheld we will make them expire every 2 to reduce possible issues a constant connection).
                Boolean GenerateNewToken = false;
                if (_pageHelper.TokenExpiry == null)
                    GenerateNewToken = true;
                else
                {
                    if (_pageHelper.TokenExpiry < DateTime.Now)
                        GenerateNewToken = true;
                }

                if (GenerateNewToken)
                {
                    _pageHelper.TokenTimeStamp = DateTime.Now.ToString(AppConstant.DateSaveFormat);
                    _pageHelper.Token = ModsTools.CreateToken(loginModel.UserName, loginModel.Password, _pageHelper.TokenTimeStamp);
                    Settings.AccessToken = _pageHelper.Token;
                    _pageHelper.TokenExpiry = DateTime.Now.AddHours(2);
                    _pageHelper.UnitID = Settings.UnitID;

                    Result = ModsTools.ValidateToken(_pageHelper.Token, _pageHelper.TokenTimeStamp);
                }
                else
                    Result = ModsTools.CheckForInternetConnection(_pageHelper.Token);
            }           
            if(!Result)
            {
                _pageHelper = new PageHelper();
            }
            return Result;         
        }

        public async Task<bool> GetValidCompletionToken(LoginModel loginModel)
        {
            Boolean Result = false;

            if (loginModel.UserName != null && loginModel.Password != null)
            {
                //Check to see if we need a new token (tokens expire every 3 hours on server, but on handheld we will make them expire every 2 to reduce possible issues a constant connection).
                Boolean GenerateNewToken = false;
                if (_CompletionpageHelper.CompletionTokenExpiry == null)
                    GenerateNewToken = true;
                else
                {
                    if (_CompletionpageHelper.CompletionTokenExpiry < DateTime.Now)
                        GenerateNewToken = true;
                }

                if (GenerateNewToken)
                {
                    _CompletionpageHelper.CompletionTokenTimeStamp = DateTime.Now.ToString(AppConstant.DateSaveFormat);
                    _CompletionpageHelper.CompletionToken = ModsTools.CompletionsCreateToken(loginModel.UserName, loginModel.Password, _CompletionpageHelper.CompletionTokenTimeStamp);
                     Settings.CompletionAccessToken = _CompletionpageHelper.CompletionToken;
                    _CompletionpageHelper.CompletionTokenExpiry = DateTime.Now.AddHours(2);
                    _CompletionpageHelper.CompletionUnitID = Settings.UnitID;

                    Result = ModsTools.CompletionsValidateToken(_CompletionpageHelper.CompletionToken, _CompletionpageHelper.CompletionTokenTimeStamp);
                }
                else
                    Result = ModsTools.CheckForInternetConnection(_CompletionpageHelper.CompletionToken);
            }
            if (!Result)
            {
                _CompletionpageHelper = new CompletionPageHelper();
            }
            return Result;
        }
        public static string GenerateHashForString(string input)
        {
            HashAlgorithm Hasher = new SHA256CryptoServiceProvider();
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] strHash = Hasher.ComputeHash(strBytes);
            return BitConverter.ToString(strHash).Replace("-", "").ToLowerInvariant().Trim();
        }
    }
}

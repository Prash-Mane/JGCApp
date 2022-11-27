using Acr.UserDialogs;
using JGC.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models
{
    public class OperationResult<TResult>
    {
        OperationResult() { }

        public TResult Result { get; private set; }
        public string ErrorMessage { get; private set; }
        public Exception Exception { get; private set; }

        public static OperationResult<TResult> CreateSuccessResult(TResult result)
        {
            return new OperationResult<TResult> { Result = result };
        }

        public static OperationResult<TResult> CreateFailure(string nonSuccessMessage, Exception ex = null)
        {
            return new OperationResult<TResult> { ErrorMessage = nonSuccessMessage, Exception = ex };
        }

        public bool ValidateResponse(bool displayAlert = true)
        {
            var success = string.IsNullOrEmpty(ErrorMessage) && Exception == null;
            if (ErrorMessage == AppConstant.CANCELLED || ErrorMessage == "Unauthorized" || ErrorMessage == AppConstant.NETWORK_FAILURE) //those are handled via page navigation
                displayAlert = false;
            if (!success && displayAlert)
            {
                UserDialogs.Instance.Alert(ErrorMessage);
            }
            return success;
        }
    }
}

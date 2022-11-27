using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using JGC.Common.Interfaces;
using UIKit;

namespace JGC.iOS.DependancyObjects
{
    public class ToastMessage : IToastMessage
    {
        public void LongAlert(string message)
        {
            throw new NotImplementedException();
        }

        public void ShortAlert(string message)
        {
            throw new NotImplementedException();
        }

        public void Show(string message)
        {
            throw new NotImplementedException();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC.Common.Interfaces
{
    public interface IDeviceID
    {
        Task<string> GetDeviceID();
    }
}

﻿using JGC.UserControls.CustomControls;
using JGC.UWP.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ValidationEntry), typeof(ValidationEntryRenderer))]
namespace JGC.UWP.Renderers
{
    public class ValidationEntryRenderer : EntryRenderer
    {
    }
}

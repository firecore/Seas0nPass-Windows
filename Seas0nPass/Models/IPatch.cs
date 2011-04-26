////
//
//  Seas0nPass
//
//  Copyright 2011 FireCore, LLC. All rights reserved.
//  http://firecore.com
//
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seas0nPass.Models
{
    public interface IPatch
    {
        string PerformPatch();
        string CurrentMessage { get; }
        event EventHandler CurrentMessageChanged;
        int CurrentProgress { get; }
        event EventHandler CurrentProgressChanged;
    }
}

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

namespace Seas0nPass.Interfaces
{
    public class ITunesInfo
    {
        public string RequiredVersion { get; set; }
        public string InstalledVersion { get; set; }
        public bool IsCompatible { get; set; }
    }
}

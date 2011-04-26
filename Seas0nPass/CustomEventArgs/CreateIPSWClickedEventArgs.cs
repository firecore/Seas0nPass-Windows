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
using Seas0nPass.Models;

namespace Seas0nPass.CustomEventArgs
{
    public class CreateIPSWClickedEventArgs : EventArgs
    {
        public string FileName { get; private set; }

        public CreateIPSWClickedEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}

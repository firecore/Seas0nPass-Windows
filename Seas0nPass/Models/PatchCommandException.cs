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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Seas0nPass.Models.PatchCommands;

namespace Seas0nPass.Models
{
    [Serializable]
    public class PatchCommandException : Exception
    {
        public PatchCommandException() { }
        public PatchCommandException(string message) : base(message) { }
        public PatchCommandException(string message, Exception inner) : base(message, inner) { }
        protected PatchCommandException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

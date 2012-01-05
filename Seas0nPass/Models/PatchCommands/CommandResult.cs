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

namespace Seas0nPass.Models.PatchCommands
{
    public class CommandResult : ICommandResult
    {
        public bool Success { get; set; }
        public string ErrorText { get; set; }

        public CommandResult(bool success, string errorText = null)
        {
            Success = success;
            ErrorText = errorText;
        }
    }
}

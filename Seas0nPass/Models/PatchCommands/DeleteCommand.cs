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
using System.IO;
using Seas0nPass.Utils;

namespace Seas0nPass.Models.PatchCommands
{
    public class DeleteCommand : PatchCommand
    {
        public DeleteCommand()
            : base("delete")
        {
        }

        public override ICommandResult Execute(IDictionary<string, string> vars, params string[] args)
        {
            if (args.Length != 1)
                return ArgsCountError(1, args);

            SubstituteVariables(vars, args);

            string filePath = args[0];

            if (string.IsNullOrWhiteSpace(filePath))
                return Error("the file path was empty or white space");
            
            SafeFile.Delete(filePath);

            return Success();
        }
    }
}

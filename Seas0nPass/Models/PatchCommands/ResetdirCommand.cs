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
using Seas0nPass.Utils;

namespace Seas0nPass.Models.PatchCommands
{
    public class ResetdirCommand : PatchCommand
    {
        public ResetdirCommand()
            : base("resetdir")
        {
        }

        public override ICommandResult Execute(IDictionary<string, string> vars, params string[] args)
        {
            if (args.Length != 1)
                return ArgsCountError(1, args);

            SubstituteVariables(vars, args);
            string dir = args[0];

            if (string.IsNullOrWhiteSpace(dir))
                return Error("the directory name was empty or white space");

            MiscUtils.RecreateDirectory(dir);

            return Success();
        }
    }
}

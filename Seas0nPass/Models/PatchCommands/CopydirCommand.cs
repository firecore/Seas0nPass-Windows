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
    public class CopydirCommand : PatchCommand
    {
        public CopydirCommand()
            : base("copydir")
        {
        }

        public override ICommandResult Execute(IDictionary<string, string> vars, params string[] args)
        {
            if (args.Length != 2)
                return ArgsCountError(2, args);

            SubstituteVariables(vars, args);

            string source = args[0];
            string dest = args[1];

            if (string.IsNullOrWhiteSpace(source))
                return Error("the source folder path was empty or white space");
            if (string.IsNullOrWhiteSpace(dest))
                return Error("the destination folder path was empty or white space");

            Utils.CopyDirectory(source, dest);

            return Success();
        }
    }
}

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
    public class UnzipCommand : PatchCommand
    {
        public UnzipCommand()
            : base("unzip")
        {
        }

        public override ICommandResult Execute(IDictionary<string, string> vars, params string[] args)
        {
            if (args.Length != 2)
                return ArgsCountError(2, args);

            SubstituteVariables(vars, args);

            string zip = args[0];
            string folder = args[1];

            if (string.IsNullOrWhiteSpace(zip))
                return Error("the zip archive path was empty or white space");
            if (string.IsNullOrWhiteSpace(folder))
                return Error("the destination unzip folder path was empty or white space");

            ArchiveUtils.ExtractZipFile(zip, null, folder);

            return Success();
        }
    }
}

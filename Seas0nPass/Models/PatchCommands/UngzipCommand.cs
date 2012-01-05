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
    public class UngzipCommand : PatchCommand
    {
        public UngzipCommand()
            : base("ungzip")
        {
        }

        public override ICommandResult Execute(IDictionary<string, string> vars, params string[] args)
        {
            if (args.Length != 3)
                return ArgsCountError(3, args);

            SubstituteVariables(vars, args);

            string archivePath = args[0];
            string destPath = args[1];
            string destFile = args[2];

            if (string.IsNullOrWhiteSpace(archivePath))
                return Error("the gzip archive path was empty or white space");
            if (string.IsNullOrWhiteSpace(destPath))
                return Error("the destination folder path was empty or white space");
            if (string.IsNullOrWhiteSpace(destFile))
                return Error("the destination filename was empty or white space");

            ArchiveUtils.ExtractGZip(archivePath, destPath, destFile);

            return Success();
        }
    }
}

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
    public class ProgressCommand : PatchCommand
    {
        private readonly UniversalPatch _patch;

        public ProgressCommand(UniversalPatch patch)
            : base("progress")
        {
            _patch = patch;
        }

        public override ICommandResult Execute(IDictionary<string, string> vars, params string[] args)
        {
            if (args.Length != 1)
                return ArgsCountError(1, args);

            int progress;
            if (!int.TryParse(args[0], out progress))
                return Error(string.Format("Progress value must be integer but was [{0}]", args[0]));

            _patch.UpdateProgress(progress);

            return Success();
        }
    }
}

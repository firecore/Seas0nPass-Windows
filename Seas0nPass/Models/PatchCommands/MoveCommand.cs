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

namespace Seas0nPass.Models.PatchCommands
{
    public class MoveCommand : PatchCommand
    {
        public MoveCommand()
            : base("move")
        {
        }

        public override ICommandResult Execute(IDictionary<string, string> vars, params string[] args)
        {
            if (args.Length != 2)
                return ArgsCountError(2, args);

            SubstituteVariables(vars, args);

            string from = args[0];
            string to = args[1];

            if (string.IsNullOrWhiteSpace(from))
                return Error("the source path was empty or white space");
            if (string.IsNullOrWhiteSpace(to))
                return Error("the destination path was empty or white space");

            File.Move(from, to);

            return Success();
        }
    }
}

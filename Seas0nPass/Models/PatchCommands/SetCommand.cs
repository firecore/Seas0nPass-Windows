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
    public class SetCommand : PatchCommand
    {
        public SetCommand()
            : base("set")
        {
        }

        public override ICommandResult Execute(IDictionary<string, string> vars, params string[] args)
        {
            if (args.Length != 2)
                return ArgsCountError(2, args);
            
            string key = args[0];
            string value = args[1];

            if (string.IsNullOrWhiteSpace(key))
                return Error("the key was empty or white space");
            if (string.IsNullOrWhiteSpace(value))
                return Error("the value was empty or white space");

            vars[key] = value;

            return Success();
        }
    }
}

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
    public class MessageCommand : PatchCommand
    {
        private readonly UniversalPatch _patch;

        public MessageCommand(UniversalPatch patch)
            : base("message")
        {
            _patch = patch;
        }

        public override ICommandResult Execute(IDictionary<string, string> vars, params string[] args)
        {
            if (args.Length != 1)
                return ArgsCountError(1, args);
            
            SubstituteVariables(vars, args);

            string message = args[0];

            _patch.UpdateCurrentMessage(message);

            return Success();
        }
    }
}

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
    public abstract class PatchCommand : IPatchCommand
    {
        public string Name { get; private set;  }

        protected PatchCommand(string name)
        {
            Name = name;
        }

        public abstract ICommandResult Execute(IDictionary<string, string> vars, params string[] args);

        protected CommandResult Error(string text)
        {
            return new CommandResult(false, text);
        }

        protected CommandResult ArgsCountError(int expected, string[] args)
        {
            return Error(string.Format("{0} command expected {1} argument but received {2} [{3}]", Name, expected, args.Length, string.Join(", ", args)));
        }

        protected CommandResult Success()
        {
            return new CommandResult(true);
        }

        protected string[] SubstituteVariables(IDictionary<string, string> vars, string[] args)
        {
            foreach (var item in vars.OrderByDescending(x => x.Key.Length))
            {
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = args[i].Replace(item.Key, item.Value);
                }
            }
            return args;
        }
    }
}

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
using System.Diagnostics;
using System.IO;
using Seas0nPass.Utils;

namespace Seas0nPass.Models.PatchCommands
{
    public class ExecCommand : PatchCommand
    {
        public ExecCommand()
            : base("exec")
        { }            

        public override ICommandResult Execute(IDictionary<string, string> vars, params string[] args)
        {
            if (args.Length < 1)
                return Error("exec command can't be used without arguments (at least tool name must be specified)");

            SubstituteVariables(vars, args);

            var exePath = Path.Combine(@".\bin\", args[0]);
            string argsString = string.Join(" ", args.Skip(1).ToArray());
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = exePath,
                Arguments = argsString,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = SafeDirectory.GetCurrentDirectory(),
            };

            LogUtil.LogEvent(string.Format("Running process: {0}, args: {1} ", processStartInfo.FileName, processStartInfo.Arguments));
            var p = WinProcessUtil.StartNewProcess(processStartInfo);
            p.WaitForExit();
            if (p.ExitCode != 0)
            {
                return Error(string.Format("Process: {0}, args: {1} exited with non-zero code", p.StartInfo.FileName, p.StartInfo.Arguments));
            }

            return Success();
        }
    }
}

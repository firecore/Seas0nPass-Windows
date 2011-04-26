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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Seas0nPass.Models.PatchCommands;

namespace Seas0nPass.Models
{
    public class UniversalPatch : IPatch
    {
        private static readonly Regex _regex = new Regex(@"(?<match>[^\s""]+)|\""(?<match>[^""]*)""");

        private IDictionary<string, IPatchCommand> _patchCommands;
        private readonly string _commandsText;
        
        public event EventHandler CurrentMessageChanged;
        public event EventHandler CurrentProgressChanged;

        public IDictionary<string, string> Variables { get; private set; }

        public UniversalPatch(string commandsText)
        {
            _commandsText = commandsText;
            Variables = new Dictionary<string, string>();

            InitPatchCommands();
        }

        public static void GetVariables(IDictionary<string, string> vars, string commandsText)
        {
            IEnumerable<string> commands = GetCommands(commandsText);
            string[] args;
            string name;
            var setCommand = new SetCommand();
            foreach (string command in commands)
            {
                ParseCommand(command, out args, out name);
                if (name == setCommand.Name)
                {
                    ICommandResult result = setCommand.Execute(vars, args);
                    if (!result.Success)
                        throw new PatchCommandException(string.Format("Error while retrieving patch info.\nSet command args: {1}\nError text: {2}",
                            string.Join(", ", args), result.ErrorText));
                }
            }
        }

        private static IEnumerable<string> GetCommands(string commandsText)
        {
            IEnumerable<string> commands = commandsText
                .Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x));
            return commands;
        }

        public string PerformPatch()
        {
            Directory.SetCurrentDirectory(Utils.WORKING_FOLDER);

            UpdateProgress(0);

            IEnumerable<string> commands = GetCommands(_commandsText);
            foreach (var command in commands)
            {
                ExecuteCommand(command);
            }

            var fullOutputFileName = Path.Combine(Directory.GetCurrentDirectory(), Utils.OUTPUT_FIRMWARE_NAME);

            Directory.SetCurrentDirectory(Utils.OUTPUT_FOLDER_NAME);

            ArchiveUtils.CreateSample(fullOutputFileName, null, Utils.GetAllFileInFodler(Directory.GetCurrentDirectory()));

            UpdateProgress(300);

            return fullOutputFileName;
        }

        public string CurrentMessage { get; private set; }
        public int CurrentProgress { get; private set; }

        public void UpdateCurrentMessage(string message)
        {
            CurrentMessage = message;
            if (CurrentMessageChanged != null)
                CurrentMessageChanged(this, EventArgs.Empty);
        }

        public void UpdateProgress(int progress)
        {
            CurrentProgress = progress;
            if (CurrentProgressChanged != null)
                CurrentProgressChanged(this, EventArgs.Empty);
        }

        private void InitPatchCommands()
        {
            _patchCommands = new Dictionary<string, IPatchCommand>();
            var types = from t in Assembly.GetAssembly(typeof(PatchCommand)).GetTypes()
                        where t.IsClass && t.Namespace == "Seas0nPass.Models.PatchCommands"
                        && !t.IsAbstract && t.IsSubclassOf(typeof(PatchCommand))
                        select t;

            foreach (Type type in types)
            {
                IPatchCommand patchCommand = null;
                try
                {
                    patchCommand = (IPatchCommand)Activator.CreateInstance(type);
                }
                catch (MissingMethodException)
                {
                    patchCommand = (IPatchCommand)Activator.CreateInstance(type, this);
                }
                _patchCommands.Add(patchCommand.Name, patchCommand);
            }
        }

        private static void ParseCommand(string command, out string[] args, out string name)
        {
            args = _regex.Matches(command).Cast<Match>().Select(x => x.Groups["match"].Value).ToArray();
            name = args[0];
            args = args.Skip(1).ToArray();
        }

        private void ExecuteCommand(string command)
        {
            string[] args;
            string name;
            ParseCommand(command, out args, out name);
            if (_patchCommands.ContainsKey(name))
            {
                ICommandResult result = _patchCommands[name].Execute(Variables, args);
                if (!result.Success)
                    throw new PatchCommandException(string.Format("Command name: {0}\nCommand args: {1}\nError text: {2}",
                        name, string.Join(", ", args), result.ErrorText));
            }
            else
            {
                throw new PatchCommandException(string.Format("Unknown command (Name: {0}, Args: {1})", name, string.Join(", ", args)));
            }
        }
    }
}

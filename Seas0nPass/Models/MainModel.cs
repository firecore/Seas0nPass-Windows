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
using Seas0nPass.Interfaces;
using System.IO;
using System.Diagnostics;
using Seas0nPass.Utils;

namespace Seas0nPass.Models
{
    public class MainModel : IMainModel
    {
        public bool IsTetherPossible()
        {
            return SafeFile.Exists(Path.Combine(firmwareVersionModel.AppDataFolder, MiscUtils.KERNEL_CACHE_FILE_NAME)) &&
                   SafeFile.Exists(Path.Combine(firmwareVersionModel.AppDataFolder, MiscUtils.IBSS_FILE_NAME));
        }

        private IFirmwareVersionModel firmwareVersionModel;

        public void SetFirmwareVersionModel(IFirmwareVersionModel firmwareVersionModel)
        {
            this.firmwareVersionModel = firmwareVersionModel;           
        }

        public IEnumerable<string> GetProgramsToWarnNames()
        {
            var programsToWarn = new List<Tuple<string, List<string>>>();

            foreach (var line in ScriptResource.ProgramsToWarn.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var splittedValues = line.Split(';');
                var programToWarn = new Tuple<string, List<string>>(splittedValues[0], new List<string>());
                for (int i = 1; i < splittedValues.Length; i++)
                    programToWarn.Item2.Add(splittedValues[i]);
                programsToWarn.Add(programToWarn);
            }

            var processListNames = Process.GetProcesses().Select(x => x.ProcessName);

            return
            from programToWarn in programsToWarn
            where processListNames.Intersect(programToWarn.Item2).Any()
            select programToWarn.Item1;                           
        }
    }
}

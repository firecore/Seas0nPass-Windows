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

namespace Seas0nPass.Models
{
    public class FirmwareVersionModel : IFirmwareVersionModel
    {
        public FirmwareVersionModel()
        {
            InitBinaries();
            InitVersionsList();
            SelectedVersion = KnownVersions.OrderByDescending(x => x.Code).FirstOrDefault();
        }

        public List<FirmwareVersion> KnownVersions { get; set; }
        public FirmwareVersion SelectedVersion { get; set; }

        public void CheckVersion(string path)
        {
            var md5 = Utils.ComputeMD5(path);
            SelectedVersion = KnownVersions.FirstOrDefault(x => x.MD5 == md5);
        }

        private string GetOriginalFileName()
        {
            if (SelectedVersion != null)
                return SelectedVersion.OriginalFileName;
            throw new InvalidOperationException("Unknown firmware version");
        }

        private string GetPatchedFirmwareName()
        {
            if (SelectedVersion != null)
                return SelectedVersion.PatchedFileName;
            throw new InvalidOperationException("Unknown firmware version");
        }

        private string GetFolderName()
        {
            if (SelectedVersion != null)
                return SelectedVersion.Folder;
            throw new InvalidOperationException("Unknown firmware version");
        }

        private string DownloadedFirmwarePath
        {
            get
            {
                return Path.Combine(Utils.DOCUMENTS_HOME, "Downloads", GetOriginalFileName());
            }
        }

        public string PatchedFirmwarePath
        {
            get
            {
                return Path.Combine(Utils.DOCUMENTS_HOME, GetPatchedFirmwareName());
            }
        }

        public string AppDataFolder
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Seas0nPass", GetFolderName());
            }
        }

        private string customFileLocation;

        public string ExistingFirmwarePath
        {
            get
            {
                return String.IsNullOrWhiteSpace(customFileLocation) ? DownloadedFirmwarePath : customFileLocation;
            }
            set
            {
                customFileLocation = value;
            }
        }

        public string CorrectFirmwareMD5
        {
            get
            {
                if (SelectedVersion != null)
                    return SelectedVersion.MD5;
                throw new InvalidOperationException("Unknown firmware version");
            }

        }

        public string DownloadUri
        {
            get
            {
                if (SelectedVersion != null)
                    return SelectedVersion.DownloadUrl;
                throw new InvalidOperationException("Unknown firmware version");
            }
        }

        private void InitVersionsList()
        {
            KnownVersions = new List<FirmwareVersion>();
            string[] directories = Directory.GetDirectories(Utils.PATCHES_DIRECTORY);
            foreach (var dir in directories)
            {
                string commandsPath = Path.Combine(dir+@"\", Utils.COMMANDS_FILE_NAME);
                FirmwareVersion version = GetFirmwareVersion(commandsPath);
                KnownVersions.Add(version);
            }
        }

        private FirmwareVersion GetFirmwareVersion(string commandsPath)
        {
            using (var sr = new StreamReader(commandsPath))
            {
                var vars = new Dictionary<string, string>();
                string commandsText = sr.ReadToEnd();
                UniversalPatch.GetVariables(vars, commandsText);
                return new FirmwareVersion()
                {
                    Code = vars["$fw_code"],
                    Name = vars["$name"],
                    MD5 = vars["$md5"],
                    OriginalFileName = vars["$orig_filename"],
                    PatchedFileName = vars["$patched_filename"],
                    Folder = vars["$folder"],
                    DownloadUrl = vars["$downUrl"],
                    NeedTether = bool.Parse(vars["$needTether"]),
                    CommandsText = commandsText
                };
            }
        }

        private void InitBinaries()
        {
            var resource = Seas0nPass.ScriptResource.Binaries;
            Utils.RecreateDirectory(Utils.WORKING_FOLDER); 
            ArchiveUtils.GetViaZipInput(new MemoryStream(resource), Utils.WORKING_FOLDER);
        }
    }
}

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
using System.Configuration;
using Seas0nPass.Utils;

namespace Seas0nPass.Models
{
    public class FirmwareVersionModel : IFirmwareVersionModel
    {
        private static readonly string DEFAULT_VERSION = "9A406a";

        public FirmwareVersionModel()
        {
            InitBinaries();
            InitVersionsList();
            SelectedVersion = KnownVersions.Where(x => x.Code == DEFAULT_VERSION).FirstOrDefault();
        }

        public List<FirmwareVersion> KnownVersions { get; set; }
        private FirmwareVersion selectedVersion;
        public FirmwareVersion SelectedVersion
        {
            get
            {
                return selectedVersion;
            }
            set
            {
                customFileLocation = null;
                selectedVersion = value;
            }
        }

        public void CheckVersion(string path)
        {
            var md5 = MiscUtils.ComputeMD5(path);
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
                return Path.Combine(MiscUtils.DOCUMENTS_HOME, "Downloads", GetOriginalFileName());
            }
        }

        public string PatchedFirmwarePath
        {
            get
            {
                return Path.Combine(MiscUtils.DOCUMENTS_HOME, GetPatchedFirmwareName());
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
            string[] directories = SafeDirectory.GetDirectories(MiscUtils.PATCHES_DIRECTORY);
            foreach (var dir in directories)
            {
                string commandsPath = Path.Combine(dir + @"\", MiscUtils.COMMANDS_FILE_NAME);
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
                    Save_iBEC = bool.Parse(vars["$save_iBEC"]),
                    CommandsText = commandsText
                };
            }
        }

        private const string BINARIES_RESOURCE_NAME = "Seas0nPass.Resources.Binaries.zip";
        private void InitBinaries()
        {
#if DEBUG
            // for debug purpuses use local "Binaries" folder
            string binariesPath = ConfigurationManager.AppSettings["binariesPath"];
            if (string.IsNullOrEmpty(binariesPath))
                throw new ArgumentException("Use binariesPath variable in app.config for debug!!!");

            MiscUtils.RecreateDirectory(MiscUtils.WORKING_FOLDER);
            CopyFolder(binariesPath, MiscUtils.WORKING_FOLDER);
#else
            // for production use embedded Binaries.zip
            using (Stream io = this.GetType().Assembly.GetManifestResourceStream(BINARIES_RESOURCE_NAME))
            {
                MiscUtils.RecreateDirectory(MiscUtils.WORKING_FOLDER);
                ArchiveUtils.GetViaZipInput(io, MiscUtils.WORKING_FOLDER);
            }
#endif
        }

#if DEBUG
        private static void CopyFolder(string sourcePath, string destinationPath)
        {
            //Now Create all of the directories
            var directories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)
                .Where(x => !x.Contains("\\.svn")); // Exclude svn folders
            foreach (string dirPath in directories)
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            //Copy all the files
            var allFiles = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)
                .Where(x => !x.Contains(".svn\\")); // Exclude files from svn folders
            foreach (string newPath in allFiles)
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath));
        }
#endif
    }
}

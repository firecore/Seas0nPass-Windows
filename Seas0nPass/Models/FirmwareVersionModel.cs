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
            Version = FirmwareVersions.Version43_8F191m;
        }
        public FirmwareVersions Version
        {
            get;
            set;
        }

        public void CheckVersion(string path)
        {
            var md5 = Utils.ComputeMD5(path);

            if (md5 == Utils.CORRECT_FIRMWARE_421_8C154_MD5)
                Version = FirmwareVersions.Version421_8C154;
            else if (md5 == Utils.CORRECT_FIRMWARE_43_8F191m_MD5)
                Version = FirmwareVersions.Version43_8F191m;
            else Version = FirmwareVersions.Unknown;

        }

        private readonly string ORIGINAL_421_8C154 = "AppleTV2,1_4.2.1_8C154_Restore.ipsw";
        private readonly string ORIGINAL_43_8F191m = "AppleTV2,1_4.3_8F191m_Restore.ipsw";

        private string DownloadedFirmwarePath
        {
            get
            {
                return Path.Combine(
                     Utils.DOCUMENTS_HOME, "Downloads",
                     (Version == FirmwareVersions.Version421_8C154) ?
                     ORIGINAL_421_8C154 : ORIGINAL_43_8F191m
                 );
            }
        }

        private readonly string PATCHED_421_8C154 = "AppleTV2,1_4.2.1_8C154_SP_Restore.ipsw";
        private readonly string PATCHED_43_8F191m = "AppleTV2,1_4.3_8F191m_SP_Restore.ipsw";

        public string PatchedFirmwarePath
        {
            get
            {
                return Path.Combine(
                    Utils.DOCUMENTS_HOME,
                    (Version == FirmwareVersions.Version421_8C154) ?
                    PATCHED_421_8C154 : PATCHED_43_8F191m
                );
            }
        }

        private readonly string FOLDER_421_8C154 = "AppleTV2,1_4.2.1_8C154";
        private readonly string FOLDER_43_8F191m = "AppleTV2,1_4.3_8F5166b";

        public string AppDataFolder
        {
            get
            {
                return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SeanOnPass",
                    (Version == FirmwareVersions.Version421_8C154) ?
                    FOLDER_421_8C154 : FOLDER_43_8F191m
            );

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
                return (Version == FirmwareVersions.Version421_8C154) ?
                Utils.CORRECT_FIRMWARE_421_8C154_MD5 : Utils.CORRECT_FIRMWARE_43_8F191m_MD5;
            }
        }

        public string DownloadUri 
        {
            get
            {
                return (Version == FirmwareVersions.Version421_8C154) ? 
                    Seas0nPass.ScriptResource.firmware_421_8C154_Url : Seas0nPass.ScriptResource.firmware_43_8F191m_Url;
            }
        }





        public void InitBinaries()
        {
            var resource = Seas0nPass.ScriptResource.Binaries;
            ArchiveUtils.GetViaZipInput(new MemoryStream(resource), Utils.WORKING_FOLDER);
        }
    }
}

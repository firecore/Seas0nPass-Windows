using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seas0nPass.Interfaces
{
    public interface IFirmwareVersionModel
    {
        FirmwareVersions Version { get; set; }
        void CheckVersion(string path);

        string ExistingFirmwarePath
        {
            get;
            set;
        }
        string AppDataFolder { get; }
        string PatchedFirmwarePath { get; }
        string CorrectFirmwareMD5 { get; }
        string DownloadUri { get; }

        void InitBinaries();
    }
}

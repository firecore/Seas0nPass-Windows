using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seas0nPass.Models;

namespace Seas0nPass.Interfaces
{
    public interface IFirmwareVersionModel
    {
        void CheckVersion(string path);

        string ExistingFirmwarePath { get; set; }
        string AppDataFolder { get; }
        string PatchedFirmwarePath { get; }
        string CorrectFirmwareMD5 { get; }
        string DownloadUri { get; }
        List<FirmwareVersion> KnownVersions { get; set; }
        FirmwareVersion SelectedVersion { get; set; }
    }
}

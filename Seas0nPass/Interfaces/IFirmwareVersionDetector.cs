using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seas0nPass.Interfaces
{
    public interface IFirmwareVersionDetector
    {
        FirmwareVersions Version { get; }

        void SaveState(FirmwareVersions Version);
    }
}

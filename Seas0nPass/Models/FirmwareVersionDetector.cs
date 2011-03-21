using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seas0nPass.Interfaces;
using System.Xml.Serialization;
using System.IO;

namespace Seas0nPass.Models
{
    public class FirmwareVersionDetector : IFirmwareVersionDetector
    {
        private readonly string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SeanOnPass", "version.xml");

        public FirmwareVersions Version
        {
            get
            {
                if (!File.Exists(fileName)) 
                    return FirmwareVersions.Unknown;
                try
                {
                    using (var fileStream = new FileStream(fileName, FileMode.Open))
                        return (FirmwareVersions)new XmlSerializer(typeof(FirmwareVersions)).Deserialize(fileStream);
                }
                catch (InvalidOperationException)
                {
                    return FirmwareVersions.Unknown;
                }
            }
        }

        public void SaveState(FirmwareVersions version)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create))
                new XmlSerializer(typeof(FirmwareVersions)).Serialize(fileStream, version);
        }
    }
}

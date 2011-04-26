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
using System.Xml.Serialization;
using System.IO;

namespace Seas0nPass.Models
{
    public class FirmwareVersionDetector : IFirmwareVersionDetector
    {
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(FirmwareVersion));
        private readonly string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Seas0nPass", "version.xml");

        public FirmwareVersion Version
        {
            get
            {
                if (!File.Exists(fileName))
                    return null;
                try
                {
                    using (var fileStream = new FileStream(fileName, FileMode.Open))
                    {
                        return (FirmwareVersion)_xmlSerializer.Deserialize(fileStream);
                    }
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public void SaveState(FirmwareVersion version)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                _xmlSerializer.Serialize(fileStream, version);
            }
        }
    }
}

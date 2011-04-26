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
    public class MainModel : IMainModel
    {
        public bool IsTetherPossible()
        {
            return File.Exists(Path.Combine(firmwareVersionModel.AppDataFolder, Utils.KERNEL_CACHE_FILE_NAME)) &&
                   File.Exists(Path.Combine(firmwareVersionModel.AppDataFolder, Utils.IBSS_FILE_NAME));
        }

        private IFirmwareVersionModel firmwareVersionModel;

        public void SetFirmwareVersionModel(IFirmwareVersionModel firmwareVersionModel)
        {
            this.firmwareVersionModel = firmwareVersionModel;
        }
    }
}

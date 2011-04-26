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

namespace Seas0nPass.Interfaces
{
    public interface IDownloadModel
    {
        void StartDownload();
        void CancelDownload();

        event EventHandler ProgressChanged;
        event EventHandler DownloadCompleted;
        event EventHandler DownloadFailed;
        event EventHandler DownloadCanceled;

        int Percentage { get; }

        void SetFirmwareVersionModel(IFirmwareVersionModel firmwareVersionModel);
    }
}
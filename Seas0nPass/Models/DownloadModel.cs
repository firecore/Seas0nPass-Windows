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
using System.Net;
using System.Configuration;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace Seas0nPass.Models
{
    public class DownloadModel : IDownloadModel
    {
        private int percentage;
        private WebClient webClient = new WebClient();

        private readonly string fileName = Path.Combine(Utils.WORKING_FOLDER, Utils.DOWNLOADED_FILE_PATH);

        

        private void PerformStart()
        {
            if (File.Exists(firmwareVersionModel.ExistingFirmwarePath) &&
                Utils.ComputeMD5(firmwareVersionModel.ExistingFirmwarePath) == firmwareVersionModel.CorrectFirmwareMD5)
            {
                LogUtil.LogEvent("Original firmware found on disk");

                File.Copy(firmwareVersionModel.ExistingFirmwarePath, Path.Combine(Utils.WORKING_FOLDER, Utils.DOWNLOADED_FILE_PATH), true);
                if (DownloadFinished != null)
                    DownloadFinished(this, EventArgs.Empty);
                return;
            }

            LogUtil.LogEvent("Starting download");

            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
            webClient.DownloadFileAsync(new Uri(firmwareVersionModel.DownloadUri), fileName);
        }

        public void StartDownload()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler((sender, e) => PerformStart());
            worker.RunWorkerAsync();
        }


        void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            LogUtil.LogEvent("Download completed");

            File.Copy(Path.Combine(Utils.WORKING_FOLDER, Utils.DOWNLOADED_FILE_PATH), firmwareVersionModel.ExistingFirmwarePath, true);

            LogUtil.LogEvent("Downloaded file copied to Documents folder");

            if (DownloadFinished != null)
                DownloadFinished(sender, e);
        }

        void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                percentage = e.ProgressPercentage;
                ProgressChanged(sender, e);
            }

        }

        public event EventHandler ProgressChanged;

        public event EventHandler DownloadFinished;
        

        public int Percentage
        {
            get { return percentage; }
        }
        

        public void CancelDownload()
        {
            LogUtil.LogEvent("Cancelling download");
            webClient.CancelAsync();
            while (webClient.IsBusy)
            {
                Thread.Sleep(50);
            }
            if (File.Exists(fileName))
                File.Delete(fileName);


        }


        private IFirmwareVersionModel firmwareVersionModel;

        public void SetFirmwareVersionModel(IFirmwareVersionModel firmwareVersionModel)
        {
            this.firmwareVersionModel = firmwareVersionModel;
        }
    }
}

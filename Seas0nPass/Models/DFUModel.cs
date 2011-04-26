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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Globalization;

namespace Seas0nPass.Models
{
    public class DFUModel : IDFUModel
    {
        public event EventHandler ProcessFinished;
        public event EventHandler CurrentMessageChanged;
        public event EventHandler ProgressChanged;

        private string currentMessage;
        public string CurrentMessage
        {
            get { return currentMessage; }
        }

        private int progressPercentage;
        public int ProgressPercentage
        {
            get { return progressPercentage; }
        }

        private IFirmwareVersionModel firmwareVersionModel;
        public void SetFirmwareVersionModel(IFirmwareVersionModel firmwareVersionModel)
        {
            this.firmwareVersionModel = firmwareVersionModel;
        }

        public void StartProcess()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            worker.RunWorkerAsync();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ProcessFinished != null)
                ProcessFinished(sender, e);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            RunDFU();
        }

        private void RestoreDFUFile()
        {
            LogUtil.LogEvent("Restoring DFU file");

            File.Copy(Path.Combine(firmwareVersionModel.AppDataFolder, Utils.IBSS_FILE_NAME),
                Path.Combine(Utils.BIN_DIRECTORY, Utils.IBSS_FILE_NAME), true);
        }

        private void RunDFU()
        {
            RestoreDFUFile();

            LogUtil.LogEvent("DFU process starting");

            Directory.SetCurrentDirectory(Utils.BIN_DIRECTORY);

            var p = new Process();
            p.StartInfo.FileName = @"dfu.exe";
            p.StartInfo.Arguments = @"iBSS.k66ap.RELEASE.dfu";

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.OutputDataReceived +=new DataReceivedEventHandler((sender, e) => HandleOutputData(e.Data));
            p.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => HandleOutputData(e.Data));
                        
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            p.WaitForExit();
            if (p.ExitCode != 0)
            {
                var errorString = string.Format("Process: {0}, args: {1} exited with non-zero code", p.StartInfo.FileName, p.StartInfo.Arguments);
                LogUtil.LogEvent(errorString);
                throw new InvalidOperationException(errorString);
            }
        }

        public void HandleOutputData(string data)
        {
            if (data == null)
                return;

            LogUtil.LogEvent(string.Format("Output received: {0}", data));

            if (data.StartsWith("::"))
            {
                currentMessage = data.Substring(2);
                if (CurrentMessageChanged != null)
                    CurrentMessageChanged(this, EventArgs.Empty);
            }

            if (data.StartsWith("##"))
            {
                var percentString = data.Substring(3, data.Length - 4);
                var info = new CultureInfo("en-US");
                progressPercentage = Convert.ToInt32(double.Parse(percentString, info), info);

                if (ProgressChanged != null)
                    ProgressChanged(this, EventArgs.Empty);
            }
        }
    }
}

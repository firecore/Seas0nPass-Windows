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
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace Seas0nPass.Models
{
    public class PatchModel : IPatchModel
    {
        private IPatch patch;
        private IFirmwareVersionModel firmwareVersionModel;
        private int currentProgress;
        private string currentMessage;

        public event EventHandler CurrentMessageChanged;
        public event EventHandler ProgressUpdated;
        public event EventHandler Finished;

        public int CurrentProgress
        {
            get { return currentProgress; }
        }

        public string CurrentMessage
        {
            get { return currentMessage; }
        }

        public void SetFirmwareVersionModel(IFirmwareVersionModel firmwareVersionModel)
        {
            this.firmwareVersionModel = firmwareVersionModel;
        }

        public void StartProcess()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            PerformPatch();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        private void UpdateProgress(int value)
        {
            currentProgress = value;
            if (ProgressUpdated != null)
                ProgressUpdated(this, EventArgs.Empty);
        }

        private void UpdateCurrentMessage(string message)
        {
            LogUtil.LogEvent(string.Format("Patching message changed to: {0}", message));

            currentMessage = message;
            if (CurrentMessageChanged != null)
                CurrentMessageChanged(this, EventArgs.Empty);
        }

        private void SaveDFUAndTetherFiles()
        {
            LogUtil.LogEvent(string.Format("Saving {0} and {1} files", Utils.KERNEL_CACHE_FILE_NAME, Utils.IBSS_FILE_NAME));

            Utils.RecreateDirectory(firmwareVersionModel.AppDataFolder);

            LogUtil.LogEvent(string.Format("Directory {0} recreated successfully", firmwareVersionModel.AppDataFolder));

            File.Copy(Path.Combine(Utils.WORKING_FOLDER, Utils.OUTPUT_FOLDER_NAME, Utils.KERNEL_CACHE_FILE_NAME),
                      Path.Combine(firmwareVersionModel.AppDataFolder, Utils.KERNEL_CACHE_FILE_NAME), true);

            LogUtil.LogEvent(string.Format("{0} file copied successfully", Utils.KERNEL_CACHE_FILE_NAME));

            File.Copy(Path.Combine(Utils.WORKING_FOLDER, Utils.OUTPUT_FOLDER_NAME, Utils.FIRMWARE_FOLDER_NAME, Utils.DFU_FOLDER_NAME, Utils.IBSS_FILE_NAME),
                      Path.Combine(firmwareVersionModel.AppDataFolder, Utils.IBSS_FILE_NAME), true);

            LogUtil.LogEvent(string.Format("{0} file copied successfully", Utils.IBSS_FILE_NAME));
        }

        private IPatch GetPatch()
        {
            if (firmwareVersionModel.SelectedVersion != null)
                return new UniversalPatch(firmwareVersionModel.SelectedVersion.CommandsText);
            throw new InvalidOperationException("Unknown firmware version");
        }

     

        private void PerformPatch()
        {
            patch = GetPatch();

            patch.CurrentMessageChanged += patch_CurrentMessageChanged;
            patch.CurrentProgressChanged += patch_CurrentProgressChanged;

            string resultFile = patch.PerformPatch();

            patch.CurrentMessageChanged -= patch_CurrentMessageChanged;
            patch.CurrentProgressChanged -= patch_CurrentProgressChanged;
            patch = null;

            SaveDFUAndTetherFiles();

            File.Copy(resultFile, firmwareVersionModel.PatchedFirmwarePath, true);           

            if (Finished != null)
                Finished(this, EventArgs.Empty);
        }

        private void patch_CurrentMessageChanged(object sender, EventArgs args)
        {
            UpdateCurrentMessage(patch.CurrentMessage);
        }

        private void patch_CurrentProgressChanged(object sender, EventArgs args)
        {
            UpdateProgress(patch.CurrentProgress);
        }
    }
}

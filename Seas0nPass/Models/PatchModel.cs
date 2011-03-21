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



        private int currentProgress;

        private void UpdateProgress(int value)
        {
            currentProgress = value;
            if (ProgressUpdated != null)
                ProgressUpdated(this, EventArgs.Empty);
        }

        public event EventHandler CurrentMessageChanged;

        private void UpdateCurrentMessage(string message)
        {
            LogUtil.LogEvent(string.Format("Patching message changed to: {0}", message));

            currentMessage = message;
            if (CurrentMessageChanged != null)
                CurrentMessageChanged(this, EventArgs.Empty);
        }

        private string currentMessage;
        public string CurrentMessage { get {return currentMessage;} }


        private void SaveDFUAndTetherFiles()
        {
            LogUtil.LogEvent(string.Format("Saving {0} and {1} files", Utils.KERNEL_CACHE_FILE_NAME, Utils.IBSS_FILE_NAME));

            Utils.RecreateDirectory(firmwareVersionModel.AppDataFolder);

            File.Copy(Path.Combine(Utils.WORKING_FOLDER, Utils.OUTPUT_FOLDER_NAME, Utils.KERNEL_CACHE_FILE_NAME),
                      Path.Combine(firmwareVersionModel.AppDataFolder, Utils.KERNEL_CACHE_FILE_NAME), true);
            File.Copy(Path.Combine(Utils.WORKING_FOLDER, Utils.OUTPUT_FOLDER_NAME, Utils.FIRMWARE_FOLDER_NAME, Utils.DFU_FOLDER_NAME, Utils.IBSS_FILE_NAME),
                      Path.Combine(firmwareVersionModel.AppDataFolder, Utils.IBSS_FILE_NAME), true);
        }


        private void PerformPatch()
        {
            IPatch patch = firmwareVersionModel.Version == FirmwareVersions.Version421_8C154 ? 
                (IPatch)new Patch_421_8C154() : 
                (IPatch)new Patch_43_8F191m();
            
            patch.CurrentMessageChanged += (sender, args) => UpdateCurrentMessage(patch.CurrentMessage);
            patch.CurrentProgressChanged += (sender, args) => UpdateProgress(patch.CurrentProgress);

            firmwareVersionModel.InitBinaries();

            string resultFile = patch.PerformPatch();

            SaveDFUAndTetherFiles();

            File.Copy(resultFile, firmwareVersionModel.PatchedFirmwarePath, true);

            Utils.OpenExplorerWindow(firmwareVersionModel.PatchedFirmwarePath);

            if (Finished != null)
                Finished(this, EventArgs.Empty);
        }


        public void StartProcess()
        {

            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            PerformPatch();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public event EventHandler ProgressUpdated;

        public event EventHandler Finished;



        


        public int CurrentProgress
        {
            get { return currentProgress; }
        }


        private IFirmwareVersionModel firmwareVersionModel;

        public void SetFirmwareVersionModel(IFirmwareVersionModel firmwareVersionModel)
        {
            this.firmwareVersionModel = firmwareVersionModel;
        }
    }
}

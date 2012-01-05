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
using Seas0nPass.Utils;

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
            worker.RunWorkerCompleted += (sender, args) => { if (args.Error != null) throw args.Error; };
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
            LogUtil.LogEvent(string.Format("Saving {0} and {1} files", MiscUtils.KERNEL_CACHE_FILE_NAME, MiscUtils.IBSS_FILE_NAME));

            MiscUtils.RecreateDirectory(firmwareVersionModel.AppDataFolder);

            LogUtil.LogEvent(string.Format("Directory {0} recreated successfully", firmwareVersionModel.AppDataFolder));

            string kernelcache = Path.Combine(MiscUtils.WORKING_FOLDER, MiscUtils.OUTPUT_FOLDER_NAME, MiscUtils.KERNEL_CACHE_FILE_NAME);
            if (SafeFile.Exists(kernelcache))
            {
                SafeFile.Copy(kernelcache, Path.Combine(firmwareVersionModel.AppDataFolder, MiscUtils.KERNEL_CACHE_FILE_NAME), true);
                LogUtil.LogEvent(string.Format("{0} file copied successfully", MiscUtils.KERNEL_CACHE_FILE_NAME));
            }

            string iBSS = Path.Combine(MiscUtils.WORKING_FOLDER, MiscUtils.OUTPUT_FOLDER_NAME, MiscUtils.FIRMWARE_FOLDER_NAME, MiscUtils.DFU_FOLDER_NAME, MiscUtils.IBSS_FILE_NAME);
            if (SafeFile.Exists(iBSS))
            {
                SafeFile.Copy(iBSS, Path.Combine(firmwareVersionModel.AppDataFolder, MiscUtils.IBSS_FILE_NAME), true);
                LogUtil.LogEvent(string.Format("{0} file copied successfully", MiscUtils.IBSS_FILE_NAME));
            }

            string iBEC = Path.Combine(MiscUtils.WORKING_FOLDER, MiscUtils.OUTPUT_FOLDER_NAME, MiscUtils.FIRMWARE_FOLDER_NAME, MiscUtils.DFU_FOLDER_NAME, MiscUtils.IBEC_FILE_NAME);
            if (firmwareVersionModel.SelectedVersion.Save_iBEC && SafeFile.Exists(iBEC))
            {
                SafeFile.Copy(iBEC, Path.Combine(firmwareVersionModel.AppDataFolder, MiscUtils.IBEC_FILE_NAME), true);
                LogUtil.LogEvent(string.Format("{0} file copied successfully", MiscUtils.IBEC_FILE_NAME));
            }
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

            SafeFile.Copy(resultFile, firmwareVersionModel.PatchedFirmwarePath, true);           

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

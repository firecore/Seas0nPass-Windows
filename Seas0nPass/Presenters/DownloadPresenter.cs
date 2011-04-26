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

namespace Seas0nPass.Presenters
{
    public class DownloadPresenter
    {
        public enum ProcessResult
        {
            Completed,
            Cancelled,
            Failed
        }

        private ProcessResult result = ProcessResult.Completed;
        public ProcessResult Result
        {
            get { return result; }
        }

        private IDownloadModel model;
        private IDownloadView view;

        public event EventHandler ProcessFinished;

        private IFirmwareVersionModel firmwareVersionModel;

        public void SetFirmwareVersionModel(IFirmwareVersionModel firmwareVersionModel)
        {
            this.firmwareVersionModel = firmwareVersionModel;
        }

        public DownloadPresenter(IDownloadModel model, IDownloadView view)
        {
            this.model = model;
            this.view = view;

            model.ProgressChanged += model_ProgressChanged;
            view.ActionButtonClicked += view_ActionButtonClicked;
            model.DownloadCompleted += model_DownloadFinished;
            model.DownloadFailed += model_DownloadFailed;
            model.DownloadCanceled += model_DownloadCanceled;
        }

        private void model_DownloadFailed(object sender, EventArgs e)
        {
            result = ProcessResult.Failed;
            if (ProcessFinished != null)
                ProcessFinished(sender, e);
        }

        private void model_DownloadFinished(object sender, EventArgs e)
        {
            result = ProcessResult.Completed;
            if (ProcessFinished != null)
                ProcessFinished(sender, e);
        }

        private void model_DownloadCanceled(object sender, EventArgs e)
        {
            result = ProcessResult.Cancelled;
            if (ProcessFinished != null)
                ProcessFinished(sender, e);
        }

        public void StartProcess()
        {
            view.SetMessageText(string.Format("Downloading {0}...",  Path.GetFileName(firmwareVersionModel.ExistingFirmwarePath)));
            view.SetActionButtonText("Cancel");
            model.StartDownload();
        }

        private void view_ActionButtonClicked(object sender, EventArgs e)
        {
            model.CancelDownload();
        }

        private void model_ProgressChanged(object sender, EventArgs e)
        {
            view.UpdateProgress(model.Percentage);
        }
    }
}

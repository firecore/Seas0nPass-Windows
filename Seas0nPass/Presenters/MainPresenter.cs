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
using Seas0nPass.CustomEventArgs;
using System.IO;

namespace Seas0nPass.Presenters
{
    public class MainPresenter
    {
        private DFUPresenter dfuPresenter;
        private PatchPresenter patchPresetner;
        private TetherPresenter tetherPresenter;
        private IMainView view;
        public MainPresenter(IMainView view)
        {
            this.view = view;
        }

        private IStartView startControl;
        private IDownloadModel downloadModel;
        private IDownloadView downloadView;
        private IPatchView patchControl;
        private IPatchModel patchModel;
        private IDFUView dfuControl;
        private IDFUModel dfuModel;
        private IMainModel mainModel;

        private DownloadPresenter downloadPresenter;

        private IDFUSuccessControl dfuSuccessControl;
        private ITetherSuccessControl tetherSuccessControl;
        private IFirmwareVersionModel firmwareVersionModel;
        private IFirmwareVersionDetector firmwareVersionDetector;

        private ITetherView tetherView;
        private ITetherModel tetherModel;

        private void InstantiateModelsAndViews()
        {
            startControl = IoC.Resolve<IStartView>();
            downloadModel = IoC.Resolve<IDownloadModel>();
            downloadView = IoC.Resolve<IDownloadView>();
            patchControl = IoC.Resolve<IPatchView>();
            patchModel = IoC.Resolve<IPatchModel>();
            dfuControl = IoC.Resolve<IDFUView>();
            dfuModel = IoC.Resolve<IDFUModel>();
            dfuSuccessControl = IoC.Resolve<IDFUSuccessControl>();
            tetherSuccessControl = IoC.Resolve<ITetherSuccessControl>();
            mainModel = IoC.Resolve<IMainModel>();
            firmwareVersionModel = IoC.Resolve<IFirmwareVersionModel>();
            tetherView = IoC.Resolve<ITetherView>();
            tetherModel = IoC.Resolve<ITetherModel>();
            firmwareVersionDetector = IoC.Resolve<IFirmwareVersionDetector>();

            mainModel.SetFirmwareVersionModel(firmwareVersionModel);
            downloadModel.SetFirmwareVersionModel(firmwareVersionModel);
            patchModel.SetFirmwareVersionModel(firmwareVersionModel);
            dfuModel.SetFirmwareVersionModel(firmwareVersionModel);
            tetherModel.SetFirmwareVersionModel(firmwareVersionModel);

            tetherPresenter = new TetherPresenter(tetherModel, tetherView);
            tetherPresenter.ProcessFinished += tetherPresenter_ProcessFinished;
            patchPresetner = new PatchPresenter(patchControl, patchModel);
            patchPresetner.Finished += patchPresetner_Finished;
            dfuPresenter = new DFUPresenter(dfuModel, dfuControl);
            dfuPresenter.ProcessFinished += dfuPresenter_ProcessFinished;
            downloadPresenter = new DownloadPresenter(downloadModel, downloadView);
            downloadPresenter.ProcessFinished += downloadPresenter_ProcessFinished;
        }

        private void ShowStartPage()
        {
            var patchedVersion = firmwareVersionDetector.Version;

            if (patchedVersion == null)
                startControl.DisableTether();
            else if (patchedVersion.NeedTether)
                startControl.EnableTether();
            else
                startControl.SetTetherNotRequiredState();

            view.ShowControl(startControl);
        }

        public void Init()
        {
            InstantiateModelsAndViews();

            startControl.InitFirmwaresList(firmwareVersionModel.KnownVersions.ToArray());

            startControl.CreateIPSWClicked += startControl_CreateIPSWClicked;
            startControl.CreateIPSW_fwVersion_Clicked += startControl_CreateIPSW_fwVersion_Clicked;
            startControl.TetherClicked += startControl_TetherClicked;
            dfuSuccessControl.ButtonClicked += dfuSuccessControl_ButtonClicked;
            tetherSuccessControl.ButtonClicked += tetherSuccessControl_ButtonClicked;

            ShowStartPage();
        }

        private void startControl_CreateIPSW_fwVersion_Clicked(object sender, CreateIPSWFirmwareClickedEventArgs e)
        {
            firmwareVersionModel.SelectedVersion = e.FirmwareVersion;
            DoDownload();
        }

        private void tetherSuccessControl_ButtonClicked(object sender, EventArgs e)
        {
            ShowStartPage();
        }

        private void dfuSuccessControl_ButtonClicked(object sender, EventArgs e)
        {
            ShowStartPage();
        }

        private void StartTetherProcess()
        {
            view.ShowControl(tetherView);
            tetherPresenter.StartProcess();
        }

        private void startControl_TetherClicked(object sender, EventArgs e)
        {
            var detectedVersion = firmwareVersionDetector.Version;
            if (detectedVersion == null)
                return;

            firmwareVersionModel.SelectedVersion = detectedVersion;

            if (detectedVersion.NeedTether)
                StartTetherProcess();
            else
                view.ShowTetherMessage(detectedVersion.Folder);
        }

        private void tetherPresenter_ProcessFinished(object sender, EventArgs e)
        {
            view.ShowControl(tetherSuccessControl);
        }

        private void DoDownload()
        {
            downloadPresenter.SetFirmwareVersionModel(firmwareVersionModel);
            view.ShowControl(downloadView);
            downloadPresenter.StartProcess();
        }

        private void startControl_CreateIPSWClicked(object sender, CreateIPSWClickedEventArgs e)
        {
            var fileName = e.FileName;

            if (!String.IsNullOrEmpty(fileName))
            {
                firmwareVersionModel.CheckVersion(fileName);

                if (firmwareVersionModel.SelectedVersion == null)
                    return;
                firmwareVersionModel.ExistingFirmwarePath = fileName;
            }

            DoDownload();
        }

        private void downloadPresenter_ProcessFinished(object sender, EventArgs e)
        {
            if (downloadPresenter.Result == DownloadPresenter.ProcessResult.Failed)
            {
                view.ShowDownloadFailedMessage();
                ShowStartPage();
                return;
            }

            if (downloadPresenter.Result == DownloadPresenter.ProcessResult.Cancelled)
            {
                ShowStartPage();
                return;
            }

            view.ShowControl(patchControl);
            patchPresetner.StartPatch();
        }

        private void patchPresetner_Finished(object sender, EventArgs e)
        {
            firmwareVersionDetector.SaveState(firmwareVersionModel.SelectedVersion);

            view.ShowControl(dfuControl);
            dfuPresenter.StartProcess();
        }

        private void dfuPresenter_ProcessFinished(object sender, EventArgs e)
        {
            dfuSuccessControl.SetFileName(Path.GetFileName(firmwareVersionModel.PatchedFirmwarePath));
            view.ShowControl(dfuSuccessControl);
        }
    }
}

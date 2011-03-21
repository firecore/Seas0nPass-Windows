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


        }

        private void ShowStartPage()
        {
            var patchedVersion = firmwareVersionDetector.Version;

            switch (patchedVersion)
            {
                case FirmwareVersions.Version421_8C154:
                    startControl.SetTetherNotRequiredState();
                    break;
                case FirmwareVersions.Version43_8F191m:
                    startControl.EnableTether();
                    break;
                case FirmwareVersions.Unknown:
                    startControl.DisableTether();
                    break;
                default:
                    throw new InvalidOperationException("Unknown version detected");
            }

            view.ShowControl(startControl);
        }

        public void Init()
        {
            InstantiateModelsAndViews();
            startControl.CreateIPSWClicked += startControl_CreateIPSWClicked;
            startControl.CreateIPSW_421_8C154_Clicked += new EventHandler(startControl_CreateIPSW_421_8C154_Clicked);
            startControl.CreateIPSW_43_8F191m_Clicked += new EventHandler(startControl_CreateIPSW_43_8F191m_Clicked);
            startControl.TetherClicked += new EventHandler(startControl_TetherClicked);
            dfuSuccessControl.ButtonClicked += new EventHandler(dfuSuccessControl_ButtonClicked);
            tetherSuccessControl.ButtonClicked += new EventHandler(tetherSuccessControl_ButtonClicked);
            mainModel.InitWorkingFolder();
            ShowStartPage();
        }

        void startControl_CreateIPSW_43_8F191m_Clicked(object sender, EventArgs e)
        {
            firmwareVersionModel.Version = FirmwareVersions.Version43_8F191m;
            DoDownload();
        }

        void startControl_CreateIPSW_421_8C154_Clicked(object sender, EventArgs e)
        {
            firmwareVersionModel.Version = FirmwareVersions.Version421_8C154;
            DoDownload();
        }

        void tetherSuccessControl_ButtonClicked(object sender, EventArgs e)
        {
            ShowStartPage();
        }

        void dfuSuccessControl_ButtonClicked(object sender, EventArgs e)
        {
            ShowStartPage();
        }

        void startControl_TetherClicked(object sender, EventArgs e)
        {
            var detectedVersion = firmwareVersionDetector.Version;
            if (detectedVersion == FirmwareVersions.Unknown)
                return;

            firmwareVersionModel.Version = detectedVersion;

            switch (detectedVersion)
            {
                case FirmwareVersions.Version43_8F191m:                    
                    var tetherPresenter = new TetherPresenter(tetherModel, tetherView);
                    tetherPresenter.ProcessFinished += new EventHandler(tetherPresenter_ProcessFinished);
                    view.ShowControl(tetherView);
                    tetherPresenter.StartProcess();
                    break;
                case FirmwareVersions.Version421_8C154:
                    view.ShowTetherMessage();
                    break;
            }
        }

        void tetherPresenter_ProcessFinished(object sender, EventArgs e)
        {
            view.ShowControl(tetherSuccessControl);
        }

        private void DoDownload()
        {
            downloadPresenter = new DownloadPresenter(downloadModel, downloadView);
            downloadPresenter.SetFirmwareVersionModel(firmwareVersionModel);
            downloadPresenter.ProcessFinished += new EventHandler(downloadPresenter_ProcessFinished);
            view.ShowControl(downloadView);
            downloadPresenter.StartProcess();

        }


        void startControl_CreateIPSWClicked(object sender, CreateIPSWClickedEventArgs e)
        {
            var fileName = e.FileName;


            if (!String.IsNullOrEmpty(fileName))
            {
                firmwareVersionModel.CheckVersion(fileName);

                if (firmwareVersionModel.Version == FirmwareVersions.Unknown)
                    return;
                firmwareVersionModel.ExistingFirmwarePath = fileName;
            }

            DoDownload();


        }

        void downloadPresenter_ProcessFinished(object sender, EventArgs e)
        {
            if (downloadPresenter.Result == DownloadPresenter.ProcessResult.Cancelled)
            {
                ShowStartPage();
                return;
            }

            view.ShowControl(patchControl);
            var presetner = new PatchPresenter(patchControl, patchModel);
            presetner.Finished += new EventHandler(patchPresetner_Finished);

            presetner.StartPatch();

        }

        void patchPresetner_Finished(object sender, EventArgs e)
        {
            firmwareVersionDetector.SaveState(firmwareVersionModel.Version);

            view.ShowControl(dfuControl);
            var presenter = new DFUPresenter(dfuModel, dfuControl);
            presenter.ProcessFinished += new EventHandler(dfuPresenter_ProcessFinished);
            presenter.StartProcess();
        }

        void dfuPresenter_ProcessFinished(object sender, EventArgs e)
        {
            dfuSuccessControl.SetFileName(Path.GetFileName(firmwareVersionModel.PatchedFirmwarePath));
            view.ShowControl(dfuSuccessControl);
        }
    }
}

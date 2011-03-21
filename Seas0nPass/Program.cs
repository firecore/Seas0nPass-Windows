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
using System.Windows.Forms;
using Seas0nPass.Presenters;
using System.Diagnostics;
using System.IO;

namespace Seas0nPass
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitDocumentsHome();

            LogUtil.Init();
            LogUtil.LogEvent("Application start");
            LogUtil.LogEvent(string.Format("{0} {1}", Application.ProductName, Application.ProductVersion));

            Utils.CleanUp();
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new MainForm();
            var mainPresenter = new MainPresenter(form);
            mainPresenter.Init();
            Application.Run(form);
        }

        private static void InitDocumentsHome()
        {
            if (!Directory.Exists(Utils.DOCUMENTS_HOME))
                Directory.CreateDirectory(Utils.DOCUMENTS_HOME);

            var downloadsFolder = Path.Combine(Utils.DOCUMENTS_HOME, "Downloads");
            if (!Directory.Exists(downloadsFolder))
                Directory.CreateDirectory(downloadsFolder);

        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Utils.CleanUp();
        }
    }
}

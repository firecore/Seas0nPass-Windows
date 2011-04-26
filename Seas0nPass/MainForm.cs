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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Seas0nPass.Interfaces;

namespace Seas0nPass
{
    public partial class MainForm : Form, IMainView
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public void ShowDownloadFailedMessage()
        {
            MessageBox.Show(
                text: "Unable to download firmware. Please check your internet connection or firewall settings and try again.",
                caption: "Seas0nPass",
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Exclamation
            );
        }

        public void ShowTetherMessage(string fwName)
        {
            MessageBox.Show(string.Format("The {0} firmware is untethered and does not require this process!", fwName),
                "Untethered Jailbreak");
        }

        public void ShowControl(IView control)
        {
            Action action = delegate
            {
                contentPanel.Controls.Clear();
                var castedControl = (Control)control;
                contentPanel.Controls.Add(castedControl);
                castedControl.Dock = DockStyle.Fill;
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
    }
}

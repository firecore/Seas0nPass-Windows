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

        public void ShowTetherMessage()
        {
            MessageBox.Show("The AppleTV2,1_4.2.1_8C154 firmware is untethered and does not require this process!", "Untethered Jailbreak");
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

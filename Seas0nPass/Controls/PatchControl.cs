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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Seas0nPass.Interfaces;

namespace Seas0nPass
{
    public partial class PatchControl : UserControl, IPatchView
    {
        public event EventHandler ActionButtonClicked;

        public PatchControl()
        {
            InitializeComponent();
        }

        public void SetMessageText(string text)
        {
            Action action = () => this.label.Text = text; ;
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        public void UpdateProgress(int value)
        {
            Action action = delegate { this.progressBar.Value = value; this.progressBar.Refresh(); };

            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private void actionButton_Click(object sender, EventArgs e)
        {
            if (ActionButtonClicked != null)
                ActionButtonClicked(sender, e);
        }      

        public void SetActionButtonText(string text)
        {
            Action action = () => actionButton.Text = text;

            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
    }
}

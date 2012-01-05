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
using System.Security.Principal;
using Seas0nPass.CustomEventArgs;
using Seas0nPass.Models;
using System.Threading;

namespace Seas0nPass.Controls
{
    public partial class StartControl : UserControl, IStartView
    {
        private readonly Image tetherEnabledImage;
        private readonly Image tetherDisabledImage;
        private readonly Image tetherNotRequiredImage;

        public StartControl()
        {
            InitializeComponent();
            tetherNotRequiredImage = tetheredPictureBox.BackgroundImage;
            tetherDisabledImage = tetheredPictureBox.InitialImage;
            tetherEnabledImage = tetheredPictureBox.ErrorImage;

        }

        public event EventHandler<CreateIPSWFirmwareClickedEventArgs> CreateIPSW_fwVersion_Clicked;
        public event EventHandler<CreateIPSWClickedEventArgs> CreateIPSWClicked;
        public event EventHandler TetherClicked;

        public void SetTetherEnabledState(bool isEnabled)
        {
            tetheredPictureBox.Enabled = tetherLabel.Enabled = isEnabled;
            tetheredPictureBox.BackgroundImage = isEnabled ? tetherEnabledImage : tetherDisabledImage;
        }

        private void ipswPictureBox_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "IPSW files (*.ipsw)|*.ipsw";
                if (dialog.ShowDialog() == DialogResult.OK)
                    fileName = dialog.FileName;
                else
                    return;
            }

            if (CreateIPSWClicked != null)
                CreateIPSWClicked(sender, new CreateIPSWClickedEventArgs(fileName));
        }

        private void tetheredPoctureBox_Click(object sender, EventArgs e)
        {
            if (TetherClicked != null)
                TetherClicked(sender, e);
        }

        public void DisableTether()
        {
            tetheredPictureBox.BackgroundImage = tetherDisabledImage;
            tetheredPictureBox.Enabled = false;
            tetherLabel.Enabled = false;
        }

        public void EnableTether()
        {
            tetheredPictureBox.BackgroundImage = tetherEnabledImage;
            tetheredPictureBox.Enabled = true;
            tetherLabel.Enabled = true;
        }

        public void SetTetherNotRequiredState()
        {
            tetheredPictureBox.BackgroundImage = tetherNotRequiredImage;
            tetherLabel.Enabled = true;
        }

        public void InitFirmwaresList(FirmwareVersion[] firmwares)
        {
            ipswContextMenuStrip.Items.Clear();
            var items = new ToolStripMenuItem[firmwares.Length];
            for (int i = 0; i < firmwares.Length; i++)
            {
                items[i] = new ToolStripMenuItem(firmwares[i].Name, null, ipswPictureBoxFirmware_Click)
                {
                    Tag = firmwares[i]
                };
            }
            ipswContextMenuStrip.Items.AddRange(items);
        }

        private void ipswPictureBoxFirmware_Click(object sender, EventArgs e)
        {
            var fwVersion = (FirmwareVersion)((ToolStripMenuItem)sender).Tag;
            if (CreateIPSW_fwVersion_Clicked != null)
                CreateIPSW_fwVersion_Clicked(this, new CreateIPSWFirmwareClickedEventArgs(fwVersion));
        }

        private void SetPressedState(Control control, bool isPressed)
        {
            control.BackColor = isPressed ? Color.FromArgb(255, Color.DarkGray) : Color.White;
        }

        private void ipswPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            SetPressedState((Control)sender, true);
        }

        private void ipswPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            SetPressedState((Control)sender, false);
        }

        private void tetheredPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            SetPressedState((Control)sender, true);
        }

        private void tetheredPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            SetPressedState((Control)sender, false);
        }

        public SynchronizationContext SyncContext
        {
            get { return new WindowsFormsSynchronizationContext(); }
        }


        public void ResetState()
        {
            SetPressedState(tetheredPictureBox, false);
            SetPressedState(ipswPictureBox, false);          

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seas0nPass.Interfaces;
using System.Diagnostics;
using System.Windows.Automation;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;

namespace Seas0nPass.Models
{
    public class ITunesAutomationModel : IITunesAutomationModel
    {
        public SynchronizationContext SyncContext { get; set; }
        public IFirmwareVersionModel FirmwareVersionModel { get; set; }
        public IITunesInfoProvider ITunesInfoProvider { get; set; }

        private readonly AutomationElement desktop = AutomationElement.RootElement;
        public void Run()
        {
            var iTunesProcess = GetITunesProcess();

            AutomationElement mainForm = null;
            do
            {
                mainForm = desktop.FindFirst(TreeScope.Children,
                    new AndCondition(
                        new PropertyCondition(AutomationElement.NameProperty, "iTunes"),
                        new OrCondition(
                            new PropertyCondition(AutomationElement.ClassNameProperty, "iTunes"),
                            new PropertyCondition(AutomationElement.ClassNameProperty, "ITWindow"))
                    ));
                LogControlFound(mainForm, "MainForm");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            while (mainForm == null);


            bool dialogOk = false;
            do
            {
                AutomationElement panel;
                while (true)
                {
                    panel = GetAppleTVPanel(mainForm);
                    LogControlFound(panel, "aTV panel");
                    if (panel != null) break;
                    Thread.Sleep(100);
                }

                Thread.Sleep(100);

                CloseModalDialogs(mainForm);

                LogUtil.LogEvent("Set iTunes to foreground");

                var mainFormHandle = new IntPtr(mainForm.Current.NativeWindowHandle);
                SetForegroundWindow(mainFormHandle);

                LogUtil.LogEvent(string.Format("Apple TV panel found! Handle: {0}", panel.Current.NativeWindowHandle.ToString("X")));

                var restoreButton = panel.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "Button"));

                LogControlFound(restoreButton, "Firmware restore button");

                LogUtil.LogEvent(string.Format("Restore button found! Handle: {0}", restoreButton.Current.NativeWindowHandle.ToString("X")));

                SetForegroundWindow(mainFormHandle);

                LogUtil.LogEvent("Set iTunes to foreground");

                LogUtil.LogEvent("Pressing Shift");

                keybd_event((byte)Keys.ShiftKey, (byte)MapVirtualKey((uint)Keys.ShiftKey, 0), 0, UIntPtr.Zero);

                Action _delegate = () =>
                {
                    LogUtil.LogEvent("Before Restore Click");
                    ClickButton(restoreButton);
                    LogUtil.LogEvent("After Restore Click");
                };

                _delegate.BeginInvoke(null, null);

                Thread.Sleep(500);

                LogUtil.LogEvent("Releasing Shift");
                keybd_event((byte)Keys.ShiftKey, (byte)MapVirtualKey((uint)Keys.ShiftKey, 0), 2, UIntPtr.Zero);

                bool confirmOk = false;
                do
                {
                    dialogOk = WorkInDialog(mainForm);
                    confirmOk = WorkInConfirmDialog(mainForm);
                } while (dialogOk && !confirmOk);

            } while (!dialogOk);
        }

        private void CloseModalDialogs(AutomationElement mainForm)
        {
            LogUtil.LogEvent("Closing Modal Dialogs");

            var dialogForm = GetOpenedCustomDialog(mainForm);

            LogControlFound(dialogForm, "Found modal dialog");
            
            while (dialogForm != null)
            {
                var handle = new IntPtr(dialogForm.Current.NativeWindowHandle);
                SetForegroundWindow(handle);
                SyncContext.Send(x => SendKeys.Send("{ESC}"), null);

                Thread.Sleep(100);

                dialogForm = GetOpenedCustomDialog(mainForm);
            }
        }

        private AutomationElement GetOpenedCustomDialog(AutomationElement mainForm)
        {
            return mainForm.FindFirst(TreeScope.Subtree,
                new PropertyCondition(AutomationElement.ClassNameProperty, "iTunesCustomModalDialog"));
        }

        private void ClickButton(AutomationElement button)
        {
            var handle = new IntPtr(button.Current.NativeWindowHandle);
            var pointToClick = new IntPtr(ConvertDWord(5, 5));

            long lngResult = SendMessage(handle, WM_LBUTTONDOWN, IntPtr.Zero, pointToClick);
            lngResult = SendMessage(handle, WM_LBUTTONUP, IntPtr.Zero, pointToClick);

        }

        public static long ConvertDWord(int low, int high)
        {
            long lDWord = (long)(low + (65536 * high));
            return lDWord;
        }

        //Left Button - Mouse Down
        public const int WM_LBUTTONDOWN = 0x0201;
        //Left Button - Mouse Up
        public const int WM_LBUTTONUP = 0x0202;

        [DllImport("user32.dll",CharSet=CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        private AutomationElement FindCustomDialog(AutomationElement mainForm)
        {
            return mainForm.FindFirst(TreeScope.Subtree,
                new PropertyCondition(AutomationElement.ClassNameProperty, "iTunesCustomModalDialog"));
        }

        private bool WorkInConfirmDialog(AutomationElement mainForm)
        {
            LogUtil.LogEvent("Work with confirmation dialog");

            AutomationElement dialog = mainForm.FindFirst(TreeScope.Subtree,
                new PropertyCondition(AutomationElement.ClassNameProperty, "iTunesCustomModalDialog"));

            int attemptNum = 0;
            while (attemptNum < 10 && dialog == null)
            {
                dialog = mainForm.FindFirst(TreeScope.Subtree,
                    new PropertyCondition(AutomationElement.ClassNameProperty, "iTunesCustomModalDialog"));
                Thread.Sleep(100);
                attemptNum++;
            }
            if (attemptNum >= 10)
                return false;

            LogControlFound(dialog, "Confirmation dialog");

            SetForegroundWindow((IntPtr)dialog.Current.NativeWindowHandle);
            SyncContext.Send(x => SendKeys.Send("~"), null); //Enter
            return true;
        }

        private bool WorkInDialog(AutomationElement mainForm)
        {
            LogUtil.LogEvent("Open File Dialog processing");

            var dialogForm = mainForm.FindFirst(TreeScope.Subtree,
                new PropertyCondition(AutomationElement.ClassNameProperty, "#32770"));

            LogControlFound(dialogForm, "Open File Dialog");

            int attemptNumber = 0;
            while (attemptNumber < 10 && (dialogForm == null || dialogForm.Current.ClassName == "iTunesCustomModalDialog"))
            {
                if (dialogForm != null)
                {
                    LogUtil.LogEvent("Not Open File Dialog Opened");
                    var handle = new IntPtr(dialogForm.Current.NativeWindowHandle);
                    
                    LogUtil.LogEvent("Closing it");
                    SetForegroundWindow(handle);
                    SyncContext.Send(x => SendKeys.Send("{ESC}"), null);
                }

                LogUtil.LogEvent("Looking for another dialog");

                Thread.Sleep(100);

                dialogForm = mainForm.FindFirst(TreeScope.Subtree,
                    new PropertyCondition(AutomationElement.ClassNameProperty, "#32770"));

                LogControlFound(dialogForm, "Open File Dialog");
                
                attemptNumber++;
            }
            if (attemptNumber >= 10)
                return false;

            ValuePattern valuePattern = null;
            do
            {
                var pathControls = dialogForm.FindAll(TreeScope.Subtree,
                        new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ComboBox)
                    );

                foreach (AutomationElement pathControl in pathControls)
                {
                    object pattern;
                    if (pathControl.TryGetCurrentPattern(ValuePattern.Pattern, out pattern))
                    {
                        valuePattern = (ValuePattern)pattern;

                        if (valuePattern != null)
                        {
                            LogControlFound(pathControl, "Path TextBox");
                            LogUtil.LogEvent("Path text box found");
                            break;
                        }
                    }
                }

                Thread.Sleep(100);

            } while (valuePattern == null);

            LogUtil.LogEvent("Path value set");
            valuePattern.SetValue(FirmwareVersionModel.PatchedFirmwarePath);

            LogUtil.LogEvent("Set dialog as ForegroundWindow and send enter to it");
            SetForegroundWindow((IntPtr)dialogForm.Current.NativeWindowHandle);
            SyncContext.Send(x => SendKeys.Send("~"), null); // Enter
            return true;
        }

        [DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        static extern int SetForegroundWindow(IntPtr Hwnd);

        private AutomationElement GetAppleTVPanel(AutomationElement root)
        {
            var panels = (IEnumerable<AutomationElement>)root.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.ClassNameProperty, "Static")).Cast<AutomationElement>();
            return panels.Where(panel => isAppleTVPanel(panel)).FirstOrDefault();
        }

        private bool isAppleTVPanel(AutomationElement panel)
        {
            return panel.FindFirst(TreeScope.Children,
                new PropertyCondition(AutomationElement.NameProperty, "Apple TV")) != null;
        }

        private Process GetITunesProcess()
        {
            var processes = Process.GetProcessesByName("ITunes");
            if (processes.Length > 0)
                return processes[0];
            var iTunesPath = GetITunesPath();

            LogUtil.LogEvent(string.Format("Starting iTunes at path {0}", iTunesPath));

            return Process.Start(iTunesPath);
        }

        private void LogControlFound(AutomationElement control, string controlName)
        {
            var controlHandle = control == null ? "(null)" :
                "Handle: " + control.Current.NativeWindowHandle.ToString("X");

            LogUtil.LogEvent(string.Format("Control named \"{0}\" found! Handle: {1}", controlName, controlHandle));

        }

        private string GetITunesPath()
        {
            return ITunesInfoProvider.GetITunesExePath();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace Seas0nPass.Utils
{
    public static class WinProcessUtil
    {
        private static List<Process> startedProcesses = new List<Process>();

        public static Process StartNewProcess()
        {
            var p = new Process();
            startedProcesses.Add(p);
            return p;
        }

        public static Process StartNewProcess(ProcessStartInfo processStartInfo)
        {
            var p = Process.Start(processStartInfo);
            startedProcesses.Add(p);
            return p;
        }

        public static void KillAllProcesses()
        {
            foreach (var p in startedProcesses.Where(x => !x.HasExited))
            {
                try
                {
                    p.Kill();
                    p.WaitForExit(1000); // wait for exit no longer than 1 second
                }
                catch (Win32Exception)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
        }
    }
}

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
using System.IO;
using System.Diagnostics;
using Seas0nPass.Properties;
using System.Security.Cryptography;

namespace Seas0nPass
{
    public static class Utils
    {
        public static readonly string DOWNLOADED_FILE_PATH = "firmware.ipsw";
        public static readonly string KERNEL_CACHE_FILE_NAME = @"kernelcache.release.k66";
        public static readonly string OUTPUT_FOLDER_NAME = @"OUTPUT";
        public static readonly string FIRMWARE_FOLDER_NAME = "Firmware";
        public static readonly string IBSS_FILE_NAME = "iBSS.k66ap.RELEASE.dfu";
        public static readonly string DFU_FOLDER_NAME = "dfu";
        public static readonly string OUTPUT_FIRMWARE_NAME = "output.ipsw";        
        public static readonly string WORKING_FOLDER = Path.Combine(Path.GetTempPath(), "Seas0nPass");
        public static readonly string BIN_DIRECTORY = Path.Combine(WORKING_FOLDER, "BIN");
        public static readonly string PATCHES_DIRECTORY = Path.Combine(WORKING_FOLDER, "PATCHES");
        public static readonly string COMMANDS_FILE_NAME = "commands.fc";

        public static readonly string DOCUMENTS_HOME = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Seas0nPass");

        public static string ComputeMD5(string filePath)
        {
            var sb = new StringBuilder();
            byte[] hash;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                var md5 = new MD5CryptoServiceProvider();
                hash = md5.ComputeHash(fs);
            }
            foreach (byte hex in hash)
                sb.Append(hex.ToString("x2"));
            return sb.ToString();
        }

        public static List<string> GetAllFileInFodler(string folder)
        {
            return new List<string>(
                from original in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
                select original.Remove(0, (folder + Path.DirectorySeparatorChar).Length)
            );
        }

        public static void RecreateDirectory(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                try
                {
                    Directory.Delete(dirPath, true);
                }
                catch (IOException ex)
                {
                    LogUtil.LogException(ex);
                    if (Directory.EnumerateFiles(dirPath).Any())
                        throw;
                    else
                        return; // Can't delete dir, but it is empty => skip it
                }
            }
            Directory.CreateDirectory(dirPath);
        }

        public static void CopyDirectory(string Src, string Dst)
        {
            String[] Files;

            if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
                Dst += Path.DirectorySeparatorChar;
            if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
            Files = Directory.GetFileSystemEntries(Src);
            foreach (string Element in Files)
            {
                // Sub directories

                if (Directory.Exists(Element))
                    CopyDirectory(Element, Dst + Path.GetFileName(Element));
                // Files in directory

                else
                    File.Copy(Element, Dst + Path.GetFileName(Element), true);
            }
        }

        private static IEnumerable<ProcessStartInfo> ParseResource(string resourceName)
        {
            var lines = Seas0nPass.ScriptResource.ResourceManager.GetString(resourceName);

            foreach (var line in lines.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var exePath = line.Substring(0, line.IndexOf(' ')).Trim();
                var args = line.Substring(exePath.Length).Trim();
                yield return new ProcessStartInfo()
                {
                    FileName = exePath,
                    Arguments = args,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                };
            }

            yield break;
        }

        public static void ExecuteResource(string resourceName)
        {
            foreach (var processStartInfo in ParseResource(resourceName))
            {
                LogUtil.LogEvent(string.Format("Running process: {0}, args: {1} ", processStartInfo.FileName, processStartInfo.Arguments));
                var p = Process.Start(processStartInfo);
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    var errorString = string.Format("Process: {0}, args: {1} exited with non-zero code", p.StartInfo.FileName, p.StartInfo.Arguments);
                    LogUtil.LogEvent(errorString);
                    throw new InvalidOperationException(errorString);
                }
            }
        }

        public static void OpenExplorerWindow(string fileToSelect)
        {
            LogUtil.LogEvent("opening explorer window");
            if (!File.Exists(fileToSelect))
            {
                return;
            }

            // combine the arguments together
            // it doesn't matter if there is a space after ','
            string argument =  string.Format("/select, \"{0}\"",fileToSelect);

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        public static void CleanUp()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            LogUtil.LogEvent("clean up");
            try 
            {
                foreach (var name in GetProcessesToKill())
                {
                    foreach (var process in Process.GetProcessesByName(name))
                    {
                        LogUtil.LogEvent(string.Format("{0} process kill", name));
                        process.Kill();
                        process.WaitForExit(1000); // wait for exit no longer than 1 second
                    }
                }
                if (Directory.Exists(WORKING_FOLDER))
                    Directory.Delete(WORKING_FOLDER, true);
            } 
            catch (Exception ex) 
            {
                LogUtil.LogException(ex);
                // Do nothing
            }
        }

        public static IEnumerable<string> GetProcessesToKill()
        {
            foreach(var line in ScriptResource.ProgramsToKill.Split(new [] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                yield return line;
            }
        }
    }
}

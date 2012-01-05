using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Seas0nPass.Utils
{
    public static class SafeFile
    {       

        public static void Copy(string from, string to)
        {
            BaseIOUtils.RepeatActionWithDelay(() => File.Copy(from, to));
        }

        public static void Copy(string from, string to, bool _override)
        {
            BaseIOUtils.RepeatActionWithDelay(() => File.Copy(from, to, _override));
        }



        public static FileStream Create(string name)
        {
            return BaseIOUtils.RepeatActionWithDelay<FileStream>(() => File.Create(name));
        }

        public static FileStream OpenRead(string name)
        {
            return BaseIOUtils.RepeatActionWithDelay<FileStream>(() => File.OpenRead(name));
        }

        public static bool Exists(string name)
        {
            return BaseIOUtils.RepeatActionWithDelay<bool>(() => File.Exists(name));
        }

        public static void Delete(string name)
        {
            BaseIOUtils.RepeatActionWithDelay(() => File.Delete(name));
        }

        public static void Move(string from, string to)
        {
            BaseIOUtils.RepeatActionWithDelay(() => File.Move(from, to));
        }
        
    
    }
}

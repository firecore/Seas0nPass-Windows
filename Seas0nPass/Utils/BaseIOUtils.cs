using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Seas0nPass.Utils
{
    public static class BaseIOUtils
    {
        public static void RepeatActionWithDelay(Action action)
        {
            try
            {
                action();
            }
            catch (IOException)
            {
                Thread.Sleep(500);
                action();
            }
        }

        public static T RepeatActionWithDelay<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (IOException)
            {
                Thread.Sleep(500);
                return action();
            }

        }
    }
}

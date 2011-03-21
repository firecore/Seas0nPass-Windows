using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seas0nPass.CustomEventArgs
{
    public class CreateIPSWClickedEventArgs : EventArgs
    {
        public string FileName { get; private set; }

        public CreateIPSWClickedEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}

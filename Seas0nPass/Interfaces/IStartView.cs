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
using Seas0nPass.CustomEventArgs;
using Seas0nPass.Models;
using System.Threading;

namespace Seas0nPass.Interfaces
{
    public interface IStartView : IView
    {
        event EventHandler<CreateIPSWClickedEventArgs> CreateIPSWClicked;
        event EventHandler<CreateIPSWFirmwareClickedEventArgs> CreateIPSW_fwVersion_Clicked; 
        event EventHandler TetherClicked;

        SynchronizationContext SyncContext { get; }

        void DisableTether();
        void EnableTether();
        void SetTetherNotRequiredState();
        void InitFirmwaresList(FirmwareVersion[] firmwares);

        void ResetState();
    }
}

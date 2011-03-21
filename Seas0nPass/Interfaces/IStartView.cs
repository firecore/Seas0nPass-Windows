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

namespace Seas0nPass.Interfaces
{
    public interface IStartView : IView
    {
        event EventHandler<CreateIPSWClickedEventArgs> CreateIPSWClicked;
        event EventHandler CreateIPSW_421_8C154_Clicked;
        event EventHandler CreateIPSW_43_8F191m_Clicked;
        event EventHandler TetherClicked;

        void DisableTether();
        void EnableTether();
        void SetTetherNotRequiredState();

    }
}

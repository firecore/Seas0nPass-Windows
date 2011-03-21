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
using Seas0nPass.Interfaces;

namespace Seas0nPass.Presenters
{
    public class TetherPresenter
    {
        private ITetherModel model;
        private ITetherView view;
        public TetherPresenter(ITetherModel model, ITetherView view)
        {
            this.model = model;
            this.view = view;
            model.CurrentMessageChanged += new EventHandler(model_CurrentMessageChanged);
            model.ProgressChanged += new EventHandler(model_ProgressChanged);
            model.ProcessFinished += new EventHandler(model_ProcessFinished);
        }

        void model_ProgressChanged(object sender, EventArgs e)
        {
            view.UpdateProgress(model.ProgressPercentage);
        }

        void model_CurrentMessageChanged(object sender, EventArgs e)
        {
            view.SetMessageText(model.CurrentMessage);
        }

        void model_ProcessFinished(object sender, EventArgs e)
        {
            if (ProcessFinished != null)
                ProcessFinished(sender, e);

            view.Clear();
        }

        public event EventHandler ProcessFinished;

        public void StartProcess()
        {
            model.StartProcess();
        }
    }
}

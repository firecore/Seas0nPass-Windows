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
    public class DFUPresenter
    {

        private IDFUModel model;
        private IDFUView view;

        public event EventHandler ProcessFinished;

        public DFUPresenter(IDFUModel model, IDFUView view )
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

        void model_ProcessFinished(object sender, EventArgs e)
        {
            if (ProcessFinished != null)
                ProcessFinished(sender, e);

            view.Clear();
        }

        void model_CurrentMessageChanged(object sender, EventArgs e)
        {
            if (model.CurrentMessage == "Found device in DFU mode...")            
                view.HintVisibility = false;            
            view.SetMessageText(model.CurrentMessage);
        }

        public void StartProcess()
        {
            view.HintVisibility = true;            
            model.StartProcess();
        }
    }
}

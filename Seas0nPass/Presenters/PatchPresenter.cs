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
    public class PatchPresenter
    {
        private IPatchView view;
        private IPatchModel model;
        public PatchPresenter(IPatchView view, IPatchModel model)
        {
            this.view = view;
            this.model = model;            
            model.ProgressUpdated += new EventHandler(model_ProgressUpdated);
            model.CurrentMessageChanged += new EventHandler(model_CurrentMessageChanged);
            model.Finished += new EventHandler(model_Finished);

        }

        void model_CurrentMessageChanged(object sender, EventArgs e)
        {
            view.SetMessageText(model.CurrentMessage);
        }

        void model_Finished(object sender, EventArgs e)
        {
            if (Finished != null)
                Finished(sender, e);
        }

        void model_ProgressUpdated(object sender, EventArgs e)
        {
            view.UpdateProgress(model.CurrentProgress);
        }

        public event EventHandler Finished;

        public void StartPatch()
        {            
            view.SetActionButtonText("Cancel");
            model.StartProcess();
        }
    }
}

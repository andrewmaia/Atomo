using System;
using System.Web.UI;
using System.ComponentModel;
using System.Collections.Generic;

namespace Atomo.Web
{
    public abstract class WebControl : System.Web.UI.WebControls.WebControl 
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Create();
        }

        public void Create()
        {
            ControlBuilder controlBuilder = new ControlBuilder();
            controlBuilder.Build(this.Page, GetCssLinked(), GetCssResource(), GetCss(), GetJsLinked(), GetJsResource(), GetJs());
        }

        #region Virtual Methods

        public virtual List<string> GetCssLinked()
        {
            return null;
        }
        public virtual string GetCss()
        {
            return string.Empty;
        }

        public virtual List<string> GetJsLinked()
        {
            return null;
        }
        public virtual string GetJs()
        {
            return string.Empty;
        }

        public virtual List<string> GetCssResource()
        {
            return null;
        }
        public virtual List<string> GetJsResource()
        {
            return null;
        }

        #endregion

    }
}

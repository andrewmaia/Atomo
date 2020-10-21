using System;
using System.Web.UI;
using System.Collections.Generic;

namespace Atomo.Web
{
    public abstract  class UserControl: System.Web.UI.UserControl  
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Create();
        }

        public void Create()
        {
            ControlBuilder controlBuilder = new ControlBuilder();
            controlBuilder.Build (this.Page,GetCssLinked(),null,GetCss(),GetJsLinked(),null,GetJs());
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
        #endregion
    }
}

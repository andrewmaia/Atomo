using System;
using System.Web.UI;
using System.Collections.Generic;

namespace Atomo.Web
{
    public abstract  class MasterPage: System.Web.UI.MasterPage
    {
        private JsContainer jsContainer;
        private CssContainer cssContainer;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Master != null)
            {
                System.Web.UI.MasterPage master = Master;

                while (master.Master != null)
                    master = master.Master;

                if (!(master is Atomo.Web.MasterPage))
                    throw new Exception("A Master Page tem que herdar de Atomo.Web.MasterPage!");

                this.jsContainer = ((Atomo.Web.MasterPage)master).JsContainer;
                this.cssContainer = ((Atomo.Web.MasterPage)master).CssContainer;
            }
            else
            {
                this.jsContainer = new JsContainer();
                this.cssContainer = new CssContainer();
            }

            this.Create();

        }

        #region Properties
        public JsContainer JsContainer
        {
            get { return jsContainer; }
        }

        public CssContainer CssContainer
        {
            get { return cssContainer; }
        }
        #endregion


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            PageBuilder pageBuilder = new PageBuilder(this.Controls, cssContainer, this.GetAllJs());
            pageBuilder.Build();
        }

        public void Create()
        {
            this.RegisterLinks();
            this.SetCssID();
            this.SetCssIsLocal();
            this.SetJsIsTop();
            this.SetJsIsLocal();

            jsContainer.Create(this.GetJsCommon(), null, this.GetJs());
            cssContainer.Create(this.GetCssCommon(), null, this.GetCss());
        }

        #region Virtual Methods


        public virtual void RegisterLinks()
        {}

        public virtual List<string> GetCssCommon()
        {
            return null; 
        }

        public virtual string GetCss()
        {
            return string.Empty;
        }
        public virtual void SetCssID()
        {
        }
        public virtual void SetCssIsLocal()
        {
        }

        public virtual List<string> GetJsCommon()
        {
            return null;
        }

        public virtual string GetJs()
        {
            return string.Empty;
        }
        public virtual JsContainer GetAllJs()
        {
            return this.jsContainer;
        }
        public virtual void SetJsIsTop()
        {
        }
        public virtual void SetJsIsLocal()
        {
        }

        #endregion

    }
}


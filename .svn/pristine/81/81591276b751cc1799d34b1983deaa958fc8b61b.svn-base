using System;
using System.Web.UI;
using System.ComponentModel;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace Atomo.Web.Controls
{
    [ToolboxData("<{0}:Add runat=server></{0}:Add>")]
    public class Add : Atomo.Web.WebControl
    {
        #region Properties

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string ImageUrl
        {
            get
            {
                String s = (String)ViewState["ImageUrl"];
                return ((s == null) ? string.Empty : s);
            }

            set
            {
                ViewState["ImageUrl"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Description
        {
            get
            {
                String s = (String)ViewState["Description"];
                return ((s == null) ? string.Empty : s);
            }

            set
            {
                ViewState["Description"] = value;
            }
        }

        #endregion

        protected override void CreateChildControls()
        {
 
            HtmlGenericControl div = new HtmlGenericControl("div");
            HtmlImage image = new HtmlImage();
            image.Src = this.ImageUrl; 
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.InnerText = this.Description; 
            div.Controls.Add(image);
            div.Controls.Add(span);
            this.Controls.Add(div); 
        }

        #region Css

        public override string GetCss()
        {
            return "h1 {color: #FF0000;}";
        }

        public override List<string> GetCssLinked()
        {
            List<string> l = new List<string>();
            //"webControl" tem que estar registado em register links do site
            l.Add("webControl");
            return l;
        }

        public override List<string> GetCssResource()
        {
            List<string> l = new List<string>();
            l.Add(Page.ClientScript.GetWebResourceUrl(GetType(), "Atomo.Web.Controls.css.Stylesheet1.css"));
            return l;


        }

        #endregion

        #region Js

        public override string GetJs()
        {
            return "alert('webControl');";
        }

        public override List<string> GetJsLinked()
        {
            List<string> l = new List<string>();
            //"webControl" tem que estar registado em register links do site
            l.Add("webControl");
            return l;
        }

        public override List<string> GetJsResource()
        {
            List<string> l = new List<string>();
            //Necessário ter um resource embutido na DLL
            l.Add(Page.ClientScript.GetWebResourceUrl(GetType(), "Atomo.Web.Controls.css.JScript1.js"));
            return l;
        }

        #endregion

    }
}


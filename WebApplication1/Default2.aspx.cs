using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Atomo.Web;
using System.Collections.Generic;

namespace WebApplication1
{
    public partial class Default2 : Atomo.Web.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void RegisterLinks()
        {
            this.CssContainer.CssLinkedInfo.Add("site", new CssInfo("/Css/CssTeste.css", "all"));
        }


        public override System.Collections.Generic.List<string> GetCssLinked()
        {
            List<string> l = new List<string>();
            l.Add("site");
            return l;
        }
    }
}
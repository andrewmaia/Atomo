using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Atomo.Web;

namespace WebApplication1
{
    public partial class MasterAvo : Atomo.Web.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void RegisterLinks()
        {
            this.CssContainer.CssLinkedInfo.Add("site", new CssInfo("/Css/CssTeste.css", "all"));
        }
    }
}
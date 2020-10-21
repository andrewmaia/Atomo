using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class WebUserControl1 : Atomo.Web.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override List<string> GetCssLinked()
        {
            List<string> l = new List<string>();
            l.Add("site");
            return l;
        }
    }
}
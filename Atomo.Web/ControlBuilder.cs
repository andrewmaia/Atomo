using System;
using System.Collections.Generic ;


namespace Atomo.Web
{
    public class ControlBuilder
    {
        public void Build(System.Web.UI.Page page, List<string> cssLinked, List<string> cssResource, string css, List<string> jsLinked, List<string> jsResource, string js)
        {
            CssContainer cssContainer;
            JsContainer jsContainer;

            if (page.Master != null)
            {
                System.Web.UI.MasterPage master = page.Master;

                while (master.Master != null)
                    master = master.Master;

                if (!(master is Atomo.Web.MasterPage))
                    throw new Exception("A Master Page tem que herdar de Atomo.Web.MasterPage!");

                jsContainer = ((Atomo.Web.MasterPage)master).JsContainer;
                cssContainer = ((Atomo.Web.MasterPage)master).CssContainer;

            }
            else
            {
                Page page_ = (Page)page;
                cssContainer = page_.CssContainer;
                jsContainer = page_.JsContainer;
            }

            cssContainer.AddToCssLinked(cssLinked);
            cssContainer.AddToCss(css);
            cssContainer.AddToCssResource(cssResource); 
            jsContainer.AddToJsLinked(jsLinked);
            jsContainer.AddToJs(js);
            jsContainer.AddToJsResource(jsResource); 
            
        }

    }
}


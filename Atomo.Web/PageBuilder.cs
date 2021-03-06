﻿using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Atomo.Web
{
    public class PageBuilder
    {
        private ControlCollection controlCollection;
        private CssContainer cssContainer;
        private JsContainer  jsContainer;

        public PageBuilder(ControlCollection controlCollection, CssContainer cssContainer, JsContainer jsContainer)
        {
            this.controlCollection = controlCollection;
            this.cssContainer = cssContainer;
            this.jsContainer = jsContainer;
        }

        public void Build()
        {
            foreach (Control control in controlCollection)
            {
                if (control.GetType() == typeof(HtmlHead))
                {
                    BuildCss(control);
                    if (jsContainer.JsIsTop)
                        BuildJs(control); 
                }
                else if (control.GetType() == typeof(LiteralControl))
                {
                    LiteralControl literalControl = (LiteralControl)control;
                    string text = literalControl.Text.ToUpper();

                    if (!jsContainer.JsIsTop)
                        if (text.Contains("</BODY"))
                        {
                            Control container = new Control();
                            BuildJs(container);
                            int i = text.IndexOf("</BODY");
                            literalControl.Text = string.Concat(literalControl.Text.Substring(0, i), GetLiteralValue(container) , literalControl.Text.Substring(i));

                        }

                    if (cssContainer.CssId != string.Empty)
                        if (text.Contains("<BODY"))
                        {
                            int i = text.IndexOf("<BODY") + 5;
                            literalControl.Text = string.Concat(literalControl.Text.Substring(0, i), @" id=""", cssContainer.CssId, @""" ", literalControl.Text.Substring(i));
                        }
                }
            }
        }

        private void BuildCss(Control control)
        {

            string link = string.Empty;

            foreach (Control child in control.Controls)
            {
                if (child.GetType() == typeof(LiteralControl))
                    link = ((LiteralControl)child).Text;
                else if (child.GetType() == typeof(HtmlLink))
                    link = ((HtmlLink)child).Href;

                if (link != string.Empty)
                    cssContainer.ValidateLink(link);
            }

            if (cssContainer.CssCommonLinked.Length > 0) 
                control.Controls.Add(new LiteralControl("<!-- Common CSS-->"));
            foreach (CssInfo cssInfo in cssContainer.CssCommonLinked)
                control.Controls.Add(CreateCssLink(cssContainer.CssIsLocal ? cssInfo.LocPath : cssInfo.AkaPath, cssInfo.Media));

            if (cssContainer.CssPageLinked.Length > 0) 
                control.Controls.Add(new LiteralControl("<!-- Page CSS-->"));
            foreach (CssInfo cssInfo in cssContainer.CssPageLinked)
                control.Controls.Add(CreateCssLink(cssContainer.CssIsLocal ? cssInfo.LocPath : cssInfo.AkaPath, cssInfo.Media));

            if (cssContainer.CssPage != string.Empty)
                control.Controls.Add(CreateCss(cssContainer.CssPage));

            if (cssContainer.CssModuleLinked.Length > 0) 
                control.Controls.Add(new LiteralControl("<!-- Module CSS-->"));
            foreach (CssInfo cssInfo in cssContainer.CssModuleLinked)
                control.Controls.Add(CreateCssLink(cssContainer.CssIsLocal ? cssInfo.LocPath : cssInfo.AkaPath, cssInfo.Media));

            if (cssContainer.CssModule !=string.Empty)
                control.Controls.Add(CreateCss(cssContainer.CssModule));

            foreach (string path in cssContainer.CssModuleResource)
                control.Controls.Add(CreateCssLink(path, "all"));

        }

        private void BuildJs(Control control)
        {
            if (jsContainer.JsCommonLinked.Length > 0) 
                control.Controls.Add(new LiteralControl("<!-- Common JS-->"));
            foreach (JsInfo jsInfo in jsContainer.JsCommonLinked)
                control.Controls.Add(CreateJsLink(jsContainer.JsIsLocal ? jsInfo.LocPath : jsInfo.AkaPath));

            if (jsContainer.JsPageLinked.Length > 0) 
                control.Controls.Add(new LiteralControl("<!-- Page JS-->"));
            foreach (JsInfo jsInfo in jsContainer.JsPageLinked)
                control.Controls.Add(CreateJsLink(jsContainer.JsIsLocal ? jsInfo.LocPath : jsInfo.AkaPath));

            if (jsContainer.JsPage!= string.Empty)
                control.Controls.Add(CreateJs(jsContainer.JsPage));

            if (jsContainer.JsModuleLinked.Length > 0) 
                control.Controls.Add(new LiteralControl("<!-- Module JS-->"));
            foreach (JsInfo jsInfo in jsContainer.JsModuleLinked)
                control.Controls.Add(CreateJsLink(jsContainer.JsIsLocal ? jsInfo.LocPath : jsInfo.AkaPath));

            if (jsContainer.JsModule != string.Empty)
                control.Controls.Add(CreateJs(jsContainer.JsModule));

            foreach (string path in jsContainer.JsModuleResource)
                control.Controls.Add(CreateJsLink(path));
        }

        private  string GetLiteralValue(Control container)
        {
            string value = string.Empty;

            foreach (Control control in container.Controls)
                if (control.GetType() == typeof(LiteralControl))
                    value += ((LiteralControl)control).Text;

            return value;
        }

        private LiteralControl CreateCssLink(string path, string media)
        {
            return new LiteralControl(string.Concat(@"<link href=""", path, @""" media=""",media, @""" type=""text/css"" ", @"rel=""stylesheet"" ", "/>"));
        }

        private LiteralControl CreateCss(string css)
        {
            return new LiteralControl(string.Concat(@"<style type=""text/css"" media=""all"">", css, "</style>"));
        }

        private LiteralControl CreateJsLink(string path)
        {
            return new LiteralControl(string.Concat(@"<script src=""", path, @""" type=""text/javascript"">", "</script>"));
        }

        private LiteralControl CreateJs(string js)
        {
            return new LiteralControl(string.Concat(@"<script type=""text/javascript"">", js, "</script>"));
        }

    }
}

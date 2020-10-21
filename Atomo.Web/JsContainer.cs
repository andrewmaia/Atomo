using System;
using System.Collections.Generic;

namespace Atomo.Web
{
    public class JsContainer
    {
        private string jsContainer;
        private string jsModule;
        private List<JsInfo> jsPageLinked;
        private List<JsInfo> jsModuleLinked;
        private List<JsInfo> jsCommonLinked;
        private List<string> jsModuleResource;
        private Dictionary<string, JsInfo> jsLinkedInfo;
        private List<string> jsLinkedUsed;
        private bool jsIsLocal;
        private bool jsIsTop;

        public JsContainer() : this(false, false)
        {
        }

        public JsContainer(bool jsIsLocal, bool jsIsTop)
        {
            this.jsCommonLinked = new List<JsInfo>();
            this.jsContainer = string.Empty;
            this.jsPageLinked = new List<JsInfo>();
            this.jsModule= string.Empty;
            this.jsModuleLinked = new List<JsInfo>();
            this.jsModuleResource = new List<string>();
            this.jsLinkedInfo = new Dictionary<string, JsInfo>();
            this.jsLinkedUsed = new List<string>();
            this.jsIsLocal = jsIsLocal;
            this.jsIsTop = jsIsTop;
        }

        #region Properties

        public bool JsIsTop
        {
            set { jsIsTop = value; }
            get { return jsIsTop; }
        }

        public bool JsIsLocal
        {
            set { jsIsLocal = value; }
            get { return jsIsLocal ; }
        }

        public Dictionary<string, JsInfo> JsLinkedInfo
        {
            get { return jsLinkedInfo; }
        }

        public JsInfo[] JsCommonLinked
        {
            get
            {
                return (JsInfo[]) jsCommonLinked.ToArray().Clone() ;
            }
        }


        public JsInfo[] JsPageLinked
        {
            get
            {
                return (JsInfo[])jsPageLinked.ToArray().Clone(); 
            }
        }

        public JsInfo[] JsModuleLinked
        {
            get
            {
                return (JsInfo[])jsModuleLinked.ToArray().Clone(); 
            }
        }

        public string JsPage
        {
            get
            {
                return jsContainer;
            }
        }

        public string JsModule
        {
            get
            {
                return jsModule;
            }
        }

        public string [] JsModuleResource
        {
            get
            {
                return (string[])jsModuleResource.ToArray().Clone();
            }
        }

        #endregion


        #region Methods

        public void Create(List<string> jsKeysCommon, List<string> jsKeysPage, string jsPage)
        {
            this.SetJsCommon(jsKeysCommon);
            this.SetJsPage(jsKeysPage, jsPage);
        }

        private void SetJsCommon(List<string> jsKeys)
        {
            foreach (JsInfo jsInfo in this.ManageJsLinked(jsKeys))
                this.jsCommonLinked.Add(jsInfo); 
        }

        private void SetJsPage(List<string> jsKeys, string js)
        {
            foreach (JsInfo jsInfo in this.ManageJsLinked(jsKeys))
                this.jsPageLinked.Add(jsInfo);

            if (js!=string.Empty)
                this.jsContainer += string.Concat(js, " "); 
        }

        private List<JsInfo> ManageJsLinked(List<string> keys)
        {
            List<JsInfo> jsInfoLista = new List<JsInfo>();

            if (keys != null)
            {
                foreach (string key in keys)
                {
                    if (!jsLinkedInfo.ContainsKey(key))
                        throw new Exception("There is no js key");

                    if (!jsLinkedUsed.Contains(key))
                    {
                        jsLinkedUsed.Add(key);
                        jsInfoLista.Add(jsLinkedInfo[key]);
                    }
                }
            }

            return jsInfoLista;
        }

        public void AddToJsLinked(List<string> keys)
        {
            foreach (JsInfo jsInfo in this.ManageJsLinked(keys))
                this.jsModuleLinked.Add(jsInfo);
        }

        public void AddToJs(string js)
        {
            if (js.Trim() != string.Empty) 
                this.jsModule += string.Concat(js, " "); 
        }

        public void AddToJsResource(List<string> references)
        {
            if (references != null)
            {
                foreach (string reference in references)
                    if (!jsModuleResource.Contains(reference))
                        jsModuleResource.Add(reference);
            }
        }


        #endregion
    }
}

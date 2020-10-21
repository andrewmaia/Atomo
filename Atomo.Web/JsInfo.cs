using System;

namespace Atomo.Web
{
    public class JsInfo
    {
        private string locPath;
        private string akaPath;

        public JsInfo(string akaPath): this (akaPath, akaPath)
        {
        }

        public JsInfo(string locPath, string akaPath)
        {
            this.locPath = locPath;
            this.akaPath = akaPath;
        }

        public string LocPath
        {
           get {return locPath;}
        }

        public string AkaPath
        {
           get {return akaPath;}
        }
    }
}

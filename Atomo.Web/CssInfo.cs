﻿using System;
using System.Web.UI.HtmlControls;

namespace Atomo.Web
{
    public class CssInfo
    {
        private string locPath;
        private string akaPath;
        private string media;

        public CssInfo(string akaPath, string media): this (akaPath, akaPath, media)
        {
        }

        public CssInfo(string locPath, string akaPath,string media)
        {
            this.locPath = locPath;
            this.akaPath = akaPath;
            this.media = media;
        }

        public string LocPath
        {
           get {return locPath;}
        }

        public string AkaPath
        {
           get {return akaPath;}
        }

        public string Media
        {
            get { return (media==string.Empty?"all":media); }
        }



    }
}

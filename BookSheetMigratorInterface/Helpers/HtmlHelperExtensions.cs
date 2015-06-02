﻿using System.Configuration;
using System.Text;
using System.Web.Mvc;

namespace BookSheetMigratorInterface.Helpers
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// An Html helper for Require.js
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="main">Location of the main js file.</param>
        /// <returns></returns>
        public static MvcHtmlString RequireJs(this HtmlHelper helper, string main)
        {
            var require = new StringBuilder();
            string jsLocation = ConfigurationManager.AppSettings["JsLocation"];
            require.AppendLine(string.Format("<script src=\"{0}require.js\" data-main=\"{0}{1}\"></script>", jsLocation, main));

            return new MvcHtmlString(require.ToString());
        }
    }
}
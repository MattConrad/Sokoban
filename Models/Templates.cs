using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.IO;
using System.Web.Caching;

namespace Sokoban.Models
{
    /// <summary>
    /// This is a static class used to create and cache Sokoban level templates.
    /// All the interesting stuff happens in Game.
    /// </summary>
    public static class Templates
    {
        public static List<string[]> GetSokoTemplates()
        {
            //Cache is perhaps a bit overkill here, but perhaps someday we'll be grabbing levels over 
            // the intertubes or something like that.
            object oLvls = HttpRuntime.Cache.Get("SokoTemplates");

            if (oLvls == null) 
            {
                oLvls = CreateSokoTemplates();
                HttpRuntime.Cache.Add("SokoTemplates", oLvls, null, 
                    DateTime.Now.AddDays(7), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }

            return (List<string[]>)oLvls;
        }

        private static List<string[]> CreateSokoTemplates()
        {
            Assembly assemb = Assembly.GetExecutingAssembly();
            System.IO.TextReader streamTemplates = new System.IO.StreamReader(assemb.GetManifestResourceStream("Sokoban.Models.SokobanLevelTemplates.txt"));

            List<string[]> lstSokoTemplates = new List<string[]>();
            try
            {
                List<string> lstTemplateLines = new List<string>();
                while (streamTemplates.Peek() != -1)
                {
                    string sLine = streamTemplates.ReadLine();

                    if (sLine.Trim().StartsWith("--")) { }      // comment, not part of a level, do nothing
                    else if (sLine.Trim().Length < 1)           // we just finished a Template, add it and start a new one.
                    {
                        lstSokoTemplates.Add(GetRectangularizedLines(lstTemplateLines));
                        lstTemplateLines = new List<string>();
                    }
                    else                                        // standard row in Template layout.
                        lstTemplateLines.Add(sLine);
                }
            }
            finally
            {
                streamTemplates.Close();
            }

            return lstSokoTemplates;
        }

        /// <summary>
        /// We need each level to be in a rectangle, but the source file is jagged.
        /// This method pads out a level with spaces to create a regular rectangle.
        /// </summary>
        private static string[] GetRectangularizedLines(List<string> lstJagged)
        {
            List<string> lstRect = new List<string>();

            int iMaxLen = 0;
            foreach (string s in lstJagged)
                iMaxLen = (s.Length > iMaxLen) ? s.Length : iMaxLen;

            foreach (string s in lstJagged)
                lstRect.Add(s.PadRight(iMaxLen));

            return lstRect.ToArray();
        }
    }

}
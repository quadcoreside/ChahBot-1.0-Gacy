using ChahBot_1_0_Gacy.SearchClass;
using DataBase;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChahBot_1_0_Gacy.Controller
{
    class ScannerDork
    {
        private enum enInjectableType : byte
        {
            UnArchived,
            Archived
        }
        private struct stSearchFirtParamAPI
        {
            public string Google;
            public string Bing;
            public string Yahoo;
            public string Aol;
            public string Yandex;
            public string Ask;
            public string Wow;
            public string WebCrawler;
            public string MyWebSearch;
            public string Sapo;
        }
        private struct stSearchEndParamAPI
        {
            public string Google;
            public string Bing;
            public string Yahoo;
            public string Aol;
            public string Yandex;
            public string Ask;
            public string Wow;
            public string WebCrawler;
            public string MyWebSearch;
            public string Sapo;
        }
        private struct stScannerEngines
        {
            public SearchEngineUrls Google;
            public SearchEngineUrls Bing;
            public SearchEngineUrls Yahoo;
            public SearchEngineUrls Aol;
            public SearchEngineUrls Yandex;
            public SearchEngineUrls Ask;
            public SearchEngineUrls Wow;
            public SearchEngineUrls WebCrawler;
            public SearchEngineUrls MyWebSearch;
            public SearchEngineUrls Sapo;
            public bool Complete()
            {
                return Google.Complete() & Bing.Complete() & Yahoo.Complete() & Aol.Complete() & Yandex.Complete() & Ask.Complete() & Wow.Complete() & WebCrawler.Complete() & MyWebSearch.Complete() & Sapo.Complete();
            }
        }
        public struct stScannerConf
        {
            public dynamic Order;
            public int TimeOut;
            public int Delay;
            public string Url;
            public enHTTPMethod Method;
            public string Search;
            public List<string> DorkList;
            public string DorkIndex;
            public int DorkDelay;
            public string Domain;
            public string RegExp;
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
            public int TotalEngines;
            public int Threads;
            public int Retry;

            public TimeSpan TimeSpan;
        }
        private struct stScanner
        {
            public int TimeOut;

            public int Delay;

            public Hashtable Urls;

            public enHTTPMethod Method;

            public byte Threads;

            public bool TrashMsAccess;

            public bool TrashUnknown;
        }
        private struct stSearchSummary
        {
            public int Added;
            public int InQueue;
            public int InQueueAsSimilar;
            public int Unsupported;
            public int Found;

            public int Loaded()
            {
                return checked(Added + InQueue + InQueueAsSimilar + Unsupported);
            }
        }
        private struct stAnalizerUrl
        {
            public string Url;

            public Types Method;
        }
        private class ThreadScanner
        {
            private int __ID;

            private DateTime __Started;

            [CompilerGenerated]
            private bool _TrashMsAccess;

            [CompilerGenerated]
            private bool _TrashUnknown;

            [CompilerGenerated]
            private Thread _Thread;

            [CompilerGenerated]
            private DateTime _Started;

            [CompilerGenerated]
            private object _Tag;

            [CompilerGenerated]
            private HTTP _HTTP;

            [CompilerGenerated]
            private string _OriginalUrl;

            [CompilerGenerated]
            private string _URL;

            [CompilerGenerated]
            private int _Delay;

            public bool TrashMsAccess
            {
                get
                {
                    return _TrashMsAccess;
                }
                set
                {
                    _TrashMsAccess = value;
                }
            }

            public bool TrashUnknown
            {
                get
                {
                    return _TrashUnknown;
                }
                set
                {
                    _TrashUnknown = value;
                }
            }

            public Thread Thread
            {
                get
                {
                    return _Thread;
                }
                set
                {
                    _Thread = value;
                }
            }

            public DateTime Started
            {
                get
                {
                    return _Started;
                }
                set
                {
                    _Started = value;
                }
            }

            public object Tag
            {
                get
                {
                    return _Tag;
                }
                set
                {
                    _Tag = RuntimeHelpers.GetObjectValue(value);
                }
            }

            public int ID
            {
                get
                {
                    return __ID;
                }
            }

            public HTTP HTTP
            {
                get
                {
                    return _HTTP;
                }
                set
                {
                    _HTTP = value;
                }
            }

            public string OriginalUrl
            {
                get
                {
                    return _OriginalUrl;
                }
                set
                {
                    _OriginalUrl = value;
                }
            }

            public string URL
            {
                get
                {
                    return _URL;
                }
                set
                {
                    _URL = value;
                }
            }

            public int Delay
            {
                get
                {
                    return _Delay;
                }
                set
                {
                    _Delay = value;
                }
            }

            public ThreadScanner(int id)
            {
                __ID = id;
                __Started = DateAndTime.Now;
            }
        }
        private class ThreadAnalizer
        {
            private int __ID;

            private Thread __Thread;

            private string __URL;

            private Types __Method;

            private int __Threads;

            public Thread Thread
            {
                get
                {
                    return __Thread;
                }
                set
                {
                    __Thread = value;
                }
            }

            public int ID
            {
                get
                {
                    return __ID;
                }
                set
                {
                    __ID = value;
                }
            }

            public Types Method
            {
                get
                {
                    return __Method;
                }
                set
                {
                    __Method = value;
                }
            }

            public string URL
            {
                get
                {
                    return __URL;
                }
                set
                {
                    __URL = value;
                }
            }

            public int Threads
            {
                get
                {
                    return __Threads;
                }
                set
                {
                    __Threads = value;
                }
            }

            public ThreadAnalizer(int id)
            {
                __ID = id;
            }
        }
       
        private stScannerEngines __ScannerEngines;
        private stScannerConf __ScannerConf_;
        private stSearchSummary __SearchSummary;
        private stSearchEndParamAPI __stSearchEndParamAPI__;
        private stSearchFirtParamAPI __SearchFirtParamAPI__;
        private List<string> __SerachQueueSimilarURL;
        private static bool __RunningWorker;
        public Hashtable __URLS__ = new Hashtable();

        [AccessedThroughProperty("bckWorkerSearch")]
	    private BackgroundWorker _bckWorkerSearch_;

        public ScannerDork()
        {
            this.bckWorkerSearch = new BackgroundWorker();
            this.bckWorkerSearch.WorkerReportsProgress = true;
            this.bckWorkerSearch.WorkerSupportsCancellation = true;
        }

        private void bckWorkerGetLinks_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                __RunningWorker = true;
                using (HTTP hTTP = new HTTP(__ScannerConf_.TimeOut, false))
                {
                    HTTP arg_4A_0 = hTTP;
                    string arg_4A_1 = __ScannerConf_.Url;
                    enHTTPMethod arg_4A_2 = __ScannerConf_.Method;
                    string text = "";
                    object arg_4A_4 = null;
                    NetworkCredential arg_4A_5 = null;
                    bool arg_4A_6 = false;
                    string text2 = "";
                    string hTML = arg_4A_0.GetHTML(arg_4A_1, arg_4A_2, ref text, arg_4A_4, arg_4A_5, arg_4A_6, ref text2);

                    if (!string.IsNullOrEmpty(hTML))
                    {
                        new List<ListViewItem>();
                        using (RegExp regExp = new RegExp())
                        {
                            try
                            {
                                IEnumerator enumerator = regExp.GetLinks(__ScannerConf_.Url, hTML, __ScannerConf_.RegExp).Values.GetEnumerator();
                                while (enumerator.MoveNext())
                                {
                                    Link link = (Link)enumerator.Current;
                                    if (string.IsNullOrEmpty(__ScannerConf_.Domain) || !Globals.G_Utilities.IsUrlValid(link.Url) || Strings.InStr(link.Url, __ScannerConf_.Domain, CompareMethod.Binary) != 0)
                                    {
                                        AddURL(link.Url, new string[0]);
                                    }
                                }
                            }
                            finally
                            {
                                IEnumerator enumerator = null;
                                if (enumerator is IDisposable)
                                {
                                    (enumerator as IDisposable).Dispose();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception expr_11F)
            {
                ProjectData.SetProjectError(expr_11F);
                Exception ex = expr_11F;
                MessageBox.Show(ex.Message, "Please report this bug!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                ProjectData.ClearProjectError();
            }
            finally
            {
            }
        }

        private void AddURL(string sURL, params string[] sValues)
        {
            ProjectData.ClearProjectError();
            this.__SearchSummary.InQueue++;

            string domain = Globals.G_Utilities.GetDomain(sURL);

            try
            {
                if (!__URLS__.ContainsKey(domain))
            {
                    __URLS__.Add(domain, Convert.ToString(sURL));
                    this.__SearchSummary.Added++;
                }
                else
                {
                    __SearchSummary.InQueueAsSimilar++;
                }
            }
            catch
            {
            }
        }

        private void IniScannerObjects()
	    {
		    __SearchSummary = default(stSearchSummary);
		    __SearchFirtParamAPI__ = default(stSearchFirtParamAPI);
	    }

        public void StartWorker(dynamic order, int TimeOut, int numSearchDelay, List<string> DorkList, int nbrThread, int numHttpRetry = 2, bool google = false, bool bing = false, bool yahoo = false, bool aol = false, bool yandex = false, bool ask = false, bool wow = false, bool webCrawler = false, bool myWebSearch = false, bool sapo = false)
        {
            /*if (__RunningWorker)
            {
                return;
            }*/
            checked
            {
                IniScannerObjects();
                stScannerConf stScannerConf = default(stScannerConf);
                stScannerConf.Order = order;
                Globals.UpDateStatus(order, "Scanner DORKS", "Scanner thread, loading please wait..");
                stScannerConf.TimeOut = Convert.ToInt32(TimeOut);
                stScannerConf.Delay = Convert.ToInt32(numSearchDelay);
                stScannerConf.Url = "";
                stScannerConf.Method = enHTTPMethod.GET;
                stScannerConf.Threads = Convert.ToInt32(nbrThread);
                stScannerConf.Retry = Convert.ToInt32(numHttpRetry);
                stScannerConf.DorkList = DorkList;
                
                if (stScannerConf.DorkList.Count == 0)
                {
                    Globals.UpDateStatus(__ScannerConf_.Order, "Scanner DORKS", "Scanner thread, Dork list is empty.");
                    Interaction.Beep();
                    return;
                }
                stScannerConf.Google = google;
                stScannerConf.Bing = bing;
                stScannerConf.Yahoo = yahoo;
                stScannerConf.Aol = aol;
                stScannerConf.Yandex = yandex;
                stScannerConf.Ask = ask;
                stScannerConf.Wow = wow;
                stScannerConf.WebCrawler = webCrawler;
                stScannerConf.MyWebSearch = myWebSearch;
                stScannerConf.Sapo = sapo;
                if (stScannerConf.Google)
                {
                    stScannerConf.TotalEngines++;
                }
                if (stScannerConf.Bing)
                {
                    stScannerConf.TotalEngines++;
                }
                if (stScannerConf.Yahoo)
                {
                    stScannerConf.TotalEngines++;
                }
                if (stScannerConf.Aol)
                {
                    stScannerConf.TotalEngines++;
                }
                if (stScannerConf.Yandex)
                {
                    stScannerConf.TotalEngines = 1;
                }
                if (stScannerConf.Ask)
                {
                    stScannerConf.TotalEngines++;
                }
                if (stScannerConf.Wow)
                {
                    stScannerConf.TotalEngines++;
                }
                if (stScannerConf.WebCrawler)
                {
                    stScannerConf.TotalEngines++;
                }
                if (stScannerConf.MyWebSearch)
                {
                    stScannerConf.TotalEngines++;
                }
                if (stScannerConf.Sapo)
                {
                    stScannerConf.TotalEngines++;
                }
                if (stScannerConf.TotalEngines == 0)
                {
                    global::Globals.UpDateStatus(__ScannerConf_.Order, "Scanner DORKS", "Scanner thread, select one or more search engine.");
                    Interaction.Beep();
                    return;
                }

                //Commencer
                this.bckWorkerSearch.RunWorkerAsync(stScannerConf);

                goto IL_FD3;
                Interaction.Beep();
                return;
            IL_FD3:
                Console.WriteLine("SetGUI OK ==> 159");
            }
        }

        internal virtual BackgroundWorker bckWorkerSearch
	    {
		    get
		    {
			    return this._bckWorkerSearch_;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    RunWorkerCompletedEventHandler value2 = new RunWorkerCompletedEventHandler(this.bckWorkerSearch_RunWorkerCompleted);
			    ProgressChangedEventHandler value3 = new ProgressChangedEventHandler(this.bckWorkerSearch_ProgressChanged);
			    DoWorkEventHandler value4 = new DoWorkEventHandler(this.bckWorkerSearch_DoWork);
			    if (this._bckWorkerSearch_ != null)
			    {
				    this._bckWorkerSearch_.RunWorkerCompleted -= value2;
				    this._bckWorkerSearch_.ProgressChanged -= value3;
				    this._bckWorkerSearch_.DoWork -= value4;
			    }
			    this._bckWorkerSearch_ = value;
			    if (this._bckWorkerSearch_ != null)
			    {
				    this._bckWorkerSearch_.RunWorkerCompleted += value2;
				    this._bckWorkerSearch_.ProgressChanged += value3;
				    this._bckWorkerSearch_.DoWork += value4;
			    }
		    }
	    }

        private void bckWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            checked
            {
                /*try
                {*/
                    __RunningWorker = true;
                    this.__ScannerConf_ = (stScannerConf)e.Argument;

                    //On les initialise tous
                    __ScannerEngines = default(stScannerEngines);
                    __ScannerEngines.Google = new SearchEngineUrls(SearchEngineUrls.Host.Google, __ScannerConf_.Retry, __ScannerConf_.TimeOut);
                    ScannerAddEvents(__ScannerEngines.Google);
                    __ScannerEngines.Bing = new SearchEngineUrls(SearchEngineUrls.Host.Bing, __ScannerConf_.Retry, __ScannerConf_.TimeOut);
                    ScannerAddEvents(__ScannerEngines.Bing);
                    __ScannerEngines.Yahoo = new SearchEngineUrls(SearchEngineUrls.Host.Yahoo, __ScannerConf_.Retry, __ScannerConf_.TimeOut);
                    ScannerAddEvents(__ScannerEngines.Yahoo);
                    __ScannerEngines.Aol = new SearchEngineUrls(SearchEngineUrls.Host.Aol, __ScannerConf_.Retry, __ScannerConf_.TimeOut);
                    ScannerAddEvents(__ScannerEngines.Aol);
                    __ScannerEngines.Yandex = new SearchEngineUrls(SearchEngineUrls.Host.Yandex, __ScannerConf_.Retry, __ScannerConf_.TimeOut);
                    ScannerAddEvents(__ScannerEngines.Yandex);
                    __ScannerEngines.Ask = new SearchEngineUrls(SearchEngineUrls.Host.Ask, __ScannerConf_.Retry, __ScannerConf_.TimeOut);
                    ScannerAddEvents(__ScannerEngines.Ask);
                    __ScannerEngines.Wow = new SearchEngineUrls(SearchEngineUrls.Host.Wow, __ScannerConf_.Retry, __ScannerConf_.TimeOut);
                    ScannerAddEvents(__ScannerEngines.Wow);
                    __ScannerEngines.WebCrawler = new SearchEngineUrls(SearchEngineUrls.Host.WebCrawler, __ScannerConf_.Retry, __ScannerConf_.TimeOut);
                    ScannerAddEvents(__ScannerEngines.WebCrawler);
                    __ScannerEngines.MyWebSearch = new SearchEngineUrls(SearchEngineUrls.Host.MyWebSearch, __ScannerConf_.Retry, __ScannerConf_.TimeOut);
                    ScannerAddEvents(__ScannerEngines.MyWebSearch);
                    __ScannerEngines.Sapo = new SearchEngineUrls(SearchEngineUrls.Host.Sapo, __ScannerConf_.Retry, __ScannerConf_.TimeOut);
                    ScannerAddEvents(__ScannerEngines.Sapo);

                    //on demmare ceux sélectionner
                    if (__ScannerConf_.Google)
                    {
                        __ScannerEngines.Google.StartScanning(ScannerBuildDorks(SearchEngineUrls.Host.Google, __ScannerConf_.DorkList), __ScannerConf_.Threads, __ScannerConf_.Delay);
                    }
                    if (__ScannerConf_.Bing)
                    {
                        __ScannerEngines.Bing.StartScanning(ScannerBuildDorks(SearchEngineUrls.Host.Bing, __ScannerConf_.DorkList), __ScannerConf_.Threads, __ScannerConf_.Delay);
                    }
                    if (__ScannerConf_.Yahoo)
                    {
                        __ScannerEngines.Yahoo.StartScanning(ScannerBuildDorks(SearchEngineUrls.Host.Yahoo, __ScannerConf_.DorkList), __ScannerConf_.Threads, __ScannerConf_.Delay);
                    }
                    if (__ScannerConf_.Aol)
                    {
                        __ScannerEngines.Aol.StartScanning(ScannerBuildDorks(SearchEngineUrls.Host.Aol, __ScannerConf_.DorkList), __ScannerConf_.Threads, __ScannerConf_.Delay);
                    }
                    if (__ScannerConf_.Yandex)
                    {
                        __ScannerEngines.Yandex.StartScanning(ScannerBuildDorks(SearchEngineUrls.Host.Yandex, __ScannerConf_.DorkList), __ScannerConf_.Threads, __ScannerConf_.Delay);
                    }
                    if (__ScannerConf_.Ask)
                    {
                        __ScannerEngines.Ask.StartScanning(ScannerBuildDorks(SearchEngineUrls.Host.Ask, __ScannerConf_.DorkList), __ScannerConf_.Threads, __ScannerConf_.Delay);
                    }
                    if (__ScannerConf_.Wow)
                    {
                        __ScannerEngines.Wow.StartScanning(ScannerBuildDorks(SearchEngineUrls.Host.Wow, __ScannerConf_.DorkList), __ScannerConf_.Threads, __ScannerConf_.Delay);
                    }
                    if (__ScannerConf_.WebCrawler)
                    {
                        __ScannerEngines.WebCrawler.StartScanning(ScannerBuildDorks(SearchEngineUrls.Host.WebCrawler, __ScannerConf_.DorkList), __ScannerConf_.Threads, __ScannerConf_.Delay);
                    }
                    if (__ScannerConf_.MyWebSearch)
                    {
                        __ScannerEngines.MyWebSearch.StartScanning(ScannerBuildDorks(SearchEngineUrls.Host.MyWebSearch, __ScannerConf_.DorkList), __ScannerConf_.Threads, __ScannerConf_.Delay);
                    }
                    if (__ScannerConf_.Sapo)
                    {
                        __ScannerEngines.Sapo.StartScanning(ScannerBuildDorks(SearchEngineUrls.Host.Sapo, __ScannerConf_.DorkList), __ScannerConf_.Threads, __ScannerConf_.Delay);
                    }

                    while (true)
                    {
                        if (__ScannerEngines.Complete())
                        {
                            __RunningWorker = false;
                            Globals.GUI.sendUrls(__ScannerConf_.Order, __URLS__);
                            break;
                        }
                        ScannerProgressStatusBar();
                        int num = 0;
                        do
                        {
                            Application.DoEvents();
                            Thread.Sleep(100);
                            num++;
                        }
                        while (num <= 9);
                    }
               /* }
                catch (Exception expr_6A8)
                {
                    ProjectData.SetProjectError(expr_6A8);
                    ProjectData.ClearProjectError();
                }
                finally
                {
                }*/
            }
        }
        private void bckWorkerSearch_ProgressChanged(object sender, ProgressChangedEventArgs e)
	    {
            Globals.GUI.setProgress(__ScannerConf_.Order, e.ProgressPercentage);
            Console.WriteLine(e.ProgressPercentage);
	    }
        private void bckWorkerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           __RunningWorker = false;
            if (__SearchSummary.Added == 0 && !e.Cancelled)
            {
                global::Globals.UpDateStatus(__ScannerConf_.Order, "Scanner DORKS", "Scanner thread done, search not found or possible IP temp. banned by server.");
            }
            else
            {
                global::Globals.UpDateStatus(__ScannerConf_.Order, "Scanner DORKS", "Scanner thread done, URL's Added:" + Conversions.ToString(__SearchSummary.Added));
                Globals.GUI.sendUrls(__ScannerConf_.Order, __URLS__);
            }
        }

        private List<string> ScannerBuildDorks(SearchEngineUrls.Host h, List<string> dorks)
        {
            string[] array = new string[2];
            switch (h)
            {
                case SearchEngineUrls.Host.Google:
                    array[0] = __SearchFirtParamAPI__.Google;
                    array[1] = " " + Convert.ToString(__SearchFirtParamAPI__.Google);
                    break;
                case SearchEngineUrls.Host.Bing:
                    array[0] = __SearchFirtParamAPI__.Bing;
                    array[1] = " " + __SearchFirtParamAPI__.Bing;
                    break;
                case SearchEngineUrls.Host.Yahoo:
                    array[0] = __SearchFirtParamAPI__.Yahoo;
                    array[1] = " " + __SearchFirtParamAPI__.Yahoo;
                    break;
                case SearchEngineUrls.Host.Aol:
                    array[0] = __SearchFirtParamAPI__.Aol;
                    array[1] = " " + __SearchFirtParamAPI__.Aol;
                    break;
                case SearchEngineUrls.Host.Yandex:
                    array[0] = __SearchFirtParamAPI__.Yandex;
                    array[1] = " " + __SearchFirtParamAPI__.Yandex;
                    break;
                case SearchEngineUrls.Host.Ask:
                    array[0] = __SearchFirtParamAPI__.Ask;
                    array[1] = " " + __SearchFirtParamAPI__.Ask;
                    break;
                case SearchEngineUrls.Host.Wow:
                    array[0] = __SearchFirtParamAPI__.Wow;
                    array[1] = " " + __SearchFirtParamAPI__.Wow;
                    break;
                case SearchEngineUrls.Host.WebCrawler:
                    array[0] = __SearchFirtParamAPI__.WebCrawler;
                    array[1] = " " + __SearchFirtParamAPI__.WebCrawler;
                    break;
                case SearchEngineUrls.Host.MyWebSearch:
                    array[0] = __SearchFirtParamAPI__.MyWebSearch;
                    array[1] = " " + __SearchFirtParamAPI__.MyWebSearch;
                    break;
                case SearchEngineUrls.Host.Sapo:
                    array[0] = __SearchFirtParamAPI__.Sapo;
                    array[1] = " " + __SearchFirtParamAPI__.Sapo;
                    break;
            }
            List<string> list = new List<string>();
            try
            {
                List<string>.Enumerator enumerator = dorks.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    string text = array[0] + current + array[1];
                    list.Add(text.Trim());
                }
            }
            finally
            {
                List<string>.Enumerator enumerator = new List<string>.Enumerator();
                ((IDisposable)enumerator).Dispose();
            }
            return list;
        }

        private void ScannerAddEvents(SearchEngineUrls o)
        {
            o.Scanner_Progress += new SearchEngineUrls.Scanner_ProgressEventHandler(Scanner_Progress);
            o.Scanner_LoadedLink += new SearchEngineUrls.Scanner_LoadedLinkEventHandler(Scanner_LoadedLink);
            o.Scanner_Done += new SearchEngineUrls.Scanner_DoneEventHandler(Scanner_Done);
        }
        public void Scanner_Progress(int percentage, SearchEngineUrls.Host h)
        {
            Globals.GUI.setProgress(__ScannerConf_.Order, percentage);
            Console.WriteLine(percentage + "% => " + h.ToString());
        }
        public void Scanner_LoadedLink(List<string> urls, SearchEngineUrls.Host h)
        {
           /* try
            {*/
                List<string>.Enumerator enumerator = urls.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    AddURL(current, new string[0]);
                }
            /*}
            finally
            {
                List<string>.Enumerator enumerator = new List<string>.Enumerator();
                ((IDisposable)enumerator).Dispose();
            }*/
        }
        public void Scanner_Done(SearchEngineUrls.Host h)
        {
            Console.WriteLine("Scanner_Done " + h.ToString());
        }

        private void ScannerProgressStatusBar()
        {
            checked
            {
                int num = __ScannerEngines.Google.Progress + __ScannerEngines.Bing.Progress + __ScannerEngines.Yahoo.Progress + __ScannerEngines.Aol.Progress + __ScannerEngines.Yandex.Progress + __ScannerEngines.Ask.Progress + __ScannerEngines.Wow.Progress + __ScannerEngines.WebCrawler.Progress + __ScannerEngines.MyWebSearch.Progress + __ScannerEngines.Sapo.Progress;
                if (num == 0)
                {
                    num = 1;
                }
                Globals.GUI.setProgress(__ScannerConf_.Order, num);
                global::Globals.UpDateStatus(__ScannerConf_.Order, "Scanner DORKS", string.Format("[ " + ((int)Math.Round(Math.Round((double)num / (double)__ScannerConf_.TotalEngines))).ToString().PadLeft(2, '0') + "% ] Scanner thread, loading please wait .. URL's Added " + Conversions.ToString(__SearchSummary.Added), new object[0]));
                ScannerSummary();
            }
        }
        private void ScannerSummary()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("URL's Loaded");
            if (__ScannerEngines.Google.LinksLoaded > 0)
            {
                stringBuilder.AppendLine("[" + __ScannerEngines.Google.Progress.ToString().PadLeft(2, '0') + "%] Google: " + Strings.FormatNumber(__ScannerEngines.Google.LinksLoaded, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__ScannerEngines.Bing.LinksLoaded > 0)
            {
                stringBuilder.AppendLine("[" + __ScannerEngines.Bing.Progress.ToString().PadLeft(2, '0') + "%] Bing: " + Strings.FormatNumber(__ScannerEngines.Bing.LinksLoaded, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__ScannerEngines.Yahoo.LinksLoaded > 0)
            {
                stringBuilder.AppendLine("[" + __ScannerEngines.Yahoo.Progress.ToString().PadLeft(2, '0') + "%] Yahoo: " + Strings.FormatNumber(__ScannerEngines.Yahoo.LinksLoaded, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__ScannerEngines.Aol.LinksLoaded > 0)
            {
                stringBuilder.AppendLine("[" + __ScannerEngines.Aol.Progress.ToString().PadLeft(2, '0') + "%] Aol: " + Strings.FormatNumber(__ScannerEngines.Aol.LinksLoaded, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__ScannerEngines.Yandex.LinksLoaded > 0)
            {
                stringBuilder.AppendLine("[" + __ScannerEngines.Yandex.Progress.ToString().PadLeft(2, '0') + "%] Yandex: " + Strings.FormatNumber(__ScannerEngines.Yandex.LinksLoaded, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__ScannerEngines.Ask.LinksLoaded > 0)
            {
                stringBuilder.AppendLine("[" + __ScannerEngines.Ask.Progress.ToString().PadLeft(2, '0') + "%] Ask: " + Strings.FormatNumber(__ScannerEngines.Ask.LinksLoaded, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__ScannerEngines.Wow.LinksLoaded > 0)
            {
                stringBuilder.AppendLine("[" + __ScannerEngines.Wow.Progress.ToString().PadLeft(2, '0') + "%] Wow: " + Strings.FormatNumber(__ScannerEngines.Wow.LinksLoaded, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__ScannerEngines.WebCrawler.LinksLoaded > 0)
            {
                stringBuilder.AppendLine("[" + __ScannerEngines.WebCrawler.Progress.ToString().PadLeft(2, '0') + "%] WebCrawler: " + Strings.FormatNumber(__ScannerEngines.WebCrawler.LinksLoaded, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__ScannerEngines.MyWebSearch.LinksLoaded > 0)
            {
                stringBuilder.AppendLine("[" + __ScannerEngines.MyWebSearch.Progress.ToString().PadLeft(2, '0') + "%] MyWebSearch: " + Strings.FormatNumber(__ScannerEngines.MyWebSearch.LinksLoaded, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__ScannerEngines.Sapo.LinksLoaded > 0)
            {
                stringBuilder.AppendLine("[" + __ScannerEngines.Sapo.Progress.ToString().PadLeft(2, '0') + "%] Sapo: " + Strings.FormatNumber(__ScannerEngines.Sapo.LinksLoaded, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Duplicates");
            if (__SearchSummary.InQueue > 0)
            {
                stringBuilder.AppendLine("Queue: " + Strings.FormatNumber(__SearchSummary.InQueue, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__SearchSummary.InQueueAsSimilar > 0)
            {
                stringBuilder.AppendLine("QueueSimilar: " + Strings.FormatNumber(__SearchSummary.InQueueAsSimilar, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            /*if (__SearchSummary.InExploitable > 0)
            {
                stringBuilder.AppendLine("Exploitable: " + Strings.FormatNumber(__SearchSummary.InExploitable, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__SearchSummary.NonInjectable > 0)
            {
                stringBuilder.AppendLine("Non-Injectable: " + Strings.FormatNumber(__SearchSummary.NonInjectable, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__SearchSummary.InInjectableDomain > 0)
            {
                stringBuilder.AppendLine("Injectable Domain: " + Strings.FormatNumber(__SearchSummary.InInjectableDomain, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }
            if (__SearchSummary.InInTrash > 0)
            {
                stringBuilder.AppendLine("Trash: " + Strings.FormatNumber(__SearchSummary.InInTrash, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            }*/
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Total: " + Strings.FormatNumber(__SearchSummary.Loaded(), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            stringBuilder.AppendLine("Skiped: " + Strings.FormatNumber(checked(__SearchSummary.Loaded() - __SearchSummary.Added), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            stringBuilder.AppendLine("Valid Added: " + Strings.FormatNumber(__SearchSummary.Added, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
            stringBuilder.AppendLine();
            StringBuilder arg_6B5_0 = stringBuilder;
            string arg_6B0_0 = "Elapsed: ";
            TimeSpan ts = new TimeSpan(DateAndTime.Now.Ticks);
            arg_6B5_0.AppendLine(arg_6B0_0 + __ScannerConf_.TimeSpan.Subtract(ts).ToString("dd\\hh\\:mm\\:ss"));
            Console.WriteLine(stringBuilder.ToString());
        }
    }
}

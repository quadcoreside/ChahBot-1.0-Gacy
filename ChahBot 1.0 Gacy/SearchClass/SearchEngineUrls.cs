using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace ChahBot_1_0_Gacy.SearchClass
{
    class SearchEngineUrls
    {
	    public enum Host
	    {
		    Google,
		    Bing,
		    Yahoo,
		    Aol,
		    Yandex,
		    Ask,
		    Wow,
		    WebCrawler,
		    MyWebSearch,
		    Sapo
	    }
	    public enum Worker
	    {
		    Ide,
		    Working,
		    Paused
	    }
	    public delegate void Scanner_ProgressEventHandler(int percentage, SearchEngineUrls.Host h);
	    public delegate void Scanner_LoadedLinkEventHandler(List<string> urls, SearchEngineUrls.Host h);
	    public delegate void Scanner_DoneEventHandler(SearchEngineUrls.Host h);
	    private class ThreadScanner
	    {
		    [CompilerGenerated]
		    private Thread _Thread;

		    [CompilerGenerated]
		    private string _Dork;

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

		    public string Dork
		    {
			    get
			    {
				    return this._Dork;
			    }
			    set
			    {
				    this._Dork = value;
			    }
		    }
	    }

	    private SearchEngineUrls.Host __Host;
	    private CookieCollection __Cookie;
	    private int __Retry;
	    private int __TimeOut;
	    private SearchEngineUrls.Worker __State;
	    private ThreadPool __ThreadPool;
	    private int __Delay;
	    private int __Threads;
	    private List<string> __Dorks;
	    private List<WebProxy> __BlackListProxy;

	    [CompilerGenerated]
	    private int _Progress;

	    [CompilerGenerated]
	    private int _LinksLoaded;

	    private SearchEngineUrls.Scanner_ProgressEventHandler Scanner_ProgressEvent;
	    private SearchEngineUrls.Scanner_LoadedLinkEventHandler Scanner_LoadedLinkEvent;
	    private SearchEngineUrls.Scanner_DoneEventHandler Scanner_DoneEvent;
	    private long ScannerDelay_LastTick;

	    public event SearchEngineUrls.Scanner_ProgressEventHandler Scanner_Progress
	    {
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    add
		    {
			    this.Scanner_ProgressEvent = (SearchEngineUrls.Scanner_ProgressEventHandler)Delegate.Combine(this.Scanner_ProgressEvent, value);
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    remove
		    {
			    this.Scanner_ProgressEvent = (SearchEngineUrls.Scanner_ProgressEventHandler)Delegate.Remove(this.Scanner_ProgressEvent, value);
		    }
	    }
	    public event SearchEngineUrls.Scanner_LoadedLinkEventHandler Scanner_LoadedLink
	    {
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    add
		    {
			    this.Scanner_LoadedLinkEvent = (SearchEngineUrls.Scanner_LoadedLinkEventHandler)Delegate.Combine(this.Scanner_LoadedLinkEvent, value);
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    remove
		    {
			    this.Scanner_LoadedLinkEvent = (SearchEngineUrls.Scanner_LoadedLinkEventHandler)Delegate.Remove(this.Scanner_LoadedLinkEvent, value);
		    }
	    }
	    public event SearchEngineUrls.Scanner_DoneEventHandler Scanner_Done
	    {
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    add
		    {
			    this.Scanner_DoneEvent = (SearchEngineUrls.Scanner_DoneEventHandler)Delegate.Combine(this.Scanner_DoneEvent, value);
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    remove
		    {
			    this.Scanner_DoneEvent = (SearchEngineUrls.Scanner_DoneEventHandler)Delegate.Remove(this.Scanner_DoneEvent, value);
		    }
	    }

	    public int Progress
	    {
		    get
		    {
			    return this._Progress;
		    }
		    set
		    {
			    this._Progress = value;
		    }
	    }
	    public int LinksLoaded
	    {
		    get
		    {
			    return this._LinksLoaded;
		    }
		    set
		    {
			    this._LinksLoaded = value;
		    }
	    }

	    public SearchEngineUrls(SearchEngineUrls.Host h, int iRetry, int iTimeOut)
	    {
		    this.__Host = h;
		    this.__Retry = iRetry;
		    this.__TimeOut = iTimeOut;
		    this.__State = SearchEngineUrls.Worker.Ide;
		    this.__Cookie = new CookieCollection();
	    }

	    public bool Complete()
	    {
		    return this.__State == SearchEngineUrls.Worker.Ide;
	    }
	    private bool ScannerStoped()
	    {
		    while (this.__State != SearchEngineUrls.Worker.Ide && this.__State != SearchEngineUrls.Worker.Working)
		    {
			    Thread.Sleep(500);
		    }
		    return this.__State == SearchEngineUrls.Worker.Ide;
	    }
	    private void ScannerDelay()
	    {
		    if (this.__Delay == 0)
		    {
			    return;
		    }
		    if (ScannerDelay_LastTick < 1L)
		    {
			    ScannerDelay_LastTick = 1L;
		    }
		    checked
		    {
			    do
			    {
				    long num = (long)Math.Round((double)DateTime.UtcNow.Ticks / 10000.0);
				    if (num - ScannerDelay_LastTick > unchecked((long)this.__Delay))
				    {
					    break;
				    }
				    Thread.Sleep(100);
				    Application.DoEvents();
			    }
			    while (!this.ScannerStoped());
			    ScannerDelay_LastTick = (long)Math.Round((double)DateTime.UtcNow.Ticks / 10000.0);
		    }
	    }

	    public void StartScanning(List<string> dorks, int threads, int delay)
	    {
		    if (this.__State != SearchEngineUrls.Worker.Ide)
		    {
			    MessageBox.Show("Scanner already running!");
			    return;
		    }
		    if (dorks.Count <= threads)
		    {
			    threads = dorks.Count;
		    }
		    this.__ThreadPool = new global::ThreadPool(threads);
		    this.__BlackListProxy = new List<WebProxy>();
		    this.Progress = 0;
		    this.LinksLoaded = 0;
		    this.__Delay = delay;
		    this.__Dorks = dorks;
		    this.__Threads = threads;
		    this.__State = SearchEngineUrls.Worker.Working;
		    Thread thread = new Thread(new ThreadStart(this.DoWork));
		    thread.Start();
	    }
	    private void DoWork()
	    {
		    checked
		    {
			    try
			    {
				    if (this.__Dorks.Count <= this.__Threads)
				    {
					    this.__Threads = this.__Dorks.Count;
				    }
				    this.__ThreadPool = new ThreadPool(this.__Threads);
				    int count = this.__Dorks.Count;
				    int num = count - 1;
				    for (int i = 0; i <= num; i++)
				    {
					    if (this.ScannerStoped())
					    {
						    break;
					    }
					    this.Progress = (int)Math.Round(Math.Round((double)(100 * i) / (double)count));
					    SearchEngineUrls.Scanner_ProgressEventHandler scanner_ProgressEvent = this.Scanner_ProgressEvent;
					    if (scanner_ProgressEvent != null)
					    {
						    scanner_ProgressEvent(this.Progress, this.__Host);
					    }
					    Thread thread = new Thread(delegate(object a0)
					    {
						    this.ScannThread((SearchEngineUrls.ThreadScanner)a0);
					    });
					    thread.Start(new SearchEngineUrls.ThreadScanner
					    {
						    Thread = thread,
						    Dork = this.__Dorks[i]
					    });
					    this.__ThreadPool.Open(thread);
					    this.__ThreadPool.WaitForThreads();
				    }
			    }
			    catch (Exception expr_F0)
			    {
				    ProjectData.SetProjectError(expr_F0);
				    ProjectData.ClearProjectError();
			    }
			    finally
			    {
				    this.__ThreadPool.AllJobsPushed();
				    while (!this.ScannerStoped() && !this.__ThreadPool.Finished && this.__ThreadPool.ThreadCount != 0)
				    {
				    }
				    this.__State = SearchEngineUrls.Worker.Ide;
				    this.Progress = 100;
				    SearchEngineUrls.Scanner_DoneEventHandler scanner_DoneEvent = this.Scanner_DoneEvent;
				    if (scanner_DoneEvent != null)
				    {
					    scanner_DoneEvent(this.__Host);
				    }
			    }
		    }
	    }

	    public void StopScanning()
	    {
		    this.__State = SearchEngineUrls.Worker.Ide;
	    }
	    public void PauseScanning(bool state)
	    {
		    if (this.__State != SearchEngineUrls.Worker.Ide)
		    {
			    if (state)
			    {
				    this.__State = SearchEngineUrls.Worker.Paused;
			    }
			    else
			    {
				    this.__State = SearchEngineUrls.Worker.Working;
			    }
		    }
	    }

	    private void ScannThread(SearchEngineUrls.ThreadScanner t)
	    {
		    checked
		    {
			   /*try
			    {*/
				    int num = 0;
				    do
				    {
					    bool flag = false;
					    List<string> links = this.GetLinks(t.Dork, num, ref flag);
					    if (links.Count > 0)
					    {
						    SearchEngineUrls.Scanner_LoadedLinkEventHandler scanner_LoadedLinkEvent = this.Scanner_LoadedLinkEvent;
						    if (scanner_LoadedLinkEvent != null)
						    {
							    scanner_LoadedLinkEvent(links, this.__Host);
						    }
					    }
					    if (!flag)
					    {
						    break;
					    }
					    if (this.ScannerStoped())
					    {
						    break;
					    }
					    num++;
				    }
                    //Nombre de page boucle default 100
				    while (num <= 100);
			    /*}
			    catch (Exception expr_59)
			    {
				    ProjectData.SetProjectError(expr_59);
				    Thread.Sleep(2000);
				    this.__ThreadPool.Close(t.Thread);
				    ProjectData.ClearProjectError();
			    }*/
		    }
	    }

	    private WebProxy GetProxy(ref bool AllBlacklisted)
	    {
		    if (global::Globals.G_Proxy.Enable)
		    {
			    WebProxy proxy;
			    while (true)
			    {
				    proxy = global::Globals.G_Proxy.Proxy_;
				    if (global::Globals.G_Proxy.List.Count == this.__BlackListProxy.Count)
				    {
					    break;
				    }
				    if (!this.__BlackListProxy.Contains(proxy))
				    {
					    return proxy;
				    }
			    }
			    AllBlacklisted = true;
			    return proxy;
		    }
		    return null;
	    }

	    private List<string> GetLinks(string sQuery, int iPage, ref bool bNextPage)
	    {
		    List<string> list = new List<string>();
		    string address = this.GetAddress(sQuery, iPage);
		    int arg_19_0 = 1;
		    int _Retry = this.__Retry;
		    int i = arg_19_0;
		    checked
		    {
			    while (i <= _Retry)
			    {
				    WebProxy proxy = this.GetProxy(ref bNextPage);
				    if (!bNextPage)
				    {
					    if (!this.ScannerStoped())
					    {
						    string codeSource = this.LoadPage(address, proxy);
						    if (!this.CheckedCaptcha(codeSource))
						    {
							    goto IL_77;
						    }
                            //Probleme ===> Si declecher ====>
						    if (Information.IsNothing(proxy))
						    {
							    bNextPage = false;
							    return list;
						    }
						    if (this.__BlackListProxy.Contains(proxy))
						    {
							    goto IL_77;
						    }
						    this.__BlackListProxy.Add(proxy);
						    IL_148:
						    i++;
						    continue;
						        IL_77:
						        if (this.__Host == SearchEngineUrls.Host.Aol)
						        {
							        if (iPage == 0 && !codeSource.ToLower().Contains(">Web Results</h3>".ToLower()))
							        {
								        codeSource = this.LoadPage(address, proxy);
							        }
						        }
                                else if (this.__Host == SearchEngineUrls.Host.Google && codeSource.ToLower().Contains("tyle=\"display:block\">"))
						        {
                                    while (codeSource.ToLower().Contains("tyle=\"display:block\">"))
                                    {
                                        codeSource = codeSource.Substring(codeSource.IndexOf("tyle=\"display:block\">"));
							            list = this.RegExp("\\s+href\\s*=\\s*\"?([^\" >]+)\"", codeSource);
							            if (list.Count > 0)
							            {
								            int arg_101_0 = 1;
								            int _Retry2 = this.__Retry;
								            for (int j = arg_101_0; j <= _Retry2; j++)
								            {
                                                string by_url = (list[0].StartsWith("http")) ? list[0] : "https://www.google.fr" + HttpUtility.HtmlDecode(list[0]);
                                                codeSource = this.LoadPage(by_url, proxy);
									            if (!string.IsNullOrEmpty(codeSource))
									            {
										            break;
									            }
								            }
								            list.Clear();
								            i = this.__Retry;
							            }
                                        Thread.Sleep(100);
                                    }
						        }
						        /*if (string.IsNullOrEmpty(codeSource) || !this.IsValidePage(codeSource, ref bNextPage))
						        {
                                    if (i < this.__Retry)
                                    {
                                        goto IL_148;
                                    }
                                    else
                                    {
                                        return list;
                                    }
						        }*/
						        list = this.ParseURLs(codeSource);
					    }
					    return list;
				    }
				    bNextPage = false;
				    return list;
			    }
			    return list;
		    }
	    }
	    private string GetAddress(string sQuery, int iPage)
	    {
		    StringBuilder stringBuilder = new StringBuilder();
		    checked
		    {
			    switch (this.__Host)
			    {
			        case SearchEngineUrls.Host.Google:
				        stringBuilder.Append("https://www.google.com/search?");
				        stringBuilder.Append("newwindow=1");
				        stringBuilder.Append("&site=");
				        stringBuilder.Append("&q=" + HttpUtility.UrlEncode(sQuery));
				        stringBuilder.Append("&start=" + Convert.ToString(iPage * 100));
				        stringBuilder.Append("&num=100");
				        break;
			        case SearchEngineUrls.Host.Bing:
				        stringBuilder.Append("http://www.bing.com/search?");
				        stringBuilder.Append("q=" + HttpUtility.UrlEncode(sQuery));
				        if (iPage > 0)
				        {
					        stringBuilder.Append("&first=" + Convert.ToString(iPage * 50));
				        }
				        stringBuilder.Append("&count=50");
				        break;
			        case SearchEngineUrls.Host.Yahoo:
				        stringBuilder.Append("http://search.yahoo.com/search?");
				        stringBuilder.Append("n=100");
				        stringBuilder.Append("&p=" + HttpUtility.UrlEncode(sQuery));
				        if (iPage > 0)
				        {
					        stringBuilder.Append("&b=" + Convert.ToString(iPage * 100 + 1));
				        }
				        break;
			        case SearchEngineUrls.Host.Aol:
				        stringBuilder.Append("http://search.aol.com/aol/search?");
				        stringBuilder.Append("&q=" + HttpUtility.UrlEncode(sQuery));
				        if (iPage > 0)
				        {
					        stringBuilder.Append("&page=" + Convert.ToString(iPage + 1));
				        }
				        break;
			        case SearchEngineUrls.Host.Yandex:
				        stringBuilder.Append("http://www.yandex.com/yandsearch?");
				        stringBuilder.Append("text=" + HttpUtility.UrlEncode(sQuery));
				        if (iPage > 0)
				        {
					        stringBuilder.Append("&p=" + Convert.ToString(iPage));
				        }
				        break;
			        case SearchEngineUrls.Host.Ask:
				        stringBuilder.Append("http://www.ask.com/web?");
				        stringBuilder.Append("q=" + HttpUtility.UrlEncode(sQuery));
				        if (iPage > 0)
				        {
					        stringBuilder.Append("&page=" + Convert.ToString(iPage));
				        }
				        break;
			        case SearchEngineUrls.Host.Wow:
				        stringBuilder.Append("http://www.wow.com/search?");
				        stringBuilder.Append("q=" + HttpUtility.UrlEncode(sQuery));
				        if (iPage > 0)
				        {
					        stringBuilder.Append("&page=" + Convert.ToString(iPage + 1));
				        }
				        break;
			        case SearchEngineUrls.Host.WebCrawler:
				        stringBuilder.Append("http://www.webcrawler.com/search/web?");
				        stringBuilder.Append("q=" + HttpUtility.UrlEncode(sQuery));
				        if (iPage > 0)
				        {
					        stringBuilder.Append("&ridx=" + Convert.ToString(iPage + 1));
				        }
				        break;
			        case SearchEngineUrls.Host.MyWebSearch:
				        stringBuilder.Append("http://search.mywebsearch.com/mywebsearch/GGmain.jhtml?");
				        stringBuilder.Append("searchfor=" + HttpUtility.UrlEncode(sQuery));
				        if (iPage > 0)
				        {
					        stringBuilder.Append("&pn=" + Convert.ToString(iPage + 1));
				        }
				        break;
			        case SearchEngineUrls.Host.Sapo:
				        stringBuilder.Append("http://pesquisa.sapo.pt/?");
				        stringBuilder.Append("q=" + HttpUtility.UrlEncode(sQuery));
				        if (iPage > 0)
				        {
					        stringBuilder.Append("&page=" + Convert.ToString(iPage + 1));
				        }
				        break;
			    }
			    return stringBuilder.ToString();
		    }
	    }

	    private bool IsValidePage(string sData, ref bool bNextPage)
	    {
		    switch (this.__Host)
		    {
		        case SearchEngineUrls.Host.Google:
			        bNextPage = sData.ToLower().Contains("display:block;margin-left:53px".ToLower());
                    if (sData.ToLower().Contains("/images/nav_logo"))
                    {
                        return true;
                    }
                    return false;
		        case SearchEngineUrls.Host.Bing:
			        bNextPage = sData.ToLower().Contains("sb_pagN".ToLower());
			        return sData.ToLower().Contains("schemas.live.com/Web/".ToLower());
		        case SearchEngineUrls.Host.Yahoo:
			        bNextPage = sData.ToLower().Contains("pg-next".ToLower());
			        return sData.ToLower().Contains("l.yimg.com".ToLower());
		        case SearchEngineUrls.Host.Aol:
			        bNextPage = sData.ToLower().Contains("class=\"\"nextRes\"".ToLower());
			        return sData.ToLower().Contains("search.aol.com/rdf/finds".ToLower());
		        case SearchEngineUrls.Host.Yandex:
			        bNextPage = sData.ToLower().Contains("button__text\">+5<".ToLower());
			        return sData.ToLower().Contains("www.yandex.com/clck".ToLower());
		        case SearchEngineUrls.Host.Ask:
			        bNextPage = sData.ToLower().Contains("nextPageLink".ToLower());
			        return sData.ToLower().Contains("sp.ask.com".ToLower());
		        case SearchEngineUrls.Host.Wow:
			        bNextPage = sData.ToLower().Contains("class=\"\"nextRes\"".ToLower());
			        return sData.ToLower().Contains("aolcdn.com".ToLower());
		        case SearchEngineUrls.Host.WebCrawler:
			        bNextPage = sData.ToLower().Contains("results-bottom".ToLower());
			        return sData.ToLower().Contains("class=\"groupTitle\">".ToLower());
		        case SearchEngineUrls.Host.MyWebSearch:
			        bNextPage = sData.ToLower().Contains("class=\"pag-label\">".ToLower());
			        return sData.ToLower().Contains("akd.search.mywebsearch.com".ToLower());
		        case SearchEngineUrls.Host.Sapo:
			        bNextPage = sData.ToLower().Contains(">seguinte &gt;&gt;<".ToLower());
			        return sData.ToLower().Contains("sapo.pt/Prototype/".ToLower());
		        default:
		        {
                    //Conditions switch depracted donc true
			        bool result = true;
			        return result;
		        }
		    }
	    }
	    private bool CheckedCaptcha(string sData)
	    {
		    switch (this.__Host)
		    {
		        case SearchEngineUrls.Host.Google:
			        return sData.ToLower().Contains("google.com/websearch/answer/86640".ToLower());
		        case SearchEngineUrls.Host.Yandex:
			        return sData.ToLower().Contains("b-captcha__layout".ToLower());
		        case SearchEngineUrls.Host.Ask:
			        return sData.ToLower().Contains("bmailto:unauthorized@ask.com".ToLower());
		    }
            return false;
	    }
	    private List<string> ParseURLs(string sData)
	    {
		    string sRegExp = "";
		    switch (this.__Host)
		    {
		        case SearchEngineUrls.Host.Google:
			        sRegExp = Regex.Escape("/url?q=") + "?([^;]+)&amp;";
			        break;
		        case SearchEngineUrls.Host.Bing:
			        sRegExp = "<a\\s+href\\s*=\\s*\"?([^\" >]+)\"";
			        break;
		        case SearchEngineUrls.Host.Yahoo:
			        sRegExp = "RU\\s*=\\s*\"?([^\" >]+)\"";
			        break;
		        case SearchEngineUrls.Host.Aol:
			        sRegExp = "href\\s*=\\s*\"?([^\" >]+)\"";
			        break;
		        case SearchEngineUrls.Host.Yandex:
			        sRegExp = Regex.Escape("url=") + "?([^;]+)&amp;";
			        break;
		        case SearchEngineUrls.Host.Ask:
			        sRegExp = "\\s+href\\s*=\\s*\"?([^\" >]+)\"";
			        break;
		        case SearchEngineUrls.Host.Wow:
			        sRegExp = "href\\s*=\\s*\"?([^\" >]+)\"";
			        break;
		        case SearchEngineUrls.Host.WebCrawler:
			        sRegExp = Regex.Escape("ru=") + "?([^;]+)&amp;";
			        break;
		        case SearchEngineUrls.Host.MyWebSearch:
			        sRegExp = Regex.Escape("class=\"algouri\">") + "?([^<]+)" + Regex.Escape("<");
			        break;
		        case SearchEngineUrls.Host.Sapo:
			        sRegExp = "<a\\s+href\\s*=\\s*\"?([^\" >]+)\"";
			        break;
		    }
		    List<string> list = this.RegExp(sRegExp, sData);
		    List<string> list2 = new List<string>();
		    checked
		    {
			   /* try
			    {*/
				    List<string>.Enumerator enumerator = list.GetEnumerator();
				    while (enumerator.MoveNext())
				    {
					    string text = enumerator.Current;
					    if (this.__Host == SearchEngineUrls.Host.Google)
					    {
						    if (text.IndexOf("&amp;") > 1)
						    {
							    text = text.Substring(0, text.IndexOf("&amp;"));
						    }
						    if (text.IndexOf("url=") > 1)
						    {
							    text = text.Substring(text.IndexOf("url=") + "url=".Length);
						    }
						    if (text.IndexOf("&goto=") > 1)
						    {
							    text = text.Substring(text.IndexOf("&goto=") + "&goto=".Length);
							    text = "http://" + text;
						    }
						    if (text.IndexOf("link=") > 1)
						    {
							    text = text.Substring(text.IndexOf("link=") + "link=".Length);
						    }
						    text = HttpUtility.UrlDecode(text);
						    text = Strings.Split(text, "=http", -1, CompareMethod.Binary)[0];
					    }
					    else if (this.__Host == SearchEngineUrls.Host.Yahoo)
					    {
						    text = Strings.Split(text, "/RK", -1, CompareMethod.Binary)[0].Trim();
						    text = HttpUtility.UrlDecode(text);
						    if (text.Contains("/search/srpcache"))
						    {
							    text = "";
						    }
					    }
					    else if (this.__Host == SearchEngineUrls.Host.WebCrawler)
					    {
						    text = Strings.Split(text, "&amp", -1, CompareMethod.Binary)[0].Trim();
					    }
					    text = HttpUtility.UrlDecode(text).Trim();
					    if (UrlRight(text) && UrlCanBeExploited(text) && !list2.Contains(text))
					    {
						    list2.Add(text);
					    }
				    }
			   /* }
			    finally
			    {
				    List<string>.Enumerator enumerator = new List<string>.Enumerator();
				    ((IDisposable)enumerator).Dispose();
			    }*/
			    this.LinksLoaded += list2.Count;
			    return list2;
		    }
	    }
	    private List<string> RegExp(string sRegExp, string sData)
	    {
		    List<string> list = new List<string>();
		   /* try
		    {*/
			    Regex regex = new Regex(sRegExp, RegexOptions.IgnoreCase);
			    Match match = regex.Match(sData);
			    while (match.Success)
			    {
				    string text = match.Groups[1].Value.Trim();
				    if (!string.IsNullOrEmpty(text))
				    {
					    list.Add(text);
				    }
				    match = match.NextMatch();
			    }
		   /* }
		    catch (Exception expr_52)
		    {
			    ProjectData.SetProjectError(expr_52);
			    ProjectData.ClearProjectError();
		    }*/
		    return list;
	    }

	    private string LoadPage(string sUrl, WebProxy oProxy)
	    {
            bool flag = false;
		    string codeSource = string.Empty;
		    string sStatus = "";
		    string erreurText = "";
		    checked
		    {
			    try
			    {
				    CookieContainer cookieContainer = new CookieContainer();
				    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(sUrl);
				    /*try
				    {*/
					    IEnumerator enumerator = this.__Cookie.GetEnumerator();
					    while (enumerator.MoveNext())
					    {
						    Cookie cookie = (Cookie)enumerator.Current;
						    cookieContainer.Add(cookie);
					    }
				   /* }
				    finally
				    {
					    IEnumerator enumerator = null;
					    if (enumerator is IDisposable)
					    {
						    (enumerator as IDisposable).Dispose();
					    }
				    }*/
                    httpWebRequest.UserAgent = Globals.G_Utilities.getUseragent();
				    httpWebRequest.Method = "GET";
				    httpWebRequest.Timeout = this.__TimeOut;
				    httpWebRequest.ReadWriteTimeout = this.__TimeOut;
                    httpWebRequest.CookieContainer = new CookieContainer();
				    httpWebRequest.CookieContainer = cookieContainer;
				    httpWebRequest.AllowAutoRedirect = true;
				    httpWebRequest.Proxy = oProxy;
                    httpWebRequest.KeepAlive = true;
                    
				    if (flag)
				    {
					    if (!Information.IsNothing(oProxy))
					    {
						    erreurText = oProxy.Address.Host + ":" + Convert.ToString(oProxy.Address.Port);
					    }
                        Console.WriteLine(sUrl, erreurText);
					    TimeSpan ts = new TimeSpan(DateAndTime.Now.Ticks);
				    }
				    this.ScannerDelay();
				    using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				    {
					    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
					    {
						    codeSource = streamReader.ReadToEnd();
					    }
					    if (httpWebResponse.StatusCode == HttpStatusCode.OK)
					    {
						    this.__Cookie = global::HttpRequest.SyncCookies(this.__Cookie, httpWebResponse.Cookies);
					    }
					    sStatus = Convert.ToString((int)httpWebResponse.StatusCode) + " - " + httpWebResponse.StatusDescription;
				    }
			    }
			    catch (Exception expr_1DB)
			    {
                    if (flag)
                    {
                        Console.WriteLine(expr_1DB + " --> Timeout");
                    } 
				    sStatus = "Timeout";
			    }
			    finally
			    {
				    if (flag)
				    {
					    TimeSpan timeSpan = new TimeSpan(DateAndTime.Now.Ticks);
					    TimeSpan timeSpan2 = timeSpan;
					    TimeSpan ts = new TimeSpan();
					    double totalMilliseconds = timeSpan2.Subtract(ts).TotalMilliseconds;
                        Console.WriteLine(erreurText, sUrl, Globals.G_Utilities.FormatBytes((double)codeSource.Length), Conversions.ToString(totalMilliseconds) + "ms", sStatus);
				    }
				    Globals.TRAFFIC_RECEIVED += unchecked((long)codeSource.Length);
			    }
			    return codeSource;
		    }
	    }

	    [DebuggerStepThrough, CompilerGenerated]
	    private void _Lambda__1(object a0)
	    {
		    this.ScannThread((SearchEngineUrls.ThreadScanner)a0);
	    }

        internal bool UrlCanBeExploited(string strURL)
        {
            strURL = strURL.ToLower();
            if (!strURL.Contains("="))
            {
                return false;
            }
            string[] array = Config.Config.excludeUrlWords.Split(new char[]
	        {
		        ';'
	        });
            checked
            {
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i];
                    if (!string.IsNullOrEmpty(text) && strURL.Contains(text.ToLower()))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
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

    
    }
}

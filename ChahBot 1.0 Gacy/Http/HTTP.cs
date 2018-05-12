using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public enum enHTTPMethod
{
    GET,
    POST
}

public class HTTP : IDisposable
{
    internal global::HttpRequest WebRequest;

    internal ChahBot_1_0_Gacy.HttpResponse WebResponse;

    private int __TimeOut;

    private bool __DesableProxy;

    private const string HTTP_DEFAUFT_ACCEPT = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

    private const string HTTP_DEFAUFT_GET_CONTENT_TYPE = "text/html; charset=utf-8";

    private const string HTTP_DEFAUFT_POST_CONTENT_TYPE = "application/x-www-form-urlencoded";

    private bool __Disposed;

    public HTTP(int iTimeOut, bool bDesableProxy = false)
    {
        this.__TimeOut = iTimeOut;
        this.__DesableProxy = bDesableProxy;
    }

    public string GetHTML(string sUrl, enHTTPMethod oMethod, ref string sPostData, object oCokies, NetworkCredential oCredentials, bool bIsDebugable, ref string sErrDesc,  bool randUserAgent = true)
    {
        string source = "";
        string sProxy = "N/A";
        bool flag = true;
        string sID = "";
        checked
        {
            string sStatus;
            long value = 0;
            try
            {
                if (flag)
                {
                    TimeSpan ts = new TimeSpan(DateAndTime.Now.Ticks);
                }
                Uri requestUri = new Uri(sUrl);
                string cookieText = "";
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUri);
                httpWebRequest.AllowAutoRedirect = Globals.HTTP_ALLOW_AUTO_REDIRECT;
                httpWebRequest.Credentials = oCredentials;
                Proxy g_Proxy = global::Globals.G_Proxy;
                lock (g_Proxy)
                {
                    if (global::Globals.G_Proxy.Enable & !this.__DesableProxy)
                    {
                        httpWebRequest.Proxy = global::Globals.G_Proxy.Proxy_;
                        sProxy = Globals.G_Proxy.Proxy_.Address.Host + ":" + Globals.G_Proxy.Proxy_.Address.Port;
                    }
                }
                if (randUserAgent)
                {
                    httpWebRequest.UserAgent = Globals.G_Utilities.getUseragent();
                }
                else
                {
                    httpWebRequest.UserAgent = Globals.USER_AGENT;
                }
                httpWebRequest.Timeout = this.__TimeOut;
                httpWebRequest.ReadWriteTimeout = this.__TimeOut;
                httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                httpWebRequest.AutomaticDecompression = (DecompressionMethods)(-1);
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Referer = sUrl;

                httpWebRequest.CookieContainer = new CookieContainer();
                if (oCokies is string)
                {
                    cookieText = Conversions.ToString(oCokies);
                }
                else if (oCokies is CookieCollection && oCokies != null)
                {
                    object arg_1A8_0 = httpWebRequest.CookieContainer;
                    Type arg_1A8_1 = null;
                    string arg_1A8_2 = "Add";
                    object[] array = new object[]
					{
						RuntimeHelpers.GetObjectValue(oCokies)
					};
                    object[] arg_1A8_3 = array;
                    string[] arg_1A8_4 = null;
                    Type[] arg_1A8_5 = null;
                    bool[] array2 = new bool[]
					{
						true
					};
                    NewLateBinding.LateCall(arg_1A8_0, arg_1A8_1, arg_1A8_2, arg_1A8_3, arg_1A8_4, arg_1A8_5, array2, true);
                    if (array2[0])
                    {
                        oCokies = RuntimeHelpers.GetObjectValue(array[0]);
                    }
                }
                if (!string.IsNullOrEmpty(cookieText))
                {
                    string[] array3 = cookieText.Split(new char[]
					{
						';'
					});
                    string[] array4 = array3;
                    for (int i = 0; i < array4.Length; i++)
                    {
                        string text3 = array4[i];
                        try
                        {
                            string[] array5 = text3.Split(new char[]
							{
								'='
							});
                            array5[0] = array5[0].Trim();
                            array5[1] = array5[1].Trim();
                            if (array5.Length > 2)
                            {
                                int arg_240_0 = 2;
                                int num = array5.Length - 1;
                                for (int j = arg_240_0; j <= num; j++)
                                {
                                    string[] array6 = array5;
                                    array6[1] = array6[1] + "=" + array5[j];
                                }
                            }
                            if (array5.Length >= 2 && (array5[0].Trim().Length > 0 & array5[1].Trim().Length > 0))
                            {
                                httpWebRequest.CookieContainer.Add(new Uri(sUrl), new Cookie(array5[0], array5[1]));
                            }
                        }
                        catch (Exception expr_2B8)
                        {
                            ProjectData.SetProjectError(expr_2B8);
                            ProjectData.ClearProjectError();
                        }
                    }
                }

                if (oMethod == enHTTPMethod.GET)
                {
                    httpWebRequest.ContentType = "text/html; charset=utf-8";
                    httpWebRequest.Method = global::HttpRequest.HttpMethod.Get.ToString();
                }
                else
                {
                    if (Globals.HTTP_ENCONDING_HEADERS)
                    {
                        sPostData = Globals.G_Utilities.EncodeURL(sPostData);
                    }
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.Method = global::HttpRequest.HttpMethod.Post.ToString();
                    httpWebRequest.ContentLength = unchecked((long)sPostData.Length);
                    httpWebRequest.KeepAlive = false;
                    using (Stream requestStream = httpWebRequest.GetRequestStream())
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(sPostData);
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                
                }

                if (flag)
                {
                    Console.WriteLine("HTTP ==> " + sProxy);
                }

                this.WebRequest = new global::HttpRequest(httpWebRequest);
                this.WebResponse = this.WebRequest.GetRequest();
                if (this.WebResponse != null)
                {
                    source = this.WebResponse.SourceCode;
                    sStatus = Conversions.ToString((int)this.WebResponse.HTTPResponse.StatusCode) + " - " + this.WebResponse.HTTPResponse.StatusDescription;
                }
                else
                {
                    source = "";
                    sStatus = "Not Found";
                }
            }
            catch (Exception expr_425)
            {
                ProjectData.SetProjectError(expr_425);
                Exception ex = expr_425;
                sErrDesc = ex.Message;
                sStatus = sErrDesc;
                ProjectData.ClearProjectError();
            }
            finally
            {
                if (flag)
                {
                    TimeSpan timeSpan = new TimeSpan(DateAndTime.Now.Ticks);
                    TimeSpan timeSpan2 = timeSpan;
                    TimeSpan ts = new TimeSpan();
                    value = (long)Math.Round(timeSpan2.Subtract(ts).TotalMilliseconds);
                }
            }
            try
            {
                if (flag)
                {
                   // Console.WriteLine(sID + " " + sUrl + " " + global::Globals.G_Utilities.FormatBytes((double)source.Length) + " " + Conversions.ToString(value) + " ms ", sStatus);
                }
                global::Globals.TRAFFIC_RECEIVED += unchecked((long)source.Length);
            }
            catch (Exception expr_4EC)
            {
                ProjectData.SetProjectError(expr_4EC);
                ProjectData.ClearProjectError();
            }
            source = HttpUtility.HtmlDecode(source);
            return source;
        }
    }

    protected virtual void Dispose(bool b)
    {
        if (!this.__Disposed)
        {
            this.WebRequest = null;
            this.WebResponse = null;
        }
        this.__Disposed = true;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

   
}


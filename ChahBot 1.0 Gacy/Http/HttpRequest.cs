using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading;
using ChahBot_1_0_Gacy;

public class HttpRequest
{
    public enum HttpMethod
    {
        Get,
        Post,
        Head,
        Put,
        Trace,
        Options,
        Delete,
        PostMultiPart
    }

    private WebRequest _request;

    public WebRequest Request
    {
        get
        {
            return this._request;
        }
        set
        {
            this._request = value;
        }
    }

    public HttpRequest(WebRequest webRequest)
    {
        this._request = webRequest;
    }

    public HttpResponse GetRequest()
    {
        HttpResponse result;
        try
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)this.Request;
            HttpWebResponse httpWebResponse = null;
            DateTime now = DateTime.Now;
            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException expr_28)
            {
                ProjectData.SetProjectError(expr_28);
                WebException ex = expr_28;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ConnectFailure:
                        throw new Exception("Couldn't connect to the website.");
                    case WebExceptionStatus.ProtocolError:
                        httpWebResponse = (HttpWebResponse)ex.Response;
                        ProjectData.ClearProjectError();
                        goto IL_B8;
                    case WebExceptionStatus.Timeout:
                        throw new Exception("Timeout Error");
                }
                throw ex;
            }
            catch (ThreadAbortException expr_A2)
            {
                ProjectData.SetProjectError(expr_A2);
                ThreadAbortException ex2 = expr_A2;
                throw ex2;
            }
            catch (Exception expr_AD)
            {
                ProjectData.SetProjectError(expr_AD);
                Exception ex3 = expr_AD;
                throw ex3;
            }
        IL_B8:
            string sourceCode = string.Empty;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                sourceCode = streamReader.ReadToEnd();
            }
            TimeSpan responseTime = DateTime.Now.Subtract(now);
            ChahBot_1_0_Gacy.HttpResponse httpResponse = new ChahBot_1_0_Gacy.HttpResponse(httpWebResponse.ResponseUri, httpWebResponse.StatusCode, sourceCode, responseTime, httpWebRequest, httpWebResponse);
            httpWebResponse.Close();
            result = httpResponse;
        }
        catch (Exception expr_123)
        {
            ProjectData.SetProjectError(expr_123);
            Exception ex4 = expr_123;
            throw ex4;
        }
        return result;
    }

    public static CookieCollection SyncCookies(CookieCollection oldCookieCollection, CookieCollection addCookieCollection)
    {
        oldCookieCollection.Add(addCookieCollection);
        CookieCollection cookieCollection = new CookieCollection();
        try
        {
            IEnumerator enumerator = oldCookieCollection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Cookie cookie = (Cookie)enumerator.Current;
                if (DateTime.Compare(cookie.Expires, DateTime.MinValue) == 0 || !cookie.Expired)
                {
                    cookieCollection.Add(cookie);
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
        return cookieCollection;
    }

    public static string RequestAsRaw(HttpWebRequest webRequest)
    {
        string str = null;
        str = str + webRequest.Method + " ";
        str = str + webRequest.RequestUri.PathAndQuery + " ";
        str = str + "HTTP/" + webRequest.ProtocolVersion.ToString() + " ";
        str += "\r\n";
        return str + "Host : " + webRequest.RequestUri.Host + " ";
    }
}

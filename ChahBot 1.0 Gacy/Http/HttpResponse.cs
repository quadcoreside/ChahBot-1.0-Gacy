using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Net;
using System.Text;

namespace ChahBot_1_0_Gacy
{
    public class HttpResponse
    {
        private string _sourceCode;

        private TimeSpan _responseTime;

        private HttpStatusCode _statusCode;

        private Uri _responseUri;

        private CookieCollection _Cookies;

        private HttpWebResponse _HTTPResponse;

        private HttpWebRequest _HTTPRequest;

        private string _PostData;
        private Uri uri;
        private System.Net.HttpStatusCode httpStatusCode;
        private System.Net.CookieCollection cookieCollection;
        private System.Net.HttpWebRequest httpWebRequest;
        private System.Net.HttpWebResponse httpWebResponse;

        public string SourceCode
        {
            get
            {
                return this._sourceCode;
            }
            set
            {
                this._sourceCode = value;
            }
        }

        public TimeSpan ResponseTime
        {
            get
            {
                return this._responseTime;
            }
            set
            {
                this._responseTime = value;
            }
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return this._statusCode;
            }
        }

        public Uri ResponseUri
        {
            get
            {
                return this._responseUri;
            }
        }

        public CookieCollection Cookies
        {
            get
            {
                return this._Cookies;
            }
        }

        public WebHeaderCollection Headers
        {
            get
            {
                return this.HTTPRequest.Headers;
            }
        }

        public HttpWebResponse HTTPResponse
        {
            get
            {
                return this._HTTPResponse;
            }
            set
            {
                this._HTTPResponse = value;
            }
        }

        public HttpWebRequest HTTPRequest
        {
            get
            {
                return this._HTTPRequest;
            }
            set
            {
                this._HTTPRequest = value;
            }
        }

        public string PostData
        {
            get
            {
                return this._PostData;
            }
            set
            {
                this._PostData = value;
            }
        }

        public HttpResponse(Uri responseUri, HttpStatusCode statusCode, string sourceCode, TimeSpan responseTime, HttpWebRequest HTTPRequest, HttpWebResponse HTTPResponse)
        {
            this._sourceCode = string.Empty;
            this._sourceCode = sourceCode;
            this._statusCode = statusCode;
            this._responseUri = responseUri;
            this._responseTime = responseTime;
            this._HTTPRequest = HTTPRequest;
            this._HTTPResponse = HTTPResponse;
        }

        public HttpResponse(Uri uri, System.Net.HttpStatusCode httpStatusCode, string sourceCode, System.Net.CookieCollection cookieCollection, TimeSpan responseTime, System.Net.HttpWebRequest httpWebRequest, System.Net.HttpWebResponse httpWebResponse)
        {
            // TODO: Complete member initialization
            this.uri = uri;
            this.httpStatusCode = httpStatusCode;
            this.SourceCode = sourceCode;
            this.cookieCollection = cookieCollection;
            this.ResponseTime = responseTime;
            this.httpWebRequest = httpWebRequest;
            this.httpWebResponse = httpWebResponse;
        }

        public string RawRequest()
        {
            StringBuilder stringBuilder = new StringBuilder(1000);
            StringBuilder stringBuilder2 = stringBuilder;
            stringBuilder2.Append(this.HTTPRequest.Method);
            stringBuilder2.Append(" ");
            stringBuilder2.Append(Globals.G_Utilities.DecodeURL(this.HTTPRequest.RequestUri.PathAndQuery));
            stringBuilder2.Append(" ");
            stringBuilder2.Append("HTTP/");
            stringBuilder2.Append(this.HTTPRequest.ProtocolVersion);
            stringBuilder2.AppendLine();
            try
            {
                IEnumerator enumerator = this.HTTPRequest.Headers.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string text = Conversions.ToString(enumerator.Current);
                    stringBuilder2.Append(text);
                    stringBuilder2.Append(":");
                    stringBuilder2.AppendLine(this.HTTPRequest.Headers.Get(text));
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
            if (Operators.CompareString(this.HTTPRequest.Method, "POST", false) == 0)
            {
                stringBuilder2.AppendLine();
                stringBuilder2.AppendLine(Globals.G_Utilities.DecodeURL(this.PostData));
                stringBuilder2.AppendLine();
            }
            stringBuilder2 = null;
            return stringBuilder.ToString();
        }

        public string HeadersAsString()
        {
            WebHeaderCollection headers = this.Headers;
            checked
            {
                if (headers != null)
                {
                    StringBuilder stringBuilder = new StringBuilder(1000);
                    StringBuilder stringBuilder2 = stringBuilder;
                    int arg_26_0 = 0;
                    int num = headers.Count - 1;
                    for (int i = arg_26_0; i <= num; i++)
                    {
                        string key = headers.GetKey(i);
                        string[] values = headers.GetValues(key);
                        stringBuilder2.AppendLine(key);
                        if (values.Length > 0)
                        {
                            int arg_56_0 = 0;
                            int num2 = values.Length - 1;
                            for (int j = arg_56_0; j <= num2; j++)
                            {
                                stringBuilder2.AppendLine("  " + values[j]);
                            }
                        }
                    }
                    return stringBuilder.ToString();
                }
                return string.Empty;
            }
        }
    }
}
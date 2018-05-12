using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Proxy
{
	public enum Order
	{
		Order,
		Random
	}

    public WebProxy __Proxy;
    public bool __Enable;
    public ArrayList __List = new ArrayList();
	public string __IP;
    public const string TEST_ADDRESS = "http://www.google.com/";
    public const int TEST_TIMEOUT = 5000;
    public bool __IsList;
    public Proxy.Order __ListOrder;
    public int __ListIndex;
    public int LastI;
    public StaticLocalInitFlag LastInit;

	public bool IsList
	{
		get
		{
			return this.__IsList;
		}
		set
		{
			this.__IsList = value;
		}
	}
	public Proxy.Order ListType
	{
		get
		{
			return this.__ListOrder;
		}
		set
		{
			this.__ListOrder = value;
		}
	}
	public ArrayList List
	{
		get
		{
			return this.__List;
		}
		set
		{
			this.__List = value;
		}
	}
	public string IP
	{
		get
		{
			return this.__IP;
		}
	}

	public WebProxy Proxy_
	{
		get
		{
			return this.GetProxy();
		}
	}
	public bool Enable
	{
		get
		{
			return this.__Enable | this.__IsList;
		}
        set
        {
            this.__Enable = value;
        }
	}
	public Proxy()
	{
		this.__ListIndex = -1;
		this.LastInit = new StaticLocalInitFlag();
	}

    public void addProxy(string proxy)
    {
        __List.Add(proxy);
    }

	private WebProxy GetProxy()
	{
		checked
		{
			if (this.__IsList)
			{
				WebProxy result;
				if (this.__ListOrder == global::Proxy.Order.Order)
				{
					if (this.__ListIndex >= this.__List.Count - 1)
					{
						this.__ListIndex = 0;
					}
					else
					{
						this.__ListIndex++;
					}
					result = this.DoProxy(this.__ListIndex);
				}
				else
				{
					result = this.DoProxy(-1);
				}
				return result;
			}
			return this.__Proxy;
		}
	}

	private WebProxy DoProxy(int intIndex = -1)
	{
		if (intIndex == -1)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this.LastInit, ref flag);
				if (this.LastInit.State == 0)
				{
					this.LastInit.State = 2;
					this.LastI = -1;
				}
				else if (this.LastInit.State == 2)
				{
					throw new IncompleteInitialization();
				}
			}
			finally
			{
				this.LastInit.State = 1;
				if (flag)
				{
					Monitor.Exit(this.LastInit);
				}
			}
			Random random = new Random(DateTime.Now.Millisecond);
			int num;
			do
			{
				num = random.Next(0, checked(this.__List.Count - 1));
			}
			while (num == this.LastI);
			this.LastI = num;
			return (WebProxy)this.__List[num];
		}
		return (WebProxy)this.__List[intIndex];
	}

	public bool IniProxy(string sURL, string sUserName, string sPassword, string sDomain, ref string sServer, ref string sCode, bool bMsgBox = true)
	{
		bool result = false;
		try
		{
			try
			{
				if (sURL.StartsWith("http"))
				{
					this.__Proxy = new WebProxy(new Uri(sURL));
				}
				else
				{
					this.__Proxy = new WebProxy(sURL);
				}
			}
			catch (UriFormatException expr_2E)
			{
				ProjectData.SetProjectError(expr_2E);
				UriFormatException ex = expr_2E;
				if (bMsgBox)
				{
					MessageBox.Show("URI Format is wrong for proxy address, please fix it!\r\nFormat: address_or_IP:port\r\n" + ex.Message, "URI format is wrong", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
				}
				result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			this.__Proxy.BypassProxyOnLocal = false;
			bool flag = false;
			if (false == (string.IsNullOrEmpty(sUserName) & string.IsNullOrEmpty(sPassword) & string.IsNullOrEmpty(sDomain)))
			{
				this.__Proxy.Credentials = new NetworkCredential(sUserName, sPassword, sDomain);
			}
			else if (flag == (string.IsNullOrEmpty(sUserName) & string.IsNullOrEmpty(sPassword)))
			{
				this.__Proxy.Credentials = new NetworkCredential(sUserName, sPassword);
			}
			if (this.TestOK(this.__Proxy, ref sServer, ref sCode))
			{
				this.__Enable = true;
			}
			else
			{
				this.__Enable = false;
			}
			result = this.__Enable;
		}
		catch (WebException expr_ED)
		{
			ProjectData.SetProjectError(expr_ED);
			WebException ex2 = expr_ED;
			if (bMsgBox)
			{
				MessageBox.Show(ex2.ToString());
			}
			ProjectData.ClearProjectError();
		}
		return result;
	}

	public void TermProxy()
	{
		this.__Proxy = null;
		this.__Enable = false;
		this.__IsList = false;
		this.__ListOrder = global::Proxy.Order.Order;
		this.__List = null;
		this.__ListIndex = 0;
	}

	public bool TestOK(WebProxy oProxy, ref string sServer, ref string sCode)
	{
		string value = "";
		string text = "";
		try
		{
			if (oProxy != null)
			{
				this.__Enable = true;
				using (HTTP hTTP = new HTTP(5000, false))
				{
					HTTP arg_3A_0 = hTTP;
					string arg_3A_1 = "http://www.google.com/";
					enHTTPMethod arg_3A_2 = enHTTPMethod.GET;
					string text2 = "";
					value = arg_3A_0.GetHTML(arg_3A_1, arg_3A_2, ref text2, null, null, false, ref text);
					if (hTTP.WebResponse != null && hTTP.WebResponse.HTTPResponse != null)
					{
						sServer = hTTP.WebResponse.HTTPResponse.Server;
						sCode = Conversions.ToString((int)hTTP.WebResponse.HTTPResponse.StatusCode) + " - " + hTTP.WebResponse.HTTPResponse.StatusDescription;
					}
				}
				this.__Enable = false;
				if (!string.IsNullOrEmpty(text))
				{
					MessageBox.Show(text, "Proxy Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}
		catch (WebException expr_C4)
		{
			ProjectData.SetProjectError(expr_C4);
			WebException ex = expr_C4;
			throw ex;
		}
		return !string.IsNullOrEmpty(value);
	}
}

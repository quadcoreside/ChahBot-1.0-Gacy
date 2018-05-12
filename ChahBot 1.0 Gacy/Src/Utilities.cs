using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

public class Utilities
{
    public int FindNumeric(string sText, bool bLastIndexOf)
    {
        Regex regex = new Regex("\\d+");
        Match match = regex.Match(sText);
        if (!match.Success)
        {
            return 0;
        }
        checked
        {
            int num = 0;
            if (bLastIndexOf)
            {
                while (match.Success)
                {
                    num = match.Index + (match.Value.Length + 1);
                    match = match.NextMatch();
                }
            }
            if (num == 0)
            {
                return match.Index + (match.Value.Length + 1);
            }
            return 0;
        }
    }

    public string GetDomain(string sUrlEntered)
    {
        string result;
        try
        {
            Uri uri = new Uri(sUrlEntered);
            result = uri.Host.Replace("www.", "");
        }
        catch (Exception expr_1F)
        {
            ProjectData.SetProjectError(expr_1F);
            result = "";
            ProjectData.ClearProjectError();
        }
        return result;
    }

    public string GetDomainCode(string sUrlEntered)
    {
        try
        {
            Uri uri = new Uri(sUrlEntered);
            int num = uri.Host.LastIndexOf(".");
            if (num > 0)
            {
                return uri.Host.Substring(checked(num + 1));
            }
        }
        catch (Exception expr_2F)
        {
            ProjectData.SetProjectError(expr_2F);
            ProjectData.ClearProjectError();
        }
        return "";
    }

    public long CharCount(string OrigString, string Chars, bool CaseSensitive = false)
    {
        long result = 0;
        if (Operators.CompareString(OrigString, "", false) != 0)
        {
            long num = (long)Strings.Len(OrigString);
            long num2 = (long)Strings.Len(Chars);
            checked
            {
                long num3 = num - num2 + 1L;
                byte compare = Conversions.ToByte(Interaction.IIf(CaseSensitive, CompareMethod.Binary, CompareMethod.Text));
                long arg_4E_0 = 1L;
                long num4 = num3;
                long num6 = arg_4E_0;
                for (long num5 = arg_4E_0; num5 <= num4; num5 += 1L)
                {
                    string @string = Strings.Mid(OrigString, (int)num5, (int)num2);
                    if (Strings.StrComp(@string, Chars, (CompareMethod)compare) == 0)
                    {
                        num6 += 1L;
                    }
                }
                result = num6;
            }
        }
        return result;
    }

    public string ConvertHexToText(string sText)
    {
        checked
        {
            string result;
            try
            {
                if (string.IsNullOrEmpty(sText))
                {
                    result = "";
                }
                else
                {
                    string text = sText;
                    string text2 = "";
                    if (text.StartsWith("0x"))
                    {
                        text = text.Substring(2);
                    }
                    for (int i = 0; i < text.Length; i += 2)
                    {
                        string s = text.Substring(i, 2);
                        text2 += Conversions.ToString(Strings.ChrW((int)uint.Parse(s, NumberStyles.HexNumber)));
                    }
                    if (text2.Contains("\u0012"))
                    {
                        text2 = Conversions.ToString(Conversion.Val("&H" + sText));
                    }
                    result = text2;
                }
            }
            catch (Exception expr_9A)
            {
                ProjectData.SetProjectError(expr_9A);
                result = sText;
                ProjectData.ClearProjectError();
            }
            return result;
        }
    }

    public string ConvertTextToHex(string sText)
    {
        checked
        {
            string result;
            try
            {
                if (string.IsNullOrEmpty(sText))
                {
                    result = "";
                }
                else
                {
                    char[] chars = sText.ToCharArray();
                    byte[] bytes = Encoding.ASCII.GetBytes(chars);
                    string text = "";
                    int arg_31_0 = 0;
                    int num = bytes.Length - 1;
                    for (int i = arg_31_0; i <= num; i++)
                    {
                        text += string.Format("{0:x2}", bytes[i]);
                    }
                    result = "0x" + text;
                }
            }
            catch (Exception expr_69)
            {
                ProjectData.SetProjectError(expr_69);
                result = sText;
                ProjectData.ClearProjectError();
            }
            return result;
        }
    }

    public string ConvertTextToSQLChar(string sText, bool bGroupChar, string sDelimiter = "+", string sChar = "char")
    {
        checked
        {
            string result;
            try
            {
                if (string.IsNullOrEmpty(sText))
                {
                    result = "";
                }
                else
                {
                    char[] chars = sText.ToCharArray();
                    byte[] bytes = Encoding.ASCII.GetBytes(chars);
                    string text = "";
                    int arg_34_0 = 0;
                    int num = bytes.Length - 1;
                    for (int i = arg_34_0; i <= num; i++)
                    {
                        if (bGroupChar)
                        {
                            if (i == 0)
                            {
                                text = text + sChar + "(" + Convert.ToString(bytes[i]);
                            }
                            else
                            {
                                text = text + "," + Convert.ToString(bytes[i]);
                            }
                        }
                        else if (i == 0)
                        {
                            text = string.Concat(new string[]
							{
								text,
								sChar,
								"(",
								Convert.ToString(bytes[i]),
								")"
							});
                        }
                        else
                        {
                            text = string.Concat(new string[]
							{
								text,
								sDelimiter,
								sChar,
								"(",
								Convert.ToString(bytes[i]),
								")"
							});
                        }
                    }
                    if (bGroupChar)
                    {
                        text += ")";
                    }
                    result = text;
                }
            }
            catch (Exception expr_11B)
            {
                ProjectData.SetProjectError(expr_11B);
                Exception ex = expr_11B;
                result = "ERROR: " + ex.Message;
                ProjectData.ClearProjectError();
            }
            return result;
        }
    }

    public string EncodeURL(string sData)
    {
        if (string.IsNullOrEmpty(sData))
        {
            return "";
        }
        sData = sData.Replace("  ", " ");
        return HttpUtility.UrlEncodeUnicode(sData);
    }

    public string DecodeURL(string sData)
    {
        if (string.IsNullOrEmpty(sData))
        {
            return "";
        }
        if (sData.ToLower().StartsWith("http://") | sData.ToLower().StartsWith("https://"))
        {
            return HttpUtility.UrlDecode(sData);
        }
        return HttpUtility.HtmlDecode(sData);
    }

    public string EncodeCookie(string sData, bool encodeEqualSign)
    {
        if (string.IsNullOrEmpty(sData))
        {
            return "";
        }
        sData = sData.Replace("%", "%25");
        sData = sData.Replace("#", "%23");
        sData = sData.Replace("+", "%2B");
        sData = sData.Replace(" ", "+");
        sData = sData.Replace("'", "%27");
        if (encodeEqualSign)
        {
            sData = sData.Replace("=", "%3D");
        }
        sData = sData.Replace("(", "%28");
        sData = sData.Replace(")", "%29");
        sData = sData.Replace(";", "%3b");
        return sData;
    }

    public Cookie GetAsCookie(string CookieString, Uri Uri)
    {
        string empty = string.Empty;
        string text = string.Empty;
        this.ParseParameter(CookieString, ref text, ref empty, '=');
        text = text.Replace("\n", string.Empty);
        text = text.Replace("\r", string.Empty);
        text = text.Replace("\t", string.Empty);
        text = text.TrimStart(new char[]
		{
			' '
		});
        return new Cookie(text, empty, "/", Uri.Host);
    }

    public void ParseParameter(string RawString, ref string Name, ref string Value, char Identifier = '=')
    {
        int num = RawString.IndexOf(Identifier);
        if (num == -1)
        {
            return;
        }
        Name = RawString.Substring(0, num);
        Value = checked(RawString.Substring(num + 1, RawString.Length - num - 1));
    }

    public char Char(int asciiCode)
    {
        return Conversions.ToChar(Conversions.ToChar(Conversions.ToString(asciiCode)).ToString());
    }

    public string GetNewGETUrl(string newQuery, string currentUri)
    {
        Uri uri = new Uri(currentUri);
        return uri.GetLeftPart(UriPartial.Path) + "?" + newQuery;
    }

    public bool IsNumeric(string valueToCheck)
    {
        bool result;
        try
        {
            int.Parse(valueToCheck);
            return true;
        }
        catch (Exception arg_09_0)
        {
            ProjectData.SetProjectError(arg_09_0);
            result = false;
            ProjectData.ClearProjectError();
        }
        return result;
    }

    public bool IsIpAddressValid(string ipAddress)
    {
        string pattern = "\\b(?:\\d{1,3}\\.){3}\\d{1,3}\\b";
        Regex regex = new Regex(pattern, RegexOptions.None);
        Match match = regex.Match(ipAddress);
        return match.Success;
    }

    public bool IsUrlValid(string sURL)
    {
        bool result = false;
        try
        {
            if (sURL.StartsWith("http://") | sURL.StartsWith("https://"))
            {
                result = true;
            }
        }
        catch (Exception arg_1F_0)
        {
            ProjectData.SetProjectError(arg_1F_0);
            ProjectData.ClearProjectError();
        }
        return result;
    }

    public bool IsDomain(string domain)
    {
        Regex regex = new Regex("^([a-zA-Z0-9]([a-zA-Z0-9\\-]{0,61}[a-zA-Z0-9])?\\.)+[a-zA-Z]{2,6}$");
        return regex.IsMatch(domain);
    }

    public bool IsIP(string ip)
    {
        Regex regex = new Regex("^(?:(?:25[0-5]|2[0-4]\\d|[01]\\d\\d|\\d?\\d)(?(\\.?\\d)\\.)){4}$");
        bool result;
        try
        {
            result = regex.IsMatch(ip);
        }
        catch (Exception expr_15)
        {
            ProjectData.SetProjectError(expr_15);
            result = false;
            ProjectData.ClearProjectError();
        }
        return result;
    }

    public string FormatBytes(double dblBytes)
    {
        if (dblBytes >= 1125899906842624.0)
        {
            return Conversions.ToString(Math.Round(dblBytes / 1125899906842624.0, 2)) + " PiB";
        }
        if (dblBytes >= 1099511627776.0)
        {
            return Conversions.ToString(Math.Round(dblBytes / 1099511627776.0, 2)) + " TiB";
        }
        if (dblBytes >= 1073741824.0)
        {
            return Conversions.ToString(Math.Round(dblBytes / 1073741824.0, 2)) + " GiB";
        }
        if (dblBytes >= 1048576.0)
        {
            return Conversions.ToString(Math.Round(dblBytes / 1048576.0, 2)) + " MiB";
        }
        if (dblBytes >= 1024.0)
        {
            return Conversions.ToString(Math.Round(dblBytes / 1024.0, 2)) + " KiB";
        }
        return Conversions.ToString(dblBytes) + " Bytes";
    }

    public string StrExtract(char cStart, char cEnd, string sData)
    {
        string result;
        try
        {
            int num = sData.IndexOf(cStart);
            int num2 = sData.IndexOf(cEnd);
            result = checked(sData.Substring(num + 1, num2 - num - 1));
        }
        catch (Exception arg_21_0)
        {
            ProjectData.SetProjectError(arg_21_0);
            result = "";
            ProjectData.ClearProjectError();
        }
        return result;
    }

    public string StrJoin(char sEnd, char cStart, string sData)
    {
        string result;
        try
        {
            int num = sData.IndexOf(sEnd);
            int num2 = sData.LastIndexOf(cStart);
            result = checked(sData.Substring(num + 1, num2 - num - 1));
        }
        catch (Exception arg_21_0)
        {
            ProjectData.SetProjectError(arg_21_0);
            result = "";
            ProjectData.ClearProjectError();
        }
        return result;
    }

    public IPAddress HostNameToIP(string sAddress)
    {
        checked
        {
            IPAddress result;
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(sAddress);
                IPAddress[] addressList = hostEntry.AddressList;
                for (int i = 0; i < addressList.Length; i++)
                {
                    IPAddress iPAddress = addressList[i];
                    if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        result = iPAddress;
                        return result;
                    }
                }
                result = null;
            }
            catch (Exception expr_3B)
            {
                ProjectData.SetProjectError(expr_3B);
                result = null;
                ProjectData.ClearProjectError();
            }
            return result;
        }
    }

    public string UrlToIP(string sURL)
    {
        try
        {
            string sAddress = "";
            bool flag = true;
            if (true == sURL.ToLower().StartsWith("http"))
            {
                string[] array = Regex.Split(sURL.Trim(), "/");
                if (!Information.IsNothing(array) && array.Length >= 2)
                {
                    sAddress = array[2];
                }
            }
            else if (flag == sURL.Contains(":"))
            {
                sAddress = sURL.Split(new char[]
				{
					':'
				})[0];
            }
            else
            {
                sAddress = sURL;
            }
            return global::Globals.G_Utilities.HostNameToIP(sAddress).ToString();
        }
        catch (Exception arg_7E_0)
        {
            ProjectData.SetProjectError(arg_7E_0);
            ProjectData.ClearProjectError();
        }
        return "";
    }
    public string getUseragent()
    {
        string[] ua = { "Mozilla/5.0 (X11; Linux i686) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.52 Safari/536.5",
                        "Mozilla/5.0 (Windows; U; Windows NT 5.1; it; rv:1.8.1.11) Gecko/20071127 Firefox/2.0.0.11",
                        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
                        "Mozilla/5.0 (compatible; Konqueror/3.5; Linux) KHTML/3.5.5 (like Gecko) (Kubuntu)",
                        "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.8.0.12) Gecko/20070731 Ubuntu/dapper-security Firefox/1.5.0.12",
                        "Mozilla/5.0 (iPad; U; CPU OS 3_2 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Version/4.0.4 Mobile/7B334b Safari/531.21.102011-10-16 20:23:50",
                        "Mozilla/5.0 (BlackBerry; U; BlackBerry 9800; en) AppleWebKit/534.1+ (KHTML, like Gecko) Version/6.0.0.337 Mobile Safari/534.1+2011-10-16 20:21:10",
                        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; en) Opera 8.0",
                        "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2066.0 Safari/537.36",
                        "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-GB; rv:1.8.1.6) Gecko/20070725 Firefox/2.0.0.6",
                        "Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Ubuntu/10.10 Chromium/8.0.552.237",
                        "Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.7 (KHTML, like Gecko) Ubuntu/11.10 Chromium/16.0.912.21",
                        "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.2 (KHTML, like Gecko) Ubuntu/11.04 Chromium/15.0.871.0 ",
                        "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.20 (KHTML, like Gecko) Chrome/11.0.672.2 Safari/534.20",
                        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.45 Safari/535.19",
                        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.93 Safari/537.36",
                        "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.1 (KHTML, like Gecko) Comodo_Dragon/14.0.1.0 Chrome/14.0.835.163 Safari/535.1",
                        "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.105 CoRom/30.0.1599.105 Safari/537.36",
                        "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2049.0 Safari/537.36",
                        "Mozilla/5.0 (IE 11.0; Windows NT 6.3; WOW64; Trident/7.0; Touch; rv:11.0) like Gecko",
                        "Mozilla/5.0 (Linux; U; Android 4.2; en-us; Nexus 10 Build/JVP15I) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Safari/534.30",
                        "Opera/10.61 (J2ME/MIDP; Opera Mini/5.1.21219/19.999; en-US; rv:1.9.3a5) WebKit/534.5 Presto/2.6.30",
                        "Mozilla/5.0 (Windows NT 5.1; rv:31.0) Gecko/20100101 Firefox/31.0",
                        "Mozilla/5.0 (Linux; U; Android 2.3.4; en-US; MT11i Build/4.0.2.A.0.62) AppleWebKit/534.31 (KHTML, like Gecko) UCBrowser/9.0.1.275 U3/0.8.0 Mobile Safari/534.31",
                        "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_6; en-US) AppleWebKit/534.7 (KHTML, like Gecko) Flock/3.5.3.4628 Chrome/7.0.517.450 Safari/534.7",
                        "Mozilla/5.0 (Windows NT 6.1; rv:26.0) Gecko/20100101 Firefox/26.0 IceDragon/26.0.0.2"
        };
        Random rand = new Random();
        int i = rand.Next(0, ua.Length - 1);
        string ua2 = ua[i];
        return ua2;
    }
    
}

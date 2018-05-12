using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChahBot_1_0_Gacy.Src;


public class RegExp : IDisposable
{
    private bool __Disposed;

    protected virtual void Dispose(bool b)
    {
        if (this.__Disposed)
        {
        }
        this.__Disposed = true;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public string Replace(string sPattern, string sInput, string sValue)
    {
        Regex regex = new Regex(sPattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        regex.Match(sInput);
        string text = regex.Replace(sInput, sValue);
        if (string.IsNullOrEmpty(text))
        {
            return sInput;
        }
        return text;
    }

    public Hashtable Match(string sPattern, string sData, RegexOptions o)
    {
        Hashtable hashtable = new Hashtable();
        Regex regex = new Regex(sPattern, o);
        Match match = regex.Match(sData);
        while (match.Success)
        {
            RegExpResult regExpResult = new RegExpResult();
            regExpResult.Index = match.Index;
            regExpResult.Value = match.Groups[1].Value;
            hashtable.Add(match.Index.ToString(), regExpResult);
            match = match.NextMatch();
        }
        return hashtable;
    }

    public Hashtable GetData(string sData, string sRegExPattern)
    {
        Hashtable hashtable = this.Match(sRegExPattern, sData, RegexOptions.IgnoreCase);
        Hashtable hashtable2 = new Hashtable();
        if (hashtable.Count == 0)
        {
            return hashtable2;
        }
        try
        {
            IEnumerator enumerator = hashtable.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                RegExpResult regExpResult = (RegExpResult)enumerator.Current;
                hashtable2.Add(hashtable2.Count.ToString(), regExpResult.Value);
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
        return hashtable2;
    }

    public string GetItemData(string sData, string sPattern, string sDefautValue = "")
    {
        Regex regex = new Regex(sPattern, RegexOptions.IgnoreCase);
        Match match = regex.Match(sData);
        while (match.Success)
        {
            if (!string.IsNullOrEmpty(match.Groups[1].Value))
            {
                return match.Groups[1].Value;
            }
            match = match.NextMatch();
        }
        return sDefautValue;
    }

    public Hashtable SplitData(string sData, string sFind)
    {
        Hashtable hashtable = new Hashtable();
        string[] array = Regex.Split(sData, sFind);
        checked
        {
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (!hashtable.Contains(text))
                {
                    hashtable.Add(text, text);
                }
            }
            return hashtable;
        }
    }

    public Hashtable GetLinks(string sUrl, string sData, string sRegExPattern)
    {
        Hashtable hashtable = this.Match(sRegExPattern, sData, RegexOptions.IgnoreCase);
        Hashtable hashtable2 = new Hashtable();
        if (hashtable.Count == 0)
        {
            return hashtable2;
        }
        if (string.IsNullOrEmpty(sUrl))
        {
            try
            {
                IEnumerator enumerator = hashtable.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    RegExpResult regExpResult = (RegExpResult)enumerator.Current;
                    hashtable2.Add(hashtable2.Count.ToString(), regExpResult.Value);
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
            return hashtable2;
        }
        string[] array = Regex.Split(sUrl, "/");
        string str = array[0] + "//" + array[2];
        try
        {
            IEnumerator enumerator2 = hashtable.Values.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                RegExpResult regExpResult2 = (RegExpResult)enumerator2.Current;
                if (regExpResult2.Value.StartsWith("http://"))
                {
                    hashtable2.Add(hashtable2.Count.ToString(), new Link(regExpResult2.Value, regExpResult2.Value, regExpResult2));
                }
                else
                {
                    if (regExpResult2.Value.StartsWith("./"))
                    {
                        regExpResult2.Value = regExpResult2.Value.Replace("./", "/");
                    }
                    if (regExpResult2.Value.StartsWith("/") | regExpResult2.Value.StartsWith("./"))
                    {
                        hashtable2.Add(hashtable2.Count.ToString(), new Link(regExpResult2.Value, str + regExpResult2.Value, regExpResult2));
                    }
                    else
                    {
                        hashtable2.Add(hashtable2.Count.ToString(), new Link(regExpResult2.Value, str + "/" + regExpResult2.Value, regExpResult2));
                    }
                }
            }
        }
        finally
        {
            IEnumerator enumerator2 = null;
            if (enumerator2 is IDisposable)
            {
                (enumerator2 as IDisposable).Dispose();
            }
        }
        return hashtable2;
    }

}

public class RegExpResult
{
    private int __Index;

    private string __Value;

    public int Index
    {
        get
        {
            return this.__Index;
        }
        set
        {
            this.__Index = value;
        }
    }

    public string Value
    {
        get
        {
            return this.__Value;
        }
        set
        {
            this.__Value = value;
        }
    }
}


public class Link
{
    private string __UrlSite;

    private string __Url;

    private RegExpResult __Result;

    public string UrlSite
    {
        get
        {
            return this.__UrlSite;
        }
        set
        {
            this.__UrlSite = value;
        }
    }

    public string Url
    {
        get
        {
            return this.__Url;
        }
        set
        {
            this.__Url = value;
        }
    }

    public RegExpResult Result
    {
        get
        {
            return this.__Result;
        }
        set
        {
            this.__Result = value;
        }
    }

    public Link(string sUrlSite, string sUrl, RegExpResult oResult)
    {
        this.__UrlSite = sUrlSite;
        this.__Url = sUrl;
        this.__Result = oResult;
    }
}

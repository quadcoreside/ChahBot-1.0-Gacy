using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.IO;
using System.Xml;

public class cXML : IDisposable
{
    private string m_strFilePath;

    private string m_strRootName;

    private char m_crSectionSeparator;

    private XmlDocument m_objXMLDoc;

    private bool disposedValue;

    public XmlDocument XmlDocument
    {
        get
        {
            return this.m_objXMLDoc;
        }
    }

    public string FilePath
    {
        get
        {
            return this.m_strFilePath;
        }
        set
        {
            this.m_strFilePath = value.Trim();
        }
    }

    public string RootName
    {
        get
        {
            return this.m_strRootName;
        }
        set
        {
            this.m_strRootName = value.Trim();
        }
    }

    public char SectionSeparator
    {
        get
        {
            return this.m_crSectionSeparator;
        }
        set
        {
            this.m_crSectionSeparator = value;
        }
    }

    public cXML(string sPath, string sRootName = "SQLi_Dumper", char cSectionSeparator = ';')
    {
        this.disposedValue = false;
        this.m_strFilePath = sPath.Trim();
        this.m_strRootName = sRootName.Trim();
        this.m_crSectionSeparator = cSectionSeparator;
        this.OpenFile();
    }

    /*protected override void Finalize()
    {
        this.m_objXMLDoc = null;
        base.Finalize();
    }*/

    private void OpenFile()
    {
        if (this.m_strFilePath.ToLower().StartsWith("http"))
        {
            try
            {
                XmlTextReader xmlTextReader = new XmlTextReader(this.m_strFilePath);
                this.m_objXMLDoc = new XmlDocument();
                this.m_objXMLDoc.Load(xmlTextReader);
                xmlTextReader.Close();
                return;
            }
            catch (Exception expr_47)
            {
                ProjectData.SetProjectError(expr_47);
                ProjectData.ClearProjectError();
                return;
            }
        }
        try
        {
            if (File.Exists(this.m_strFilePath))
            {
                XmlTextReader xmlTextReader2 = new XmlTextReader(this.m_strFilePath);
                this.m_objXMLDoc = new XmlDocument();
                this.m_objXMLDoc.Load(xmlTextReader2);
                xmlTextReader2.Close();
            }
        }
        catch (XmlException expr_92)
        {
            ProjectData.SetProjectError(expr_92);
            ProjectData.ClearProjectError();
        }
        finally
        {
            if (this.m_objXMLDoc == null)
            {
                this.m_objXMLDoc = new XmlDocument();
                this.m_objXMLDoc.LoadXml(string.Concat(new string[]
				{
					"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<",
					this.m_strRootName,
					">\r\n</",
					this.m_strRootName,
					">"
				}));
            }
        }
    }

    public string GetAttribute(string sTagName, string sKey, string sDefaut = "")
    {
        string text = "";
        try
        {
            XmlNodeList elementsByTagName = this.m_objXMLDoc.GetElementsByTagName(sTagName);
            if (elementsByTagName.Count > 0)
            {
                text = elementsByTagName[0].Attributes[sKey].Value;
                if (Versioned.IsNumeric(text))
                {
                    text = Strings.FormatNumber(Conversions.ToDouble(text), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault).ToString();
                }
            }
            if (string.IsNullOrEmpty(text))
            {
                text = sDefaut;
            }
        }
        catch (Exception expr_65)
        {
            ProjectData.SetProjectError(expr_65);
            ProjectData.ClearProjectError();
        }
        return text;
    }

    internal Hashtable GetAllSections()
    {
        Hashtable hashtable = new Hashtable();
        XmlNodeList xmlNodeList = this.m_objXMLDoc.SelectNodes("//Section");
        if (xmlNodeList != null)
        {
            try
            {
                IEnumerator enumerator = xmlNodeList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    XmlNode xmlNode = (XmlNode)enumerator.Current;
                    hashtable.Add(xmlNode.Attributes["Name"].Value, xmlNode);
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
        }
        return hashtable;
    }

    internal Hashtable GetAllSettings(string sSections)
    {
        Hashtable hashtable = new Hashtable();
        string[] array;
        if (Strings.InStr(sSections, Conversions.ToString(this.m_crSectionSeparator), CompareMethod.Binary) != 0)
        {
            array = Strings.Split(sSections, Conversions.ToString(this.m_crSectionSeparator), -1, CompareMethod.Binary);
        }
        else
        {
            array = new string[]
			{
				sSections
			};
        }
        checked
        {
            try
            {
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text = array2[i];
                    XmlNode xmlNode = this.m_objXMLDoc.SelectSingleNode("//Section[@Name='" + text + "']");
                    if (xmlNode != null)
                    {
                        XmlNodeList xmlNodeList = xmlNode.SelectNodes("descendant::Key");
                        if (xmlNodeList != null)
                        {
                            try
                            {
                                IEnumerator enumerator = xmlNodeList.GetEnumerator();
                                while (enumerator.MoveNext())
                                {
                                    XmlNode xmlNode2 = (XmlNode)enumerator.Current;
                                    XMLTag xMLTag = new XMLTag();
                                    xMLTag.Name = xmlNode2.Attributes["Name"].Value;
                                    xMLTag.Value = xmlNode2.Attributes["Value"].Value;
                                    xMLTag.Section = text;
                                    hashtable.Add(xmlNode2.Attributes["Name"].Value, xMLTag);
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
                        }
                    }
                }
            }
            catch (Exception expr_140)
            {
                ProjectData.SetProjectError(expr_140);
                ProjectData.ClearProjectError();
            }
            return hashtable;
        }
    }

    internal string GetSetting(string sSection, string sKey, string sDefValue)
    {
        string setting = this.GetSetting(sSection, sKey);
        if (string.IsNullOrEmpty(setting))
        {
            return sDefValue;
        }
        return setting;
    }

    internal string GetSetting(string sSection, string sKey)
    {
        string result = string.Empty;
        XmlNode xmlNode = this.m_objXMLDoc.SelectSingleNode("//Section[@Name='" + sSection + "']");
        if (xmlNode != null)
        {
            XmlNode xmlNode2 = xmlNode.SelectSingleNode("descendant::Key[@Name='" + sKey + "']");
            if (xmlNode2 != null)
            {
                result = xmlNode2.Attributes["Value"].Value;
            }
        }
        return result;
    }

    internal void SaveSetting(string sSection, string sKeyName, string sValue)
    {
        if (this.m_objXMLDoc.DocumentElement == null)
        {
            this.m_objXMLDoc.LoadXml(string.Concat(new string[]
			{
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<",
				this.m_strRootName,
				">\r\n</",
				this.m_strRootName,
				">"
			}));
            return;
        }
        XmlNode xmlNode = this.m_objXMLDoc.SelectSingleNode("//Section[@Name='" + sSection + "']");
        if (xmlNode == null)
        {
            try
            {
                xmlNode = this.m_objXMLDoc.CreateNode(XmlNodeType.Element, "Section", "");
                XmlAttribute xmlAttribute = this.m_objXMLDoc.CreateAttribute("Name");
                xmlAttribute.Value = sSection;
                xmlNode.Attributes.SetNamedItem(xmlAttribute);
                XmlNode documentElement = this.m_objXMLDoc.DocumentElement;
                documentElement.AppendChild(xmlNode);
            }
            catch (XmlException expr_CA)
            {
                ProjectData.SetProjectError(expr_CA);
                ProjectData.ClearProjectError();
            }
        }
        XmlNode xmlNode2 = xmlNode.SelectSingleNode("descendant::Key[@Name='" + sKeyName + "']");
        if (xmlNode2 == null)
        {
            try
            {
                xmlNode2 = this.m_objXMLDoc.CreateNode(XmlNodeType.Element, "Key", "");
                XmlAttribute xmlAttribute = this.m_objXMLDoc.CreateAttribute("Name");
                xmlAttribute.Value = sKeyName;
                xmlNode2.Attributes.SetNamedItem(xmlAttribute);
                xmlAttribute = this.m_objXMLDoc.CreateAttribute("Value");
                xmlAttribute.Value = sValue;
                xmlNode2.Attributes.SetNamedItem(xmlAttribute);
                xmlNode.AppendChild(xmlNode2);
                goto IL_181;
            }
            catch (XmlException expr_15D)
            {
                ProjectData.SetProjectError(expr_15D);
                ProjectData.ClearProjectError();
                goto IL_181;
            }
        }
        xmlNode2.Attributes["Value"].Value = sValue;
    IL_181:
        xmlNode = null;
    }

    internal void DeleteKey(string sSection, string sKeyName)
    {
        XmlNode xmlNode = this.m_objXMLDoc.SelectSingleNode("//Section[@Name='" + sSection + "']");
        if (xmlNode != null)
        {
            XmlNode xmlNode2 = xmlNode.SelectSingleNode("descendant::Key[@Name='" + sKeyName + "']");
            if (xmlNode2 != null)
            {
                xmlNode.RemoveChild(xmlNode2);
            }
        }
    }

    internal void DeleteSection(string sSection)
    {
        XmlNode xmlNode = this.m_objXMLDoc.SelectSingleNode("//Section[@Name='" + sSection + "']");
        if (xmlNode != null)
        {
            XmlNode documentElement = this.m_objXMLDoc.DocumentElement;
            documentElement.RemoveChild(xmlNode);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.disposedValue)
        {
        }
        this.disposedValue = true;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}

public class XMLTag
{
	private string __Name;

	private string __Value;

	private string __Section;

	internal string Name
	{
		get
		{
			return this.__Name;
		}
		set
		{
			this.__Name = value;
		}
	}

	internal string Value
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

	internal string Section
	{
		get
		{
			return this.__Section;
		}
		set
		{
			this.__Section = value;
		}
	}
}

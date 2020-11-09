using System.Collections;
using UnityEngine;
using System.Xml;
using System;

public class LanguageReader : MonoBehaviour
{
    public static LanguageReader Instance;

    private Hashtable XML_Strings;

    private string currentLanguage;

    public string defaultLanguage = "FRA"; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            OpenLocalXML(defaultLanguage);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void OpenLocalXML(string language)
    {
        TextAsset xmlFile;

        if ((xmlFile = Resources.Load("Langages/" + language) as TextAsset) != null)
            SetLocalLanguage(xmlFile.text, language);
        else
        {
            xmlFile = Resources.Load("Langages/" + defaultLanguage) as TextAsset;
            SetLocalLanguage(xmlFile.text, defaultLanguage);
        }
    }

    public void SelectLanguage(string language)
    {
        if (language != currentLanguage)
        {
            currentLanguage = language;
            OpenLocalXML(language);
        }
    }

    public string GetString(string _name)
    {
        if (!XML_Strings.ContainsKey(_name))
        {
            Debug.LogWarning("This string is not present in the XML file where you're reading: " + _name);
            return "";
        }
        return (string)XML_Strings[_name];
    }

    public void SetLocalLanguage(string xmlContent, string language)
    {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(xmlContent);
        XML_Strings = new Hashtable();
        XmlElement element = xml.DocumentElement[language];
        if (element != null)
        {
            var elemEnum = element.GetEnumerator();

            while (elemEnum.MoveNext())
            {
                XML_Strings.Add((elemEnum.Current as XmlElement).GetAttribute("name"), (elemEnum.Current as XmlElement).InnerText.Replace(@"\n", Environment.NewLine));
            }
        }
        else
        {
            Debug.LogError("The specified language does not exist: " + language);
        }
    }

    
}
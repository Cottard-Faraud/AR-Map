using System;
using System.Collections;
using System.Xml;
using UnityEngine;

public class BoutiqueReader : MonoBehaviour
{
    public static BoutiqueReader Instance;

    private Hashtable XML_Strings;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void OpenLocalXML(string boutique_path)
    {
        TextAsset textAsset;
        if ((textAsset = Resources.Load("Boutiques/" + boutique_path) as TextAsset) != null)
        {
            SetLocalBoutique(textAsset.text, boutique_path);
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

    public void SetLocalBoutique(string xmlContent, string language)
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
            Debug.LogError("The specified shop does not exist: " + language);
        }
    }

    
}

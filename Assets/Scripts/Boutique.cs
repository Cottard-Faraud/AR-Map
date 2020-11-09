using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boutique : MonoBehaviour
{
    private Variables variables;
    private Etage etage;

    private GameObject logo3D;

    public string xml_path;

    private GameObject entreeBoutique;

    private BoutiqueType boutiqueType;

    private MeshRenderer mr;

    private void Start()
    {
        variables = Variables.Instance;

        if (transform.childCount > 0)
            entreeBoutique = transform.GetChild(0).gameObject;

        mr = this.GetComponent<MeshRenderer>();
        SetIddleMaterial();
    }
    
    public void OnMouseDown()
    {
        if (IsPointerOverUIObject())
            return;
        etage.SelectedBoutique(this);
    }

    public BoutiqueInfos GetBoutiqueInfos()
    {
        return new BoutiqueInfos(this);
    }

    public void SetEtage(Etage e)
    {
        etage = e;
    }

    public void SetIddleMaterial()
    {
        mr.material = variables.iddleMaterial;
    }

    public void SetSelectedMaterial()
    {
        mr.material = variables.selectedMaterial;
    }

    public GameObject GetEntreeBoutique()
    {
        return entreeBoutique;
    }

    public void ActiverLogo(bool value)
    {
        if (value)
        {
            if (logo3D == null)
            {
                logo3D = Instantiate(variables.logo3DPrefab);
                logo3D.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                logo3D.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Logos/Materials/" + GetBoutiqueInfos().GetNomBoutique().ToLower().Replace(" ", "_"));
                logo3D.GetComponent<MeshRenderer>().materials[1].color = GetBoutiqueInfos().GetBoutiqueType().boutiqueColor;
                logo3D.transform.SetParent(etage.transform);
                StartCoroutine(LogoTranslate());
            }
        }
        else
        {
            if (logo3D != null)
                Destroy(logo3D);
        }
    }

    IEnumerator LogoTranslate()
    {
        while (logo3D != null && logo3D.transform.position != new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z))
        {
            logo3D.transform.position = Vector3.MoveTowards(logo3D.transform.position, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), 0.1f);
            yield return null;
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}

public class BoutiqueInfos
{
    private string nom_boutique;
    private string logo_boutique;
    private string horaires_boutique;
    private string telephone_boutique;
    private string site_boutique;
    private string contenu_boutique;

    private BoutiqueType boutiqueType;

    private BoutiqueReader boutiqueReader;

    public BoutiqueInfos(Boutique boutique)
    {
        if (boutiqueReader == null)
            boutiqueReader = BoutiqueReader.Instance;

        boutiqueReader.OpenLocalXML(boutique.xml_path);

        nom_boutique = boutiqueReader.GetString("Nom_boutique");
        logo_boutique = boutiqueReader.GetString("Logo_boutique");
        horaires_boutique = boutiqueReader.GetString("Horaires_boutique");
        telephone_boutique = boutiqueReader.GetString("Telephone_boutique");
        site_boutique = boutiqueReader.GetString("Site_boutique");
        contenu_boutique = boutiqueReader.GetString("Contenu_boutique");

        boutiqueType = new BoutiqueType(boutiqueReader.GetString("Type_boutique"));
    }

    public string GetNomBoutique()
    {
        return nom_boutique;
    }

    public string GetLogoBoutique()
    {
        return logo_boutique;
    }

    public string GetHorairesBoutique()
    {
        return horaires_boutique;
    }

    public string GetTelephoneBoutique()
    {
        return telephone_boutique;
    }

    public string GetSiteBoutique()
    {
        return site_boutique;
    }

    public string GetContenuBoutique()
    {
        return contenu_boutique;
    }

    public BoutiqueType GetBoutiqueType()
    {
        return boutiqueType;
    }

    public void UpdateLanguage()
    {
        boutiqueType.UpdateBoutiqueTypeName();
    }
}

public class BoutiqueType
{
    public string boutiqueTypeName;
    private string boutiqueTypeCode;
    public Color boutiqueColor;

    public BoutiqueType(string s)
    {
        switch (s)
        {
            case "Alimentaire":
                boutiqueTypeCode = "Boutique_Type1";
                boutiqueColor = Color.red;
                break;
            case "Vestimentaire":
                boutiqueTypeCode = "Boutique_Type2";
                boutiqueColor = Color.blue;
                break;
            case "Cosmetique":
                boutiqueTypeCode = "Boutique_Type3";
                boutiqueColor = Color.yellow;
                break;
            case "Loisir":
                boutiqueTypeCode = "Boutique_Type4";
                boutiqueColor = Color.green;
                break;
            case "Autre":
                boutiqueTypeCode = "Boutique_Type5";
                boutiqueColor = Color.black;
                break;
        }
        boutiqueTypeName = LanguageReader.Instance.GetString(boutiqueTypeCode);
    }

    public string GetBoutiqueTypeCode()
    {
        return boutiqueTypeCode;
    }

    public void UpdateBoutiqueTypeName()
    {
        boutiqueTypeName = LanguageReader.Instance.GetString(boutiqueTypeCode);
    }
}

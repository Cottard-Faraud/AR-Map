using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Batiment batiment;

    [Header("Panneau Menu")]
    public GameObject menuDefaut;

    public GameObject panneauMenu;
    public GameObject boutiqueSelectionnee;
    public GameObject boutiqueNonSelectionnee;

    public Text texte_nom;
    public Image image_logo;
    public Text texte_horaires;
    public Text texte_telephone;
    public Text texte_site;
    public Text texte_contenu;

    public Button boutonType;

    [Header("Panneau Recherche")]
    public GameObject rechercheTitre;
    public GameObject panneauRecherche;
    public InputField inputfield;

    public GameObject parentBoutons;
    public GameObject boutonPrefab;

    public GameObject bouton1;
    public GameObject bouton2;
    public GameObject bouton3;
    public GameObject bouton4;
    public GameObject bouton5;

    private List<GameObject> inputfieldBoutons;

    [Header("Panneau Parametres")]
    public GameObject parametresTitre;
    public GameObject panneauParametres;

    private Animator animator;

    private Boutique boutiqueActuelle;

    private bool panelOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        batiment = Batiment.Instance;
        animator = this.GetComponent<Animator>();

        inputfieldBoutons = new List<GameObject>();
    }

    public void CacherInterface()
    {
        panneauMenu.SetActive(false);
        panneauRecherche.SetActive(false);
        panneauParametres.SetActive(false);
    }

    public void Panneau()
    {
        ChangerEtatMenu();

        if (panelOpen)
            AfficherMenuPrincipal();
    }

    public void Reset()
    {
        boutiqueActuelle = null;
        AfficherListeType("");
        boutiqueSelectionnee.SetActive(false);
        boutiqueNonSelectionnee.SetActive(true);
    }

    public void ChangerEtatMenu()
    {
        animator.SetTrigger("ChangeState");
        panelOpen = !panelOpen;
    }

    public void AfficherMenuPrincipal()
    {
        if (!panelOpen)
            ChangerEtatMenu();

        panneauMenu.SetActive(true);
        panneauRecherche.SetActive(false);
        panneauParametres.SetActive(false);

        if (boutiqueActuelle == null)
        {
            boutiqueSelectionnee.SetActive(false);
            boutiqueNonSelectionnee.SetActive(true);
        }
        else
        {
            boutiqueSelectionnee.SetActive(true);
            boutiqueNonSelectionnee.SetActive(false);
        }
    }

    public void AfficherRecherche()
    {
        if (!panelOpen)
            ChangerEtatMenu();

        panneauMenu.SetActive(false);
        panneauRecherche.SetActive(true);
        panneauParametres.SetActive(false);

        //AfficherListeBoutiques();
        AfficherListeType("");
    }

    public void AfficherParametres()
    {
        if (!panelOpen)
            ChangerEtatMenu();

        panneauMenu.SetActive(false);
        panneauRecherche.SetActive(false);
        panneauParametres.SetActive(true);
    }

    public void AfficherBoutique(Boutique b)
    {
        boutiqueActuelle = b;
        AfficherMenuPrincipal();

        if (b != null)
        {
            if (!panelOpen)
                ChangerEtatMenu();

            boutiqueSelectionnee.SetActive(true);
            boutiqueNonSelectionnee.SetActive(false);

            BoutiqueInfos bi = b.GetBoutiqueInfos();
            BoutiqueType bt = bi.GetBoutiqueType();

            texte_nom.text = bi.GetNomBoutique(); ;

            image_logo.sprite = Resources.Load<Sprite>(bi.GetLogoBoutique());

            texte_horaires.text = bi.GetHorairesBoutique();
            texte_telephone.text = bi.GetTelephoneBoutique();
            texte_site.text = bi.GetSiteBoutique();
            texte_contenu.text = bi.GetContenuBoutique();

            boutonType.GetComponent<Image>().color = bt.boutiqueColor;
            boutonType.transform.GetChild(0).GetComponent<Text>().text = bt.boutiqueTypeName;

            boutonType.GetComponent<Button>().onClick.RemoveAllListeners();
            boutonType.GetComponent<Button>().onClick.AddListener(delegate { AfficherListeType(bt.GetBoutiqueTypeCode()); });
        }
    }
    /*
    public void TextFieldValue()
    {
        AfficherListeBoutiques();
    }

    
    public void AfficherListeBoutiques()
    {
        for (int i = inputfieldBoutons.Count - 1; i >= 0; i--)
        {
            Destroy(inputfieldBoutons[i]);
        }

        foreach (Boutique boutique in RechercheBoutique(inputfield.text))
        {
            GameObject bouton = Instantiate(boutonPrefab);
            bouton.transform.SetParent(parentBoutons.transform);
            bouton.transform.GetChild(0).GetComponent<Text>().text = boutique.GetBoutiqueInfos().GetNomBoutique();

            bouton.GetComponent<Button>().onClick.AddListener(delegate { SelectionBoutique(boutique); });

            inputfieldBoutons.Add(bouton);
        }
    }*/

    public void AfficherListeType(string boutiqueTypeCode)
    {
        CouleurBoutons(boutiqueTypeCode);
        for (int i = inputfieldBoutons.Count - 1; i >= 0; i--)
        {
            Destroy(inputfieldBoutons[i]);
        }

        List<Boutique> boutiques = new List<Boutique>();
        if (boutiqueTypeCode == "")
        {
            if (batiment.EtageActuel() != null)
            {
                boutiques = batiment.EtageActuel().GetListBoutiques();
            }
            else
                boutiques.Clear();
        }
        else
            boutiques = RechercheTypeBoutique(boutiqueTypeCode);

        foreach (Boutique boutique in boutiques)
        {
            GameObject bouton = Instantiate(boutonPrefab);
            bouton.transform.SetParent(parentBoutons.transform);
            bouton.transform.GetChild(0).GetComponent<Text>().text = boutique.GetBoutiqueInfos().GetNomBoutique();

            bouton.GetComponent<Button>().onClick.AddListener(delegate { SelectionBoutique(boutique); });

            inputfieldBoutons.Add(bouton);
        }

        if (batiment.EtageActuel() != null)
            batiment.EtageActuel().SelectedType(boutiqueTypeCode);
    }

    public void CouleurBoutons(string str)
    {
        bouton1.GetComponent<Image>().color = Color.white;
        bouton2.GetComponent<Image>().color = Color.white;
        bouton3.GetComponent<Image>().color = Color.white;
        bouton4.GetComponent<Image>().color = Color.white;
        bouton5.GetComponent<Image>().color = Color.white;

        switch (str)
        {
            case "Boutique_Type1":
                bouton1.GetComponent<Image>().color = Color.gray;
                break;
            case "Boutique_Type2":
                bouton2.GetComponent<Image>().color = Color.gray;
                break;
            case "Boutique_Type3":
                bouton3.GetComponent<Image>().color = Color.gray;
                break;
            case "Boutique_Type4":
                bouton4.GetComponent<Image>().color = Color.gray;
                break;
            case "Boutique_Type5":
                bouton5.GetComponent<Image>().color = Color.gray;
                break;
        }
    }

    public void SelectionBoutique(Boutique boutique)
    {
        batiment.EtageActuel().SelectedBoutique(boutique);
    }

    public List<Boutique> RechercheBoutique(string str)
    {
        List<Boutique> listeTemp = new List<Boutique>();
        List<Boutique> listeBoutiques = new List<Boutique>(); ;

        if (batiment.EtageActuel() != null)
        {
            listeBoutiques = batiment.EtageActuel().GetListBoutiques();

            foreach (Boutique b in listeBoutiques)
            {
                if (b.GetBoutiqueInfos().GetNomBoutique().ToUpper().StartsWith(str.ToUpper()))
                {
                    listeTemp.Add(b);
                }
            }

            listeTemp.Sort((Boutique b1, Boutique b2) => b1.GetBoutiqueInfos().GetNomBoutique().CompareTo(b2.GetBoutiqueInfos().GetNomBoutique()));
        }

        return listeTemp;
    }

    public List<Boutique> RechercheTypeBoutique(string boutiqueTypeCode)
    {
        List<Boutique> listeTemp = new List<Boutique>();
        List<Boutique> listeBoutiques = new List<Boutique>(); ;

        if (batiment.EtageActuel() != null)
        {
            listeBoutiques = batiment.EtageActuel().GetListBoutiques();

            foreach (Boutique b in listeBoutiques)
            {
                if (b.GetBoutiqueInfos().GetBoutiqueType().GetBoutiqueTypeCode() == boutiqueTypeCode)
                {
                    listeTemp.Add(b);
                }
            }

            listeTemp.Sort((Boutique b1, Boutique b2) => b1.GetBoutiqueInfos().GetNomBoutique().CompareTo(b2.GetBoutiqueInfos().GetNomBoutique()));
        }

        return listeTemp;
    }

    public void ChangerLangue(string langue)
    {
        LanguageReader.Instance.SelectLanguage(langue);
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        if (boutiqueActuelle != null)
            boutonType.transform.GetChild(0).GetComponent<Text>().text = boutiqueActuelle.GetBoutiqueInfos().GetBoutiqueType().boutiqueTypeName;

        menuDefaut.GetComponent<Text>().text = LanguageReader.Instance.GetString("Menu_Boutique_Defaut");
        rechercheTitre.GetComponent<Text>().text = LanguageReader.Instance.GetString("Menu_Recherche_Titre");
        parametresTitre.GetComponent<Text>().text = LanguageReader.Instance.GetString("Menu_Parametre_Titre1");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Batiment : MonoBehaviour
{
    public static Batiment Instance;

    private UIManager uiManager;

    private List<Etage> listeEtages;

    public GameObject nom_etage;

    private Etage currentEtage;
    private Etage lastEtageDisplays;
    private Etage lastEtageScan;

    public float maxTimeWaiting = 60.0f;
    private bool waitIsOver = true;

    public GameObject positionUp;
    public GameObject positionDown;

    public GameObject menu;

    private bool deplacementTermine = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        uiManager = UIManager.Instance;

        currentEtage = null;
        listeEtages = new List<Etage>();
        Etage etage;
        foreach (Transform t in this.transform)
        {
            if (t.gameObject.GetComponent<Etage>())
            {
                etage = t.gameObject.GetComponent<Etage>();
                listeEtages.Add(etage);
                etage.SetBatiment(this);
            }
        }
        AfficherInterface(false);
        AfficherEtages(false);
    }

    public Etage EtageActuel()
    {
        return currentEtage;
    }

    public void AfficherInterface(bool status)
    {
        menu.SetActive(status);
    }

    public void AfficherEtages(bool status)
    {
        foreach (Etage e in listeEtages)
            e.gameObject.SetActive(false);
    }

    public void AfficherEtage(Etage etage)
    {
        AfficherInterface(true);
        nom_etage.GetComponent<Text>().text = etage.nom_etage;

        foreach (Etage e in listeEtages)
        {
            if (e == etage)
                e.gameObject.SetActive(true);
            else
                e.gameObject.SetActive(false);
        }
        uiManager.AfficherListeType("");
    }

    //Etage changé avec un click
    public void ChangeEtage(string str)
    {
        int number;
        if (deplacementTermine)
        {
            switch (str)
            {
                case "up":
                    number = FindElement(currentEtage) + 1;
                    if (number < listeEtages.Count)
                    {
                        ResetInfos();
                        StartCoroutine(TransitionEtage(listeEtages[number], currentEtage, positionUp, positionDown));
                        currentEtage = listeEtages[number];
                        lastEtageDisplays = currentEtage;
                        nom_etage.GetComponent<Text>().text = currentEtage.nom_etage;
                    }
                    break;
                case "down":
                    number = FindElement(currentEtage) - 1;
                    if (number >= 0)
                    {
                        ResetInfos();
                        StartCoroutine(TransitionEtage(listeEtages[number], currentEtage, positionDown, positionUp));
                        currentEtage = listeEtages[number];
                        lastEtageDisplays = currentEtage;
                        nom_etage.GetComponent<Text>().text = currentEtage.nom_etage;
                    }
                    break;
            }
        }
        uiManager.AfficherListeType("");
    }

    //Etage detecté par Vuforia
    public void EtageTrouve(GameObject etage)
    {
        Etage eTemp = etage.GetComponent<Etage>();

        if (eTemp != lastEtageScan || waitIsOver)
        {
            currentEtage = eTemp;
            lastEtageDisplays = currentEtage;
        }
        else if (currentEtage == null)
        {
            currentEtage = lastEtageDisplays;
        }
        
        lastEtageScan = eTemp;
        AfficherEtage(currentEtage);
    }

    //Etage perdu par Vuforia
    public void EtagePerdu(GameObject etage)
    {
        if (uiManager == null)
            return;
        AfficherInterface(false);
        if (etage.GetComponent<Etage>() == currentEtage)
        {
            etage.gameObject.SetActive(false);
        }
        currentEtage = null;
        StartCoroutine(Waiting());
        uiManager.AfficherListeType("");
        PathFindingManager.Instance.ResetLineRenderer();
    }

    public void ResetInfos()
    {
        PathFindingManager.Instance.ResetLineRenderer();
        currentEtage.ResetSelectedBoutique();
    }

    public int FindElement(Etage etage)
    {
        if (etage != null)
        {
            int cpt = 0;
            foreach (Etage e in listeEtages)
            {
                if (e == etage)
                    return cpt;
                cpt++;
            }
        }
        return -1;
    }

    IEnumerator TransitionEtage(Etage nouveau, Etage ancien, GameObject marqueur1, GameObject marqueur2)
    {
        deplacementTermine = false;
        nouveau.gameObject.SetActive(true);
        ancien.gameObject.SetActive(true);

        nouveau.Fade(false);
        ancien.Fade(true);

        nouveau.gameObject.transform.position = marqueur1.transform.position;

        while (nouveau.gameObject.transform.position != new Vector3(0, 0, 0))
        {
            nouveau.gameObject.transform.position = Vector3.MoveTowards(nouveau.gameObject.transform.position, new Vector3(0, 0, 0), 0.1f);
            ancien.gameObject.transform.position = Vector3.MoveTowards(ancien.gameObject.transform.position, marqueur2.transform.position, 0.1f);
            yield return null;
        }
        deplacementTermine = true;
        ancien.gameObject.SetActive(false);
    }

    IEnumerator Waiting()
    {
        waitIsOver = false;
        yield return new WaitForSeconds(maxTimeWaiting);
        waitIsOver = true;
    }
}

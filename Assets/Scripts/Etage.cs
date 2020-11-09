using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etage : MonoBehaviour
{
    private Batiment batiment;
    private UIManager uiManager;

    public string nom_etage;

    public GameObject boutiques;
    public GameObject reperes;
    public GameObject navmesh;

    public GameObject origine;

    private List<Boutique> listeBoutiques;
    private List<GameObject> listeReperes;

    private bool nouvelleValeurSpawn = false;

    private void Start()
    {
        uiManager = UIManager.Instance;

        listeBoutiques = new List<Boutique>();
        listeReperes = new List<GameObject>();

        Boutique boutique_temp;
        foreach (Transform t in boutiques.transform)
        {
            if (t.GetComponent<Boutique>())
            {
                boutique_temp = t.gameObject.GetComponent<Boutique>();
                listeBoutiques.Add(boutique_temp);
                boutique_temp.SetEtage(this);
            }
        }

        foreach (Transform t in reperes.transform)
        {
            listeReperes.Add(t.gameObject);
        }
    }

    public List<Boutique> GetListBoutiques()
    {
        return listeBoutiques;
    }

    public void SetBatiment(Batiment b)
    {
        batiment = b;
    }

    public void SelectedBoutique(Boutique boutique)
    {
        StartCoroutine(SelectedBoutiqueCoroutine(boutique));
    }

    public void SelectedType(string type)
    {
        nouvelleValeurSpawn = true;
        StartCoroutine(SelectedTypeCoroutine(type));
    }

    public void ResetSelectedBoutique()
    {
        foreach (Boutique b in listeBoutiques)
        {
            b.SetIddleMaterial();
            b.ActiverLogo(false);
        }
    }

    public void Fade(bool status)
    {/*
        foreach (Boutique b in listeBoutiques)
        {
            b.gameObject.SetActive(true);
            b.gameObject.GetComponent<Fade>().FadeStart(status);
        }

        foreach(GameObject g in listeReperes)
        {
            g.gameObject.SetActive(true);
            g.gameObject.GetComponent<Fade>().FadeStart(status);
        }
        */
        navmesh.SetActive(true);
        navmesh.gameObject.GetComponent<Fade>().FadeStart(status);
    }

    IEnumerator SelectedBoutiqueCoroutine(Boutique boutique)
    {
        nouvelleValeurSpawn = true;

        yield return new WaitForSeconds(0.1f);

        nouvelleValeurSpawn = false;

        foreach (Boutique b in listeBoutiques)
        {
            if (b != boutique)
                b.ActiverLogo(false);
            else
                b.ActiverLogo(true);
        }
        uiManager.AfficherBoutique(boutique);

        GameObject temp;
        if ((temp = boutique.GetEntreeBoutique()) != null)
            PathFindingManager.Instance.GeneratePath(origine, temp);
        else
            PathFindingManager.Instance.GeneratePath(origine, boutique.gameObject);
    }

    IEnumerator SelectedTypeCoroutine(string type)
    {
        yield return new WaitForSeconds(0.1f);
        nouvelleValeurSpawn = false;
        foreach (Boutique b in listeBoutiques)
        {
            if (type != b.GetBoutiqueInfos().GetBoutiqueType().GetBoutiqueTypeCode())
                b.ActiverLogo(false);
        }

        foreach (Boutique b in listeBoutiques)
        {
            if (nouvelleValeurSpawn == true)
            {
                yield break;
            }
            if (type == b.GetBoutiqueInfos().GetBoutiqueType().GetBoutiqueTypeCode())
            {
                b.ActiverLogo(true);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}

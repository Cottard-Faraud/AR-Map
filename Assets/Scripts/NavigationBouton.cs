using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavigationBouton : MonoBehaviour
{
    private static Batiment batiment;
    public string input;

    private void Start()
    {
        batiment = Batiment.Instance;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void OnMouseDown()
    {
        //if (!EventSystem.current.IsPointerOverGameObject(-1))
         //   return;
        if (IsPointerOverUIObject())
            return;

        batiment.ChangeEtage(input);
    }
}



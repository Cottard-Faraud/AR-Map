using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public void FadeStart(bool status)
    {
        StartCoroutine(FadeCoroutine(status, GetComponent<MeshRenderer>().material, GetComponent<MeshRenderer>().material.color));
    }

    IEnumerator FadeCoroutine(bool status, Material material, Color color)
    {
        if (status)
        {
            while (material.color.a > 0)
            {
                color = material.color;
                material.color = new Color(color.r, color.g, color.b, color.a - (1.0f * Time.deltaTime));
                yield return null;
            }
            gameObject.SetActive(false);
            material.color = new Color(color.r, color.g, color.b, 1.0f);
        }
        else
        {
            gameObject.SetActive(true);
            material.color = new Color(color.r, color.g, color.b, 0);

            while (material.color.a < 1.0f)
            {
                color = material.color;
                material.color = new Color(color.r, color.g, color.b, color.a + (1.0f * Time.deltaTime));
                yield return null;
            }
        }
    }
}

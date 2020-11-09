using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables : MonoBehaviour
{
    public static Variables Instance;

    public Material selectedMaterial;
    public Material iddleMaterial;

    public GameObject logo3DPrefab;

    private void Awake()
    {
        Instance = this;
    }
}

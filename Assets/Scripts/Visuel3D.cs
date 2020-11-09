using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visuel3D : MonoBehaviour
{
    public float pingpongSpeed = 0.01f;
    public float rotationSpeed = 0.3f;

    private float rotationY;

    public Type type;

    private void Start()
    {
        rotationY = 90.0f;
    }

    void Update()
    {
        switch (type)
        {
            case Type.iddle:
                transform.localPosition = new Vector3(transform.localPosition.x, Mathf.PingPong(Time.time * pingpongSpeed, 0.1f), transform.localPosition.z);
                break;
            case Type.rotation:
                //transform.Rotate(0, 6.0f * rotationPerMinute * Time.deltaTime, 0, Space.Self);
                rotationY = rotationY + rotationSpeed;
                transform.localEulerAngles = new Vector3(-90, rotationY, 0);
                break;
        }
    }
}

public enum Type
{
    iddle,
    rotation
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneDrawer : MonoBehaviour
{
    public Transform[] prueba;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetPlaneObject(GetComponent<Room>().roomPlanes[0],ref prueba[0]);
       // SetPlaneObject(GetComponent<Room>().roomPlanes[1],ref prueba[1]);
       // SetPlaneObject(GetComponent<Room>().roomPlanes[2],ref prueba[2]);
       // SetPlaneObject(GetComponent<Room>().roomPlanes[3],ref prueba[3]);
    }
    void SetPlaneObject(Plane plane, ref Transform transform)
    {
        transform.position = plane.normal * plane.distance;
        transform.up = plane.normal;
    }
}

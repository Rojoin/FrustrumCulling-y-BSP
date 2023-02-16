using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaneCreation : MonoBehaviour
{
    private Plane plane;
    [SerializeField] Transform[] planePos;
    [SerializeField] bool use3Points;
    [SerializeField] MeshRenderer mr;
    void Start()
    {
        if (use3Points)
        {
     plane = new Plane(-planePos[0].position, -planePos[1].position, -planePos[2].position);
        plane.Flip();
        }
        else
        {
            plane = new Plane(transform.forward, transform.position);
        }
           
    }

    // Update is called once per frame
    void Update()
    {
        if (use3Points)
        {
            plane.Set3Points(-planePos[0].position, -planePos[1].position, -planePos[2].position);
        }
        else
        {
            plane.SetNormalAndPosition(transform.forward, transform.position);
        }
        mr.material.color = plane.GetSide(mr.transform.position) ? Color.blue : Color.red;
    }

    void OnDrawGizmos()
    {
        DrawPlane(transform.position,plane.normal);
 
    }
    public void DrawPlane(Vector3 position, Vector3 normal)
    {
        Vector3 v3;
        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;
        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;
        Debug.DrawLine(corner0, corner2, Color.green);
        Debug.DrawLine(corner1, corner3, Color.green);
        Debug.DrawLine(corner0, corner1, Color.green);
        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner3, Color.green);
        Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.red);
    }
}

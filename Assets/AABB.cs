using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AABB : Volume
{
    [SerializeField] private Vector3 center = Vector3.zero;
    [SerializeField] private Vector3 extents = Vector3.one;
    [SerializeField] private Vector3[] points;
    AABB(Vector3 min, Vector3 max)
    {
        this.center = (max + min) * 0.5f;
        this.extents = max - center;
    }
    //Sacar cordenadas de los vertices
    AABB(Vector3 inCenter, float iI, float iJ, float iK)
    {
        center = inCenter;
        extents = new Vector3(iI, iJ, iK);
    }

    void Start()
    {
        points = new Vector3[8];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Vector3.zero;
        }

        Vector3 globalCenter = transform.worldToLocalMatrix * new Vector4(center.x, center.y, center.z, 1.0f);

        // Scaled orientation
        Vector3 right = transform.right * transform.localScale.x;
        Vector3 up = transform.up * transform.localScale.y;
        Vector3 forward = transform.forward * transform.localScale.z;

        float newIi =
            Mathf.Abs(Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), right)) +
            Mathf.Abs(Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), up)) +
            Mathf.Abs(Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), forward));

        float newIj = Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 1.0f, 0.0f), right)) +
                      Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 1.0f, 0.0f), up)) +
                      Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 1.0f, 0.0f), forward));

        float newIk =
            Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), right)) +
            Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), up)) +
            Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), forward));

        MeshBoudingBox();
        CreateAABB(globalCenter, newIi, newIj, newIk);
    }

    void CreateAABB(Vector3 inCenter, float iI, float iJ, float iK)
    {
        center = inCenter;
        extents = new Vector3(iI, iJ, iK);
    }
    public override bool isOnFrustrum(FrustrumCulling.Frustrum camFrustum, Transform modelTransform)
    {
        //Get global scale thanks to our transform
        Vector3 globalCenter = transform.worldToLocalMatrix * new Vector4(center.x, center.y, center.z, 1.0f);

        // Scaled orientation
        Vector3 right = modelTransform.right * transform.localScale.x;
        Vector3 up = modelTransform.up * transform.localScale.y;
        Vector3 forward = modelTransform.forward * transform.localScale.z;

        float newIi =
           Mathf.Abs(Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), right)) +
           Mathf.Abs(Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), up)) +
           Mathf.Abs(Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), forward));
        ;
        float newIj = Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 1.0f, 0.0f), right)) +
            Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 1.0f, 0.0f), up)) +
            Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 1.0f, 0.0f), forward));

        float newIk =
            Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), right)) +
            Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), up)) +
            Mathf.Abs(Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), forward));

       // CreateAABB(globalCenter, newIi, newIj, newIk);

        return (isPointOnPlane(camFrustum) || isPointOnPlane(camFrustum) ||isPointOnPlane(camFrustum)|| isPointOnPlane(camFrustum) || 
                isPointOnPlane(camFrustum) ||
                isPointOnPlane(camFrustum) ||
                isPointOnPlane(camFrustum) ||
                isPointOnPlane(camFrustum) );

    }
    public override bool isOnOrForwardPlan(FrustrumCulling.Plane plan)
    {
        float r = extents.x * Mathf.Abs(plan.normal.x) + extents.y * Mathf.Abs(plan.normal.y) + extents.z * Mathf.Abs(plan.normal.z);

        return -r <= plan.getSignedDistanceToPlan(center);
    }


    public bool isPointOnPlane(FrustrumCulling.Frustrum camFrustum)
    {
       
        
        return (isOnOrForwardPlan(camFrustum.leftPlane) &&
                isOnOrForwardPlan(camFrustum.rightPlane) &&
                isOnOrForwardPlan(camFrustum.topPlane) &&
                isOnOrForwardPlan(camFrustum.bottomPlane) &&
                isOnOrForwardPlan(camFrustum.nearPlane) &&
                isOnOrForwardPlan(camFrustum.farPlane));
    }
    Vector3[] GetVertices()
    {
        MeshFilter[] mfs = GetComponents<MeshFilter>();
        List<Vector3> vList = new List<Vector3>();
        foreach (MeshFilter mf in mfs)
        {
            vList.AddRange(mf.mesh.vertices);
        }
        return vList.ToArray();
    }

    void MeshBoudingBox()
    {
        Vector3[] mesh = GetVertices();

        for (int i = 0; i < mesh.Length; i++)
        {
            if (mesh[i].x > points[0].x && mesh[i].z > points[0].z && mesh[i].y > points[0].y)
            {
                points[0] = mesh[i];
            }
            if (mesh[i].x < points[1].x && mesh[i].z > points[1].z && mesh[i].y > points[1].y)
            {
                points[1] = mesh[i];
            }
            if (mesh[i].x > points[2].x && mesh[i].z < points[2].z && mesh[i].y > points[2].y)
            {
                points[2] = mesh[i];
            }
            if (mesh[i].x > points[3].x && mesh[i].z > points[3].z && mesh[i].y < points[3].y)
            {
                points[3] = mesh[i];
            }
            if (mesh[i].x < points[4].x && mesh[i].z < points[4].z && mesh[i].y > points[4].y)
            {
                points[4] = mesh[i];
            }
            if (mesh[i].x < points[5].x && mesh[i].z > points[5].z && mesh[i].y < points[5].y)
            {
                points[5] = mesh[i];
            }
            if (mesh[i].x > points[6].x && mesh[i].z < points[6].z && mesh[i].y < points[6].y)
            {
                points[6] = mesh[i];
            }
            if (mesh[i].x < points[7].x && mesh[i].z < points[7].z && mesh[i].y < points[7].y)
            {
                points[7] = mesh[i];
            }
          
        }

    }
}


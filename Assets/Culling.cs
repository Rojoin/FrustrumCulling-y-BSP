using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culling : MonoBehaviour
{

    //Inicializa las constantes

    #region Variables

    private const int maxPlanes = 6;
    private const int maxObjecTest = 5;
    private const int AABBPoints = 8;
    private int maxGameObjects;

    [SerializeField] bool drawVertices = false;
    #endregion
    Camera camera;
    [SerializeField] GameObject[] objectsTest = new GameObject[maxObjecTest];
    [SerializeField] FrustrumObjects[] objs = new FrustrumObjects[maxObjecTest];

    #region PlanePoints
    Plane[] plane = new Plane[maxPlanes];
    [SerializeField] Vector3 nearTopLeft;
    [SerializeField] Vector3 nearTopRight;
    [SerializeField] Vector3 nearDownLeft;
    [SerializeField] Vector3 nearDownRight;
    [SerializeField] Vector3 farTopLeft;
    [SerializeField] Vector3 farTopRight;
    [SerializeField] Vector3 farDownLeft;
    [SerializeField] Vector3 farDownRight;

    #endregion
    public struct FrustrumObjects
    {
        public GameObject gameObject;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public Vector3[] aabb;
        public Vector3 scale;
        public Vector3 extents;
        public bool isAABBInside;
    }

    private void Awake()
    {
        camera = Camera.main;
    }

    void Start()
    {

        for (int i = 0; i < maxPlanes; i++)
        {
            plane[i] = new Plane();
        }

        maxGameObjects = GameObject.FindGameObjectsWithTag("Objects").Length;
        objectsTest = GameObject.FindGameObjectsWithTag("Objects");

        for (int i = 0; i < maxGameObjects; i++)
        {
            CreateFrustrumObjects(ref objs[i],objectsTest[i]);
        }
    }

    private void FixedUpdate()
    {
        UpdateFrustrumPlanes();
    }

    void CreateFrustrumObjects(ref FrustrumObjects fobj,GameObject realObj)
    {
        fobj.gameObject = realObj;
        fobj.meshFilter = realObj.GetComponent<MeshFilter>();
        fobj.meshRenderer = realObj.GetComponent<MeshRenderer>();
        fobj.aabb = new Vector3[AABBPoints];
        fobj.extents = fobj.meshRenderer.bounds.extents;
        fobj.scale = fobj.meshRenderer.bounds.size;
        fobj.isAABBInside = false;
    }
    void UpdateFrustrumPlanes()
    {

        var nearPlanePos = camera.transform.position + camera.transform.forward * camera.nearClipPlane;
        var farPlanePos = camera.transform.position + camera.transform.forward * camera.farClipPlane;
        plane[0].SetNormalAndPosition(camera.transform.forward, nearPlanePos);
        plane[1].SetNormalAndPosition(camera.transform.forward * -1, farPlanePos);

        SetFarPoints(farPlanePos);
        SetNearPoints(nearPlanePos);

        plane[2].Set3Points(camera.transform.position, farDownLeft, farTopLeft);
        plane[3].Set3Points(camera.transform.position, farTopRight, farDownRight);
        plane[4].Set3Points(camera.transform.position, farTopLeft, farTopRight);
        plane[5].Set3Points(camera.transform.position, farDownRight, farDownLeft);

        for (int i = 2; i < maxPlanes; i++)
        {
            plane[i].Flip();
        }

        for (int i = 0; i < maxGameObjects; i++)
        {
            SetAABB(ref objs[i]);
        }

        for (int i = 0; i < maxGameObjects; i++)
        {
            ObjectCollision(ref objs[i]);
        }
    }


    public void SetFarPoints(Vector3 farPlanePos)
    {
        float halfVSide = Mathf.Tan((camera.fieldOfView / 2) * Mathf.Deg2Rad) * camera.farClipPlane;
        float halfHSide = (camera.aspect * halfVSide);

        var farPlaneDistance = camera.transform.position + camera.transform.forward * camera.farClipPlane;

        farTopLeft = farPlaneDistance + (camera.transform.up * halfVSide) - (camera.transform.right * halfHSide);

        farTopRight = farPlaneDistance + (camera.transform.up * halfVSide) + (camera.transform.right * halfHSide);

        farDownLeft = farPlaneDistance - (camera.transform.up * halfVSide) - (camera.transform.right * halfHSide);

        farDownRight = farPlaneDistance - (camera.transform.up * halfVSide) + (camera.transform.right * halfHSide);
    }


    public void SetNearPoints(Vector3 nearPlanePos)
    {
        float halfVSide = Mathf.Tan((camera.fieldOfView / 2) * Mathf.Deg2Rad) * camera.nearClipPlane;
        float halfHSide = (camera.aspect * halfVSide);

        var nearPlaneDistance = camera.transform.position + (camera.transform.forward * camera.nearClipPlane);

        nearTopLeft = nearPlaneDistance + (camera.transform.up * halfVSide) - (camera.transform.right * halfHSide);

        nearTopRight = nearPlaneDistance + (camera.transform.up * halfVSide) + (camera.transform.right * halfHSide);

        nearDownLeft = nearPlaneDistance - (camera.transform.up * halfVSide) - (camera.transform.right * halfHSide);

        nearDownRight = nearPlaneDistance - (camera.transform.up * halfVSide) + (camera.transform.right * halfHSide);
    }


    public void SetAABB(ref FrustrumObjects actualObj)
    {
        if (actualObj.scale != actualObj.gameObject.transform.localScale)
        {
            Quaternion rotation = actualObj.gameObject.transform.rotation;
            actualObj.gameObject.transform.rotation = Quaternion.identity;
            actualObj.extents = actualObj.meshRenderer.bounds.extents;
            actualObj.scale = actualObj.gameObject.transform.localScale;
            actualObj.gameObject.transform.rotation = rotation;
        }

        Vector3 size = actualObj.extents;
        Vector3 center = actualObj.meshRenderer.bounds.center;


        actualObj.aabb[0] = new Vector3(center.x - size.x, center.y + size.y, center.z - size.z);
        actualObj.aabb[1] = new Vector3(center.x + size.x, center.y + size.y, center.z - size.z);
        actualObj.aabb[2] = new Vector3(center.x - size.x, center.y - size.y, center.z - size.z);
        actualObj.aabb[3] = new Vector3(center.x + size.x, center.y - size.y, center.z - size.z);
        actualObj.aabb[4] = new Vector3(center.x - size.x, center.y + size.y, center.z + size.z);
        actualObj.aabb[5] = new Vector3(center.x + size.x, center.y + size.y, center.z + size.z);
        actualObj.aabb[6] = new Vector3(center.x - size.x, center.y - size.y, center.z + size.z);
        actualObj.aabb[7] = new Vector3(center.x + size.x, center.y - size.y, center.z + size.z);


        actualObj.aabb[0] = transform.TransformPoint(actualObj.aabb[0]);
        actualObj.aabb[1] = transform.TransformPoint(actualObj.aabb[1]);
        actualObj.aabb[2] = transform.TransformPoint(actualObj.aabb[2]);
        actualObj.aabb[3] = transform.TransformPoint(actualObj.aabb[3]);
        actualObj.aabb[4] = transform.TransformPoint(actualObj.aabb[4]);
        actualObj.aabb[5] = transform.TransformPoint(actualObj.aabb[5]);
        actualObj.aabb[6] = transform.TransformPoint(actualObj.aabb[6]);
        actualObj.aabb[7] = transform.TransformPoint(actualObj.aabb[7]);


        actualObj.aabb[0] = RotatePointAroundPivot(actualObj.aabb[0], actualObj.gameObject.transform.position, actualObj.gameObject.transform.rotation.eulerAngles);
        actualObj.aabb[1] = RotatePointAroundPivot(actualObj.aabb[1], actualObj.gameObject.transform.position, actualObj.gameObject.transform.rotation.eulerAngles);
        actualObj.aabb[2] = RotatePointAroundPivot(actualObj.aabb[2], actualObj.gameObject.transform.position, actualObj.gameObject.transform.rotation.eulerAngles);
        actualObj.aabb[3] = RotatePointAroundPivot(actualObj.aabb[3], actualObj.gameObject.transform.position, actualObj.gameObject.transform.rotation.eulerAngles);
        actualObj.aabb[4] = RotatePointAroundPivot(actualObj.aabb[4], actualObj.gameObject.transform.position, actualObj.gameObject.transform.rotation.eulerAngles);
        actualObj.aabb[5] = RotatePointAroundPivot(actualObj.aabb[5], actualObj.gameObject.transform.position, actualObj.gameObject.transform.rotation.eulerAngles);
        actualObj.aabb[6] = RotatePointAroundPivot(actualObj.aabb[6], actualObj.gameObject.transform.position, actualObj.gameObject.transform.rotation.eulerAngles);
        actualObj.aabb[7] = RotatePointAroundPivot(actualObj.aabb[7], actualObj.gameObject.transform.position, actualObj.gameObject.transform.rotation.eulerAngles);


    }


    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    public void ObjectCollision(ref FrustrumObjects actualObj)
    {
        actualObj.isAABBInside = false;

        for (int i = 0; i < AABBPoints; i++)
        {
            int counter = maxPlanes;

            for (int j = 0; j < maxPlanes; j++)
            {
                if (plane[j].GetSide(actualObj.aabb[i]))
                {
                    counter--;
                }
            }

            if (counter == 0)
            {
                actualObj.isAABBInside = true;

                break;
            }
        }

        if (actualObj.isAABBInside)
        {
            for (int i = 0; i < actualObj.meshFilter.mesh.vertices.Length; i++)
            {
                int counter = maxPlanes;

                for (int j = 0; j < maxPlanes; j++)
                {
                    if (plane[j].GetSide(actualObj.gameObject.transform.TransformPoint(actualObj.meshFilter.mesh.vertices[i]))) //Si el vertice esta en el lado positivo del plano al contador se le resta 1, se cheque con todos los vertices.
                    {
                        counter--;
                    }
                }

                if (counter == 0)
                {
                    actualObj.gameObject.SetActive(true);

                    break;
                }
            }
        }

        else
        {
            if (actualObj.gameObject.activeSelf)
            {
                actualObj.gameObject.SetActive(false);
            }
        }

    }

    public void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        Gizmos.color = Color.green;
        DrawPlane(farTopRight, farDownRight, farDownLeft, farTopLeft);
        DrawPlane(nearTopRight, nearDownRight, nearDownLeft, nearTopLeft);
        DrawPlane(nearTopLeft, farTopLeft, farDownLeft, nearDownLeft);
        DrawPlane(nearTopRight, farTopRight, farDownRight, nearDownRight);
        DrawPlane(nearTopLeft, farTopLeft, farTopRight, nearTopRight);
        DrawPlane(nearDownLeft, farDownLeft, farDownRight, nearDownRight);


        for (int i = 0; i < maxGameObjects; i++)
        {
            DrawAABB(ref objs[i]);
            if (drawVertices)
            {
                DrawVert(objs[i]);
            }
        }
    }


    public void DrawPlane(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);

        Gizmos.color = Color.green;
    }

    //Funcion para dibujar el AABB.
    public void DrawAABB(ref FrustrumObjects actualObj)
    {
        Gizmos.color = Color.magenta;

        for (int i = 0; i < AABBPoints; i++)
        {
            Gizmos.DrawSphere(actualObj.aabb[i], 0.05f);
        }

        Gizmos.DrawLine(actualObj.aabb[0], actualObj.aabb[1]);
        Gizmos.DrawLine(actualObj.aabb[0], actualObj.aabb[4]);
        Gizmos.DrawLine(actualObj.aabb[1], actualObj.aabb[3]);
        Gizmos.DrawLine(actualObj.aabb[2], actualObj.aabb[0]);
        Gizmos.DrawLine(actualObj.aabb[3], actualObj.aabb[2]);
        Gizmos.DrawLine(actualObj.aabb[4], actualObj.aabb[5]);
        Gizmos.DrawLine(actualObj.aabb[5], actualObj.aabb[7]);
        Gizmos.DrawLine(actualObj.aabb[5], actualObj.aabb[1]);
        Gizmos.DrawLine(actualObj.aabb[6], actualObj.aabb[2]);
        Gizmos.DrawLine(actualObj.aabb[6], actualObj.aabb[4]);
        Gizmos.DrawLine(actualObj.aabb[7], actualObj.aabb[3]);
        Gizmos.DrawLine(actualObj.aabb[7], actualObj.aabb[6]);

        Gizmos.color = Color.green;
    }
    public void DrawVert(FrustrumObjects currentObject)
    {

        Gizmos.color = Color.red;

        MeshFilter mesh = currentObject.gameObject.GetComponent<MeshFilter>();

        for (int i = 0; i < mesh.mesh.vertices.Length; i++)
        {
            Gizmos.DrawSphere(currentObject.gameObject.transform.TransformPoint(mesh.mesh.vertices[i]), 0.05f);
        }

        Gizmos.color = Color.green;
    }
}
using System;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FrustrumCulling : MonoBehaviour
{
    [SerializeField] private Vector3 top;
    [SerializeField] private Vector3 bot;
    [SerializeField] private Vector3 righ;
    [SerializeField] private Vector3 left;
    [SerializeField] private Vector3 far;
    [SerializeField] private Vector3 near;
    [SerializeField] private GameObject cube;
    private Frustrum frustum;
    [SerializeField] private Camera camCurrent;
    [SerializeField] private Transform planeNear;
    [SerializeField] private Transform planeFar;
    [SerializeField] private Transform planeRight;
    [SerializeField] private Transform planeLeft;
    [SerializeField] private Transform planeTop;
    [SerializeField] private Transform planeBottom;
    public class Plane
    {
        public Vector3 normal = new Vector3(0.0f, 1.0f, 0.0f);
        public float distance = 0.0f;
        public Plane(Vector3 normal, float distance)
        {
            this.normal = normal;
            this.distance = distance;
        }
        public Plane(Vector3 p1, Vector3 norm)
        {
            this.normal = norm.normalized;
            this.distance = Vector3.Dot(norm, p1);
        }

       public float getSignedDistanceToPlan(Vector3 point)
        {
            return Vector3.Dot(normal, point) - distance;
        }

    }


    public class Frustrum
    {
        public Plane topPlane;
        public Plane bottomPlane;

        public Plane rightPlane;
        public Plane leftPlane;

        public Plane farPlane;
        public Plane nearPlane;

        public Frustrum(Plane nearPlane, Plane farPlane, Plane rightPlane, Plane leftPlane, Plane topPlane, Plane bottomPlane)
        {
            this.nearPlane = nearPlane;
            this.farPlane = farPlane;
            this.rightPlane = rightPlane;
            this.leftPlane = leftPlane;
            this.topPlane = topPlane;
            this.bottomPlane = bottomPlane;
        }
    }


    Frustrum createFrustumFromCamera(Camera cam, float aspect, float fovY, float zNear, float zFar)
    {
        Frustrum frustum;
        float halfVSide = zFar * MathF.Tan(fovY * 0.5f * Mathf.Deg2Rad);
        float halfHSide = halfVSide * aspect;
        Debug.Log(halfHSide);
        Vector3 frontMultFar = zFar * cam.transform.forward;


        Plane nearPlane = new Plane(cam.transform.position + zNear * cam.transform.forward, cam.transform.forward);

        Plane farPlane = new Plane(cam.transform.position + frontMultFar, -cam.transform.forward);

        Plane rightPlane = new Plane(cam.transform.position,- Vector3.Cross(cam.transform.up, frontMultFar + cam.transform.right * halfHSide));

        Plane leftPlane = new Plane(cam.transform.position, -Vector3.Cross(frontMultFar - cam.transform.right * halfHSide, cam.transform.up));

        Plane topPlane = new Plane(cam.transform.position, -Vector3.Cross(cam.transform.right, frontMultFar - cam.transform.up * halfVSide));

        Plane bottomPlane = new Plane(cam.transform.position, -Vector3.Cross(frontMultFar + cam.transform.up * halfVSide, cam.transform.right));

        frustum = new Frustrum(nearPlane, farPlane, rightPlane, leftPlane, topPlane, bottomPlane);

        return frustum;
    }

    void updatePlane(Plane plane, Vector3 norm,Vector3 p1)
    {
        plane.normal = norm.normalized;
        plane.distance = Vector3.Dot(norm, p1);
    }
    void updateFrustrum(Frustrum frustum)
    {

        updatePlane(frustum.nearPlane, frustum.nearPlane.normal,camCurrent.transform.position);
        updatePlane(frustum.farPlane, frustum.farPlane.normal,camCurrent.transform.position);
        updatePlane(frustum.rightPlane, frustum.rightPlane.normal,camCurrent.transform.position);
        updatePlane(frustum.leftPlane, frustum.leftPlane.normal,camCurrent.transform.position);
        updatePlane(frustum.topPlane, frustum.topPlane.normal,camCurrent.transform.position);
        updatePlane(frustum.bottomPlane, frustum.bottomPlane.normal,camCurrent.transform.position);

    }
    Vector3 CrossProduct(Vector3 firstVector3, Vector3 secondVector3)//CORRECTO
    {
        Vector3 result;

        float i = (firstVector3.y * secondVector3.z) - (firstVector3.z * secondVector3.y);
        float j = (firstVector3.x * secondVector3.z) - (firstVector3.z * secondVector3.x);
        float k = (firstVector3.x * secondVector3.y) - (firstVector3.y * secondVector3.x);
        result = new Vector3(i, -j, k);
        return result;
    }

    void SetPlaneObject(Plane plane,Transform transform)
    {
        transform.position = plane.normal* plane.distance;
        transform.up = plane.normal; 
    }
    void Start()
    {
        frustum = createFrustumFromCamera(camCurrent, camCurrent.aspect, camCurrent.fieldOfView, camCurrent.nearClipPlane, camCurrent.farClipPlane);

     
       SetPlaneObject(frustum.nearPlane,planeNear);
       SetPlaneObject(frustum.farPlane,planeFar);
       SetPlaneObject(frustum.rightPlane,planeRight);
       SetPlaneObject(frustum.leftPlane,planeLeft);
       SetPlaneObject(frustum.topPlane,planeTop);
       SetPlaneObject(frustum.bottomPlane,planeBottom);
       
        top = frustum.topPlane.normal;
        bot = frustum.bottomPlane.normal;
        near = frustum.nearPlane.normal;
        far = frustum.farPlane.normal;
        righ = frustum.rightPlane.normal;
        left = frustum.leftPlane.normal;
    }
    void Update()
    {
        updateFrustrum(frustum);
        if (cube.GetComponent<AABB>().isOnFrustrum(frustum, cube.transform))
        {
            Debug.Log("En pantalla");
            cube.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            Debug.Log("No en pantalla");

            cube.GetComponent<MeshRenderer>().enabled = false;
        }
       

    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        //DrawPlane(camCurrent.transform.position, frustum.nearPlane.normal);
        //DrawPlane(camCurrent.transform.position, frustum.farPlane.normal);
        //DrawPlane(camCurrent.transform.position, frustum.leftPlane.normal);
        //DrawPlane(camCurrent.transform.position, frustum.rightPlane.normal);
        //DrawPlane(camCurrent.transform.position, frustum.topPlane.normal);
        //DrawPlane(camCurrent.transform.position, frustum.bottomPlane.normal);

        Gizmos.color = Color.green;
        //  Gizmos.DrawFrustum(Vector3.zero, camCurrent.fieldOfView, camCurrent.farClipPlane,
        //camCurrent.nearClipPlane, camCurrent.aspect);
    }
}

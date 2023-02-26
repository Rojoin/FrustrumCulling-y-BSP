using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;
using Plane = UnityEngine.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update

    private Ray rightRoomRaycast;
    private Ray downRoomRaycast;
    public Transform rightPos;
    public Transform downPos;
    public int RoomId;

   public Transform[] planesPos;

    public Plane[] roomPlanes = new Plane[4];
    public List<Room> adyacentRooms = new List<Room>();

    public bool isRoomVisible = false;
    public bool isChecked = false;
    private float updateTimer = 2.0f;

    void Start()
    {
        rightRoomRaycast.origin = rightPos.position;
        rightRoomRaycast.direction = transform.right;
        downRoomRaycast.origin += downPos.position;
        downRoomRaycast.direction = transform.forward * -1;

        RaycastHit right;
        RaycastHit down;
        Physics.Raycast(rightRoomRaycast, out right);
        Physics.Raycast(downRoomRaycast, out down);
   
        for (int i = 0; i < 4; i++)
        {
            
            roomPlanes[i] = new Plane(planesPos[i].forward, planesPos[i].position);
      
        }
        //roomPlanes[0] = new Plane(-planesPos[0].position, -planesPos[1].position, -planesPos[2].position);
        //roomPlanes[0].Flip();

        // roomPlanes[0] = new Plane(new Vector3(1.764f, 1.2f*2, 1.764f), transform.TransformPoint(planesPos[0].position), new Vector3(1.764f, 0, -1.764f));
       // if (isRoomVisible)
       // {
       //     gameObject.SetActive(true);
       // }
       // else
       // {
       //     gameObject.SetActive(false);
       // }
    }


    // Update is called once per frame
    void Update()
    {
        rightRoomRaycast.origin = rightPos.position;
        rightRoomRaycast.direction = transform.right;
        downRoomRaycast.origin = downPos.position;
        downRoomRaycast.direction = transform.forward * -1;

        if (isRoomVisible)
        {
            Show();
        }
        else
        {
            Hide();
        }

        if (isChecked)
        {
            updateTimer -= Time.deltaTime;
            if (updateTimer < 0.0f)
            {
                updateTimer = 2.0f;
                isChecked = false;
                isRoomVisible = false;
            }
        }
    }

    public void Hide()
    {

        MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].enabled = false;
        }
    }

    public void Show()
    {
        MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].enabled = true;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rightRoomRaycast);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(downRoomRaycast);
        for (int i = 0; i < planesPos.Length; i++)
        {
            Gizmos.DrawSphere(planesPos[i].position, 0.5f);

        }

        // Gizmos.DrawSphere(transform.TransformPoint(planesPos[0].position), 0.5f);
        // Gizmos.DrawSphere(transform.TransformPoint(planesPos[1].position), 0.5f);
        // Gizmos.DrawSphere(transform.TransformPoint(planesPos[2].position), 0.5f);
        //  Gizmos.DrawSphere(new Vector3(1.764f, 1.2f * 2, 1.764f), 0.5f);
        //  Gizmos.DrawSphere(new Vector3(1.764f, 0, -1.764f), 0.5f);

        Gizmos.color = Color.green;
        for (int i = 0; i < 4; i++)
        {
            DrawPlane(planesPos[i].position, roomPlanes[i].normal);

        }

    }
    public bool isPointInside(Vector3 pos)
    {
        for (int i = 0; i < roomPlanes.Length; i++)
        {
            if (!roomPlanes[i].GetSide(pos))
            {
                return false;
            }
        }

        return true;
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
        Debug.DrawRay(position, normal, Color.magenta);
    }
}

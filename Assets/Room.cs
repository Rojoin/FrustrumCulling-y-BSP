using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update

    private Ray rightRoomRaycast;
    private Ray downRoomRaycast;
    public Transform rightPos;
    public Transform downPos;
    public int RoomId;

    [SerializeField] Transform[] planesPos;

    public Plane[] roomPlanes = new Plane[4];
    [SerializeField] GameObject rightRoom;
    [SerializeField] GameObject downRoom;


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
        if (right.transform != null)
        {
            rightRoom = right.transform.gameObject;
        }
        if (down.transform != null)
        {
            downRoom = down.transform.gameObject;
        }
        for (int i = 0; i < 4; i++)
        {
            roomPlanes[i] = new Plane(-planesPos[i].position, -planesPos[(i + 1)].position, -planesPos[(i + 2)].position);
            roomPlanes[i].Flip();
        }
        //roomPlanes[0] = new Plane(-planesPos[0].position, -planesPos[1].position, -planesPos[2].position);
        //roomPlanes[0].Flip();

        // roomPlanes[0] = new Plane(new Vector3(1.764f, 1.2f*2, 1.764f), transform.TransformPoint(planesPos[0].position), new Vector3(1.764f, 0, -1.764f));

    }

    // Update is called once per frame
    void Update()
    {
        rightRoomRaycast.origin = rightPos.position;
        rightRoomRaycast.direction = transform.right;
        downRoomRaycast.origin = downPos.position;
        downRoomRaycast.direction = transform.forward * -1;

        roomPlanes[0] = new Plane(-planesPos[0].position, -planesPos[(1)].position, -planesPos[(2)].position);
        roomPlanes[0].Flip();
        roomPlanes[1] = new Plane(-planesPos[3].position, -planesPos[(4)].position, -planesPos[(5)].position);
  
        roomPlanes[2] = new Plane(-planesPos[6].position, -planesPos[7].position, -planesPos[8].position);
      
        roomPlanes[3] = new Plane(-planesPos[9].position, -planesPos[10].position, -planesPos[11].position);
        roomPlanes[3].Flip();

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
        Gizmos.DrawSphere(new Vector3(1.764f, 0, -1.764f), 0.5f);
        Gizmos.color = Color.green;
        for (int i = 0; i < 4; i++)
        {
            DrawPlane(roomPlanes[i].distance * roomPlanes[i].normal, roomPlanes[i].normal);

        }

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

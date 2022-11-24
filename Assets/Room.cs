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

    public Vector3 plane;
    public Vector3 plane1;
    public Vector3 plane2;
    public Vector3 plane3;
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

        for (int i = 0; i < roomPlanes.Length; i++)
        {
            roomPlanes[i] = new Plane(planesPos[i].up, planesPos[i].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rightRoomRaycast.origin = rightPos.position;
        rightRoomRaycast.direction = transform.right;
        downRoomRaycast.origin = downPos.position;
        downRoomRaycast.direction = transform.forward * -1;

        plane = roomPlanes[0].normal;
        plane1 = roomPlanes[1].normal;
        plane2 = roomPlanes[2].normal;
        plane3 = roomPlanes[3].normal;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rightRoomRaycast);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(downRoomRaycast);
    }
}

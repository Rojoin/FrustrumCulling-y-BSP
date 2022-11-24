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

        roomPlanes[0] = new Plane(planesPos[0].right, planesPos[0].position);
        roomPlanes[1] = new Plane(planesPos[1].forward, planesPos[1].position);
        roomPlanes[1].Flip();
        roomPlanes[2] = new Plane(-planesPos[2].right, planesPos[2].position);
        roomPlanes[3] = new Plane(-planesPos[3].forward, planesPos[3].position);
        roomPlanes[3].Flip();
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

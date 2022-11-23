using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update

    private Ray rightRoomRaycast = new Ray(new Vector3(1.2f, 1.2f, 0), Vector3.right);
    private Ray downRoomRaycast = new Ray(new Vector3(0, 1.2f, -1.2f), Vector3.back);

    public int RoomId;

    [SerializeField] GameObject rightRoom;
    [SerializeField] GameObject downRoom;
    void Start()
    {
        rightRoomRaycast.origin += transform.position;
        rightRoomRaycast.direction = transform.right;
        downRoomRaycast.origin += transform.localPosition;
        downRoomRaycast.direction = transform.forward * -1;

        RaycastHit right;
        RaycastHit down;
        Physics.Raycast(rightRoomRaycast, out right);
        Physics.Raycast(downRoomRaycast, out down);
        if (right.transform!= null)
        {
            rightRoom = right.transform.gameObject;

        }

        if (down.transform != null)
        {
            downRoom = down.transform.gameObject;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        rightRoomRaycast.origin = transform.position + new Vector3(1.2f, 1.2f, 0);
        rightRoomRaycast.origin = new Vector3(rightRoomRaycast.origin.x,
            rightRoomRaycast.origin.y * transform.rotation.y, rightRoomRaycast.origin.z);
        rightRoomRaycast.direction = transform.right;
        downRoomRaycast.origin = transform.position + new Vector3(0, 1.2f, -1.2f);
        downRoomRaycast.direction = transform.forward * -1;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rightRoomRaycast);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(downRoomRaycast);
    }
}

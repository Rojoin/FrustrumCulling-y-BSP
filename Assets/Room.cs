using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update

    private Ray rightRoomRaycast;
    private Ray downRoomRaycast ;
    public Transform rightPos;
    public Transform downPos;
    public int RoomId;

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
        rightRoomRaycast.origin = rightPos.position;
        rightRoomRaycast.direction = transform.right;
        downRoomRaycast.origin = downPos.position;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSystem : MonoBehaviour
{
    public Room[] allRooms;
    public Room currentRoom;
    private Transform player;
    public int currentRoomIndex = -1;


    [SerializeField] private float lineMargin;
    [SerializeField] private float renderDis;
    [SerializeField] private float iterFreq;
    private Vector3[] points;
    struct Line
    {
        public Vector3 ini;
        public Vector3 dir;
    }
    private Line[] lines = new Line[10];

    void Start()
    {
        player = transform.parent;
         points = new Vector3[(int)(renderDis / iterFreq)];
    }

    void InitLines()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].ini = transform.position;
            lines[i].dir = (transform.forward * renderDis + transform.right * lineMargin * (-lines.Length / 2) + i * transform.right * lineMargin).normalized;

        }
    }
    public void DrawLines()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            Vector3[] points = new Vector3[(int)(renderDis / iterFreq)];

            Gizmos.color = Color.red;

            for (int j = 0; j < points.Length; j++)
            {
                points[j] = lines[i].ini + lines[i].dir * iterFreq * j;
                Gizmos.DrawSphere(points[j], 0.1f);
            }
        }
    }

    bool IsPointInPlane(Vector3 point, Plane plane)
    {
        if (plane.GetSide(point))
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        checkPlayerPos();
        currentRoom.isRoomVisible = true;
        currentRoom.isChecked = true;
        foreach (Room adRoom in currentRoom.adyacentRooms)
        {
               checkAdyacentRooms(adRoom);

        }
    }
    void checkPlayerPos()
    {

        for (int i = 0; i < allRooms.Length; i++)
        {
            if (allRooms[i].isPointInside(player.position))
            {
                currentRoomIndex = i;
                currentRoom = allRooms[i];
                currentRoom.isRoomVisible = true;
                currentRoom.isChecked = true;
            }

            else
            {
                allRooms[i].isRoomVisible = false;
                allRooms[i].isChecked = false;
            }

        }

    }
    void checkAdyacentRooms(Room adjRoom)
    {
        Vector3[] points = new Vector3[(int)(renderDis / iterFreq)];

      
        for (int j = 0; j < lines.Length; j++)
        {

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = lines[j].ini + lines[j].dir * iterFreq * i;
                
                if (!adjRoom.isChecked && adjRoom.isPointInside(points[i]) )
                {
                    adjRoom.isRoomVisible = true;
                    adjRoom.isChecked = true;
                    foreach (Room adRoom in adjRoom.adyacentRooms)
                    {
                        checkAdyacentRooms(adRoom);

                    }

                }
                else
                {
                    //adjRoom.isRoomVisible = false;
                }


            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        InitLines();
        DrawLines();
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawSphere(points[i], 0.1f);
        }
        foreach (Room adRoom in currentRoom.adyacentRooms)
        {
        //    checkAdyacentRooms(adRoom);

        }

    }
}

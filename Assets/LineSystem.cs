using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSystem : MonoBehaviour
{
    public Room[] currentRoom;
    public Vector3 example;
    private Transform player;
    public Transform a;
    public int currentRoomIndex = -1;
    private Vector3 nullVector = new Vector3(0, -10000, 0);
    float wallExtends = 3.5f / 2.0f;
    struct Line
    {
        public Vector3 ini;
        public Vector3 dir;
        public Vector3 end;
    }

    void Start()
    {
 player = transform.parent;
    }
    
    Vector3 intersetLine(Plane plane, Vector3 a, Vector3 b)
    {
        Vector3 ba = b - a;
        float nDotA = Vector3.Dot(plane.normal, a);
        float nDotBA = Vector3.Dot(plane.normal, ba);
        if (Mathf.Abs(nDotBA) < Mathf.Epsilon)
        {
            return nullVector;
        }
        return a + (((plane.distance - nDotA) / nDotBA) * ba);
    }
    private Line[] fline = new Line[2];
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
        checkAdyacentRoons();
    }
     void checkPlayerPos()
    {

        for (int i = 0; i < currentRoom.Length; i++)
        {
            int counter = 0;
            for (int k = 0; k < currentRoom[i].roomPlanes.Length; k++)
            {
                if (currentRoom[i].roomPlanes[k].GetSide(player.position))
                {
                    counter++;
                }

            }
            if (counter >= currentRoom[i].roomPlanes.Length)
            {
                currentRoomIndex = i;
                break;
            }

        }
        for (int i = 0; i < currentRoom.Length; i++)
        {
            currentRoom[i].gameObject.SetActive(i == currentRoomIndex);

        }
    }
    void checkAdyacentRoons()
    {

        for (int i = 0; i < currentRoom[currentRoomIndex].adyacentRooms.Count; i++)
        {
            int counter = 0;

            Vector3 point = nullVector;
            for (int j = 0; j < 5; j++)
            {
               
                for (int l = 1; l < 10; l++)
                {
                    point = fline[0].ini + fline[0].dir * l;
                    Vector3 intersectPoint = point;
                    if (intersectPoint == nullVector)
                    {
                        Debug.Log("Error");
                        continue;
                    }
                    for (int k = 0; k < currentRoom[currentRoomIndex].adyacentRooms[i].roomPlanes.Length; k++)
                    {
                        if (IsPointInPlane(intersectPoint, currentRoom[currentRoomIndex].adyacentRooms[i].roomPlanes[k]))
                        {
                            //  Debug.Log("Collision: " + intersectPoint);
                            counter++;

                        }
                    }
                    if (counter >3)
                    {
                        break;
                    }
                }
                if (counter > 3)
                {
                    break;
                }
                else
                {
                    counter = 0;
                }

            }
            bool state = counter > 3;
            Debug.Log("Habitacion: " + i + "Counter :" + counter);
            currentRoom[currentRoomIndex].adyacentRooms[i].gameObject.SetActive(state);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < fline.Length; i++)
        {
            Gizmos.DrawLine(fline[i].ini, fline[i].end);
        }
    }
}

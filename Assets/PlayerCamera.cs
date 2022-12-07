using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;

    private Transform player;
    private Rigidbody2D rb;
    public GameObject[] currentRoom;
    public Vector3 example;
    public Transform a;
    struct Line
    {
        public Vector3 ini;
        public Vector3 end;
    }

    Vector3 intersetLine(Plane plane, Vector3 a, Vector3 b)
    {
        Vector3 ba = b - a;
        float nDotA = Vector3.Dot(plane.normal, a);
        float nDotBA = Vector3.Dot(plane.normal, ba);

        return a + (((plane.distance - nDotA) / nDotBA) * ba);
    }
    private Line[] fline = new Line[2];

    private float xRotation = 0f;

    void Start()
    {
        for (int i = 0; i < fline.Length; i++)
        {
            fline[i].ini = Vector3.zero;
            fline[i].end = Vector3.zero;
        }
        Cursor.lockState = CursorLockMode.Locked;
        player = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        fline[0].ini = transform.position;
        fline[0].end = transform.position + transform.forward * 10;
        fline[1].ini = transform.position;
        fline[1].end = transform.position + transform.forward +new Vector3(30,0,0) * 10;

    }

    void Update()
    {

        fline[0].ini = transform.position;
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
        fline[0].ini = transform.position;
        fline[0].end = transform.position + transform.forward * 10;

        int counter = 0;

        for (int j = 0; j < currentRoom.Length; j++)
        {
            for (int i = 0; i < 4; i++)
            {
              
                example = intersetLine(currentRoom[j].GetComponent<Room>().roomPlanes[i], fline[0].ini, fline[0].end);
                if (Mathf.Sign(Vector3.Dot(example, transform.forward)) == 1)
                {
                    counter++;
                        break;
                }
                else if (!double.IsNaN(example.x))
                {
                    currentRoom[j].gameObject.SetActive(false);
                   
                    break;
                
                }
            }

           currentRoom[j].gameObject.SetActive(counter >= 4);
                    Debug.Log("a");
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

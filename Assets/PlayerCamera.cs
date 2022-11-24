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
    public GameObject currentRoom;
    public Vector3 example;
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
    private Line fline;

    private float xRotation = 0f;
  
    void Start()
    {
    
        Cursor.lockState = CursorLockMode.Locked;
        player = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        fline.ini = transform.position;
        fline.end = transform.position + transform.forward * 10;
    }

    void Update()
    {

        fline.ini = transform.position;
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
        fline.ini = transform.position;
        fline.end = transform.position + transform.forward * 10;
        example = intersetLine(currentRoom.GetComponent<Room>().roomPlanes[1], fline.ini, fline.end);
        if (Mathf.Sign(Vector3.Dot(example, transform.forward)) == 1)
        {
            Debug.Log("Puede ser pa");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color =Color.red;
        Gizmos.DrawLine(fline.ini,fline.end);
    }
}

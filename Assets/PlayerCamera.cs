using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;

   
    private Rigidbody2D rb;

    private Transform player;

    private float xRotation = 0f;

    void Start()
    {
      
        Cursor.lockState = CursorLockMode.Locked;
   
        rb = GetComponent<Rigidbody2D>();


    }

    void Update()
    {

       
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
      


 

    }



   
}

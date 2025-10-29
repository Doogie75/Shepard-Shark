using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class sharkfollowmouse : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; 
    [SerializeField] private float rotationSpeed = 720f; 
    [SerializeField] private float rotationOffset = 0f; 
    private Camera mainCam;

    void Start() { 
        mainCam = Camera.main; 
    }
    void Update()
    {
        //gets the mouse position
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
       //rotates the shark and moves it towards the mouse position
        Vector3 direction = mousePos - transform.position;
        if (direction.sqrMagnitude > 0.0001f)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position, mousePos, moveSpeed * Time.deltaTime);
    }
}









using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mine : MonoBehaviour
{
    public float speed = 4f;           
    public float offscreenBuffer = 2f; 

    private Rigidbody2D rb;
    private Camera cam;
    private Vector2 moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        cam = Camera.main;

        
        int side = Random.Range(0, 3);
        Rect camRect = CameraWorldRect(cam);

        Vector2 spawnPos = Vector2.zero;
        Vector2 target = Vector2.zero;
        //checks a random range from 0 to 3 which chooses which side the mione spanws left right or top
        if (side == 0) 
        {
            spawnPos = new Vector2(camRect.xMin - offscreenBuffer, Random.Range(camRect.yMin, camRect.yMax));
            target = new Vector2(camRect.xMax + offscreenBuffer, Random.Range(camRect.yMin, camRect.yMax));
        }
        else if (side == 1) 
        {
            spawnPos = new Vector2(camRect.xMax + offscreenBuffer, Random.Range(camRect.yMin, camRect.yMax));
            target = new Vector2(camRect.xMin - offscreenBuffer, Random.Range(camRect.yMin, camRect.yMax));
        }
        else
        {
            spawnPos = new Vector2(Random.Range(camRect.xMin, camRect.xMax), camRect.yMax + offscreenBuffer);
            target = new Vector2(Random.Range(camRect.xMin, camRect.xMax), camRect.yMin - offscreenBuffer);
        }

        transform.position = spawnPos; //sets the spawn position of the mine
        moveDir = (target - spawnPos).normalized;

        
        float ang = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg; //points the tip of the sprite in the direction the mine is going
        transform.rotation = Quaternion.Euler(0, 0, ang - 90f);
    }

    void FixedUpdate() //moves the mine and remmoves the clone when it is off screen
    {
        rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);

      
        if (IsOutsideCamera(cam, offscreenBuffer * 1.5f))
            Destroy(gameObject);
    }

    
    Rect CameraWorldRect(Camera cam) //gets the dimensions of the cam 
    {
        float z = -cam.transform.position.z;
        Vector3 bl = cam.ViewportToWorldPoint(new Vector3(0, 0, z));
        Vector3 tr = cam.ViewportToWorldPoint(new Vector3(1, 1, z));
        return new Rect(bl.x, bl.y, tr.x - bl.x, tr.y - bl.y);
    }

    bool IsOutsideCamera(Camera cam, float buffer) //checks if the mine is outside the cam
    {
        Rect r = CameraWorldRect(cam);
        Vector2 p = transform.position;
        return (p.x < r.xMin - buffer || p.x > r.xMax + buffer ||
                p.y < r.yMin - buffer || p.y > r.yMax + buffer);
    }
}

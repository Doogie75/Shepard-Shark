using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jellyfishmove : MonoBehaviour
{
    public float speed = 1.5f;          
    public float changeTargetTime = 2f; 
    public float edgePadding = 0.8f;    

    private Camera cam;
    private Vector2 target;
    private float timer;

    void Start()
    {
        cam = Camera.main;
        PickNewTarget();
    }

    void Update()
    {
        if (!cam) return;

        
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime); //moves towards chosen target


        timer += Time.deltaTime;
        if (timer >= changeTargetTime || Vector2.Distance(transform.position, target) < 0.2f) //picks new target after time or if its close enough to its target
        {
            PickNewTarget();
        }

        
        ClampInsideCamera();
    }

    void PickNewTarget() //random target within camera
    {
        Rect r = CameraWorldRect(cam, edgePadding);
        target = new Vector2(Random.Range(r.xMin, r.xMax), Random.Range(r.yMin, r.yMax));
        timer = 0f;
    }

    void ClampInsideCamera() //keeps jellyfish inside camera
    {
        Rect r = CameraWorldRect(cam, edgePadding);
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, r.xMin, r.xMax);
        p.y = Mathf.Clamp(p.y, r.yMin, r.yMax);
        transform.position = p;
    }

    Rect CameraWorldRect(Camera c, float pad) //holds the dimensions of the cam with the padding for jellyfish
    {
        float halfH = c.orthographicSize;
        float halfW = halfH * c.aspect;
        Vector3 cc = c.transform.position;

        float xMin = cc.x - halfW + pad;
        float xMax = cc.x + halfW - pad;
        float yMin = cc.y - halfH + pad;
        float yMax = cc.y + halfH - pad;

        return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }
}
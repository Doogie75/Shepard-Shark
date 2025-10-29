using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishSwim : MonoBehaviour
{
    [Header("Swim")]
    public float speed = 2f;
    public float wanderStrength = 0.5f;
    public float turnSpeed = 2f;

    [Header("Despawn")]
    public float despawnWorldBuffer = 1f;

    [Header("Flee From Shark")]
    public Transform shark;                 
    public string sharkObjectName = "Shark"; 
    public float fleeRadius = 3f;
    public float panicSpeedMultiplier = 1.75f;
    public float fleeTurnBoost = 1.5f;

    Vector2 direction;
    Vector2 target = Vector2.zero;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)//gives the fish a random color
        {
            float h = Random.value;            
            float s = Random.Range(0.7f, 1f);  
            float v = Random.Range(0.8f, 1f);  
            sr.color = Color.HSVToRGB(h, s, v);
        }

        if (!shark)
        {
            var byName = GameObject.Find(sharkObjectName);
            if (byName) shark = byName.transform;
        else
        {
            var byTag = GameObject.FindGameObjectWithTag("Shark");
            if (byTag) shark = byTag.transform;
        }
    }

    direction = (target - (Vector2)transform.position).normalized; //sets the direction of the fish towards the center
    }

    void Update()
    {
        Vector2 desired;

        float curSpeed = speed;
        float curTurn = turnSpeed;


            Vector2 toShark = (Vector2)shark.position - (Vector2)transform.position; //makes a vector that points from the fish to the shark
            float d = toShark.magnitude; //gets the distance from the fish to the shark

            if (d < fleeRadius && d > 0.001f) //checks fishs distance from the shark
            {
                desired = (-toShark / d); 
                curSpeed *= panicSpeedMultiplier;
                curTurn *= fleeTurnBoost;
            }
            else desired = WanderTowardCenter(); //makes the fish go to the center if they arent runnin


        direction = Vector2.Lerp(direction, desired, curTurn * Time.deltaTime).normalized;
        transform.position += (Vector3)(direction * curSpeed * Time.deltaTime);

        //makes it so the top of the fish is pointing where its swiming
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        //destroys the prefab when it leaves the camera
        if (IsOutsideCameraWithBuffer()) Destroy(gameObject);
    }

    Vector2 WanderTowardCenter() //randomly makes the fish move towards the center of the screen
    {
        Vector2 toCenter = (target - (Vector2)transform.position).normalized;
        Vector2 randomOffset = Random.insideUnitCircle * wanderStrength;
        return (toCenter + randomOffset).normalized;
    }

    bool IsOutsideCameraWithBuffer() //checks if the fish is outside my camera view with a small space 
    {
        if (!cam) cam = Camera.main;
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 c = cam.transform.position;
        float xMin = c.x - halfW - despawnWorldBuffer;
        float xMax = c.x + halfW + despawnWorldBuffer;
        float yMin = c.y - halfH - despawnWorldBuffer;
        float yMax = c.y + halfH + despawnWorldBuffer;
        Vector3 p = transform.position;
        return (p.x < xMin || p.x > xMax || p.y < yMin || p.y > yMax);
    }


}

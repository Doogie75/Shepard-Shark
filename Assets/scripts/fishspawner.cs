using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishspawner : MonoBehaviour
{
    public GameObject fishPrefab;   
    public int startCount = 10;     
    public float spawnInterval = 3f; 
    public float spawnOffset = 1f;   

    Camera cam;
    float timer;

    void Start() //spawns some fish at the start
    {
        cam = Camera.main;

        
        for (int i = 0; i < startCount; i++)
            SpawnFish();
    }

    void Update() //spawns fish over time
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnFish();
        }
    }

    void SpawnFish()
    {
        //spawns fish from the left right or top of the screen
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 camPos = cam.transform.position;

        Vector2 spawnPos = Vector2.zero;

        int side = Random.Range(0, 3);

        if (side == 0) // Left
            spawnPos = new Vector2(camPos.x - halfW - spawnOffset, Random.Range(camPos.y - halfH, camPos.y + halfH));
        else if (side == 1) // Right
            spawnPos = new Vector2(camPos.x + halfW + spawnOffset, Random.Range(camPos.y - halfH, camPos.y + halfH));
        else if (side == 2) // Top
            spawnPos = new Vector2(Random.Range(camPos.x - halfW, camPos.x + halfW), camPos.y + halfH + spawnOffset);

        Instantiate(fishPrefab, spawnPos, Quaternion.identity);
    }
}

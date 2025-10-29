using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] private ScoreCounter scoreCounter; 
    [SerializeField] private int pointsPerFish = 1;
    [SerializeField] private string fishTag = "fish1";

    [Header("Audio")]
    [SerializeField] private AudioClip blipSfx;   
    [Range(0f, 1f)][SerializeField] private float volume = 0.8f;

    private AudioSource audioSource;

    void Awake()
    {
        //sets the audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start() //sets the score counter
    {
            scoreCounter = FindObjectOfType<ScoreCounter>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.CompareTag(fishTag)) return;

        //plays the fish collection noise

            audioSource.PlayOneShot(blipSfx, volume);
        

        //updates the score
            scoreCounter.score += pointsPerFish;
            HighScore.TRY_SET_HIGH_SCORE(scoreCounter.score);
        
        //removes the fish
        Destroy(coll.gameObject);
    }
}

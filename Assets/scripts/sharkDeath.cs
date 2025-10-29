using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class sharkDeath : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI gameOverText;
    [Header("Audio")]
    public AudioClip explosionSfx;       
    [Range(0f, 1f)] public float volume = 0.9f;

    private AudioSource audioSource;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); //sets the audio that I uploaded
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {    //checks to see if the shark collides with a mine if it does then it sends the game over text
        if (other.CompareTag("mine"))
        {
                gameOverText.gameObject.SetActive(true);
            foreach (var b in GetComponents<MonoBehaviour>())
            {
                if (b != this) b.enabled = false;
            }

            //stops the game
            Time.timeScale = 0f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HighScore : MonoBehaviour
{
    private static HighScore I;                 
    private static int _score;                  
    private const string KEY = "HighScore";

    private TextMeshProUGUI ui;

    void Awake()
    {
        I = this; //sets I to the instance
        ui = GetComponent<TextMeshProUGUI>(); //grabs the text component
        
        _score = PlayerPrefs.GetInt(KEY, 0); //loads the high score from player prefs
        UpdateUI();
    }

    public static void TRY_SET_HIGH_SCORE(int scoreToTry) //checks to see if the current score is higher than high score if it is then it calls the update helper APPLEPICKER CLUTCHED
    {
        if (scoreToTry <= _score) return;
        _score = scoreToTry;
        PlayerPrefs.SetInt(KEY, _score);
        UpdateUI();
    }

    private static void UpdateUI() 
    {
        if (I != null && I.ui != null)
            I.ui.text = "Most Fish caught: " + _score.ToString("#,0"); //this updates the highscore ui
    }

    [ContextMenu("Reset High Score To 0")] //this makes it so you can reset the highscore from the inspector
    void ResetHighScoreContext()
    {
        _score = 0;
        PlayerPrefs.SetInt(KEY, 0);
        UpdateUI();
    }
}

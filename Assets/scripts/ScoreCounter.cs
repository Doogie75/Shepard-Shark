using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreCounter : MonoBehaviour
{
    [Header("Dynamic")]
    public int score = 0;
    private TextMeshProUGUI uiText;
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>(); //just grabs the text component
    }

    void Update()
    {
        uiText.text = "Fish caught: " + score.ToString("#,0"); //updates the score to the ui
    }
}

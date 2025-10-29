using System.Collections;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    [Header("Mine Settings")]
    public GameObject minePrefab;

    [Header("Spawn Timing")]
    public float normalSpawnInterval = 3f;
    public float fasterSpawnInterval = 1.5f;
    public float hardestSpawnInterval = 0.8f;  
    public float deathSpawnInterval = 0.35f;    

    [Header("Mine Limits")]
    public int normalMaxMines = 6;
    public int hardestMaxMines = 10;          
    public int deathMaxMines = 20;              

    [Header("Score Thresholds")]
    public int startMinesAt = 20;
    public int increaseDifficultyAt = 40;
    public int spawnJellyfishAt = 55;
    public int finalDifficultyAt = 80;
    public int winAt = 100;

    [Header("UI Messages")]
    public GameObject startMessage;   
    public GameObject warningText1;   
    public GameObject warningText2;  
    public GameObject warningText3;   
    public GameObject warningText4;   
    public GameObject winMessage;     

    [Header("Jellyfish")]
    public GameObject jellyfishPrefab;

    private int alive;
    private int maxMines;
    private float currentInterval;
    private ScoreCounter scoreCounter;

    void Start() //sets the values when the game starts
    {
        scoreCounter = FindObjectOfType<ScoreCounter>();
        currentInterval = normalSpawnInterval;
        maxMines = normalMaxMines;
        StartCoroutine(GameStartSequence());
    }

    IEnumerator GameStartSequence()
    {
            //shows the start message for 3 seconds
            startMessage.SetActive(true);
            yield return new WaitForSeconds(3f);
            startMessage.SetActive(false);
        

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        //at 20 fish saved mines start spawning
        yield return new WaitUntil(() => scoreCounter != null && scoreCounter.score >= startMinesAt);
        ShowWarning(warningText1);
        StartCoroutine(SpawnMinesRoutine());

        //at 40 fish saved increase the spawn rate
        yield return new WaitUntil(() => scoreCounter.score >= increaseDifficultyAt);
        ShowWarning(warningText2);
        currentInterval = fasterSpawnInterval;

        //at 55 fish saved spawn another jellyfish
        yield return new WaitUntil(() => scoreCounter.score >= spawnJellyfishAt);
        ShowWarning(warningText3);
        if (jellyfishPrefab != null)
            Instantiate(jellyfishPrefab, GetRandomPositionInsideCamera(), Quaternion.identity);

        //at 80 fish saved increase mines
        yield return new WaitUntil(() => scoreCounter.score >= finalDifficultyAt);
        ShowWarning(warningText4);
        currentInterval = hardestSpawnInterval;
        maxMines = hardestMaxMines;

        //at 100 fish saved player wins and difficulty goes much higher
        yield return new WaitUntil(() => scoreCounter.score >= winAt);

        if (winMessage != null)
        {
            winMessage.SetActive(true);
            StartCoroutine(HideAfter(winMessage, 2f));
        }

        //goes to much more difficult
        currentInterval = deathSpawnInterval;
        maxMines = deathMaxMines;
    }

    IEnumerator SpawnMinesRoutine()//keeps spawning mines as long as im under the max amount of mines set
    {
        while (true)
        {
            if (alive < maxMines)
                SpawnMine();

            yield return new WaitForSeconds(currentInterval);
        }
    }

    void SpawnMine()//spawns mine and checks how many clones are alive
    {
        var mine = Instantiate(minePrefab);
        alive++;

        var notifier = mine.AddComponent<OnDestroyNotify>();
        notifier.onDestroyed = () => alive--;
    }

    void ShowWarning(GameObject warning)//shows warning
    {
        
        StartCoroutine(ShowWarningRoutine(warning));
    }

    IEnumerator ShowWarningRoutine(GameObject warning)//shows warning for 2 seconds
    {
        warning.SetActive(true);
        yield return new WaitForSeconds(2f);
        warning.SetActive(false);
    }

    IEnumerator HideAfter(GameObject go, float seconds) //hides the object after the time
    {
        yield return new WaitForSeconds(seconds);
        if (go != null) go.SetActive(false);
    }

    Vector3 GetRandomPositionInsideCamera() //gets a random position inside the camera view
    {
        Camera cam = Camera.main;
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 c = cam.transform.position;

        float x = Random.Range(c.x - halfW + 1f, c.x + halfW - 1f);
        float y = Random.Range(c.y - halfH + 1f, c.y + halfH - 1f);
        return new Vector3(x, y, 0f);
    }

    private class OnDestroyNotify : MonoBehaviour //says when a mine is destroyed
    {
        public System.Action onDestroyed;
        void OnDestroy() => onDestroyed?.Invoke();
    }
}
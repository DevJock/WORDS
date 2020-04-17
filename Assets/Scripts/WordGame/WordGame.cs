using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGame : MonoBehaviour
{
    public GameObject currentWord;
    public GameObject nextWord;
    public List<GameObject> allWords = new List<GameObject>();
    public int currentWordLetterCount;
    public string currentWordValue;
    public float spawnSpeed;
    public float speed;
    public int correctWords;
    public int totalWords;
    public static float Timer;
    public float totalTime = 30.0f;
    public bool gameOver;
    public List<string> difficultWords;
    public List<string> easyWords;
    public int streak;
    public float multiplier;
    public bool previousCorrect;
    GameObject scoreBubble;

    private void Start()
    {
        scoreBubble = Resources.Load("Prefabs/scoreBubble") as GameObject;
        Reset();
    }


    private void Reset()
    {
        spawnSpeed = 2.0f;
        speed = 0.001f;
        difficultWords = new List<string>();
        easyWords = new List<string>();
        correctWords = 0;
        totalWords = 0;
        streak = 0;
        multiplier = 0.0f;
        Timer = totalTime;
        previousCorrect = false;
        GameController.GAME.GameState = GameController.GAME.STATE.Playing;
        InvokeRepeating("NextWord", 0, spawnSpeed);
    }


    public void Refresh()
    {
        if (allWords.Count == 1)
        {
            currentWord = allWords[0];
        }
        else if (allWords.Count > 1)
        {
            currentWord = allWords[0];
            nextWord = allWords[1];
        }
        else
        {
            return;
        }
        if (currentWord)
        {
            for (int i = 0; i < currentWord.transform.childCount; i++)
            {
                currentWord.transform.GetChild(i).GetComponent<Renderer>().material.color = Color.white;
            }
            currentWordValue = currentWord.GetComponent<WordObject>().mutatedValue;
        }
        currentWordLetterCount = 0;
    }



    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.TOGGLE_PAUSE();
            if (GameController.GAME.GameState == GameController.GAME.STATE.Playing)
            {
                Time.timeScale = 1.0f;
                PausePanel.HidePanel();
                InvokeRepeating("NextWord", 0, spawnSpeed);
            }
            else if(GameController.GAME.GameState == GameController.GAME.STATE.Paused)
            {
                Time.timeScale = 0.0f;
                PausePanel.ShowPanel();
                CancelInvoke("NextWord");
            }
        }

        if(GameController.GAME.GameState == GameController.GAME.STATE.Playing)
        {
            if (Timer > 0.0f)
            {
                Timer = (Timer - Time.deltaTime);
            }
        }

        GameController.MyScore.CurrentScore = correctWords;
        if(Timer <= 0.0f && !gameOver)
        {
            speed = 0.01f;
            GameOverPanel.ShowPanel(correctWords+"");
            CancelInvoke("NextWord");
            GameController.GAMEOVER();
            Timer = 0;
            gameOver = true;
        }

        if (!currentWord)
        {
            Refresh();
            return;
        }

        if (currentWordLetterCount == currentWordValue.Length && currentWordLetterCount != 0)
        {
            if(previousCorrect)
            {
                streak += 1;
                multiplier += 0.25f;
            }
            else
            {
                streak = 1;
                multiplier = 1f;
            }
            

            for (int i = 0; i < currentWord.transform.childCount; i++)
            {
                if (currentWord.transform.GetChild(i).gameObject.tag != "letter") continue;
                currentWord.transform.GetChild(i).GetComponent<Renderer>().material.color = Color.green;
            }
            
            if(Timer < 29.5f)
            {
                Timer += 0.5f;
            }
            easyWords.Add(currentWord.GetComponent<WordObject>().value);
            DestroyCorrectWord();
        }

        if (currentWordLetterCount == -1)
        {
            if (previousCorrect)
            {
                streak = 0;
                multiplier = 1;
                previousCorrect = false;
            }
            currentWordLetterCount = 0;
            for (int i = 0; i < currentWord.transform.childCount; i++)
            {
                if (currentWord.transform.GetChild(i).gameObject.tag != "letter") continue;
                currentWord.transform.GetChild(i).GetComponent<Renderer>().material.color = Color.red;
            }
            difficultWords.Add(currentWord.GetComponent<WordObject>().value);
            DestroyMisspelledWord();
        }
    }

    void DestroyCorrectWord()
    {
        if (currentWord.tag == "Word")
        {
            Debug.Log("CORRECT: Good Word");
        }
        else
        {
            Debug.Log("CORRECT: Bad Word");
        }
        
        GameObject scorefx = Instantiate(scoreBubble,currentWord.transform.position, Quaternion.identity);
        scorefx.GetComponent<TextMesh>().text = "+0.5";
        scorefx.name = "Correct ScoreFX";
        scorefx.GetComponent<Renderer>().material.color = Color.green;
        currentWord.GetComponent<WordFX>().Implode();
        allWords.Remove(currentWord);
        Destroy(scorefx, 0.5f);
        Refresh();
        currentWordLetterCount = 0;
        correctWords++;
        spawnSpeed -= 0.05f;
        speed += 0.0001f;
        foreach (GameObject word in allWords)
        {
            word.GetComponent<Move>().speed = speed;
        }
    }
    public void DestroyMisspelledWord()
    {
        if (currentWord.tag == "Word")
        {
            Debug.Log("WRONG: Missed Good Word");
        }
        else
        {
            Debug.Log("WRONG: Missed Bad Word");
        }
        GameObject scorefx = Instantiate(scoreBubble, currentWord.transform.position, Quaternion.identity);
        scorefx.GetComponent<TextMesh>().text = "-0.5";
        scorefx.name = "Incorrect ScoreFX";
        scorefx.GetComponent<Renderer>().material.color = Color.red;
        Destroy(scorefx, 0.5f);
        currentWord.GetComponent<WordFX>().Explode();
        allWords.Remove(currentWord);
        Refresh();
        currentWordLetterCount = 0;
    }

    void SkipWord()
    {
        if(currentWord.tag == "Word")
        {
            Debug.Log("Skipping Good Word");
        }
        else
        {
            Debug.Log("Skipping Bad Word");
        }
        allWords.Remove(currentWord);
        currentWordLetterCount = 0;
        Refresh();
    }


    private void NextWord()
    {
        GameObject word = WordManager.NewWord;
        Destroy(word.GetComponent<BoxCollider>());
        float randX = Random.Range(-0.2f, 0.2f);
        word.transform.position = new Vector3(randX, 0.3f, 0);
        WordFX wordFX = word.AddComponent<WordFX>();
        wordFX.explosiveForce = 2f;
        wordFX.explosiveRadius = 3f;
        wordFX.smoke = Resources.Load("Prefabs/smoke") as GameObject;
        wordFX.explosion = Resources.Load("Prefabs/Explosion") as GameObject;
        wordFX.implodeSpeed = 0.01f;
        wordFX.destroyTimePeriod = 1f;
        word.AddComponent<Rigidbody>().isKinematic = true;
        BoxCollider box = word.AddComponent<BoxCollider>();
        box.size = new Vector3(0.1f, 0.01f, 0.1f);
        Move move = word.AddComponent<Move>();
        move.speed = speed;
        allWords.Add(word);
        totalWords++;
    }
}

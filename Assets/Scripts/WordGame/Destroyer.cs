using UnityEngine;

public class Destroyer : MonoBehaviour
{
    WordGame wordGame;

    private void Start()
    {
        wordGame = GameObject.Find("WordGame").GetComponent<WordGame>();
    }


    void OnTriggerEnter(Collider col)
    {
        wordGame.streak = 0;
        wordGame.multiplier = 1;
        if(col.tag == "Word")
        {
            Debug.Log("Missed Good Word");
            wordGame.allWords.Remove(col.gameObject);
            Destroy(col.gameObject);
            wordGame.Refresh();
        }
        else if(col.tag == "MutatedWord")
        {
            Debug.Log("Missed Bad Word");
            wordGame.allWords.Remove(col.gameObject);
            Destroy(col.gameObject);
            wordGame.Refresh();
        }
        
    }
}

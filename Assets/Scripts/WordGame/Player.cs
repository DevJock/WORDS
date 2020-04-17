using UnityEngine;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
    public int correctWords;
    public int incorrectWords;
    private bool keyHeld;
    WordGame wordGame;

    public void Initialize()
    {
        correctWords = 0;
        incorrectWords = 0;
    }

    private void Start()
    {
        wordGame = GameObject.Find("WordGame").GetComponent<WordGame>();
        Initialize();
    }

    private void correctAction()
    {
        if(!keyHeld)
        {
            keyHeld = true;
            if (wordGame.currentWord)
            {
                wordGame.currentWord.transform.GetChild(wordGame.currentWordLetterCount).GetComponent<Renderer>().material.color = Color.yellow;
            }
            correctWords++;
            wordGame.currentWordLetterCount++;
            keyHeld = false;
        }
    }

    private void inCorrectAction()
    {
        if(!keyHeld)
        {
            keyHeld = true;
            if (wordGame.currentWord)
            {
                wordGame.currentWord.transform.GetChild(wordGame.currentWordLetterCount).GetComponent<Renderer>().material.color = Color.blue;
            }
            incorrectWords++;
            wordGame.currentWordLetterCount = -1;
            keyHeld = false;
        }
        
    }

    private void skipAction()
    {
        keyHeld = true;
        if(wordGame.currentWord)
        {
            
        }
        keyHeld = false;
    }

    private void OnGUI() {
        if (GameController.GAME.GameState != GameController.GAME.STATE.Playing) {
            return;
        }
        if (Input.anyKeyDown) {
            return;
        }
        Event e = Event.current;
        if (e.isKey) {
            if(e.keyCode == KeyCode.Space) {
                skipAction();
            }else if(e.keyCode >= KeyCode.A && e.keyCode <= KeyCode.Z) {
                if (wordGame.currentWordValue[wordGame.currentWordLetterCount] == e.keyCode.ToString()[0]) {
                    correctAction();
                } else {
                    inCorrectAction();
                }
            }
        }
    }
}
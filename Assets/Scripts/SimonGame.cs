using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonGame : MonoBehaviour {

    [SerializeField]
    Simon simonPlayer;
    [SerializeField]
    Player player;
    [SerializeField]
    List<Button> buttons = new List<Button>();

    [SerializeField]
    AudioClip winningClip;
    [SerializeField]
    AudioClip losingClip;

    [SerializeField]
    Text gameStatus;
    [SerializeField]
    Text currentLevelLabel;

    int currentLevel = 1;
    // If this is 0 then it's simons turn, if it's 1 then it's the players turn
    int turn = 0;

    public int numberOfPresses = 2;

    // Use this for initialization
    void Start() {
        foreach (Button b in buttons)
        {
            b.buttonPressed += HandleButtonPressed;
        }

        simonPlayer.TakeTurn(numberOfPresses);
        turnTaken = true;

        currentLevelLabel.color = new Color(1, 1, 1, 0);

        gameStatus.text = "Simons Turn!";
    }

    int playerButtonPress = 0;
    private void HandleButtonPressed(Button obj)
    {
        if (turn == 0)
            simonsTurn.Add(obj.name);

        if (turn == 1)
        {
            if (playerButtonPress == numberOfPresses - 1)
            {
                IncreaseLevel();
                return;
            }

            if (simonsTurn[playerButtonPress].Equals(obj.name))
            {
                Debug.Log("You hit the right button!");
            }
            else
            {
                GameOver();
                return;
            }

            playerButtonPress++;
        }
    }

    void IncreaseLevel()
    {


        Debug.Log("Great job! You successfully repeated Simons pattern!");
        currentLevel++;
        currentLevelLabel.text = "Level " + currentLevel.ToString();
        numberOfPresses++;
        Invoke("PlayWinningClip", 0.5f);
        player.isOurTurn = false;
        Invoke("SwitchTurn", 3f);
    }

    void GameOver()
    {

        Debug.Log("You hit the wrong button! Game OVER!");
        currentLevel = 1;
        currentLevelLabel.text = "Level " + currentLevel.ToString();
        Invoke("PlayLosingClip", 0.5f);
        numberOfPresses = 2;
        player.isOurTurn = false;
        Invoke("SwitchTurn", 4f);

    }

    private void PlayLosingClip()
    {
        GetComponent<AudioSource>().clip = losingClip;
        GetComponent<AudioSource>().Play();
        gameStatus.color = Color.red;
        gameStatus.text = "Game Over!";

        StartCoroutine(ShowLevelAnimation());
    }

    private void PlayWinningClip()
    {
        GetComponent<AudioSource>().clip = winningClip;
        GetComponent<AudioSource>().Play();

        gameStatus.color = Color.green;
        gameStatus.text = "Good Job!";

        StartCoroutine(ShowLevelAnimation());
    }

    void SwitchTurn()
    {
        if (gameStatus.color != Color.white)
            gameStatus.color = Color.white;

        turn = 0;
        gameStatus.text = "Simons Turn!";

        turnTaken = false;
        playerButtonPress = 0;
        simonsTurn.Clear();
    }

    private IEnumerator ShowLevelAnimation()
    {
        Color c = new Color(1, 1, 1, 0);
        LeanTween.value(currentLevelLabel.gameObject, 0, 1, 0.5f).setOnUpdate((val) =>
        {
            c.a = val;
            currentLevelLabel.color = c;
        });
        yield return new WaitForSeconds(1f);

        LeanTween.value(currentLevelLabel.gameObject, 1, 0, 0.25f).setOnUpdate((val) =>
        {
            c.a = val;
            currentLevelLabel.color = c;
        });


    }

    List<string> simonsTurn = new List<string>();
    bool turnTaken = false;

    // Update is called once per frame
    void Update() {
        // It's Simons turn
        if (turn == 0 && !turnTaken)
        {
            simonPlayer.TakeTurn(numberOfPresses);
            turnTaken = true;
        }
        else if (turn == 0 && turnTaken)
        {
            if (simonsTurn.Count == numberOfPresses)
            {
                Debug.Log("<color=green>SimonGame here! Simon has finished his turn: " + simonsSelection() + ", now it's your turn to play!</color>");
                player.isOurTurn = true;
                turn = 1;
                Invoke("IsPlayerTurn", 0.5f);
            }
        }

        // It's players turn
        if (turn == 1)
        {

        }
    }

    void IsPlayerTurn()
    {
        gameStatus.text = "Your Turn!";
    }

    private string simonsSelection()
    {
        string simonsSelectionString = "";
        foreach (string s in simonsTurn)
        {
            simonsSelectionString += s + ", ";
        }

        return simonsSelectionString;
    }
}

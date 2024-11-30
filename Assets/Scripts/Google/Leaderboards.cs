using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class Leaderboards : MonoBehaviour
{
    public static Leaderboards instance;

    public bool inBoardZone;
    // Update is called once per frame

    void Start()
    {
        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            UIController.instance.interactButton.SetActive(true);
            inBoardZone = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            UIController.instance.interactButton.SetActive(false);
            inBoardZone = false;

        }
    }
    public void ShowLeaderBoard()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            if (PlayerController.instance.isGamepad)
            {
                PlayerController.instance.canMove = false;
            }
            UIController.instance.noLoginWindow.SetActive(true);
            UIController.instance.DisableButtons();
        }
    }
}

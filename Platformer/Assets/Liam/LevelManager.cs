using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public enum Quests { RunDistance}

    public enum GameStates {  Starting, Running, Paused, PlayerDied, PlayerRespawning, PlayerRespawningStarting, PlayerWon, Ending }

    private GameStates gameState = GameStates.Starting;

    public LevelGenerator generator;

    public Transform Player;
    public Image BlackScreenImage;
    public Image WhiteScreenImage;
    public GameObject DiedPanel;
    public Text DiedSeconds;
    public GameObject WonPanel;
    public Text WonSeconds;

    public Slider DistanceProgress;

    bool playerIsDead = false;
    float playerIsDeadTimeout;

    bool fadeOut = false;
    bool fadeIn = false;

	// Use this for initialization
	void Start () {

        gameState = GameStates.Starting;

        BlackScreenImage.raycastTarget = true;

        Color c = BlackScreenImage.color;
        c.a = 1;
        BlackScreenImage.color = c;
    }
	
	// Update is called once per frame
	void Update () {
		if (Camera.main.transform.position.x + 10f > generator.levelEndsAt)
		{
			generator.needNewChunk = true;
		}

        Color c;
        int seconds;

        switch (gameState)
        {
            case GameStates.Starting:
                c = BlackScreenImage.color;
                c.a -= Time.deltaTime;
                BlackScreenImage.color = c;

                if (c.a <= 0.0f)
                {
                    BlackScreenImage.raycastTarget = false;
                    gameState = GameStates.Running;
                    generator.OnlyLoadDefault = false;
                }
                break;
            case GameStates.Running:
                CheckPlayerWinCondition();
                break;
            case GameStates.PlayerDied:
                seconds = (int)(playerIsDeadTimeout - Time.time);
                if (seconds < 0)
                    seconds = 0;
                DiedSeconds.text = seconds.ToString();

                if (playerIsDeadTimeout < Time.time)
                {
                    gameState = GameStates.Ending;
                }
                break;
            case GameStates.PlayerRespawning:
                c = WhiteScreenImage.color;
                c.a += Time.deltaTime * 2f;
                WhiteScreenImage.color = c;
                WhiteScreenImage.raycastTarget = true;

                if (c.a >= 1.0f)
                {
                    Player.GetComponent<PlayerController2D>().RespawnForContinue();
                    Player.transform.position = generator.ContinuePoint;
                    gameState = GameStates.PlayerRespawningStarting;
                    DiedPanel.SetActive(false);
                }

                break;
            case GameStates.PlayerRespawningStarting:
                c = WhiteScreenImage.color;
                c.a -= Time.deltaTime * 2f;
                if (c.a < 0)
                    c.a = 0;
                WhiteScreenImage.color = c;
                

                if (c.a <= 0.0f)
                {
                    gameState = GameStates.Running;
                    WhiteScreenImage.raycastTarget = false;
                }

                break;
            case GameStates.PlayerWon:
                seconds = (int)(playerIsDeadTimeout - Time.time);
                if (seconds < 0)
                    seconds = 0;
                WonSeconds.text = seconds.ToString();

                if (playerIsDeadTimeout < Time.time)
                {
                    gameState = GameStates.Ending;
                }
                break;
            case GameStates.Ending:
                BlackScreenImage.raycastTarget = true;

                c = BlackScreenImage.color;
                c.a += Time.deltaTime;
                BlackScreenImage.color = c;

                if (c.a >= 1.0f)
                    SceneManager.LoadScene("TavernScene");
                break;
        }


      

      
	}


    public void PlayerDied()
    {
        if (gameState != GameStates.PlayerDied)
        {
            gameState = GameStates.PlayerDied;
            playerIsDeadTimeout = Time.time + 10f;
            DiedPanel.SetActive(true);
        }
    }

    public GameStates GetGameState()
    {
        return gameState;
    }

    public void CheckPlayerWinCondition()
    {
        DistanceProgress.value = Player.position.x / 500f;
        if (Player.position.x > 300)
        {
            gameState = GameStates.PlayerWon;
            playerIsDeadTimeout = Time.time + 10f;
            WonPanel.SetActive(true);
        }
    }

    public void OnClickContinue()
    {
        gameState = GameStates.PlayerRespawning;
    }

    public void OnClickDontContinue()
    {
        gameState = GameStates.Ending;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    StartMenu = 0,
    GameRunning = 1,
    OpenDialog = 2,
    Victory = 9,
    GameOver = 10,
    EndGame = 11
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.StartMenu);
    }

    private void Update()
    {
        if (Input.GetButton("Quit"))
        {
            Application.Quit();
        }
    }

    public void ChangeState(GameState _state)
    {
        this.gameState = _state;

        switch (this.gameState)
        {
            case GameState.StartMenu:
                MenuManager.Instance.StartGame();
                break;
            case GameState.GameRunning:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                MenuManager.Instance.HideMenus();
                break;
            case GameState.OpenDialog:
                break;
            case GameState.Victory:
                break;
            case GameState.GameOver:
                AudioManager.Instance.GameOver();
                AudioManager.Instance.StopAmbientAudio();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                MenuManager.Instance.GameOver();
                break;
            case GameState.EndGame:
                AudioManager.Instance.StopAmbientAudio();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                MenuManager.Instance.GameVictory();
                break;
        }
    }
}

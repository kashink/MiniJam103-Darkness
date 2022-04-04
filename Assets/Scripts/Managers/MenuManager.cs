using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Space()]
    [Header("GameOver")]
    [SerializeField] GameObject gameOverArea;

    [Space()]
    [Header("StartGame")]
    [SerializeField] GameObject startGameArea;

    [Space()]
    [Header("Interactables")]
    [SerializeField] GameObject interactArea;
    [SerializeField] GameObject dialogArea;

    [Space()]
    [Header("GameEnd")]
    [SerializeField] GameObject endGameArea;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        HideMenus();
    }

    public void GameOver()
    {
        this.gameOverArea.SetActive(true);
    }

    public void GameVictory()
    {
        this.endGameArea.SetActive(true);
    }

    public void StartGame()
    {
        this.startGameArea.SetActive(true);
    }

    public void HideMenus()
    {
        this.endGameArea.SetActive(false);
        this.gameOverArea.SetActive(false);
        this.startGameArea.SetActive(false);
        this.interactArea.SetActive(false);
        this.dialogArea.SetActive(false);
    }

    public void ShowInteractArea()
    {
        this.interactArea.SetActive(true);
    }

    public void ShowDialogArea(string textMessage)
    {
        this.dialogArea.SetActive(true);
        this.dialogArea.GetComponent<DialogController>().Setup(textMessage);
    }
}

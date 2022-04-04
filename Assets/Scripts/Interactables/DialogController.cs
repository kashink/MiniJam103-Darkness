using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [Space()]
    [Header("Config")]
    [SerializeField] Text textArea;

    public void Setup(string textMessage)
    {
        this.textArea.text = textMessage;
    }

    private void Update()
    {
        switch (GameManager.Instance.gameState)
        {
            case GameState.OpenDialog:
                if (Input.GetButtonDown("Interact"))
                {
                    GameManager.Instance.ChangeState(GameState.GameRunning);
                    AudioManager.Instance.CloseTextArea();
                }
                break;
            case GameState.Victory:
                if (Input.GetButtonDown("Interact"))
                {
                    AudioManager.Instance.UnlockSecret();
                    GameManager.Instance.ChangeState(GameState.EndGame);
                }
                break;
        }
    }
}

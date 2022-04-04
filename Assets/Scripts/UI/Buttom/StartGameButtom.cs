using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameButtom : MonoBehaviour
{

    void Start()
    {
        Button btn = this.gameObject.GetComponent<Button>();
        btn.onClick.AddListener(StartGame);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        AudioManager.Instance.StartAmbientAudio();
        GameManager.Instance.ChangeState(GameState.GameRunning);
    }
}

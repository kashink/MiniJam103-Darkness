using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    [Space()]
    [Header("Config")]
    [SerializeField] string textMessage;
    [SerializeField] bool showHaunt;
    [SerializeField] float showHauntChance;
    [SerializeField] float hideHauntAfter;
    [SerializeField] GameObject haunt;
    [SerializeField] bool unlockShoot;
    [SerializeField] bool gameVictory;
    GameObject player;

    [Space()]
    [Header("DeactivateObject")]
    [SerializeField] bool hideObject;
    [SerializeField] GameObject objectToHide;
    bool hideObjectActivated = false;

    bool isActive = false;

    private void Start()
    {
        if (this.unlockShoot) this.player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.GameRunning) return;

        if (Input.GetButtonDown("Interact") && this.isActive)
        {
            if (this.hideObject && !this.hideObjectActivated)
            {
                this.hideObjectActivated = true;
                AudioManager.Instance.OpenDoor(this.objectToHide.transform);
                this.objectToHide.SetActive(false);
            }

            if (this.unlockShoot)
            {
                this.player.GetComponent<PlayerShoot>().UnlockShoot();
            }

            if (this.gameVictory)
            {
                GameManager.Instance.ChangeState(GameState.Victory);
            } else
            {
                GameManager.Instance.ChangeState(GameState.OpenDialog);
            }
            MenuManager.Instance.HideMenus();
            MenuManager.Instance.ShowDialogArea(textMessage);
            AudioManager.Instance.OpenTextArea();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerInteractArea")
        {
            if (this.showHaunt)
            {
                float random = Random.Range(0, 100);

                if (random <= this.showHauntChance)
                {
                    this.haunt.SetActive(true);
                    AudioManager.Instance.SpawnSmallEnemy(this.transform);
                    StartCoroutine(HideHaunt());
                }
            }

            MenuManager.Instance.ShowInteractArea();
            this.isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerInteractArea")
        {
            MenuManager.Instance.HideMenus();
            this.isActive = false;
        }
    }

    IEnumerator HideHaunt()
    {
        yield return new WaitForSeconds(this.hideHauntAfter);
        this.haunt.SetActive(false);
    }
}

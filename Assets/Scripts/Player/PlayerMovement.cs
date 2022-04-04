using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Space()]
    [Header("Config")]
    [SerializeField] float movementSpeed = 3;
    [SerializeField] float rotationSpeed = 3;
    [SerializeField] float rotationSensibility = 0.1f;

    [Space()]
    [Header("Audio")]
    [SerializeField] float timebetweenStepAudios = 1.302f;
    [SerializeField] List<AudioClip> walkAudioList;

    bool walkAudioUnlocked = true;
    bool running = false;

    void FixedUpdate()
    {
        if (GameManager.Instance.gameState != GameState.GameRunning)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }

        Move();
        Rotate();
    }

    void Move()
    {
        this.running = Input.GetButton("Sprint");

        Vector3 dir = Vector3.zero;
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
        dir.Normalize();

        if (dir != Vector3.zero && this.walkAudioUnlocked)
        {
            this.walkAudioUnlocked = false;

            int randomIndex = Random.Range(0, walkAudioList.Count);
            if (randomIndex >= walkAudioList.Count) randomIndex = walkAudioList.Count - 1;
            this.GetComponent<AudioSource>().PlayOneShot(walkAudioList[randomIndex]);

            StartCoroutine(UnlockWalkAudio());
        }

        Vector3 velocity = transform.right * dir.x + transform.forward * dir.z;

        GetComponent<Rigidbody>().velocity = velocity * (this.movementSpeed * (this.running ? 2 : 1));
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");

        if (mouseX > this.rotationSensibility || mouseX < (this.rotationSensibility * -1)) transform.Rotate(0, this.rotationSpeed * Input.GetAxis("Mouse X"), 0);
        else transform.Rotate(Vector3.zero);
    }

    IEnumerator UnlockWalkAudio()
    {
        yield return new WaitForSeconds(this.timebetweenStepAudios / (this.running ? 2 : 1));
        this.walkAudioUnlocked = true;
    }
}

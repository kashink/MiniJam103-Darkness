using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Space()]
    [Header("Audios")]
    [SerializeField] AudioSource uiOpenTextArea;
    [SerializeField] AudioSource uiCloseTextArea;
    [SerializeField] List<AudioSource> bigEnemySpawnList;
    [SerializeField] List<AudioSource> smallEnemySpawnList;
    [SerializeField] List<AudioSource> bigEnemyDeadList;
    [SerializeField] List<AudioSource> smallEnemyDeadList;
    [SerializeField] List<AudioSource> enemyDashList;
    [SerializeField] AudioSource unlockSecretAudio;
    [SerializeField] AudioSource gameOverAudio;
    [SerializeField] AudioSource openDoorAudio;

    [Space()]
    [Header("Player Damaged")]
    [SerializeField] AudioSource playerDamagedGameObject;
    [SerializeField] List<AudioClip> playerDamageList;

    [Space()]
    [Header("Player Shoot")]
    [SerializeField] AudioSource playerShootGameObject;
    [SerializeField] List<AudioClip> playerShootList;

    [Space()]
    [Header("BG Audios")]
    [SerializeField] AudioSource ambientAudio;
    [SerializeField] AudioSource defaultBGAudio;
    [SerializeField] AudioSource anxietyBaseAudio;
    [SerializeField] AudioSource anxietyTaikosAudio;

    private void Awake()
    {
        Instance = this;
    }

    public void StartAmbientAudio()
    {
        this.ambientAudio.Play();
    }

    public void StopAmbientAudio()
    {
        this.ambientAudio.Stop();
    }

    public void GameOver()
    {
        this.gameOverAudio.Play();
    }

    public void PlayerDamaged()
    {
        int randomIndex = Random.Range(0, playerDamageList.Count);
        if (randomIndex >= playerDamageList.Count) randomIndex = playerDamageList.Count - 1;
        this.playerDamagedGameObject.PlayOneShot(playerDamageList[randomIndex]);
    }

    public void PlayerShoot()
    {
        int randomIndex = Random.Range(0, playerShootList.Count);
        if (randomIndex >= playerShootList.Count) randomIndex = playerShootList.Count - 1;
        this.playerShootGameObject.PlayOneShot(playerShootList[randomIndex]);
    }

    public void OpenTextArea()
    {
        this.uiOpenTextArea.Play();
    }

    public void AnxietyGood()
    {
        this.anxietyBaseAudio.Stop();
        this.anxietyTaikosAudio.Stop();
        this.defaultBGAudio.Play();
    }

    public void AnxietyStart()
    {
        this.defaultBGAudio.Stop();
        this.anxietyBaseAudio.Play();
        this.anxietyTaikosAudio.mute = true;
        this.anxietyTaikosAudio.Play();
    }

    public void AnxietyHarder()
    {
        this.anxietyTaikosAudio.mute = false;
    }

    public void AnxietySofter()
    {
        this.anxietyTaikosAudio.mute = true;
    }

    public void CloseTextArea()
    {
        this.uiCloseTextArea.Play();
    }

    public void UnlockSecret()
    {
        this.unlockSecretAudio.Play();
    }

    public void OpenDoor(Transform door)
    {
        AudioSource doorOpenedInstance = Instantiate(this.openDoorAudio, door.position, door.rotation);
        Destroy(doorOpenedInstance, 10);
    }

    public void SpawnBigEnemy(Transform enemy)
    {
        SpawnAudioTarget(enemy, this.bigEnemySpawnList);
    }

    public void SpawnSmallEnemy(Transform enemy)
    {
        SpawnAudioTarget(enemy, this.smallEnemySpawnList);
    }

    public void DeadBigEnemy(Transform enemy)
    {
        SpawnAudioTarget(enemy, this.bigEnemyDeadList);
    }

    public void DeadSmallEnemy(Transform enemy)
    {
        SpawnAudioTarget(enemy, this.smallEnemyDeadList);
    }

    public void EnemyDash(Transform enemy)
    {
        SpawnAudioTarget(enemy, this.enemyDashList);
    }

    void SpawnAudioTarget(Transform enemy, List<AudioSource> audioList)
    {
        int randomIndex = Random.Range(0, audioList.Count);
        if (randomIndex >= audioList.Count) randomIndex = audioList.Count - 1;

        AudioSource enemyInstance = Instantiate(audioList[randomIndex], enemy.position, enemy.rotation);
        Destroy(enemyInstance, 10);
    }
}

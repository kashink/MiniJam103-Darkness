using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AudioMoment
{
    Normal = 0,
    AnxietyBase = 1,
    AnxietyTaikos = 2
}

public class AnxietyManager : MonoBehaviour
{
    [Space()]
    [Header("Config")]
    [SerializeField] AudioMoment audioMoment = AudioMoment.Normal;
    [SerializeField] GameObject shadow;
    [SerializeField] int maxAnxiety = 20;
    [SerializeField] float timeUntilGameOver = 2;

    [Space()]
    [Header("Audio config")]
    [SerializeField] float anxietyLevelToStartBase = 3;
    [SerializeField] float anxietyLevelToStartTaikos = 6;

    public static AnxietyManager Instance;

    int anxietyLevel = 0;
    float currentTimeUntilGameOver = 0;

    private void Awake()
    {
        Instance = this;
        UpdateShadow();
    }

    void Update()
    {
        if (GameManager.Instance.gameState != GameState.GameRunning) return;

        if (this.anxietyLevel >= this.maxAnxiety)
        {
            this.currentTimeUntilGameOver += 1 * Time.deltaTime;

            if (this.currentTimeUntilGameOver >= this.timeUntilGameOver)
            {
                this.currentTimeUntilGameOver = 0;
                GameManager.Instance.ChangeState(GameState.GameOver);
            }
            return;
        } else
        {
            this.currentTimeUntilGameOver = 0;
        }
    }

    public int GetAnxietyLevel()
    {
        return this.anxietyLevel;
    }

    public void EnemyDied(int anxietyRecover)
    {
        this.anxietyLevel -= anxietyRecover;
        if (this.anxietyLevel < 0) this.anxietyLevel = 0;

        UpdateShadow();
    }

    public void IncreaseAnxiety()
    {
        this.anxietyLevel++;

        UpdateShadow();
    }

    void UpdateShadow()
    {
        Color color = this.shadow.GetComponent<Image>().color;
        color.a = ((float)this.maxAnxiety - ((float)this.maxAnxiety - (float)this.anxietyLevel)) / (float)this.maxAnxiety;
        this.shadow.GetComponent<Image>().color = color;

        switch (this.audioMoment)
        {
            case AudioMoment.Normal:
                if (this.anxietyLevel >= this.anxietyLevelToStartBase)
                {
                    this.audioMoment = AudioMoment.AnxietyBase;
                    AudioManager.Instance.AnxietyStart();
                }
                break;
            case AudioMoment.AnxietyBase:
                if (this.anxietyLevel >= this.anxietyLevelToStartTaikos)
                {
                    this.audioMoment = AudioMoment.AnxietyTaikos;
                    AudioManager.Instance.AnxietyHarder();
                } else if (this.anxietyLevel <= this.anxietyLevelToStartBase)
                {
                    this.audioMoment = AudioMoment.Normal;
                    AudioManager.Instance.AnxietyGood();
                }
                break;
            case AudioMoment.AnxietyTaikos:
                if (this.anxietyLevel <= this.anxietyLevelToStartTaikos)
                {
                    this.audioMoment = AudioMoment.AnxietyBase;
                    AudioManager.Instance.AnxietySofter();
                }
                break;
        }
    }
}

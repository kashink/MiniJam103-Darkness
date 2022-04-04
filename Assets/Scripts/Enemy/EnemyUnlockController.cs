using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnlockController : MonoBehaviour
{
    [Space()]
    [Header("DeactivateObject")]
    [SerializeField] int enemyCount;
    [SerializeField] bool hideObject;
    [SerializeField] GameObject objectToHide;
    bool hideObjectActivated = false;

    void Update()
    {
        if (this.enemyCount <= 0 && !this.hideObjectActivated && this.hideObject)
        {
            this.hideObjectActivated = true;
            AudioManager.Instance.UnlockSecret();
            this.objectToHide.SetActive(false);
        }
    }

    public void EnemySpawn()
    {
        this.enemyCount++;
    }

    public void EnemyKilled()
    {
        this.enemyCount--;
    }
}

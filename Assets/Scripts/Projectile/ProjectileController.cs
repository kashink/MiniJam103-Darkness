using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    float projectileSpeed;

    void Update()
    {
        if (GameManager.Instance.gameState != GameState.GameRunning) return;

        if (this.projectileSpeed != 0) GetComponent<Rigidbody>().velocity = transform.forward * this.projectileSpeed;
    }

    public void Setup(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Wall":
                AnxietyManager.Instance.IncreaseAnxiety();
                EnemyRespawnManager.Instance.SpawnEnemy(other.gameObject);
                Destroy(this.gameObject);
                break;
            case "Enemy":
                if (other.gameObject.GetComponent<EnemyController>().hitted) return;

                other.gameObject.GetComponent<EnemyController>().GetHit();
                Destroy(this.gameObject);
                break;
        }
    }
}

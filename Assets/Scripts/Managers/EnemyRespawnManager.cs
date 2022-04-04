using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnManager : MonoBehaviour
{
    public static EnemyRespawnManager Instance;

    [Space()]
    [Header("Config")]
    [SerializeField] List<EnemyController> enemyList;
    [SerializeField] float maxOffset = 2;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnEnemy(GameObject wall)
    {
        int randomIndex = Random.Range(0, this.enemyList.Count);
        if (randomIndex >= this.enemyList.Count) randomIndex = this.enemyList.Count - 1;

        EnemyController enemyInstance = Instantiate(this.enemyList[randomIndex]);

        WallDirection wallDirection = wall.GetComponent<WallController>().wallDirection;

        if (wall.GetComponent<WallController>().enemyUnlockController != null)
        {
            enemyInstance.enemyUnlockController = wall.GetComponent<WallController>().enemyUnlockController;
            wall.GetComponent<WallController>().enemyUnlockController.EnemySpawn();
        }

        float newX = wallDirection == WallDirection.X ? Random.Range(wall.transform.position.x - this.maxOffset, wall.transform.position.x + this.maxOffset) : wall.transform.position.x;
        float newZ = wallDirection == WallDirection.Z ? Random.Range(wall.transform.position.z - this.maxOffset, wall.transform.position.z + this.maxOffset) : wall.transform.position.z;
        enemyInstance.transform.position = new Vector3(newX, enemyInstance.transform.position.y, newZ);
    }
}

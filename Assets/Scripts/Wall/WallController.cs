using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallDirection
{
    X = 0,
    Z = 1
}

public class WallController : MonoBehaviour
{
    [Space()]
    [Header("Config")]
    [SerializeField] public WallDirection wallDirection;
    [SerializeField] public EnemyUnlockController enemyUnlockController;
}

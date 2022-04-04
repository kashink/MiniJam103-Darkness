using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Small = 0,
    Big = 1
}

public enum DashDirection { Nome, Left, Right }

public class EnemyController : MonoBehaviour
{
    [Space()]
    [Header("Config")]
    [SerializeField] EnemyType enemyType;
    [SerializeField] float movementSpeed = 1;
    [SerializeField] int life = 2;
    [SerializeField] int anxietyRecover = 1;
    [SerializeField] float hitCD = 0.5f;
    [SerializeField] float intangibleCD = 0.5f;
    [SerializeField] bool dashWhenIntangible = false;
    [SerializeField] float minDistanceToDash = 2;
    [SerializeField] bool playAudioOnAwake = true;

    public EnemyUnlockController enemyUnlockController;

    DashDirection dashDirection = DashDirection.Nome;
    public bool hitted = false;

    float currentHitCD = 0;
    float currentIntangibleCD = 0;
    bool canHit = true;
    bool intangible = true;
    GameObject player;
    List<string> playerTagHitList = new List<string> { "Player", "PlayerInteractArea" };
    List<string> playerTagList = new List<string> { "Player", "PlayerInteractArea", "Enemy", "Projectile" };

    void Start()
    {
        this.player = GameObject.Find("Player");

        ToggleTangible(false);
        this.GetComponent<BoxCollider>().enabled = false;

        if (this.playAudioOnAwake)
        {
            switch (this.enemyType)
            {
                case EnemyType.Small:
                    AudioManager.Instance.SpawnSmallEnemy(this.transform);
                    break;
                case EnemyType.Big:
                    AudioManager.Instance.SpawnBigEnemy(this.transform);
                    break;
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.gameState != GameState.GameRunning)
        {
            GamePaused();
            return;
        }

        Move();
        HandleHitCD();
        HandleIntangibleCD();
    }

    void Move()
    {
        RaycastHit[] hits;
        if (this.hitted && this.dashWhenIntangible)
        {
            hits = Physics.RaycastAll(this.transform.position, this.transform.right);
            float rightDistance = 0;
            if (hits.Length > 0) rightDistance =  Vector3.Distance(hits[0].transform.position, this.transform.position);

            hits = Physics.RaycastAll(this.transform.position, this.transform.right * -1);
            float leftDistance = 0;
            if (hits.Length > 0) leftDistance = Vector3.Distance(hits[0].transform.position, this.transform.position);

            if (this.dashDirection == DashDirection.Nome) this.dashDirection = rightDistance >= leftDistance ? DashDirection.Right : DashDirection.Left;

            if ((leftDistance > this.minDistanceToDash && this.dashDirection == DashDirection.Left) || (rightDistance > this.minDistanceToDash && this.dashDirection == DashDirection.Right))
            {
                GetComponent<Rigidbody>().velocity = this.transform.right * (this.dashDirection == DashDirection.Right ? 1 : -1) * this.movementSpeed * 2;
                return;
            } else
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        Vector3 direction = this.player.transform.position - this.transform.position;
        float distance = Vector3.Distance(this.player.transform.position, this.transform.position);

        hits = Physics.RaycastAll(this.transform.position, direction, distance);
        direction.Normalize();

        if (this.intangible && !this.hitted)
        {
            GetComponent<Rigidbody>().velocity = direction * this.movementSpeed;
            return;
        }

        if (hits.Length > 0)
        {
            if (playerTagList.Contains(hits[0].collider.gameObject.tag))
            {
                GetComponent<Rigidbody>().velocity = direction * this.movementSpeed;
                return;
            }
        }

        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void HandleHitCD()
    {
        if (!this.canHit)
        {
            this.currentHitCD += 1 * Time.deltaTime;

            if (this.currentHitCD >= this.hitCD)
            {
                this.currentHitCD = 0;
                this.canHit = true;
            }
            return;
        }
    }

    void HandleIntangibleCD()
    {
        if (this.intangible)
        {
            this.currentIntangibleCD += 1 * Time.deltaTime;

            if (this.currentIntangibleCD >= this.intangibleCD)
            {
                this.currentIntangibleCD = 0;
                this.intangible = false;
                ToggleTangible(true);
                this.GetComponent<BoxCollider>().enabled = true;
            }
            return;
        }
    }

    void ToggleTangible(bool isTangible)
    {
        if (this.hitted) this.hitted = !isTangible;

        if (isTangible) this.dashDirection = DashDirection.Nome;

        Color color = this.GetComponent<SpriteRenderer>().color;
        color.a = isTangible ? 1f : 0.5f;
        this.GetComponent<SpriteRenderer>().color = color;
    }

    void GamePaused()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void GetHit()
    {
        this.life--;

        switch (this.enemyType)
        {
            case EnemyType.Small:
                AudioManager.Instance.EnemyDash(this.transform);
                this.hitted = true;
                this.intangible = true;
                ToggleTangible(false);
                break;
            case EnemyType.Big:
                if (this.life == 3)
                {
                    this.hitted = true;
                    this.intangible = true;
                    ToggleTangible(false);
                }
                break;
        }

        if (this.life <= 0)
        {
            AnxietyManager.Instance.EnemyDied(this.anxietyRecover);
            if (this.enemyUnlockController != null) this.enemyUnlockController.EnemyKilled();

            switch (this.enemyType)
            {
                case EnemyType.Small:
                    AudioManager.Instance.DeadSmallEnemy(this.transform);
                    break;
                case EnemyType.Big:
                    AudioManager.Instance.DeadBigEnemy(this.transform);
                    break;
            }

            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        HandleCollision(collision);
    }

    void HandleCollision(Collision collision)
    {
        if (playerTagHitList.Contains(collision.gameObject.tag) && this.canHit)
        {
            AudioManager.Instance.PlayerDamaged();
            AnxietyManager.Instance.IncreaseAnxiety();
            this.canHit = false;
        }
    }
}

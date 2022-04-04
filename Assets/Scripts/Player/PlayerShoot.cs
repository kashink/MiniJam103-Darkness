using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Space()]
    [Header("Config")]
    [SerializeField] GameObject hand;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileUpOffset = 0.4f;
    [SerializeField] float projectileRightOffset = 0.4f;
    [SerializeField] ProjectileController projectile;
    [SerializeField] float shootCD = 0.5f;

    float currentShootCD = 0;
    bool canShoot = true;

    bool shootUnlocked = false;

    void Update()
    {
        if (GameManager.Instance.gameState != GameState.GameRunning) return;
        if (!this.shootUnlocked) return;

        if (!this.canShoot)
        {
            this.currentShootCD += 1 * Time.deltaTime;

            if (this.currentShootCD >= this.shootCD)
            {
                this.currentShootCD = 0;
                this.canShoot = true;
            }
            return;
        }

        if (Input.GetMouseButton(0))
        {
            AudioManager.Instance.PlayerShoot();
            this.hand.GetComponent<Animator>().SetTrigger(Animator.StringToHash("Attack"));
            this.canShoot = false;
            Vector3 newPos = this.transform.position + this.transform.forward + (this.transform.right * this.projectileRightOffset) + (this.transform.up * this.projectileUpOffset);
            ProjectileController projectileInstance = Instantiate(this.projectile, newPos, this.transform.rotation);
            projectileInstance.Setup(this.projectileSpeed);
        }
    }

    public void UnlockShoot()
    {
        this.shootUnlocked = true;
        this.hand.SetActive(true);
    }
}

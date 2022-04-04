using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    [Space()]
    [Header("Config")]
    [SerializeField] bool showObject;
    [SerializeField] GameObject objectToShow;

    bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (this.activated) return;

        if (other.gameObject.tag == "PlayerInteractArea")
        {
            if (this.showObject)
            {
                this.activated = true;
                AudioManager.Instance.SpawnSmallEnemy(this.transform);
                this.objectToShow.SetActive(true);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDamageEffect : MonoBehaviour
{
    [SerializeField] private float damageIncreasePercentage = 0.1f;

    [SerializeField] private GameObject bulletPrefab; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            bulletPrefab.GetComponent<BulletController>().setDamage(bulletPrefab.GetComponent<BulletController>().getDamage() * (1 + damageIncreasePercentage));
            GameObject go = GameObject.Find("GUI");
            if (go)
            {
                PickUpEffectTextManager pickUpEffectTextManager = go.transform.Find("PickUpEffectText")?.GetComponent<PickUpEffectTextManager>();
                if (pickUpEffectTextManager)
                {
                    pickUpEffectTextManager.DisplayEffectMessage($"+{damageIncreasePercentage * 100}% Damage");
                }
            }
            Destroy(gameObject);
        }

            Destroy(gameObject);
    }
    
}



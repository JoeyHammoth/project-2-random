using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFlatDamageEffect : MonoBehaviour
{

    [SerializeField] private AudioSource eatingSteakSound;

    [SerializeField] private float damage = 5f;

    [SerializeField] private GameObject bulletPrefab; 
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            eatingSteakSound.Play();
            bulletPrefab.GetComponent<BulletController>().setDamage(bulletPrefab.GetComponent<BulletController>().getDamage() + damage);
            GameObject go = GameObject.Find("GUI");
            if (go)
            {
                PickUpEffectTextManager pickUpEffectTextManager = go.transform.Find("PickUpEffectText")?.GetComponent<PickUpEffectTextManager>();
                if (pickUpEffectTextManager)
                {
                    pickUpEffectTextManager.DisplayEffectMessage($"+{damage} Damage");
                }
            }
            StartCoroutine(Delay());
        }

        
    }
    IEnumerator<WaitUntil> Delay()
    {
        yield return new WaitUntil(() => !eatingSteakSound.isPlaying);
        Destroy(gameObject);
    }
}

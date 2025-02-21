using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

// recovers the player's health by 50%
public class RecoverEffect : MonoBehaviour
{
    [SerializeField] private float recoverPercentage = 0.5f;
    [SerializeField] private AudioSource eatingSteakSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            eatingSteakSound.Play();
            float maxHealth = other.GetComponent<PlayerHealthManager>().GetMaxHealth();
            float recover = maxHealth * recoverPercentage;
            other.GetComponent<PlayerHealthManager>().RecoverHealth(recover);
            GameObject go = GameObject.Find("GUI");
            if (go)
            {
                PickUpEffectTextManager pickUpEffectTextManager = go.transform.Find("PickUpEffectText")?.GetComponent<PickUpEffectTextManager>();
                if (pickUpEffectTextManager)
                {
                    pickUpEffectTextManager.DisplayEffectMessage($"+{recover} Health");
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


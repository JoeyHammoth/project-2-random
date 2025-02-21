using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class IncreaseMaxHPEffect : MonoBehaviour
{
    [SerializeField] private float healthIncrease = 50.0f;
    [SerializeField] private AudioSource eatingSteakSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            eatingSteakSound.Play();
            float maxHealth = other.GetComponent<PlayerHealthManager>().GetMaxHealth();
            other.GetComponent<PlayerHealthManager>().SetMaxHealth(maxHealth + healthIncrease);
            other.GetComponent<PlayerHealthManager>().RecoverHealth(healthIncrease);

            GameObject go = GameObject.Find("GUI");
            if (go)
            {
                PickUpEffectTextManager pickUpEffectTextManager = go.transform.Find("PickUpEffectText")?.GetComponent<PickUpEffectTextManager>();
                if (pickUpEffectTextManager)
                {
                    pickUpEffectTextManager.DisplayEffectMessage($"+{healthIncrease} Max HP!");
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
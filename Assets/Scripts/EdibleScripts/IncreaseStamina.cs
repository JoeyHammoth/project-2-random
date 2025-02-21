using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStamina : MonoBehaviour
{

    [SerializeField] private float staminaIncrease = 50.0f;
    [SerializeField] private AudioSource drinkingSodaSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            drinkingSodaSound.Play();
            float maxStamina = other.GetComponent<PlayerStaminaManager>().GetMaxStamina();
            float currStamina = other.GetComponent<PlayerStaminaManager>().GetStamina();
            other.GetComponent<PlayerStaminaManager>().SetMaxStamina(maxStamina + staminaIncrease);
            other.GetComponent<PlayerStaminaManager>().RecoverStamina(staminaIncrease);

            GameObject go = GameObject.Find("GUI");
            if (go)
            {
                PickUpEffectTextManager pickUpEffectTextManager = go.transform.Find("PickUpEffectText")?.GetComponent<PickUpEffectTextManager>();
                if (pickUpEffectTextManager)
                {
                    pickUpEffectTextManager.DisplayEffectMessage($"+{staminaIncrease} Max Stamina!");
                }
            }

            StartCoroutine(Delay());
        }
    }

    IEnumerator<WaitUntil> Delay()
    {
        yield return new WaitUntil(() => !drinkingSodaSound.isPlaying);
        Destroy(gameObject);
    }
}

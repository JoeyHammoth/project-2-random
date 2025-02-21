using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushroomEffect : MonoBehaviour
{   
    [SerializeField] private AudioSource eatingSteakSound;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {   
            eatingSteakSound.Play();
            other.GetComponent<PlayerController>().addMushroom();
            GameObject go = GameObject.Find("GUI");
            if (go)
            {
                PickUpEffectTextManager pickUpEffectTextManager = go.transform.Find("PickUpEffectText")?.GetComponent<PickUpEffectTextManager>();
                if (pickUpEffectTextManager)
                {
                    pickUpEffectTextManager.DisplayEffectMessage($"Hp regens while standing still");
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

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EdibleScripts
{
    public class SodaEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource drinkingSodaSound;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) 
            {
                drinkingSodaSound.Play();
                float speed = other.GetComponent<PlayerController>().getBaseSpeed();
                speed += 0.1f;
                other.GetComponent<PlayerController>().setBaseSpeed(speed);
            
                GameObject go = GameObject.Find("GUI");
                if (go)
                {
                    PickUpEffectTextManager pickUpEffectTextManager = go.transform.Find("PickUpEffectText")?.GetComponent<PickUpEffectTextManager>();
                    if (pickUpEffectTextManager)
                    {
                        pickUpEffectTextManager.DisplayEffectMessage($"+10% Speed");
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
}
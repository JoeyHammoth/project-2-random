using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private int damageToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(this.velocity * Time.deltaTime);
        transform.Rotate(Vector3.forward * 300 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthManager>().TakeDamage(damageToPlayer);
        }
    }
}

using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class GrenadeController : MonoBehaviour
    {
        [SerializeField] private float fuseTime = 3f;
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private float explosionForce = 700f;
        [SerializeField] private GameObject explosionEffect;
        [SerializeField] private int explosionDamage = 50;

        private bool hasExploded;
        
        // Cooldown variables
        private static float lastGrenadeTime = -2f; 
        private const float grenadeCooldown = 2f;

        private void Start()
        {
            if(Time.time - lastGrenadeTime < grenadeCooldown)
            {
                // If grenade is thrown within the cooldown time, destroy it without exploding
                Destroy(gameObject);
                return;
            }
            
            // Update the time of the last grenade thrown
            lastGrenadeTime = Time.time;
            
            Invoke("Explode", fuseTime);
        }

        void Explode()
        {
            if (hasExploded) return;
            hasExploded = true;
            
            var explosionParticles = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosionParticles, 3f); 

            Collider[] objectsToDamage = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider nearbyObject in objectsToDamage)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }

                if (nearbyObject.CompareTag("Player"))
                {
                    PlayerHealthManager damageableObject = nearbyObject.GetComponent<PlayerHealthManager>();
                    if (damageableObject != null)
                    {
                        damageableObject.TakeDamage(explosionDamage);
                    }
                } 
                else if (nearbyObject.CompareTag("Zombies"))
                {
                    ZombieMeleeController damageableObject = nearbyObject.GetComponent<ZombieMeleeController>();
                    if (damageableObject != null)
                    {
                        damageableObject.takeGrenadeDamage(explosionDamage);
                    }

                    ZombieRangedController damageableObject2 = nearbyObject.GetComponent<ZombieRangedController>();
                    if (damageableObject2 != null)
                    {
                        damageableObject2.takeGrenadeDamage(explosionDamage);
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}

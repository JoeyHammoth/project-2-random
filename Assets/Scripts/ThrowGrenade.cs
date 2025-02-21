using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab; 
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private AudioSource grenadeThrowAndExplodeSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Throw();
            grenadeThrowAndExplodeSound.Play();
        }
    }

    void Throw()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);

        Rigidbody rb = grenade.AddComponent<Rigidbody>(); 
        rb.velocity = transform.forward * throwForce;   
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField] private Vector3 velocity;
    [SerializeField] private float damageToZombie;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(this.velocity * Time.deltaTime);
    }

    public float getDamage()
    {
        return damageToZombie;
    }

    public void setDamage(float damage)
    {
        this.damageToZombie = damage;
    }

}

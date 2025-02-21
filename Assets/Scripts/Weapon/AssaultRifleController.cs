using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Weapon
{
    public class AssaultRifleController : WeaponController
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float fireRate = 0.1f;
        [SerializeField] private float nextFireTime;
        [SerializeField] private AudioSource rifleShootingSound;
        [SerializeField] private AudioSource rifleSwitchSound;
        private GameObject player;
        private float playerHeath;
        Quickslot quickslot;


        // Update is called once per frame
        private void Start()
        {
            quickslot = GameObject.Find("Player").GetComponent<Quickslot>();
            UpdateBulletUI();
            player = GameObject.FindGameObjectWithTag("Player");
            playerHeath = player.GetComponent<PlayerHealthManager>().GetHealth();
        }

        void Update()
        {
            if (playerHeath <= 0) return;
        
            if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextFireTime)
            {
                Fire();
                rifleShootingSound.Play();
                nextFireTime = Time.time + fireRate;
            }
        }

        private void UpdateBulletUI()
        {
            Text go = GameObject.Find("CurrAmmo_"+quickslot.currSlot).GetComponent<Text>();
            go.text = "" + quickslot.slots[quickslot.currSlot].nbLeft;
        }

        public void Switch()
        {
            rifleSwitchSound.Play();
        }

        public override void Fire()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity * firePoint.rotation);
        
            quickslot.slots[quickslot.currSlot].nbLeft--;
            UpdateBulletUI();
            if (quickslot.slots[quickslot.currSlot].nbLeft <= 0)
            {
            
                quickslot.slots[quickslot.currSlot].slotItem = null;
                quickslot.slots[quickslot.currSlot].isEmpty = true;
                GameObject currSlot = GameObject.Find("Slot_" + quickslot.currSlot);
                foreach (Transform child in currSlot.transform)
                {
                    Destroy(child.gameObject);
                }
                quickslot.slots[quickslot.currSlot].itemName = "";
                Destroy(gameObject);
            }
        

        }
    


    }
}
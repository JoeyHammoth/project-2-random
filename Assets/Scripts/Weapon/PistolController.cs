using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Weapon
{
    public class PistolController : WeaponController
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private AudioSource pistolShootingSound;
        [SerializeField] private AudioSource pistolSwitchSound;
        private GameObject player;
        private float playerHeath;
        Quickslot quickslot;
    
        private void Start()
        {
            quickslot = GameObject.Find("Player").GetComponent<Quickslot>();
            UpdateBulletUI();
            player = GameObject.FindGameObjectWithTag("Player");
            playerHeath = player.GetComponent<PlayerHealthManager>().GetHealth();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerHeath <= 0) return;
        
            if (Input.GetKeyDown(KeyCode.Mouse0)) 
            {
                Fire();
                pistolShootingSound.Play();
            }
        }
        private void UpdateBulletUI()
        {
            Text go = GameObject.Find("CurrAmmo_"+quickslot.currSlot).GetComponent<Text>();
            go.text = "" + quickslot.slots[quickslot.currSlot].nbLeft;
        }

        public void Switch()
        {
            pistolSwitchSound.Play();
        }
    
        public override void Fire()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position,Quaternion.identity * firePoint.rotation);
        
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
                Destroy(gameObject);
            
            }

        
        }
    

    }
}


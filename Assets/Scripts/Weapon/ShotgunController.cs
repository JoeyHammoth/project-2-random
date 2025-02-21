using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace Assets.Scripts.Weapon
{
    public class ShotgunController : WeaponController
    {
        [SerializeField] private Transform firePoint; 
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int pelletsCount = 8; 
        [SerializeField] private float spreadAngle = 20f;
        [SerializeField] private float fireRate = 0.8f;
        [SerializeField] private AudioSource shotgunShootingSound;
        [SerializeField] private AudioSource shotgunSwitchSound;
        private float timeToFire;
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
        
            if (Input.GetKey(KeyCode.Mouse0) && Time.time >= timeToFire) 
            {
                timeToFire = Time.time + 1 / fireRate;
                Fire();
                shotgunShootingSound.Play();
            }
        }
    
        private void UpdateBulletUI()
        {
            Text go = GameObject.Find("CurrAmmo_"+quickslot.currSlot).GetComponent<Text>();
            go.text = "" + quickslot.slots[quickslot.currSlot].nbLeft;
        }

        public void Switch()
        {
            shotgunSwitchSound.Play();
        }
    
        public override void Fire()
        {
            for (int i = 0; i < pelletsCount; i++)
            {
                float spreadRandomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
                Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0, spreadRandomAngle, 0);

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

            }
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
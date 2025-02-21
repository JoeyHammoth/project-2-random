using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerHealthManager : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 1000;
        private float currHealth;
        public PlayerHealthBar healthBar;
        private bool isAttacked;
        [SerializeField] private AudioSource takeDamageSound;
        [SerializeField] private AudioSource lowHealthSound;
        
        // Start is called before the first frame update
        void Start()
        {  
            currHealth = maxHealth; 
            healthBar.SetMaxHealth(maxHealth);
        }

        // Update is called once per frame
        void Update()
        {
            isAttacked = false;
        
            if (currHealth > 0 && currHealth <= 200 && !lowHealthSound.isPlaying)
            {
                lowHealthSound.Play();
            }
            
            if (currHealth <= 0)
            {
                GameObject go = GameObject.Find("GUI");
                GameOverManager gameOverManager = go.GetComponent<GameOverManager>();
                GameObject.Find("Player").GetComponent<PlayerController>().currentState =
                    PlayerController.GameState.GameOver;
                gameOverManager.GameOver();
            }
        }

        public bool getAttackStat() {
            return isAttacked;
        }

        public void TakeDamage(float damage)
        {
            if (currHealth > 0)
            {
                takeDamageSound.Play();
                currHealth -= damage;
                healthBar.UpdateHealthText(currHealth, maxHealth);
                healthBar.SetHealth(currHealth);
                isAttacked = true;
            
            }
        }

        public void RecoverHealth(float recover)
        {
            if (currHealth < maxHealth)
            {
                currHealth += recover;
                if (currHealth > maxHealth)
                {
                    currHealth = maxHealth;
                }
                healthBar.UpdateHealthText(currHealth, maxHealth);
                healthBar.SetHealth(currHealth);
            }
        }

        public void NotMovingRecoverHealth(int mushroomCount)
        {
            float recover = mushroomCount * maxHealth * 0.01f;
            RecoverHealth(recover);
        }

        public void SetMaxHealth(float maxHealth)
        {
            this.maxHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        public float GetHealth()
        {
            return currHealth;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }
    
    }
}
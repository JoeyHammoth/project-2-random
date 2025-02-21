using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public struct Phase
    {
        public int numberOfZombies;   
        public float spawnDuration;  
        public float breakDuration;   
        public int meleeZombieRate;
        public int rangedZombieRate;
        public int giantZombieRate;
    }

    public class ZombieSpawning : MonoBehaviour
    {
        public Phase[] phases;
        private Transform parentGameObject;
        [SerializeField] private Transform[] spawnPoints; 
        [SerializeField] private GameObject[] zombieGiantVariants;
        [SerializeField] private GameObject[] zombieMeleeVariants;
        [SerializeField] private GameObject[] zombieRangedVariants;
        [SerializeField] private GameObject zombieBoss;
    
        private void Start()
        {
            parentGameObject = GameObject.FindGameObjectWithTag("ZombiesContainer").transform;
            StartCoroutine(PhaseController());
        }

        IEnumerator PhaseController()
        {
            GameObject go = GameObject.Find("GUI");
            PickUpEffectTextManager phaseTextManager = go.transform.Find("PhaseText")?.GetComponent<PickUpEffectTextManager>();
            for (int i = 0; i < phases.Length; i++)
            {
                Debug.Log($"Starting Phase {i + 1}"); 
                
                if (phaseTextManager)
                {
                    Debug.Log("worked");

                    if (i == 2)
                    {
                        phaseTextManager.DisplayEffectMessage($"Warning: Boss Approaching!");
                    }
                    else
                    {
                        phaseTextManager.DisplayEffectMessage($"Phase {i+1}");
                    }
                   
                }
            
                yield return StartCoroutine(SpawnZombie(phases[i].numberOfZombies, phases[i].spawnDuration / phases[i].numberOfZombies,phases[i].meleeZombieRate,phases[i].rangedZombieRate,phases[i].giantZombieRate,i+1));
                yield return StartCoroutine(WaitUntilZombiesDead());
                yield return new WaitForSeconds(phases[i].breakDuration);
            }

            if (phaseTextManager)
            {
                Debug.Log("All phases completed!");
                phaseTextManager.DisplayEffectMessage($"Congratulations!");
            }

        }

        IEnumerator SpawnZombie(int zombieCount, float spawnInterval,int meleeZombRate,int rangedZombRate,int giantZombRate,int currPhase)
        {
            int spawnedZombies = 0;
            if (currPhase == 3)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject zombieType = zombieBoss;
                if (UnityEngine.AI.NavMesh.SamplePosition(spawnPoint.position, out _, 1.0f,
                        UnityEngine.AI.NavMesh.AllAreas))
                {
                    Instantiate(zombieType, spawnPoint.position, Quaternion.identity, parentGameObject);
                    Debug.Log("boss spawned");
                    spawnedZombies++;
                }

            }
        
            while (spawnedZombies < zombieCount)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject zombieType = PickRandZombie(meleeZombRate, rangedZombRate, giantZombRate);
            
                if (UnityEngine.AI.NavMesh.SamplePosition(spawnPoint.position, out _, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    Instantiate(zombieType, spawnPoint.position, Quaternion.identity, parentGameObject);
                    spawnedZombies++;
                    yield return new WaitForSeconds(spawnInterval);
                }
            }
        }

        IEnumerator WaitUntilZombiesDead()
        {
            while(true)
            {
                GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombies");
                if (zombies.Length > 0)
                {
                    yield return null;  // Wait one frame and check again
                }
                else
                {
                    break;
                }
            }
        }

        private GameObject PickRandZombie(int meleeZombRate,int rangedZombRate,int giantZombRate)
        {
            float rnd = Random.Range(0, 100);
            GameObject chosenZombie = null;
            if (rnd < meleeZombRate) 
            {
                chosenZombie = zombieMeleeVariants[Random.Range(0, zombieMeleeVariants.Length)];
            }
            else if (rnd < (meleeZombRate + rangedZombRate))
            {
                chosenZombie = zombieRangedVariants[Random.Range(0, zombieRangedVariants.Length)];
            }
            else if (rnd < (meleeZombRate + rangedZombRate + giantZombRate))
            {
                chosenZombie = zombieGiantVariants[Random.Range(0, zombieGiantVariants.Length)];
            }
            return chosenZombie;
        }
    }
}
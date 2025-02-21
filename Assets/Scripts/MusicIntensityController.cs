using UnityEngine;

public class MusicIntensityController : MonoBehaviour
{

    private readonly int radius = 30;
    [SerializeField] private AudioSource level1;
    [SerializeField] private AudioSource level2;
    [SerializeField] private AudioSource level3;
    [SerializeField] private AudioSource level4;
    [SerializeField] private AudioSource level5;
    [SerializeField] private AudioSource level6;
    static private AudioSource _currentLevel;

    private void Start()
    {
        // TODO: More music needed
        level2 = level1;
        level3 = level1;
        level4 = level1;
        level5 = level1;
        level6 = level1;
    }

    private void Update()
    {
        int numberOfZombies = NumberOfEnemiesWithinRadius();
        AudioSource transitionLevelTo;

        if (numberOfZombies < 10)
        {
            transitionLevelTo = level1;
        }
        else if (numberOfZombies < 20)
        {
            transitionLevelTo = level2;
        }
        else if (numberOfZombies < 30)
        {
            transitionLevelTo = level3;
        }        
        else if (numberOfZombies < 40)
        {
            transitionLevelTo = level4;
        }
        else if (numberOfZombies < 50)
        {
            transitionLevelTo = level5;
        }
        else
        {
            transitionLevelTo = level6;
        }

        if (transitionLevelTo != _currentLevel)
        {
            transitionLevelTo.Play();
            _currentLevel = transitionLevelTo;
        }

    }

    private int NumberOfEnemiesWithinRadius()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombies");
        Vector3 playerPosition = player.transform.position;
        int numberOfZombies = 0;
        
        foreach (GameObject zombie in zombies)
        {
            Vector3 zombiePosition = zombie.transform.position;
            
            if (Vector3.Distance(playerPosition, zombiePosition) <= radius)
            {
                numberOfZombies++;
            }
        }
        return numberOfZombies;
    }
    
    
}

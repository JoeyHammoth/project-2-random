using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Currently, the script randomizes the placement of one type of object in a specified terrain. 

// Easily extendable to other objects, but the terrain must be specified in the inspector.

public class RandomizedObjPlacement : MonoBehaviour
{
    [SerializeField] private GameObject terrain;
    [SerializeField] private GameObject gameObj;
    [SerializeField] private int numGameObj;
    private Renderer ter;
    private Renderer obj;

    private float topLeftX;
    private float topLeftZ;
    private float bottomRightX;
    private float bottomRightZ;

    private float gameObjLengthX;
    private float gameObjLengthZ;
    private float gameObjTopLeftX;
    private float gameObjTopLeftZ;
    private float gameObjBottomRightX;
    private float gameObjBottomRightZ;
    private List<float[]> gameObjCoords = new List<float[]>(); 

    // Start is called before the first frame update
    void Start()
    {
        ter = terrain.GetComponent<Renderer>();

        topLeftX = ter.bounds.min.x;
        topLeftZ = ter.bounds.max.z;
        bottomRightX = ter.bounds.max.x;
        bottomRightZ = ter.bounds.min.z;

        obj = gameObj.GetComponent<Renderer>();

        gameObjLengthX = obj.bounds.size.x;
        gameObjLengthZ = obj.bounds.size.z;

        for (int i = 0; i < numGameObj; i++) {
            Vector3 randomSpawnPos = GenerateRandPos();
            Instantiate(gameObj, randomSpawnPos, Quaternion.identity);
        }
        
    }

    Vector3 GenerateRandPos() 
    {
        Vector3 randomSpawnPos = new Vector3(Random.Range(topLeftX, bottomRightX), 5, Random.Range(topLeftZ, bottomRightZ));
        gameObjTopLeftX = randomSpawnPos.x - (gameObjLengthX / 2);
        gameObjTopLeftZ = randomSpawnPos.z + (gameObjLengthZ / 2);
        gameObjBottomRightX = randomSpawnPos.x + (gameObjLengthX / 2);
        gameObjBottomRightZ = randomSpawnPos.z - (gameObjLengthZ / 2);
        float[] gameObjArray = {gameObjTopLeftX, gameObjTopLeftZ, gameObjBottomRightX,gameObjBottomRightZ};

        if (!RectContainsRect(gameObjArray) || CheckIntersection(gameObjArray)) {
            return GenerateRandPos();
        } 
        gameObjCoords.Add(gameObjArray);
        return randomSpawnPos;
    }

    // Check if the objects intersects with any other objects (to prevent collisions)
    bool CheckIntersection(float[] arr) {
        foreach (float[] coord in gameObjCoords) {
            if (arr[0] < coord[2] && arr[2] > coord[0] && arr[1] > coord[3] && arr[3] < coord[1]) {
                return true;
            }
        }
        return false;
    }

    // Checker making sure objects are situated within the terrain

    bool RectContainsRect(float[] arr) {
        if (arr[0] < topLeftX || arr[1] > topLeftZ || arr[2] > bottomRightX || arr[3] < bottomRightZ) {
            return false;
        }
        return true;
    }
}

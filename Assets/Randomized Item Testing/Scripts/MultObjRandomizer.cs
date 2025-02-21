using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class manTerList {
    public float topLeftX;
    public float topLeftZ;
    public float bottomRightX;
    public float bottomRightZ;
}

public class MultObjRandomizer : MonoBehaviour
{   
    [SerializeField] private GameObject[] terrain;
    [SerializeField] private bool useManualTerrain = false;
    [SerializeField] private manTerList[] manualTerrainList;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private bool randomizeNumObjects = false;

    // If randomizeNumObjects is true, the numObjects is set to max. 
    [SerializeField] private int[] numObjects;

    // Only use if randomizeNumObjects is true. Set to 1 if empty but bool is true
    [SerializeField] private int[] minNumObjects;
    [SerializeField] private float spawnHeight = 5;
    [SerializeField] private float terrainSpaceLimiter = 0;
    [SerializeField] private float rotation = 180;

    // The objects will not spawn in the area specified by forbiddenArea (area where the player spawns)
    [SerializeField] private manTerList forbiddenArea;

    // Dictionary used for instantiating objects
    private IDictionary<GameObject, int> objDict = new Dictionary<GameObject, int>();

    // Coords of terrain
    private List<float[]> terCoords = new List<float[]>();

    // Lists pertaining to the lengths of the type of objects and the coordinates of all objects
    private List<float[]> ObjLengths = new List<float[]>();
    private List<float[]> totalObjcoords = new List<float[]>();

    private int numTerrain;

    // Start is called before the first frame update
    void Start()
    {
        // Get number of terrain
        numTerrain = terrain.Length;
        if (useManualTerrain) {
            numTerrain += manualTerrainList.Length;
            foreach (manTerList ter in manualTerrainList) {
                float[] terCoordsTemp = new float[4];
                terCoordsTemp[0] = ter.topLeftX;
                terCoordsTemp[1] = ter.topLeftZ;
                terCoordsTemp[2] = ter.bottomRightX;
                terCoordsTemp[3] = ter.bottomRightZ;
                terCoords.Add(terCoordsTemp);
            }
        }
        Debug.Log("Number of terrain: " + numTerrain);

        // Put objects as key and object number as value in dictionary
        for (int i = 0; i < objects.Length; i++)
        {   
            // Randomize number of objects if randomizeNumObjects is true. 
            if (randomizeNumObjects) {
                try {
                    numObjects[i] = UnityEngine.Random.Range(minNumObjects[i], numObjects[i]);
                } catch (System.IndexOutOfRangeException) {
                    Debug.Log("Min number of objects is set to 1.");
                    numObjects[i] = UnityEngine.Random.Range(1, numObjects[i]);
                }
            }
            objDict.Add(objects[i], numObjects[i]);
        }

        // Get terrain coordinates
        if (!useManualTerrain) {
            foreach (GameObject ter in terrain) {
                Renderer renTer = ter.GetComponent<Renderer>();
                float[] oneTerCoords = new float[4];
                oneTerCoords[0] = renTer.bounds.min.x + terrainSpaceLimiter;
                oneTerCoords[1] = renTer.bounds.max.z - terrainSpaceLimiter;
                oneTerCoords[2] = renTer.bounds.max.x - terrainSpaceLimiter;
                oneTerCoords[3] = renTer.bounds.min.z + terrainSpaceLimiter;
                terCoords.Add(oneTerCoords);
            }
        }

        // Get object coordinates
        // Each object has 2 float coordinates, top left and bottom right, which are kept sequentially in the list
        foreach (GameObject obj in objects)
        {
            Renderer gameObj = obj.GetComponent<Renderer>();
            float gameObjLengthX = gameObj.bounds.size.x;
            float gameObjLengthZ = gameObj.bounds.size.z;
            float[] gameObjLengths = {gameObjLengthX, gameObjLengthZ};
            ObjLengths.Add(gameObjLengths);
        }

        int j = 0;

        // Generate random positions for each object and instantiate
        foreach (KeyValuePair<GameObject, int> entry in objDict)
        {
            for (int i = 0; i < entry.Value; i++)
            {
                Vector3 randomSpawnPos = GenerateRandPos(ObjLengths[j]);
                Instantiate(entry.Key, randomSpawnPos, Quaternion.Euler(0, UnityEngine.Random.Range(0, rotation), 0));
            }
            j++;
        }
    }

    // Generate random positions for each object
    Vector3 GenerateRandPos(float[] gameObjectLengths) {
        int terIndex = UnityEngine.Random.Range(0, numTerrain);
        Vector3 randomSpawnPos = new Vector3(UnityEngine.Random.Range(terCoords[terIndex][2], terCoords[terIndex][0]), spawnHeight, UnityEngine.Random.Range(terCoords[terIndex][3], terCoords[terIndex][1]));
        float gameObjectTopLeftX = randomSpawnPos.x - (gameObjectLengths[0] / 2);
        float gameObjectTopLeftZ = randomSpawnPos.z + (gameObjectLengths[1] / 2);
        float gameObjectBottomRightX = randomSpawnPos.x + (gameObjectLengths[0] / 2);
        float gameObjectBottomRightZ = randomSpawnPos.z - (gameObjectLengths[1] / 2);
        float[] gameObjectArray = {gameObjectTopLeftX, gameObjectTopLeftZ, gameObjectBottomRightX,gameObjectBottomRightZ};

        if (!CheckTerrainIntersection(gameObjectArray, terIndex) || CheckObjIntersection(gameObjectArray) || CheckForbiddenArea(gameObjectArray)) {
            return GenerateRandPos(gameObjectLengths);
        }
        totalObjcoords.Add(gameObjectArray);
        return randomSpawnPos;
    }

    // Check if the object is intersecting with any other objects.
    bool CheckObjIntersection(float[] objCoordArr) {
        foreach (float[] coord in totalObjcoords) {
            if (objCoordArr[0] < coord[2] && objCoordArr[2] > coord[0] && objCoordArr[1] > coord[3] && objCoordArr[3] < coord[1]) {
                return true;
            }
        }
        return false;
    }

    // Check if the object is fully within the terrain.
    bool CheckTerrainIntersection(float[] objCoordArr, int terIndex) {
        if (objCoordArr[0] < terCoords[terIndex][2] || objCoordArr[2] > terCoords[terIndex][0] || objCoordArr[1] > terCoords[terIndex][1] || objCoordArr[3] < terCoords[terIndex][3]) {
            return true;
        }
        return false;
    }

    // Check if the object is within the forbidden area.
    bool CheckForbiddenArea(float[] objCoordArr) {
        if (objCoordArr[0] < forbiddenArea.bottomRightX && objCoordArr[2] > forbiddenArea.topLeftX && objCoordArr[1] > forbiddenArea.bottomRightZ && objCoordArr[3] < forbiddenArea.topLeftZ) {
            return true;
        } 
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectRandomizer : MonoBehaviour
{
    [SerializeField] private Vector3[] objPos;
    [SerializeField] private GameObject[] objPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Vector3 loc in objPos) {
            int rand = Random.Range(0, objPrefabs.Length);
            Instantiate(objPrefabs[rand], loc, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

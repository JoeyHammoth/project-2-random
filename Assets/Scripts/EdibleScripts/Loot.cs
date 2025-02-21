using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot")]
public class Loot : ScriptableObject
{
    public GameObject lootPrefab;
    public string lootName;
    public float dropRate;


}

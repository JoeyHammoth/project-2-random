using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot Table")]
public class LootTable : ScriptableObject
{
    public List<Loot> lootList = new List<Loot>();
}

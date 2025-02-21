using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SlotData
{
    
    public bool isEmpty;
    [FormerlySerializedAs("slotObj")] public GameObject slotImg;
    public GameObject slotItem;
    public int nbLeft;
    public String itemName = "";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

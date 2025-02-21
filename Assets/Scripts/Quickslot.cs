using System.Collections.Generic;
using Assets.Scripts.Weapon;
using UnityEngine;

public class  Quickslot : MonoBehaviour
{
    public List<SlotData> slots;
    public GameObject slotPrefab;
    public int currSlot;
    public GameObject nbText;
    private static string _currentWeapon;

    private int maxSlot = 7;
    // Start is called before the first frame update
    
    private int weaponSlotHold = 8;
    void Start()
    {
        GameObject slotPanel = GameObject.Find("Panel");
        for (int i = 0; i < maxSlot; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotPanel.transform,false);
            go.name = "Slot_" + i;
            SlotData slot = new SlotData();
            slot.isEmpty = true;
            slot.slotImg = go;
            slots.Add(slot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SwitchSlot();
    }

    private void SwitchSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!slots[0].isEmpty)
            {
                currSlot = 0;
                SpawnItem(slots[0].slotItem);  
                slots[0].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.red; 
                if (weaponSlotHold != 8 && weaponSlotHold != 0) {
                    slots[weaponSlotHold].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.white;
                }
                weaponSlotHold = 0; 
            }
        } 
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!slots[1].isEmpty)
            {
                currSlot = 1;
                SpawnItem(slots[1].slotItem);   
                slots[1].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.red;
                if (weaponSlotHold != 8 && weaponSlotHold != 1) {
                    slots[weaponSlotHold].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.white;
                }
                weaponSlotHold = 1;
            }
        } 
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!slots[2].isEmpty)
            {
                currSlot = 2;
                SpawnItem(slots[2].slotItem);  
                slots[2].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.red; 
                if (weaponSlotHold != 8 && weaponSlotHold != 2) {
                    slots[weaponSlotHold].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.white;
                }
                weaponSlotHold = 2;
            }
        } 
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!slots[3].isEmpty)
            {
                currSlot = 3;
                SpawnItem(slots[3].slotItem);   
                slots[3].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.red;
                if (weaponSlotHold != 8 && weaponSlotHold != 3) {
                    slots[weaponSlotHold].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.white;
                }
                weaponSlotHold = 3;
            }
        } 
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (!slots[4].isEmpty)
            {
                currSlot = 4;
                SpawnItem(slots[4].slotItem); 
                slots[4].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.red;  
                if (weaponSlotHold != 8 && weaponSlotHold != 4) {
                    slots[weaponSlotHold].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.white;
                }
                weaponSlotHold = 4;
            }
        } 
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (!slots[5].isEmpty)
            {
                currSlot = 5;
                SpawnItem(slots[5].slotItem);  
                slots[5].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.red; 
                if (weaponSlotHold != 8 && weaponSlotHold != 5) {
                    slots[weaponSlotHold].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.white;
                }
                weaponSlotHold = 5;
            }
        } 
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (!slots[6].isEmpty)
            {
                currSlot = 6;
                SpawnItem(slots[6].slotItem);  
                slots[6].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.red; 
                if (weaponSlotHold != 8 && weaponSlotHold != 6) {
                    slots[weaponSlotHold].slotImg.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.white;
                }
                weaponSlotHold = 6;
            }
        } 
    }

    private void SpawnItem(GameObject slotItem)
    {
        GameObject weaponObject = GameObject.FindWithTag("Weapon");
        if (weaponObject != null)
        {
            Destroy(weaponObject);
        }

        if (slotItem == null)
        {
            return;
        }
        
        GameObject newWeapon = Instantiate(slotItem);
        
        if (slotItem.name.Equals("Pistol"))
        {
            if (_currentWeapon != "Pistol")
            {
                newWeapon.GetComponent<PistolController>().Switch();
            }
            newWeapon.transform.SetParent(transform);
            newWeapon.transform.localPosition = new Vector3(0.0f, 1.4f, 0.52f);
            newWeapon.transform.localRotation = Quaternion.AngleAxis(90.0f,Vector3.up);
            newWeapon.name = "Pistol";
        }

        else if (slotItem.name.Equals("AssaultRifle"))
        {
            if (_currentWeapon != "AssaultRifle")
            {
                newWeapon.GetComponent<AssaultRifleController>().Switch();
            }
            newWeapon.transform.SetParent(transform);
            newWeapon.transform.localPosition = new Vector3(0.0f, 1.4f, 0.45f);
            newWeapon.transform.localRotation = Quaternion.AngleAxis(90.0f,Vector3.up);
            newWeapon.name = "AssaultRifle";

        }
        
        else if (slotItem.name.Equals("Shotgun"))
        {
            if (_currentWeapon != "Shotgun")
            {
                newWeapon.GetComponent<ShotgunController>().Switch();
            }
            
            newWeapon.transform.SetParent(transform);
            newWeapon.transform.localPosition = new Vector3(0.0f, 1.4f, 0.65f);
            newWeapon.transform.localRotation = Quaternion.AngleAxis(0.0f,Vector3.up);
            newWeapon.name = "Shotgun";
        }

        _currentWeapon = newWeapon.name;
    }

    
}

using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ItemCollect : MonoBehaviour
    {
        [FormerlySerializedAs("slotItem")] public GameObject slotImg;
        public GameObject slotItem;
        [SerializeField] private int count;
        public String itemName;

        [SerializeField] private Shader initialShader;
        [SerializeField] private Shader newShader;
        [SerializeField] private Material weaponMat;
        [SerializeField] private AudioSource pickUpWeaponSound;

        private void Start() {
            weaponMat.shader = initialShader;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                pickUpWeaponSound.Play();
                
                bool emptySlotExist = false;
                bool weaponExist = false;
                int idx = -1;
                Quickslot quickslot = other.GetComponent<Quickslot>();
                for (int i = 0; i < quickslot.slots.Count; i++)
                {
                    if (quickslot.slots[i].itemName.Equals(itemName) )
                    {
                        weaponExist = true;
                        idx = i;
                        break;
                    }

                    if (quickslot.slots[i].isEmpty && !emptySlotExist )
                    {
                        emptySlotExist = true;
                        idx = i;
                    
                    }
                
                }
                if (weaponExist)
                {
                    quickslot.slots[idx].nbLeft += count;
                    var newText = quickslot.nbText.GetComponent<Text>();
                    newText.text = "" + quickslot.slots[idx].nbLeft;
                    GameObject go = GameObject.Find("CurrAmmo_"+idx);
                    Destroy(go);
                    GameObject txt = Instantiate(quickslot.nbText, quickslot.slots[idx].slotImg.transform, false);
                    txt.name = "CurrAmmo_" + idx;
                    weaponMat.shader = newShader;
                    Destroy(this.gameObject);
                
                }else if (emptySlotExist)
                {
                    Instantiate(slotImg, quickslot.slots[idx].slotImg.transform, false);
                    quickslot.slots[idx].nbLeft = count;
                    var newText = quickslot.nbText.GetComponent<Text>();
                    newText.text = "" + quickslot.slots[idx].nbLeft;
                    GameObject txt = Instantiate(quickslot.nbText, quickslot.slots[idx].slotImg.transform, false);
                    txt.name = "CurrAmmo_" + idx;
                    quickslot.slots[idx].slotItem = slotItem;
                    quickslot.slots[idx].isEmpty = false;
                    quickslot.slots[idx].itemName = itemName;
                    weaponMat.shader = newShader;
                    Destroy(this.gameObject);
                }
        
            }
        }
    }
}

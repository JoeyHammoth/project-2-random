using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaminaManager : MonoBehaviour
{
    [SerializeField] private float maxStamina = 1000;
    [SerializeField] private float currStamina;
    [SerializeField] private float staminaRecoverRate = 0.5f;
    [SerializeField] private float staminaDecRate = 1.0f;
    [SerializeField] private bool enableStaminaText = true;
    public PlayerStaminaBar staminaBar;

    // Start is called before the first frame update
    void Start()
    {
        currStamina = maxStamina;
        staminaBar.SetMaxHealth(maxStamina);  
        if (!enableStaminaText) {
            staminaBar.HideText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currStamina < maxStamina) {
            currStamina += staminaRecoverRate;
        }
        if (enableStaminaText) {
            staminaBar.UpdateStaminaText(currStamina, maxStamina);  
        }
        staminaBar.SetStamina(currStamina);
    }

    public void Run() {
        if (currStamina > 0) {
            currStamina -= staminaDecRate;
            if (enableStaminaText) {
                staminaBar.UpdateStaminaText(currStamina, maxStamina);  
            } 
            staminaBar.SetStamina(currStamina);
        }
    }

    public void RecoverStamina(float stamina) {
        currStamina += stamina;
        if (currStamina > maxStamina) {
            currStamina = maxStamina;
        }
        if (enableStaminaText) {
            staminaBar.UpdateStaminaText(currStamina, maxStamina);  
        }
        staminaBar.SetStamina(currStamina);
    }

    public void SetMaxStamina(float stamina) {
        maxStamina = stamina;
        staminaBar.SetMaxHealth(maxStamina);  
        if (enableStaminaText) {
            staminaBar.UpdateStaminaText(currStamina, maxStamina);  
        }
        staminaBar.SetStamina(currStamina);
    }

    public float GetStamina() {
        return currStamina;
    }

    public float GetMaxStamina() {
        return maxStamina;
    }
}

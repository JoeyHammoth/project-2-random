using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStaminaBar : MonoBehaviour
{

    [SerializeField] private TMP_Text StaminaText;

    public Slider slider;

    private void Start()
    {
        UpdateStaminaText(slider.value, slider.maxValue);
    }


    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    } 

    public void SetStamina(float stamina)
    {
        slider.value = stamina;
    }

    public void UpdateStaminaText(float currStamina, float maxStamina)
    {
        StaminaText.SetText($"{currStamina}/{maxStamina}");
    }

    public void HideText()
    {
        StaminaText.enabled = false;
    }


}

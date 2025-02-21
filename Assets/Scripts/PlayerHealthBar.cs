using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{

    [SerializeField] private TMP_Text healthText;

    public Slider slider;

    private void Start()
    {
        UpdateHealthText(slider.value, slider.maxValue);
    }


    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    } 

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void UpdateHealthText(float currHealth, float maxHealth)
    {
        healthText.SetText($"{currHealth}/{maxHealth}");
    }


}

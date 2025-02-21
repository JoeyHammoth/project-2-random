using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickUpEffectTextManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pickUpEffectText;
    [SerializeField] private float displayDuration = 3.0f;
    

    // Show the pickup effect message and then hide it after the display duration
    public void DisplayEffectMessage(string message)
    {
        pickUpEffectText.SetText(message);
        pickUpEffectText.gameObject.SetActive(true);
        Invoke("HideEffectMessage", displayDuration);
    }

    // Hide the pickup effect message
    private void HideEffectMessage()
    {
        pickUpEffectText.gameObject.SetActive(false);
    }

    private void Start()
    {
        // Ensure the message is hidden initially
        pickUpEffectText.gameObject.SetActive(false);
    }
}

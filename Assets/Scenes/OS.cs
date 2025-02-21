using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class OS : MonoBehaviour
{
    [SerializeField] public static bool isWindows = false;

    public bool fromGame = SceneChanger.fromGame;

    [SerializeField] private GameObject macButton;
    [SerializeField] private GameObject windowsButton;

    // Start is called before the first frame update
    void Start()
    {
        if (fromGame == true) {
            isWindows = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isPressedMAC() {
        if (isWindows == true){
            macButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            windowsButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            isWindows = false;
        } else {
            macButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            windowsButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            isWindows = true;
        }
    }

    public void isPressedPC() {
        if (isWindows == false){
            windowsButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            macButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            isWindows = true;
        } else {
            windowsButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            macButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            isWindows = false;
        }
    }
}

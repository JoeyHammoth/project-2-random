using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static bool fromGame = true;
    public void ChangeToGameScene()
    {
        SceneManager.LoadScene("GameScene"); 
        fromGame = false;
    }
    
    public void ChangeToStartScene()
    {
        SceneManager.LoadScene("StartScene");
        fromGame = true;
    }
    
}
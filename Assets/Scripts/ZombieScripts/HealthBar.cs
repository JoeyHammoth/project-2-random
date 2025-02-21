using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    // [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private CanvasGroup canvasGroup;
    private ScoreManager scoreManager;

    [SerializeField] private int scoreWhenDied;


    private float time;
    private const float timeToFade = 5f;

    public void UpdateHealth(float health, float maxHealth)
    {
        ShowHealthBar();
        slider.value = health/maxHealth;
        if (health <= 0)
        {
            ZombieDied();
        }
    }

    private void Start() 
    {
        HideHealthBar();
        scoreManager = FindObjectOfType<ScoreManager>();

    }

    private void Update() {
        // target.transform.rotation = camera.transform.rotation;
        if (time > timeToFade) 
        {
            canvasGroup.alpha = 0f;
        }
        else 
        {
            time += Time.deltaTime;
        }
    }

    private void ShowHealthBar() 
    {
        canvasGroup.alpha = 1f;
        time = 0f;
    }

    private void HideHealthBar()
    {
        canvasGroup.alpha = 0f;
    }
    
    private void ZombieDied()
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(scoreWhenDied);
        }

    }

}

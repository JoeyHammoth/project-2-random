using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public PostProcessVolume volume;
    private ColorGrading colorGrading;
    public GameObject gameOverPanel;
    public float fadeDuration = 1.0f;
    private bool isGameOver = false;
    private bool startAnim = false;
    private bool isRevealIncreasing = true;
    private bool decay;
    [SerializeField] private float initialReveal = -0.3f;
    [SerializeField] private float revealChange = 0.01f;
    [SerializeField] private float revealLimit = 0.6f;
    [SerializeField] private float intialRevealDecay = 0.0f;
    private void Start()
    {
        volume.profile.TryGetSettings(out colorGrading);
        decay = Random.value > 0.5f;
    }
    private void Update()
    {
        if (isGameOver) {
            if (!decay) {
                if (!startAnim) {
                    gameOverPanel.GetComponent<UnityEngine.UI.Image>().material.SetFloat("_RevealValue", initialReveal);
                    gameOverPanel.GetComponent<UnityEngine.UI.Image>().material.SetFloat("_Feather", 0.55f);
                    gameOverPanel.GetComponent<UnityEngine.UI.Image>().material.SetFloat("_DecayEffect", 0.0f);
                    gameOverPanel.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                    startAnim = true;
                } else {
                    if (isRevealIncreasing) {
                        initialReveal += revealChange;
                        gameOverPanel.GetComponent<UnityEngine.UI.Image>().material.SetFloat("_RevealValue", initialReveal);
                        if (initialReveal >= revealLimit) {
                            isRevealIncreasing = false;
                        }
                    } else {
                        initialReveal -= revealChange;
                        gameOverPanel.GetComponent<UnityEngine.UI.Image>().material.SetFloat("_RevealValue", initialReveal);
                        if (initialReveal <= 0.0f) {
                            isRevealIncreasing = true;
                        }
                    }
                }
            } else {
                if (!startAnim) {
                    gameOverPanel.GetComponent<UnityEngine.UI.Image>().material.SetFloat("_RevealValue", intialRevealDecay);
                    gameOverPanel.GetComponent<UnityEngine.UI.Image>().material.SetFloat("_Feather", 0.05f);
                    gameOverPanel.GetComponent<UnityEngine.UI.Image>().material.SetFloat("_DecayEffect", 1.0f);
                    gameOverPanel.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                    startAnim = true;
                } else {
                    if (isRevealIncreasing) {
                        intialRevealDecay += revealChange;
                        gameOverPanel.GetComponent<UnityEngine.UI.Image>().material.SetFloat("_RevealValue", intialRevealDecay);
                        if (intialRevealDecay >= revealLimit) {
                            isRevealIncreasing = false;
                        }
                    } else {
                        intialRevealDecay -= revealChange;
                        gameOverPanel.GetComponent<UnityEngine.UI.Image>().material.SetFloat("_RevealValue", intialRevealDecay);
                        if (intialRevealDecay <= 0.1f) {
                            isRevealIncreasing = true;
                        }
                    }
                }
            }
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            StartCoroutine(FadeToDesaturated());
        }

    }

    IEnumerator FadeToDesaturated()
    {
        float startSaturation = colorGrading.saturation.value;
        float endSaturation = -100; 
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            colorGrading.saturation.value = Mathf.Lerp(startSaturation, endSaturation, t);
            yield return null;
        }

        ShowGameOverUI();
    }

    void ShowGameOverUI()
    {
        gameOverPanel.SetActive(true);
    }
}
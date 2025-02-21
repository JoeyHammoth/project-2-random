using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostEffectsController : MonoBehaviour
{
    // Post effects manipulates source render texture coming from game camera.
    // Script gets source render texture, manipulate it through a post effects shader and put it back on the screen.
    // Start is called before the first frame update

    [SerializeField] private Shader postShader; // shader that will be used for post effects
    [SerializeField] private Material postEffectMaterial; // material that will be used for post effects
    [SerializeField] private int effectType = 1;
    [SerializeField] private Color screenTint;
    [SerializeField] private float radius = 1.5f; // For testing purposes only!!! Don't change!!!
    [SerializeField] private float radiusMax = 1.5f;
    [SerializeField] private float radiusMin = 0.8f;
    [SerializeField] private float feather;
    [SerializeField] private float speed = 0.5f;
    private bool applyVignette = false;
    private bool postProcessingAvailable;
    public bool isWindows = OS.isWindows;

    private void Awake() {
        if (isWindows == true)
        {
            return;
        }
        postEffectMaterial = new Material(postShader);
        postProcessingAvailable = SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2;
    }


    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {

        if (postProcessingAvailable == false || isWindows == true)
        {
            Graphics.Blit(src, dest);
            return;
        }

        if (postEffectMaterial == null)
        {
            postEffectMaterial = new Material(postShader); // create a new material with the shader
            postEffectMaterial.hideFlags = HideFlags.HideAndDontSave; // hide the material from the inspector
        }

        RenderTexture
            renderTexture =
                RenderTexture.GetTemporary(src.width, src.height, effectType,
                    src.format); // get a temporary render texture

        if (effectType == 0)
        {
            postEffectMaterial.SetColor("_ScreenTint", screenTint); // set the color of the material
        }
        else if (effectType == 1)
        {
            postEffectMaterial.SetFloat("_Radius", radius);
            postEffectMaterial.SetFloat("_Feather", feather);
            postEffectMaterial.SetColor("_TintColor", screenTint);
        }

        Graphics.Blit(src, renderTexture,
            postEffectMaterial); // pass in a material and a source render texture and put it in a destination render texture

        Graphics.Blit(renderTexture, dest); // taking what the camera sees and putting it on the screen

        RenderTexture.ReleaseTemporary(renderTexture); // release the temporary render texture (memory handling)
    }

    private void Update()
    {
        // Radius of the circle vignette effect will be increased and decreased everytime the player gets attacked. 
        if (isWindows == true)
        {
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerHealthManager healthManager = player.GetComponent<PlayerHealthManager>();
            if (healthManager != null)
            {
                if (healthManager.getAttackStat() == true)
                {
                    applyVignette = true;
                }
            }
            else
            {
                Debug.LogWarning("Player object does not have a PlayerHealthManager component.");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject with the tag 'Player' found.");
        }

        if (applyVignette == true)
        {
            if (radius <= radiusMin)
            {
                applyVignette = false;
            }
            else
            {
                radius -= speed * Time.deltaTime;
            }
        }
        else
        {
            if (radius <= radiusMax)
            {
                radius += speed * Time.deltaTime;
            }
            else
            {
                radius = radiusMax;
            }
        }
    }


}

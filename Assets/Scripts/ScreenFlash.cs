using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private Canvas flashCanvas;
    [SerializeField] private Image flashImage;
    [SerializeField] private Color defaultFlashColor = Color.white;
    [SerializeField] private float defaultIntensity = 0.5f;
    [SerializeField] private float defaultDuration = 0.15f;
    
    // Singleton pattern for easy access
    private static ScreenFlash instance;
    public static ScreenFlash Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<ScreenFlash>();
                if (instance == null)
                {
                    // Create a screen flash system if none exists
                    GameObject flashManager = new GameObject("ScreenFlashManager");
                    instance = flashManager.AddComponent<ScreenFlash>();
                    instance.CreateFlashSystem();
                }
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (flashCanvas == null || flashImage == null)
            {
                CreateFlashSystem();
            }
        }
    }
    
    private void CreateFlashSystem()
    {
        // Create a canvas for screen overlay
        GameObject canvasObj = new GameObject("FlashCanvas");
        canvasObj.transform.SetParent(transform);
        
        flashCanvas = canvasObj.AddComponent<Canvas>();
        flashCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        flashCanvas.sortingOrder = 1000; // Make sure it's on top
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Create the flash image
        GameObject imageObj = new GameObject("FlashImage");
        imageObj.transform.SetParent(canvasObj.transform, false);
        
        flashImage = imageObj.AddComponent<Image>();
        flashImage.color = new Color(1f, 1f, 1f, 0f); // Start transparent
        
        // Make it fill the entire screen
        RectTransform rectTransform = flashImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        // Initially disable canvas
        flashCanvas.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Trigger a screen flash with default settings
    /// </summary>
    public void Flash()
    {
        Flash(defaultFlashColor, defaultIntensity, defaultDuration);
    }
    
    /// <summary>
    /// Trigger a screen flash with custom settings
    /// </summary>
    /// <param name="color">Flash color</param>
    /// <param name="intensity">Alpha intensity (0-1)</param>
    /// <param name="duration">How long the flash lasts</param>
    public void Flash(Color color, float intensity, float duration)
    {
        if (flashImage == null) return;
        
        StartCoroutine(FlashCoroutine(color, intensity, duration));
    }
    
    /// <summary>
    /// Quick white flash - perfect for explosions!
    /// </summary>
    public void WhiteFlash(float intensity = 0.4f)
    {
        Flash(Color.white, intensity, 0.1f);
    }
    
    /// <summary>
    /// Red damage flash
    /// </summary>
    public void DamageFlash(float intensity = 0.3f)
    {
        Flash(Color.red, intensity, 0.15f);
    }
    
    private IEnumerator FlashCoroutine(Color color, float intensity, float duration)
    {
        // Enable canvas and set initial values
        flashCanvas.gameObject.SetActive(true);
        flashImage.color = new Color(color.r, color.g, color.b, 0f);
        
        // Fade in quickly
        float fadeInTime = duration * 0.1f; // 10% of duration for fade in
        float elapsed = 0f;
        
        while (elapsed < fadeInTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0f, intensity, elapsed / fadeInTime);
            flashImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        
        // Hold at full intensity briefly
        flashImage.color = new Color(color.r, color.g, color.b, intensity);
        
        // Fade out
        float fadeOutTime = duration * 0.9f; // 90% of duration for fade out
        elapsed = 0f;
        
        while (elapsed < fadeOutTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(intensity, 0f, elapsed / fadeOutTime);
            flashImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        
        // Ensure completely transparent
        flashImage.color = new Color(color.r, color.g, color.b, 0f);
        flashCanvas.gameObject.SetActive(false);
    }
}
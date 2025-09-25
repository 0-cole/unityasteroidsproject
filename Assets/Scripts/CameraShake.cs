using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float smallShakeDuration = 0.25f;  // Noticeable but quick
    [SerializeField] private float smallShakeMagnitude = 0.2f;   // Slightly stronger
    
    [SerializeField] private float mediumShakeDuration = 0.35f;  // Good impact feel
    [SerializeField] private float mediumShakeMagnitude = 0.35f; // More intense
    
    [SerializeField] private float bigShakeDuration = 0.5f;      // BIG BOOM!
    [SerializeField] private float bigShakeMagnitude = 0.5f;     // Really feel it
    
    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;
    
    // Singleton pattern for easy access
    private static CameraShake instance;
    public static CameraShake Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<CameraShake>();
            }
            return instance;
        }
    }
    
    void Awake()
    {
        instance = this;
        originalPosition = transform.localPosition;
    }
    
    // Public methods for different shake intensities
    public void ShakeSmall()
    {
        TriggerShake(smallShakeDuration, smallShakeMagnitude);
    }
    
    public void ShakeMedium()
    {
        TriggerShake(mediumShakeDuration, mediumShakeMagnitude);
    }
    
    public void ShakeBig()
    {
        TriggerShake(bigShakeDuration, bigShakeMagnitude);
    }
    
    // Generic shake method
    public void TriggerShake(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Shake(duration, magnitude));
    }
    
    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        
        while (elapsed < duration)
        {
            // Random shake offset
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            // Apply shake with fade out over time
            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp01(percentComplete);
            
            transform.localPosition = originalPosition + new Vector3(x * damper, y * damper, 0);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Reset to original position
        transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }
}
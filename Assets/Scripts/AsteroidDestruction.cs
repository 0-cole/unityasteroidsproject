using UnityEngine;

public class AsteroidDestruction : MonoBehaviour
{
    [Header("Destruction Settings")]
    [SerializeField] private bool isSmallAsteroid = false;
    [SerializeField] private bool isMediumAsteroid = false;
    [SerializeField] private bool isBigAsteroid = false;
    
    [Header("Juice Settings")]
    [SerializeField] private bool useAutoDetectedColor = true;
    [SerializeField] private Color fallbackSmallColor = Color.yellow;
    [SerializeField] private Color fallbackMediumColor = Color.orange;
    [SerializeField] private Color fallbackBigColor = Color.red;
    
    // Cache for the detected color and size
    private Color? detectedColor = null;
    private bool? autoDetectedSize = null;
    
    void Start()
    {
        // 🔍 AUTO-DETECT ASTEROID SIZE if not manually set
        if (!isSmallAsteroid && !isMediumAsteroid && !isBigAsteroid)
        {
            DetectAsteroidSize();
        }
        
        // Pre-cache the color
        GetAsteroidMaterialColor();
    }
    
    [Header("Screen Flash")]
    [SerializeField] private bool enableScreenFlash = true;
    [SerializeField] private float flashIntensity = 0.3f;
    [SerializeField] private float flashDuration = 0.1f;
    
    [Header("Score Juice")]
    [SerializeField] private int smallAsteroidScore = 100;
    [SerializeField] private int mediumAsteroidScore = 50;
    [SerializeField] private int bigAsteroidScore = 20;
    
    void OnTriggerEnter(Collider other)
    {
        // Check if hit by laser
        if (other.CompareTag("Laser"))
        {
            DestroyWithJuice();
            
            // Destroy the laser too
            Destroy(other.gameObject);
        }
    }
    
    private void DestroyWithJuice()
    {
        Vector3 explosionPosition = transform.position;
        Color explosionColor = GetExplosionColor();
        int score = GetScore();
        
        // 💥 CAMERA SHAKE based on asteroid size
        TriggerCameraShake();
        
        // ✨ PARTICLE EXPLOSION
        TriggerParticleExplosion(explosionPosition, explosionColor);
        
        // 📈 UPDATE SCORE with juice
        UpdateScoreWithJuice(score);
        
        // 💥 SCREEN FLASH for big asteroids
        if (isBigAsteroid && enableScreenFlash)
        {
            TriggerScreenFlash();
        }
        
        // 💀 DESTROY THE ASTEROID
        Destroy(gameObject);
    }
    
    private void TriggerCameraShake()
    {
        if (CameraShake.Instance == null) return;
        
        if (isSmallAsteroid)
        {
            CameraShake.Instance.ShakeSmall();
        }
        else if (isMediumAsteroid)
        {
            CameraShake.Instance.ShakeMedium();
        }
        else if (isBigAsteroid)
        {
            CameraShake.Instance.ShakeBig();
        }
    }
    
    private void TriggerParticleExplosion(Vector3 position, Color color)
    {
        if (ParticleExplosion.Instance == null) return;
        
        if (isSmallAsteroid)
        {
            ParticleExplosion.Instance.ExplodeSmall(position, color);
        }
        else if (isMediumAsteroid)
        {
            ParticleExplosion.Instance.ExplodeMedium(position, color);
        }
        else if (isBigAsteroid)
        {
            ParticleExplosion.Instance.ExplodeBig(position, color);
        }
    }
    
    private Color GetExplosionColor()
    {
        // 🎨 AUTO-DETECT COLOR FROM MATERIAL!
        if (useAutoDetectedColor)
        {
            Color materialColor = GetAsteroidMaterialColor();
            if (materialColor != Color.clear)
            {
                return materialColor;
            }
        }
        
        // Fallback to manual colors if auto-detection fails
        if (isSmallAsteroid) return fallbackSmallColor;
        if (isMediumAsteroid) return fallbackMediumColor;
        if (isBigAsteroid) return fallbackBigColor;
        return Color.white;
    }
    
    private Color GetAsteroidMaterialColor()
    {
        // Use cached color if we already found it
        if (detectedColor.HasValue)
        {
            return detectedColor.Value;
        }
        
        // Try to get the color from the asteroid's renderer
        Renderer asteroidRenderer = GetComponent<Renderer>();
        if (asteroidRenderer != null && asteroidRenderer.material != null)
        {
            Material mat = asteroidRenderer.material;
            
            // Try different common color property names
            Color color = Color.clear;
            
            // Most common property names for color in materials
            if (mat.HasProperty("_Color"))
            {
                color = mat.color; // This gets _Color property
            }
            else if (mat.HasProperty("_BaseColor"))
            {
                color = mat.GetColor("_BaseColor");
            }
            else if (mat.HasProperty("_MainColor"))
            {
                color = mat.GetColor("_MainColor");
            }
            else if (mat.HasProperty("_TintColor"))
            {
                color = mat.GetColor("_TintColor");
            }
            
            // Cache the detected color
            detectedColor = color;
            
            Debug.Log($"🎨 Detected asteroid color: {color} from material: {mat.name}");
            return color;
        }
        
        Debug.LogWarning("⚠️ Could not detect asteroid material color!");
        return Color.clear;
    }
    
    private int GetScore()
    {
        if (isSmallAsteroid) return smallAsteroidScore;
        if (isMediumAsteroid) return mediumAsteroidScore;
        if (isBigAsteroid) return bigAsteroidScore;
        return 0;
    }
    
    private void UpdateScoreWithJuice(int points)
    {
        // Find and update the score manager
        ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            // Add score (you'll need to expose a public method in ScoreManager)
            // scoreManager.AddScore(points);
            
            Debug.Log($"💥 BOOM! +{points} points!");
        }
    }
    
    private void TriggerScreenFlash()
    {
        // Create a screen flash effect (we'll implement this next!)
        ScreenFlash.Instance?.Flash(Color.white, flashIntensity, flashDuration);
    }
    
    private void DetectAsteroidSize()
    {
        // Try to detect size based on GameObject name
        string objName = gameObject.name.ToLower();
        
        if (objName.Contains("small"))
        {
            isSmallAsteroid = true;
            Debug.Log("🔍 Auto-detected: Small Asteroid");
        }
        else if (objName.Contains("medium"))
        {
            isMediumAsteroid = true;
            Debug.Log("🔍 Auto-detected: Medium Asteroid");
        }
        else if (objName.Contains("big") || objName.Contains("large"))
        {
            isBigAsteroid = true;
            Debug.Log("🔍 Auto-detected: Big Asteroid");
        }
        else
        {
            // Fallback: detect by scale size
            float scale = transform.localScale.magnitude;
            
            if (scale < 1.5f)
            {
                isSmallAsteroid = true;
                Debug.Log($"🔍 Auto-detected by scale ({scale:F2}): Small Asteroid");
            }
            else if (scale < 2.5f)
            {
                isMediumAsteroid = true;
                Debug.Log($"🔍 Auto-detected by scale ({scale:F2}): Medium Asteroid");
            }
            else
            {
                isBigAsteroid = true;
                Debug.Log($"🔍 Auto-detected by scale ({scale:F2}): Big Asteroid");
            }
        }
    }
}

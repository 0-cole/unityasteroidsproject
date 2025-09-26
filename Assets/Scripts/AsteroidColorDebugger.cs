using UnityEngine;

/// <summary>
/// Attach this to any asteroid to see what color and properties are detected
/// Useful for debugging material color issues
/// </summary>
public class AsteroidColorDebugger : MonoBehaviour
{
    [Header("Debug Info")]
    [SerializeField] private bool debugOnStart = true;
    [SerializeField] private KeyCode debugKey = KeyCode.D;
    
    void Start()
    {
        if (debugOnStart)
        {
            DebugAsteroidProperties();
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            DebugAsteroidProperties();
        }
    }
    
    [ContextMenu("Debug Asteroid Properties")]
    public void DebugAsteroidProperties()
    {
        Debug.Log("=== ASTEROID DEBUG INFO ===");
        Debug.Log($"GameObject Name: {gameObject.name}");
        Debug.Log($"Scale: {transform.localScale}");
        Debug.Log($"Scale Magnitude: {transform.localScale.magnitude:F2}");
        
        // Check renderer and material
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Debug.Log($"Renderer found: {renderer.GetType().Name}");
            
            if (renderer.material != null)
            {
                Material mat = renderer.material;
                Debug.Log($"Material Name: {mat.name}");
                Debug.Log($"Material Shader: {mat.shader.name}");
                
                // Try to get color from different properties
                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.color;
                    Debug.Log($"🎨 _Color Property: {color} (R:{color.r:F2}, G:{color.g:F2}, B:{color.b:F2}, A:{color.a:F2})");
                }
                else
                {
                    Debug.LogWarning("❌ Material does not have _Color property");
                }
                
                if (mat.HasProperty("_BaseColor"))
                {
                    Color baseColor = mat.GetColor("_BaseColor");
                    Debug.Log($"🎨 _BaseColor Property: {baseColor}");
                }
                
                if (mat.HasProperty("_MainColor"))
                {
                    Color mainColor = mat.GetColor("_MainColor");
                    Debug.Log($"🎨 _MainColor Property: {mainColor}");
                }
                
                // Check if there's a main texture
                if (mat.mainTexture != null)
                {
                    Debug.Log($"📸 Main Texture: {mat.mainTexture.name}");
                }
                else
                {
                    Debug.Log("📸 No main texture found");
                }
            }
            else
            {
                Debug.LogError("❌ Renderer has no material!");
            }
        }
        else
        {
            Debug.LogError("❌ No Renderer component found!");
        }
        
        // Check for AsteroidDestruction component
        AsteroidDestruction destruction = GetComponent<AsteroidDestruction>();
        if (destruction != null)
        {
            Debug.Log("✅ AsteroidDestruction component found");
        }
        else
        {
            Debug.Log("⚠️ No AsteroidDestruction component found");
        }
        
        Debug.Log("=========================");
    }
}
using UnityEngine;

public class AsteroidDestruction : MonoBehaviour
{
    [Header("Destruction Settings")]
    [SerializeField] private bool isSmallAsteroid = false;
    [SerializeField] private bool isMediumAsteroid = false;
    [SerializeField] private bool isBigAsteroid = false;
    
    [Header("Juice Settings")]
    [SerializeField] private Color smallExplosionColor = Color.yellow;
    [SerializeField] private Color mediumExplosionColor = Color.orange;
    [SerializeField] private Color bigExplosionColor = Color.red;
    
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
        if (isSmallAsteroid) return smallExplosionColor;
        if (isMediumAsteroid) return mediumExplosionColor;
        if (isBigAsteroid) return bigExplosionColor;
        return Color.white;
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
}
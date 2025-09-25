using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    private const string CAN_BE_DESTROYED = "CanBeDestroyed";
    [SerializeField] GameObject smallAsteroid;
    [SerializeField] GameObject medAsteroid;
    [SerializeField] GameObject bigAsteroid;
    [SerializeField] int smallPoint = 1;
    [SerializeField] int medPoint = 5;
    [SerializeField] int bigPoint = 10;
    
    [Header("Explosion Colors")]
    [SerializeField] private Color bigExplosionColor = new Color(1f, 0.4f, 0.2f);    // Orange-red
    [SerializeField] private Color medExplosionColor = new Color(1f, 0.8f, 0.2f);    // Yellow-orange
    [SerializeField] private Color smallExplosionColor = new Color(1f, 1f, 0.4f);    // Bright yellow
    
    private Score scoreScript;
    private SpawnManager spawnManager;

    Vector3 asteroidPosition;


    private void Awake()
    {
        scoreScript = FindFirstObjectByType<Score>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Big")
        {
            asteroidPosition = collision.transform.position;
            spawnManager.SpawnMed(asteroidPosition);
            spawnManager.SpawnMed(asteroidPosition);
            scoreScript.IncreaseScore(bigPoint);
            
            // Big shake for big asteroids!
            if (CameraShake.Instance != null)
                CameraShake.Instance.ShakeBig();
            
            // BIG EXPLOSION!
            if (ParticleExplosion.Instance != null)
                ParticleExplosion.Instance.ExplodeBig(asteroidPosition, bigExplosionColor);
        }
        else if (collision.gameObject.tag == "Med")
        {
            asteroidPosition = collision.transform.position; // Get the position
            spawnManager.SpawnSmall(asteroidPosition); // Pass the position to the SpawnSmall method
            spawnManager.SpawnSmall(asteroidPosition); // Pass the position again
            scoreScript.IncreaseScore(medPoint);
            
            // Medium shake for medium asteroids
            if (CameraShake.Instance != null)
                CameraShake.Instance.ShakeMedium();
            
            // Medium explosion!
            if (ParticleExplosion.Instance != null)
                ParticleExplosion.Instance.ExplodeMedium(asteroidPosition, medExplosionColor);
        }
        else
        {
            asteroidPosition = collision.transform.position;
            scoreScript.IncreaseScore(smallPoint);
            
            // Small shake for small asteroids
            if (CameraShake.Instance != null)
                CameraShake.Instance.ShakeSmall();
            
            // Small explosion!
            if (ParticleExplosion.Instance != null)
                ParticleExplosion.Instance.ExplodeSmall(asteroidPosition, smallExplosionColor);
        }

        Destroy(collision.gameObject);
        Destroy(this.gameObject);

    }

}
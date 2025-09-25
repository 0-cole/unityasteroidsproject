using System.Collections;
using UnityEngine;

public class ParticleExplosion : MonoBehaviour
{
    [Header("Explosion Particles")]
    [SerializeField] private ParticleSystem explosionParticles;
    
    [Header("Explosion Settings")]
    [SerializeField] private int smallParticleCount = 15;
    [SerializeField] private int mediumParticleCount = 25;
    [SerializeField] private int bigParticleCount = 40;
    
    [SerializeField] private float smallExplosionForce = 5f;
    [SerializeField] private float mediumExplosionForce = 8f;
    [SerializeField] private float bigExplosionForce = 12f;
    
    [SerializeField] private float particleLifetime = 1.5f;
    [SerializeField] private float destroyDelay = 2f;
    
    // Singleton for easy access
    private static ParticleExplosion instance;
    public static ParticleExplosion Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<ParticleExplosion>();
                if (instance == null)
                {
                    // Create a new GameObject with ParticleExplosion if none exists
                    GameObject explosionManager = new GameObject("ExplosionManager");
                    instance = explosionManager.AddComponent<ParticleExplosion>();
                    instance.CreateParticleSystem();
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
            if (explosionParticles == null)
            {
                CreateParticleSystem();
            }
        }
    }
    
    private void CreateParticleSystem()
    {
        // Create particle system if not assigned
        GameObject particleObj = new GameObject("ExplosionParticles");
        particleObj.transform.SetParent(transform);
        explosionParticles = particleObj.AddComponent<ParticleSystem>();
        
        // Configure the particle system for explosions
        var main = explosionParticles.main;
        main.startLifetime = particleLifetime;
        main.startSpeed = 10f;
        main.startSize = 0.5f;
        main.startColor = Color.white;
        main.maxParticles = 1000;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
        
        // Set to burst mode
        var emission = explosionParticles.emission;
        emission.enabled = false;
        
        // Add some velocity over lifetime for spreading effect
        var velocityOverLifetime = explosionParticles.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
        velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(2f);
        
        // Add size over lifetime for fade effect
        var sizeOverLifetime = explosionParticles.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0f, 1f);
        sizeCurve.AddKey(1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);
        
        // Make it look more explosion-like with shape
        var shape = explosionParticles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f;
        
        // Don't play on awake
        main.playOnAwake = false;
        
        // Set renderer to use built-in sprite
        var renderer = explosionParticles.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
    }
    
    public void ExplodeSmall(Vector3 position, Color color)
    {
        Explode(position, color, smallParticleCount, smallExplosionForce);
    }
    
    public void ExplodeMedium(Vector3 position, Color color)
    {
        Explode(position, color, mediumParticleCount, mediumExplosionForce);
    }
    
    public void ExplodeBig(Vector3 position, Color color)
    {
        Explode(position, color, bigParticleCount, bigExplosionForce);
    }
    
    public void Explode(Vector3 position, Color color, int particleCount, float explosionForce)
    {
        // Move to explosion position
        explosionParticles.transform.position = position;
        
        // Set particle color
        var main = explosionParticles.main;
        main.startColor = color;
        main.startSpeed = explosionForce;
        
        // Emit burst of particles
        explosionParticles.Emit(particleCount);
    }
    
    // Create a standalone explosion that cleans itself up
    public void CreateStandaloneExplosion(Vector3 position, Color color, int particleCount, float explosionForce)
    {
        GameObject explosionObj = new GameObject("Explosion");
        explosionObj.transform.position = position;
        
        ParticleSystem particles = explosionObj.AddComponent<ParticleSystem>();
        
        // Configure for one-shot explosion
        var main = particles.main;
        main.startLifetime = particleLifetime;
        main.startSpeed = explosionForce;
        main.startSize = 0.5f;
        main.startColor = color;
        main.maxParticles = particleCount;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        
        var emission = particles.emission;
        emission.enabled = false;
        
        var shape = particles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f;
        
        var velocityOverLifetime = particles.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
        velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(2f);
        
        var renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        
        particles.Emit(particleCount);
        
        // Destroy after delay
        Destroy(explosionObj, destroyDelay);
    }
}
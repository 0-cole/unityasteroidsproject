using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject laserSpawnPointGameObject;
    [SerializeField] GameObject playerParent;
    
    [Header("Juice Settings")]
    [SerializeField] private float shootShakeIntensity = 0.15f;
    [SerializeField] private float shootScalePunch = 1.2f;
    [SerializeField] private float scalePunchDuration = 0.1f;
    [SerializeField] private Color laserMuzzleFlashColor = Color.cyan;
    [SerializeField] private int muzzleFlashParticles = 8;
    
    private Vector3 laserSpawnPointPosition;
    private Quaternion playerParentRotation;
    private Vector3 originalScale;

    void Start()
    {
        // Store original scale for punch effect
        originalScale = playerParent.transform.localScale;
    }

    void Update()
    {
        playerParentRotation = playerParent.transform.rotation;
        laserSpawnPointPosition = laserSpawnPointGameObject.transform.position;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireLaserWithJuice();
        }
    }
    
    private void FireLaserWithJuice()
    {
        // 🔥 SPAWN THE LASER
        Instantiate(laserPrefab, laserSpawnPointPosition, playerParentRotation);
        
        // 💥 CAMERA SHAKE - Small but satisfying
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.TriggerShake(0.1f, shootShakeIntensity);
        }
        
        // ✨ MUZZLE FLASH PARTICLES
        if (ParticleExplosion.Instance != null)
        {
            ParticleExplosion.Instance.Explode(laserSpawnPointPosition, laserMuzzleFlashColor, muzzleFlashParticles, 3f);
        }
        
        // 🚀 SHIP SCALE PUNCH - Makes it feel powerful!
        StartCoroutine(ScalePunchEffect());
    }
    
    private System.Collections.IEnumerator ScalePunchEffect()
    {
        // Scale up quickly
        playerParent.transform.localScale = originalScale * shootScalePunch;
        
        // Wait a tiny bit
        yield return new WaitForSeconds(scalePunchDuration);
        
        // Scale back to normal
        float elapsed = 0f;
        Vector3 startScale = playerParent.transform.localScale;
        
        while (elapsed < scalePunchDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / scalePunchDuration;
            playerParent.transform.localScale = Vector3.Lerp(startScale, originalScale, t);
            yield return null;
        }
        
        playerParent.transform.localScale = originalScale;
    }
}
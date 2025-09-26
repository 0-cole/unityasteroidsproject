using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float xRotateSpeed = 5f;
    [SerializeField] float yMovementSpeed = 5f;
    
    [Header("Juice Settings")]
    [SerializeField] private Color thrusterColor = Color.cyan;
    [SerializeField] private int thrusterParticles = 5;
    [SerializeField] private float thrusterForce = 2f;
    [SerializeField] private float shipTiltAmount = 15f;
    [SerializeField] private float tiltSpeed = 5f;
    
    private Vector3 shipPosition;
    private Vector3 camPos;
    private float camSizeX;
    private float camSizeY;
    private float currentTilt = 0f;

    void Start()
    {
        // Get camera position in world space (typically 0,0 for orthographic)
        camPos = Camera.main.transform.position;
        camSizeY = Camera.main.orthographicSize;
        camSizeX = camSizeY * Camera.main.aspect;
    }

    void Update()
    {
        shipPosition = transform.position;

        float yInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");
        
        // 🚀 THRUSTER PARTICLES when moving forward!
        if (yInput > 0.1f)
        {
            TriggerThrusterParticles();
        }
        
        // 🎯 SHIP TILT when turning - adds so much juice!
        float targetTilt = -xInput * shipTiltAmount;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);
        
        // Apply movement with Time.deltaTime
        transform.Rotate(0, 0, -xInput * Time.deltaTime * xRotateSpeed);
        transform.Translate(0, yInput * Time.deltaTime * yMovementSpeed, 0);
        
        // Apply tilt to the ship's Z rotation (separate from steering)
        Vector3 euler = transform.eulerAngles;
        euler.z += currentTilt;
        transform.eulerAngles = euler;

        // Screen wrap for the X-axis
        if (transform.position.x > camPos.x + camSizeX)
        {
            transform.position = new Vector3(camPos.x - camSizeX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < camPos.x - camSizeX)
        {
            transform.position = new Vector3(camPos.x + camSizeX, transform.position.y, transform.position.z);
        }

        // Screen wrap for the Y-axis
        if (transform.position.y > camPos.y + camSizeY)
        {
            transform.position = new Vector3(transform.position.x, camPos.y - camSizeY, transform.position.z);
        }
        else if (transform.position.y < camPos.y - camSizeY)
        {
            transform.position = new Vector3(transform.position.x, camPos.y + camSizeY, transform.position.z);
        }
    }

    private void TriggerThrusterParticles()
    {
        // Create thruster particles behind the ship
        if (ParticleExplosion.Instance != null)
        {
            // Calculate position behind the ship
            Vector3 thrusterPosition = transform.position - transform.up * 0.5f;
            
            ParticleExplosion.Instance.Explode(thrusterPosition, thrusterColor, thrusterParticles, thrusterForce);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Handle asteroid collision with juice!
        if (other.CompareTag("Asteroid"))
        {
            Debug.Log("😱 SHIP HIT BY ASTEROID!");
            
            // 💥 BIG CAMERA SHAKE
            if (CameraShake.Instance != null)
            {
                CameraShake.Instance.ShakeBig();
            }
            
            // 🔴 RED DAMAGE FLASH
            if (ScreenFlash.Instance != null)
            {
                ScreenFlash.Instance.DamageFlash(0.4f);
            }
            
            // TODO: Handle player damage/death here
        }
    }
}
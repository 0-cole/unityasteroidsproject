using UnityEngine;

public class AsteroidMove : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody asteroidRigidbody;

    // A changeable force value
    [SerializeField] private float force = 5f;

    void Start()
    {
        // Use the new recommended method to find the player's transform
        playerTransform = FindFirstObjectByType<Movement>().transform;

        // Use GetComponent to get the Rigidbody on the same GameObject
        asteroidRigidbody = GetComponent<Rigidbody>();

        // Ensure both player and rigidbody are found before attempting to add force
        if (playerTransform != null && asteroidRigidbody != null)
        {
            // Calculate the direction vector from the asteroid's position to the player's position
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            // Apply the impulse force in the calculated direction
            asteroidRigidbody.AddForce(direction * force, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Player or Rigidbody not found!");
        }
    }
}
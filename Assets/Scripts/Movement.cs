using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float xRotateSpeed = 5f;
    [SerializeField] float yMovementSpeed = 5f;
    Vector3 shipPosition;
    Vector3 camPos;
    float camSizeX;
    float camSizeY;

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

        float yInput = Input.GetAxis("Vertical") * Time.deltaTime * yMovementSpeed;
        float xInput = Input.GetAxis("Horizontal") * Time.deltaTime * xRotateSpeed;

        transform.Rotate(0, 0, -xInput);
        transform.Translate(0, yInput, 0);

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit Trigger");
        // This line doesn't correctly update the ship's position.
        // It should be transform.position.y = -transform.position.y;
        shipPosition.y = -transform.position.y;
    }
}
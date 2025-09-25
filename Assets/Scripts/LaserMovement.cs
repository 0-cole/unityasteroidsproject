using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    [SerializeField] float laserMovementSpeed = 100f;
    [SerializeField] float timeToWait = 2f;







    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, laserMovementSpeed * Time.deltaTime, 0);
        Destroy(this.gameObject, timeToWait);
    }
}

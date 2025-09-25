using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject laserSpawnPointGameObject;
    Vector3 laserSpawnPointPosition;
    [SerializeField] GameObject playerParent;
    Quaternion playerParentRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerParentRotation = playerParent.transform.rotation;
        laserSpawnPointPosition = laserSpawnPointGameObject.transform.position;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(laserPrefab, laserSpawnPointPosition, playerParentRotation);
        }
    }
}
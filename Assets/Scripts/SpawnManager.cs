using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] asteroids;
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject[] powerUps;
    [SerializeField] float timeToWait = 5f;

    // Define the spawn area boundaries
    [SerializeField] float xSides = 23.16f;
    [SerializeField] float ySides = 13.26f;

    Vector3 spawnPosition;
    int asteroidCounter = 0;

    void Start()
    {
        StartCoroutine("SpawnAsteroids");
    }

    IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            int edge = Random.Range(0, 4);

            switch (edge)
            {
                case 0: // Left
                    spawnPosition = new Vector3(-xSides, Random.Range(-ySides, ySides), 0);
                    break;
                case 1: // Right
                    spawnPosition = new Vector3(xSides, Random.Range(-ySides, ySides), 0);
                    break;
                case 2: // Top
                    spawnPosition = new Vector3(Random.Range(-xSides, xSides), ySides, 0);
                    break;
                case 3: // Bottom
                    spawnPosition = new Vector3(Random.Range(-xSides, xSides), -ySides, 0);
                    break;
            }

            yield return new WaitForSeconds(timeToWait);
            Instantiate(asteroids[Random.Range(0, 3)], spawnPosition, Quaternion.identity);
            asteroidCounter++;
        }
    }

    public void SpawnMed(Vector3 spawnPosition)
    {
        Instantiate(asteroids[1], spawnPosition, Quaternion.identity);
    }

    public void SpawnSmall(Vector3 spawnPosition)
    {
        Instantiate(asteroids[2], spawnPosition, Quaternion.identity);
    }
}
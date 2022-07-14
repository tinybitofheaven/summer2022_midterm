using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=cIeWhztKyAg
public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float spawnRate = 2.0f;
    public float spawnDistance = 15;
    public int spawnAmount = 1;
    public float trajectoryVariance = 15.0f;
    public bool spawning = true;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < this.spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;
            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }

    private void Update()
    {
        if (!spawning)
        {
            CancelInvoke();
            GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
            foreach (GameObject asteroid in asteroids)
            {
                Destroy(asteroid);
            }

        }
    }
}

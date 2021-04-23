using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerManager : MonoBehaviour
{
    public static int numZombies;
    public static int maxZombies = 30;
    public static float spawnTime = 5f;
    public Vector3 spawnBox;
    public GameObject zombiePrefab;

    private void Awake()
    {
        numZombies = FindObjectsOfType<ZombieController>().Length;
    }

    private void Start()
    {
        StartCoroutine(SpawnZombie());
    }

    IEnumerator SpawnZombie()
    {
        while(isActiveAndEnabled)
        {
            yield return new WaitForSeconds(spawnTime);

            if (numZombies < maxZombies)
            {
                Instantiate(zombiePrefab, FindSpawnPoint(), Quaternion.identity, transform);
                numZombies++;
            }
        }
    }

    private Vector3 FindSpawnPoint()
    {
        return transform.position + new Vector3(
                (Random.value - 0.5f) * spawnBox.x,
                (Random.value - 0.5f) * spawnBox.y,
                (Random.value - 0.5f) * spawnBox.z
             );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnBox);
    }
}

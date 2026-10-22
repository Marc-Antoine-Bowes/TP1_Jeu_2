using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawn_Manager : MonoBehaviour
{
    [SerializeField] private GameObject[] spawners;
    private GameObject[] aliens;

    void Start()
    {

        List<GameObject> temporaryAlienList = new List<GameObject>();

        foreach (Transform child in transform)
        {
            temporaryAlienList.Add(child.gameObject);

            child.gameObject.SetActive(false);
        }

        aliens = temporaryAlienList.ToArray();

        if (aliens.Length > 0 && spawners.Length > 0)
        {
            StartCoroutine(spawn());
        }
        else
        {
            Debug.LogError("Impossible de lancer le spawner");
        }
    }

    IEnumerator spawn()
    {
        while (true)
        {
            float spawnDelay = spawners.Length;

            yield return new WaitForSeconds(spawnDelay);

            spawnAlien();
        }
    }
    private void spawnAlien ()
    {
        GameObject alien = getRandomAlien();

        GameObject spawner = getRandomSpawner();

        alien.transform.position = spawner.transform.position;

        alien.SetActive(true);
    }

    private GameObject getRandomAlien ()
    {
        int randomAlienIndex = Random.Range(0, aliens.Length);
        GameObject alienToSpawn = aliens[randomAlienIndex];

        if (alienToSpawn.activeSelf == true)
        {
            getRandomAlien();
        }


        return alienToSpawn;
    }

    private GameObject getRandomSpawner() 
    {
        int randomSpawnersIndex = Random.Range(0, spawners.Length);
        GameObject spawnersToSpawn = spawners[randomSpawnersIndex];

        if(spawnersToSpawn.activeSelf == false)
        {
            getRandomSpawner();
        }

        return spawnersToSpawn;
    }


}

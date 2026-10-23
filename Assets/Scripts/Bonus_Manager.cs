using System.Collections.Generic;
using UnityEngine;

public class Bonus_Manager : MonoBehaviour
{
    private GameObject[] bonus;
    void Start()
    {

        List<GameObject> temporaryBonusList = new List<GameObject>();

        foreach (Transform child in transform)
        {
            temporaryBonusList.Add(child.gameObject);

            child.gameObject.SetActive(false);
        }

        bonus = temporaryBonusList.ToArray();

        if (bonus.Length <= 0)
        {
            Debug.LogError("Impossible de lancer le spawner");
        }
    }

    public void spawnBonus (Vector3 position)
    {
        GameObject bonus = getRandomBonus();

        bonus.SetActive(true);
        bonus.GetComponent<Transform>().position = position;
    }

    private GameObject getRandomBonus()
    {
        int randomBonusIndex = Random.Range(0, bonus.Length);
        GameObject bonusToSpawn = bonus[randomBonusIndex];

        if (bonusToSpawn.activeSelf == true)
        {
            getRandomBonus();
        }


        return bonusToSpawn;
    }
}

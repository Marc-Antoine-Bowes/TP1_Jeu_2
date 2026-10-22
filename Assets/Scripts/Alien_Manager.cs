using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Alien_Manager : MonoBehaviour
{
    [SerializeField] private GameObject bonus_manager;
    [SerializeField] private float dropChance = 1; //0.15f;
    [SerializeField] private int hp = 1;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private Alien_Life_Manager life_manager;

    [SerializeField] private int scoreValue = 10;

    private void Start()
    {
        life_manager = GetComponent<Alien_Life_Manager>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "SpaceMarine")
        {
            hurt();
        }
    }

    public void deadReset()
    {
        gameObject.SetActive(false);
        audioSource.PlayOneShot(audioClip);
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        if (UnityEngine.Random.value <= dropChance)
        {
            bonus_manager.GetComponent<Bonus_Manager>().spawnBonus(gameObject.transform.position);
        }
    }

    public void hurt()
    {
        hp -= 1;

        if (hp < 1)
        {
            deadReset();
        }
    }
}

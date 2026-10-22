using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Alien_Manager : MonoBehaviour
{
    [SerializeField] private GameObject bonus_manager;
    [SerializeField] private float dropChance = 1; //0.15f;
    [SerializeField] private int hp = 1;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private Alien_Life_Manager life_manager;

    [SerializeField] private int scoreValue = 10;
    private ParticleSystem explosion;
    private GameObject body;
    private Rigidbody rb;
    private void OnEnable()
    {
        body = transform.Find("Body").gameObject;
    }
    private void Start()
    {
        life_manager = GetComponent<Alien_Life_Manager>();
        explosion = transform.Find("Explosion").GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "SpaceMarine")
        {
            Hurt();
            if (hp <= 0) {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                DeadReset();
                StartCoroutine(Die());
            } 
        }
    }

    public void DeadReset()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        if (UnityEngine.Random.value <= dropChance)
        {
            bonus_manager.GetComponent<Bonus_Manager>().spawnBonus(gameObject.transform.position);
        }
    }

    public void Hurt()
    {
        hp -= 1;
    }
    IEnumerator Die()
    {
        audioSource.PlayOneShot(audioClip);

        explosion.gameObject.SetActive(true);

        explosion.Play();
        body.SetActive(false);
        yield return new WaitWhile(() => explosion.IsAlive(true));

        gameObject.SetActive(false);
    }
}



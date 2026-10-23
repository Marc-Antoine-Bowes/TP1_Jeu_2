using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Alien_Manager : Killable
{
    [SerializeField] private GameObject bonus_manager;
    [SerializeField] private float dropChance = 1; //0.15f;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

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
        explosion = transform.Find("Explosion").GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "SpaceMarine")
        {
            TakeDamage(999);
        }
        if (collider.gameObject.name == "Bullet")
        {
            TakeDamage(1);
        }
        if (collider.gameObject.name == "Bullet")
        {
            TakeDamage(1);
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
    IEnumerator Die()
    {
        audioSource.PlayOneShot(audioClip);

        explosion.gameObject.SetActive(true);

        explosion.Play();
        body.SetActive(false);
        yield return new WaitWhile(() => explosion.IsAlive(true));

        gameObject.SetActive(false);
    }

    public override void OnDeath()
    {
        if (IsDead())
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            DeadReset();
            StartCoroutine(Die());
        }
    }

    public void stepOnHead()
    {
        TakeDamage(999);
    }
}
   


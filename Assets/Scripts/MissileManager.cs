using System.Collections;
using TMPro;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    private GameObject rendering;
    private ParticleSystem explosion;
    private ParticleSystem smoke;
    private bool hasExploded = false;
    private Rigidbody rb;
    private SphereCollider explosionRange;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        hasExploded = false;

        explosion = transform.Find("Explosion").GetComponent<ParticleSystem>();
        smoke = transform.Find("Rendering/Smoke").GetComponent<ParticleSystem>();
        rendering = transform.Find("Rendering/Cube").gameObject;
        explosionRange = GetComponent<SphereCollider>();

        smoke.gameObject.SetActive(true);
        explosion.gameObject.SetActive(false);
        rendering.SetActive(true);
        explosionRange.enabled = false;

        smoke.Play();
    }

    void OnDisable()
    {
        explosion.gameObject.SetActive(false);
        smoke.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasExploded) return;

        hasExploded = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        smoke.Stop();
        rendering.SetActive(false);
        explosionRange.enabled = true;
        StartCoroutine(PlayExplosion());
    }

    private IEnumerator PlayExplosion()
    {
        explosion.gameObject.SetActive(true);

        explosion.Play();

        yield return new WaitWhile(() => explosion.IsAlive(true));

        gameObject.SetActive(false);
    }
}

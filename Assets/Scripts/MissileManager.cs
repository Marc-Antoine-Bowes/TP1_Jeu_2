using UnityEngine;
using System.Collections;

public class MissileManager : MonoBehaviour
{
    private ParticleSystem explosion;
    private bool hasExploded = false;

    void Start()
    {
        explosion = GetComponentInChildren<ParticleSystem>(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        hasExploded = true;

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

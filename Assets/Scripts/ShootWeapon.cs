using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class ShootWeapon : MonoBehaviour
{
    private const int BULLET_FORCE = 100;

    [SerializeField] private GameObject bulletPool;
    [SerializeField] private AudioClip bulletClip;

    private AudioSource audioSource;
    private List<GameObject> bullets = new List<GameObject>();
    private Vector3 gunEndPos;
    private InputAction shootAction;
    private float fireRateRpm = 240f;
    private float nextFireTime = 0f;


    void Start()
    {
        shootAction = InputSystem.actions.FindAction("Attack");
        audioSource = GetComponent<AudioSource>();

        if (bulletPool == null)
        {
            Debug.LogError("Bullet Pool is not assigned in the inspector.");
            return;
        }

        foreach (Transform bullet in bulletPool.transform)
        {
            bullets.Add(bullet.gameObject);
        }

        GameObject gunEnd = transform.Find("Gun end").gameObject;
        if (gunEnd != null)
        {
            gunEndPos = gunEnd.transform.position;
        }
    }

    void Update()
    {
        ManageShoot();
    }

    private void ManageShoot()
    {
        if (shootAction.IsPressed())
        {
            if (Time.time >= nextFireTime)
            {
                ShootBullet();
                nextFireTime = Time.time + 60f / fireRateRpm;
            }
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = GetBulet();
        if (bullet != null)
        {
            bullet.SetActive(true);
            bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(CreateNewForce(bullet.transform.position, BULLET_FORCE), ForceMode.Impulse);
            audioSource.PlayOneShot(bulletClip);
        }
    }

    private GameObject GetBulet()
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }

    private Vector3 CreateNewForce(Vector3 otherObjectPosition, float intensity)
    {
        Vector3 newForce = new Vector3(gunEndPos.x - otherObjectPosition.x, gunEndPos.y - otherObjectPosition.y, gunEndPos.z - otherObjectPosition.z);

        newForce = newForce.normalized;

        newForce *= intensity;

        return newForce;

    }

}

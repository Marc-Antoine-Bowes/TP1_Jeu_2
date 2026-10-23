using System.Collections.Generic;
using TMPro;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class ShootWeapon : MonoBehaviour
{
    private const int BULLET_FORCE = 100;
    private const int MISSILE_FORCE = 75;

    [Header("Projectile pools")]
    [SerializeField] private GameObject bulletPool;
    [SerializeField] private GameObject missilePool;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bulletClip;
    [SerializeField] private AudioClip missileClip;

    [Header("HUD MAG")]
    [SerializeField] private TextMeshProUGUI magText;

    private AudioSource audioSource;
    private List<GameObject> bullets = new List<GameObject>();
    private List<GameObject> missiles = new List<GameObject>();
    private InputAction shootPrimaryAction;
    private InputAction shootSecondaryAction;
    private float fireRateRpm = 240f;
    private float missileFireRateRpm = 60f;
    private float nextFireTime = 0f;
    private float nextMissileFireTime = 0f;
    private int missileMagCapacity = 5;
    private float bonusShotDuration = 0f;


    void Start()
    {
        shootPrimaryAction = InputSystem.actions.FindAction("Attack");
        shootSecondaryAction = InputSystem.actions.FindAction("SecondaryAttack");
        audioSource = GetComponent<AudioSource>();

        AssignProjectiles();
    }

    void Update()
    {
        bonusShotDuration = Mathf.Max(0f, bonusShotDuration - Time.deltaTime);
        ManageShoot();
        UpdateMagUi();
    }

    private void ManageShoot()
    {
        if (shootPrimaryAction.IsPressed())
        {
            if (Time.time >= nextFireTime)
            {
                ShootBullet();
                nextFireTime = Time.time + 60f / fireRateRpm;
            }
        }

        if (shootSecondaryAction.IsPressed())
        {
            if (Time.time >= nextMissileFireTime)
            {
                ShootMissile();
                nextMissileFireTime = Time.time + 60f / missileFireRateRpm;
            }
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = GetBulet();

        if (bullet != null)
        {
            bullet.SetActive(true);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody>().AddForce(CreateNewForce(bullet.transform, BULLET_FORCE), ForceMode.Impulse);
            audioSource.PlayOneShot(bulletClip);
        }

        if (bonusShotDuration > 0f)
        {
            GameObject bulletLeft = GetBulet();

            if(bulletLeft != null)
            {
                bulletLeft.SetActive(true);
                bulletLeft.transform.position = transform.position;
                bulletLeft.GetComponent<Rigidbody>().AddForce(CreateNewForce(bulletLeft.transform, BULLET_FORCE, -1f), ForceMode.Impulse);
            }

            GameObject bulletRight = GetBulet();
            if (bulletRight != null)
            {
                bulletRight.SetActive(true);
                bulletRight.transform.position = transform.position;
                bulletRight.GetComponent<Rigidbody>().AddForce(CreateNewForce(bulletRight.transform, BULLET_FORCE, 1f), ForceMode.Impulse);
            }
        }
    }

    private void ShootMissile()
    {
        if (missileMagCapacity <= 0) return;

        GameObject missile = GetMissile();

        if (missile != null)
        {
            missileMagCapacity--;
            missile.SetActive(true);
            missile.transform.SetPositionAndRotation(transform.position, transform.rotation);
            missile.GetComponent<Rigidbody>().AddForce(CreateNewForce(missile.transform, MISSILE_FORCE), ForceMode.Impulse);
            audioSource.PlayOneShot(missileClip);
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

    private GameObject GetMissile()
    {
        foreach (GameObject missile in missiles)
        {
            if (!missile.activeInHierarchy)
            {
                return missile;
            }
        }
        return null;
    }

    private Vector3 CreateNewForce(Transform otherObjectPosition, float intensity, float horizontalOffset = 0)
    {
        Vector3 gunEndPos = transform.Find("Gun end").position;
        Vector3 newForce = new Vector3(gunEndPos.x - otherObjectPosition.position.x, gunEndPos.y - otherObjectPosition.position.y, gunEndPos.z - otherObjectPosition.position.z);

        newForce += otherObjectPosition.right * horizontalOffset;

        newForce = newForce.normalized;

        newForce *= intensity;

        return newForce;

    }

    private void AssignProjectiles()
    {
        foreach (Transform bullet in bulletPool.transform)
        {
            bullets.Add(bullet.gameObject);
        }

        foreach (Transform missile in missilePool.transform)
        {
            missiles.Add(missile.gameObject);
        }
    }

    private void UpdateMagUi()
    {
        return;
        magText.text = missileMagCapacity.ToString();
    }

    public void AddMissilesToMag(int nb)
    {
        missileMagCapacity += nb;
    }

    public void IncreaseBonusShotDuration(float duration)
    {
        bonusShotDuration += duration;
    }

}

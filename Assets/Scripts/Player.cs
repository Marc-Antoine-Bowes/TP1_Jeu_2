using UnityEngine;

public class Player : Killable
{
    [SerializeField] GameManager gm;

    private ShootWeapon gunManager;

    void Start()
    {
        gunManager = GetComponentInChildren<ShootWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            TakeDamage(1);
        }
        if(other.CompareTag("AmmoBox"))
        {
            gunManager.AddMissilesToMag(5);
        }
        if(other.CompareTag("HealthBox"))
        {
            GainHealth(1);
        }
        if(other.CompareTag("BonusShot"))
        {
            gunManager.IncreaseBonusShotDuration(5f);
        }
    }

    public override void OnDeath()
    {
        gm.GameOver();
        gameObject.SetActive(false);
    }
}

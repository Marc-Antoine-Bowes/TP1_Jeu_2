using UnityEngine;

public class Player : Killable
{
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
    }

    public override void OnDeath()
    {
        gameObject.SetActive(false);
    }
}

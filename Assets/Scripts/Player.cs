using UnityEngine;

public class Player : Killable
{
    private ShootWeapon gunManager;

    void Start()
    {
        gunManager = transform.Find("Gun").GetComponent<ShootWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            if (other.gameObject.name == "BobbleEnemy-Head")
            {
                return; 
            }
            TakeDamage(1);
        }
        if(other.CompareTag("AmmoBox"))
        {
            TakeDamage(1);
        }
    }

    public override void OnDeath()
    {
        gameObject.SetActive(false);
    }
}

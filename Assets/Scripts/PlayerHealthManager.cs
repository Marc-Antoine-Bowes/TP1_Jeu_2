using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    private int maxHealth = 10;
    private int defaultHealth = 5;
    private int currentHealth;

    void Start()
    {
        currentHealth = defaultHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
            other.gameObject.SetActive(false);
        }
    }

    public void ResetHealth()
    {
        currentHealth = defaultHealth;
    }

    public void GainHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }
}

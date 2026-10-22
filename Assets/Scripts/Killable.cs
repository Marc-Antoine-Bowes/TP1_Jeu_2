using UnityEngine;

public abstract class Killable : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int defaultHealth = 5;

    private int currentHealth;

    void OnEnable()
    {
        currentHealth = defaultHealth;
    }

    public virtual void ResetHealth()
    {
        currentHealth = defaultHealth;
    }

    public virtual void GainHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if(IsDead())
        {
            OnDeath();
        }
    }

    public virtual bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    public virtual void OnDeath()
    {
    }
}

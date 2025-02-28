using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;

   [SerializeField] private int health = 10;
   private int healthMax;

    private void Awake()
        {
                healthMax = health;
        }
   public void Damage(int damageAmount)
   {
    AudioManager.Instance.PlaySFX("Impact");
    health -= damageAmount;
    if( health < 0)
    {
        health = 0;
    }

    OnDamaged?.Invoke(this, EventArgs.Empty);

    if(health == 0)
    {
        Die();
    }

    Debug.Log(health);

   }

     public void Heal(int healAmount)
    {
        health += healAmount;
        health = Mathf.Clamp(health, 0, healthMax); // Ensure health doesn't exceed max health

        OnHealed?.Invoke(this, EventArgs.Empty); // Trigger the healed event
        Debug.Log($"Health after healing: {health}/{healthMax}");
    }
   private void Die()
   {
    OnDead?.Invoke(this, EventArgs.Empty);
    AudioManager.Instance.PlaySFX("Death");
   }

     public int GetCurrentHealth()
    {
        return health; // Return the current health value
    }

    public int GetMaxHealth()
    {
        return healthMax; // Return the maximum health value
    }

   public float GetHealthNormalized()
   {
    return (float)health / healthMax;
   }
}

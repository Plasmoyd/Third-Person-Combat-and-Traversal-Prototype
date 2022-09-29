using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDie;

    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private bool isInvulnerable;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(int damage)
    {
        if(currentHealth <= 0) 
        {
            return; 
        }

        if (isInvulnerable)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        OnTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            OnDie?.Invoke();

            return;
        }

        Debug.Log(currentHealth);
    }

    public void SetIsInvulnerable(bool isBlocking)
    {
        this.isInvulnerable = isBlocking;
    }
}

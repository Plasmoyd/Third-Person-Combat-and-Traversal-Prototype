using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;

    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(int damage)
    {
        if(currentHealth <= 0) { return; }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        OnTakeDamage?.Invoke();

        Debug.Log(currentHealth);
    }
}

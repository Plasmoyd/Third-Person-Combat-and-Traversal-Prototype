using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{

    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private int damage;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") { return; }

        if(alreadyCollidedWith.Contains(other)) { return; }

        alreadyCollidedWith.Add(other); 

        if(!other.TryGetComponent<Health>(out Health health)) { return; }

        health.DealDamage(damage);
    }

    public void SetAttack(int damage)
    {
        this.damage = damage;
    }
}

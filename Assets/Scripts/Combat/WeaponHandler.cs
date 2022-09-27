using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weapon;

    public void EnableWeapon()
    {
        weapon.SetActive(true);
    }

    public void DisableWeapon()
    {
        weapon.SetActive(false);
    }
}

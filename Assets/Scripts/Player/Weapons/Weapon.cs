using System;
using System.Collections;
using FishNet.Object;
using TMPro;
using UnityEngine;

class WeaponAttr: Attribute
{
    internal WeaponAttr(bool isPrimary, int weaponPrice)
    {
        IsPrimary = isPrimary;
        WeaponPrice = weaponPrice;
    }
    public bool IsPrimary { get; private set; }
    public int WeaponPrice { get; private set; }
}

public abstract class Weapon : NetworkBehaviour
{
    public WeaponName weaponName;
    public int ammoCapacity;
    public int magazineSize;
    public int currentAmmo;
    public float fireRate;
    public float reloadTime;
    public bool isAutomatic;
    
    public enum WeaponName
    {
        [WeaponAttr(false, 0)] NONE,
        [WeaponAttr(false, 650)] ARMOR,
        [WeaponAttr(false, 1000)] ARMOR_HELM,
        [WeaponAttr(false, 0)] C4,
        [WeaponAttr(true, 2700)] AK47, 
        [WeaponAttr(false, 200)] USPS,
    }

    protected bool isReloading = false;

    public abstract void Fire();
    public IEnumerator Reload()
    {
        if (currentAmmo < ammoCapacity)
        {
            isReloading = true;
            yield return new WaitForSeconds(reloadTime);

            if (currentAmmo != 0)
            {
                ammoCapacity += currentAmmo;
            }
            
            if (ammoCapacity < magazineSize)
            {
                ammoCapacity = 0;
                currentAmmo = ammoCapacity;
            }
            else
            {
                ammoCapacity -= magazineSize; 
                currentAmmo = magazineSize;
            }
            
            isReloading = false;
        }
    }

    protected virtual void Start()
    {
        currentAmmo = ammoCapacity;
    }
}
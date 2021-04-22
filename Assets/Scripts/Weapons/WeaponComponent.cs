using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType
{
    None,
    Melee,
    Gun
}

[Serializable]
public struct WeaponStats
{
    public WeaponType WeaponType;
    public string WeaponName;
    public float Damage;
    public int BulletsInClip;
    public int ClipSize;
    public int BulletsAvailable;
    public float FireStartDelay;
    public float FireRate;
    public float FireDistance;
    public bool Repeating;
    public LayerMask WeaponHitLayers;
}


public class WeaponComponent : MonoBehaviour
{

    public Transform GripLocation => GripIKLocation;
    [SerializeField] private Transform GripIKLocation;

    public WeaponStats Stats => weaponStats;
    [SerializeField] private WeaponStats weaponStats;

    protected WeaponHolder WeaponHolder;
    protected CrosshairBehaviour CrosshairComponent;
    protected Camera MainCamera;

    public bool Firing { get; private set; }
    public bool Reloading { get; private set; }

    private void Awake()
    {
        MainCamera = Camera.main;
    }

    public void Initialize(WeaponHolder weaponHolder, CrosshairBehaviour crossHair)
    {
        WeaponHolder = weaponHolder;
        CrosshairComponent = crossHair;
    }

    public virtual void StartFiringWeapon()
    {
        Firing = true;

        if (weaponStats.Repeating)
            InvokeRepeating(nameof(FireWeapon), weaponStats.FireStartDelay, weaponStats.FireRate);
        else
            FireWeapon();
    }

    public virtual void StopFiringWeapon()
    {
        Firing = false;
        //if (FiringEffect) Destroy(FiringEffect.gameObject);
        CancelInvoke(nameof(FireWeapon));
    }

    protected virtual void FireWeapon()
    {
        weaponStats.BulletsInClip--;
    }
    public virtual void StartReloading()
    {
        Reloading = true;
    }

    public virtual void StopReloading()
    {
        Reloading = false;
        ReloadWeapon();
    }

    public virtual void ReloadWeapon()
    {
        //if (FiringEffect) Destroy(FiringEffect.gameObject);

        int bulletsToReload = weaponStats.ClipSize - weaponStats.BulletsAvailable;
        if (bulletsToReload < 0)
        {
            weaponStats.BulletsInClip = weaponStats.ClipSize;
            weaponStats.BulletsAvailable -= weaponStats.ClipSize;
        }
        else
        {
            weaponStats.BulletsInClip = weaponStats.BulletsAvailable;
            weaponStats.BulletsAvailable = 0;
        }
    }

}

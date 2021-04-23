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
    [SerializeField] protected Transform ParticleSpawnLocation;
    [SerializeField] protected GameObject FiringAnimation;

    public WeaponStats Stats => weaponStats;
    [SerializeField] private WeaponStats weaponStats;

    protected WeaponHolder WeaponHolder;
    protected CrosshairBehaviour CrosshairComponent;
    protected Camera MainCamera;
    protected ParticleSystem FiringEffect;

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
        if (FiringEffect) Destroy(FiringEffect.gameObject);
        CancelInvoke(nameof(FireWeapon));
    }

    protected virtual void FireWeapon()
    {
        weaponStats.BulletsInClip--;
    }
    public virtual void StartReloading()
    {
        Reloading = true;
        if (FiringEffect) Destroy(FiringEffect.gameObject);
    }

    public virtual void StopReloading()
    {
        Reloading = false;
        ReloadWeapon();
    }

    public virtual void ReloadWeapon()
    {
        int bulletsLeft = weaponStats.ClipSize - weaponStats.BulletsAvailable;
        int bulletsToReload = weaponStats.ClipSize - weaponStats.BulletsInClip;
        if (bulletsLeft < 0)
        {
            weaponStats.BulletsInClip = weaponStats.ClipSize;
            weaponStats.BulletsAvailable -= bulletsToReload;
        }
        else
        {
            weaponStats.BulletsInClip = weaponStats.BulletsAvailable;
            weaponStats.BulletsAvailable = 0;
        }
    }

}

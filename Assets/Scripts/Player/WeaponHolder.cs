using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController), typeof(Animator))]
public class WeaponHolder : MonoBehaviour
{
    [SerializeField] GameObject WeaponToSpawn;
    [SerializeField] Transform weaponSocket;
    private Transform gripIK;
    private WeaponComponent EquippedWeapon;

    PlayerController player;
    CrosshairBehaviour crosshair;
    Animator animator;
    Camera viewCam;

    public readonly int AimHorizontalHash = Animator.StringToHash("AimHorizontal");
    public readonly int AimVerticalHash = Animator.StringToHash("AimVertical");
    public readonly int WeaponTypeHash = Animator.StringToHash("WeaponType");
    public readonly int IsFiringHash = Animator.StringToHash("IsFiring");
    public readonly int IsReloadingHash = Animator.StringToHash("IsReloading");


    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
        if (player) crosshair = player.Crosshair;

        viewCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(WeaponToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipWeapon(GameObject toEquip)
    {
        //Spawn Chosen Weapon
        var spawnedWeapon = Instantiate(toEquip, weaponSocket);
        if (spawnedWeapon)
        {
            EquippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
            if (EquippedWeapon)
            {
                gripIK = EquippedWeapon.GripLocation;
                animator.SetInteger(WeaponTypeHash, (int)EquippedWeapon.type);
            }
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, gripIK.position);
    }

    public void OnLook(InputValue delta)
    {
        Vector3 independentMousePos = viewCam.ScreenToViewportPoint(crosshair.CurrentAimPos);

        animator.SetFloat(AimHorizontalHash, independentMousePos.x);
        animator.SetFloat(AimVerticalHash, independentMousePos.y);
    }

    public void OnReload(InputValue pressed)
    {
        if (player.IsReloading) return; //Don't Reload twice

        animator.SetBool(IsReloadingHash, true);
        player.IsReloading = true;
        StartCoroutine(StopReloading());
    }

    IEnumerator StopReloading() //Should be obsoleted idealy by animreloadingscript
    {
        yield return new WaitForSeconds(2f);
        player.IsReloading = false;
    }
    public void OnFire(InputValue value)
    {
        animator.SetBool(IsFiringHash, value.isPressed);
        player.IsFiring = value.isPressed;
    }
}

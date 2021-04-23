using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AK47Component : WeaponComponent
{
    protected override void FireWeapon()
    {
        if (Stats.BulletsInClip > 0 && !Reloading && !WeaponHolder.Player.IsRunning)
        {
            base.FireWeapon();

            if (!FiringEffect)
            {
                FiringEffect = Instantiate(FiringAnimation, ParticleSpawnLocation).GetComponent<ParticleSystem>();
            }

            Ray screenRay = MainCamera.ScreenPointToRay(new Vector3(CrosshairComponent.CurrentAimPos.x,
                CrosshairComponent.CurrentAimPos.y, 0));

            if (!Physics.Raycast(screenRay, out RaycastHit hit,
                Stats.FireDistance, Stats.WeaponHitLayers)) return;

            Vector3 hitDirection = hit.point - MainCamera.transform.position;
            Debug.DrawRay(MainCamera.transform.position, hitDirection.normalized * Stats.FireDistance, Color.red);
        }
        else if (Stats.BulletsInClip <= 0 && Stats.BulletsAvailable > 0)
        {
            if (!WeaponHolder) return;

            WeaponHolder.StartReloading();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponAmmoUI : MonoBehaviour
{
    WeaponComponent weapon;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text currentBulletText;
    [SerializeField] TMP_Text totalBulletText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!weapon) return;

        nameText.text = weapon.Stats.WeaponName;
        currentBulletText.text = weapon.Stats.BulletsInClip.ToString();
        totalBulletText.text = weapon.Stats.BulletsAvailable.ToString();
    }

    private void OnEnable()
    {
        PlayerEvents.OnWeaponEquipped += OnWeaponEquipped;
    }
    private void OnDisable()
    {
        PlayerEvents.OnWeaponEquipped -= OnWeaponEquipped;
    }

    void OnWeaponEquipped(WeaponComponent weapon)
    {
        this.weapon = weapon;
    }

}

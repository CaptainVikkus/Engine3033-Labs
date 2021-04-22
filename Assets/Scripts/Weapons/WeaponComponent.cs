using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public enum Type
    {
        None,
        Melee,
        Gun
    }

    public Transform GripLocation => GripIKLocation;
    [SerializeField] private Transform GripIKLocation;

    public Type type;
}

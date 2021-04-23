using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for causing death on gameobjects
public interface IKillable
{
    void Kill();
}

//Interface for causing damage to a gameobject
public interface IDamageable
{
    void Damage(float damageTaken);
}
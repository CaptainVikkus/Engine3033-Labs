using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float RotationPower = 250f;
    [SerializeField] float HorizontalDamping = 1f;
    [SerializeField] Transform FollowTarget;

    Vector2 previousAim = Vector2.zero;

    public void OnLook(InputValue value)
    {
        Vector2 aim = value.Get<Vector2>();

        //Smooth Rotation
        Quaternion addRot = Quaternion.AngleAxis(
            Mathf.Lerp(previousAim.x, aim.x, 1f / HorizontalDamping) 
            * RotationPower * Time.deltaTime,
            transform.up);
        //Rotate cam horizontal
        FollowTarget.rotation *= addRot;

        previousAim = aim;

        //Rotate player and reset cam look
        transform.rotation = Quaternion.Euler(0, FollowTarget.rotation.eulerAngles.y, 0);
        FollowTarget.localEulerAngles = Vector3.zero;
    }
}

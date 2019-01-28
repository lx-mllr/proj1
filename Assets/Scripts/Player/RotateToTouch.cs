using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RotateToTouch : MonoBehaviour
{
    [Inject] readonly IInputManager _inputManager;

    Quaternion _targetRotation;

    [Range(0.0f, 1.0f)]
    public float RotationSpeed;

    // Update is called once per frame
    void Update()
    {
        _targetRotation = Quaternion.LookRotation(_inputManager.Position3D - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, RotationSpeed);
    }
}

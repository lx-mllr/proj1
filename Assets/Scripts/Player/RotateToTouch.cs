using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RotateToTouch : MonoBehaviour
{
    [Inject] readonly IInputManager _inputManager;

    Quaternion _targetRotation;

    public Transform toRotate;
    [Range(0.0f, 1.0f)]
    public float RotationSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (toRotate) {
            _targetRotation = Quaternion.LookRotation(_inputManager.Position3D - toRotate.position, Vector3.up);
            toRotate.rotation = Quaternion.Lerp(toRotate.rotation, _targetRotation, RotationSpeed);
        }
    }
}

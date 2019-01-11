using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Shooter : MonoBehaviour
{
    Vector3 _dir;

    [Inject]
    IInputManager _inputMan;

    [Range(0f, 0.25f)]
    public float variance;

    // Start is called before the first frame update
    void Start()
    {
        _dir = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputMan.Firing) {
            Vector3 direction = _dir + new Vector3((Random.value-.5f)*variance, (Random.value-.5f)*variance, 0);
            direction = transform.rotation * direction;
            Debug.Log(direction);
            Debug.DrawRay(transform.position, direction, Color.white, Time.deltaTime);
        }
    }
}

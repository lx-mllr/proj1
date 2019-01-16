using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{

    private Marching _marching;
    private Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        _marching = GetComponentInChildren<Marching>();
        // _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (_marching) {
            transform.position += transform.forward * (_marching.MoveSpeed * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{

    private Marching marching;
    private Vector3 _dir;
    // Start is called before the first frame update
    void Start()
    {
        marching = GetComponentInChildren<Marching>();
        _dir = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (marching) {
            transform.position += _dir * (marching.MoveSpeed * Time.deltaTime);
        }
    }
}

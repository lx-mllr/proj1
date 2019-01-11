using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marching : MonoBehaviour
{
    [Range(0f, 5f)]
    public float moveSpeed = 1f;
    public Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        dir = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * (moveSpeed * Time.deltaTime);
    }
}

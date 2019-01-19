using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Vector3 target = Vector3.zero;
    [Range(0f, 2f)]
    public float speed = 1f;
    public bool lockY = true;

    float _dist;
    float y;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        _dist = Vector3.Distance(pos, new Vector3(target.x, pos.y, target.y));
        y = pos.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_dist * Mathf.Sin(speed * Time.time), 
                                        y, 
                                        _dist * Mathf.Cos(speed * Time.time));
        transform.rotation = Quaternion.LookRotation(target - transform.position);
    }
}

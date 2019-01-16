using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marching : MonoBehaviour
{
    [Range(0f, 1f)]
    public float animatedSpeedOffset = 0f;
    public float MAX_SPEED = 3f;

    public float MoveSpeed {
        get { return MAX_SPEED * animatedSpeedOffset; }
    }
}

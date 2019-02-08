using UnityEngine;
using System;
using Zenject;

public class LayerUtil {

    [Flags]
    private enum Layers {
        Everything = -1,
        Nothing = 0,
        Default = 1,

        Player = 1 << 9,
        EQ1 = 1 << 10,
        EQ2 = 1 << 11,
        EQ3 = 1 << 12,
        EQ4 = 1 << 13, 
        EQ5 = 1 << 14,
        EQ6 = 1 << 15,
        EQ7 = 1 << 16,
        EQ8 = 1 << 17
    }

    //           Z
    //    \  Q8  |  Q1  /
    //      \    |    /
    //   Q7   \  |  /    Q2
    //          \|/
    //------------------------- X
    //          /|\
    //  Q6    /  |  \   Q3
    //      / Q5 | Q4 \
    //    /      |      \

    

    // shooting the raycast
    public static int GetLayerMaskFromPos(Vector3 position) {
        Vector3 norm = position.normalized;
        float zProj = Vector3.Dot(norm, Vector3.forward);
        float xProj = Vector3.Dot(norm, Vector3.right);
        int quadrant = (int)Layers.Nothing;

        if (xProj > .9f) {
            quadrant |= (int)(Layers.EQ2 | Layers.EQ3);
        }
        else if (xProj < -.9f) {
            quadrant |= (int)(Layers.EQ6 | Layers.EQ7);
        }
        if (zProj > .9f) {
            quadrant |= (int)(Layers.EQ1 | Layers.EQ8);
        }
        else if (zProj < -.9f) {
            quadrant |= (int)(Layers.EQ4 | Layers.EQ5);
        }

        if (xProj > .4f) {
            if (zProj > 0f) {
                quadrant |= (int)Layers.EQ2;
            } else {
                quadrant |= (int)Layers.EQ3;
            }
        }
        if (xProj > 0.1f && xProj < .6f) {
            if (zProj > 0f) {
                quadrant |= (int)Layers.EQ1;
            } else {
                quadrant |= (int)Layers.EQ4;
            }
        }
        if (xProj < -.4f) {
            if (zProj > 0f) {
                quadrant |= (int)Layers.EQ7;
            } else {
                quadrant |= (int)Layers.EQ6;
            }
        }
        if (xProj < -.1f && xProj > -.6f) {
            if (zProj > 0f) {
                quadrant |= (int)Layers.EQ8;
            } else {
                quadrant |= (int)Layers.EQ5;
            }
        }

        return quadrant;
    }

    // placing the enemy
    public static int GetLayerFromPos (Vector3 position) {
        Vector3 norm = position.normalized;
        float zProj = Vector3.Dot(norm, Vector3.forward);
        float xProj = Vector3.Dot(norm, Vector3.right);
        int quadrant = (int)Layers.Nothing;

        // Q2 or Q3
        if (xProj > 0.5) {
            if (zProj > 0) {
                quadrant = (int)Layers.EQ2;
            } else {
                quadrant = (int)Layers.EQ3;
            }
        // Q1 or Q4
        } else if (xProj > 0) {
            if (zProj > 0) {
                quadrant = (int)Layers.EQ1;
            } else {
                quadrant = (int)Layers.EQ4;
            }
        // Q5 or Q8
        } else if (xProj > -.5) {
            if (zProj > 0) {
                quadrant = (int)Layers.EQ8;
            } else {
                quadrant = (int)Layers.EQ5;
            }
        // Q6 or Q7
        } else {
            if (zProj > 0) {
                quadrant = (int)Layers.EQ7;
            } else {
                quadrant = (int)Layers.EQ6;
            }
        }

        // log won't work for 1 << 31
        return (int)Mathf.Log((float)quadrant, 2f);
    }
}
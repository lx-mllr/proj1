using UnityEngine;
using System;
using Zenject;

public class LayerUtil {
    public static int GetLayerFromPos (Vector3 position) {
        Vector3 norm = position.normalized;
        float zProj = Vector3.Dot(norm, Vector3.forward);
        float xProj = Vector3.Dot(norm, Vector3.right);
        
        int quadrant = 1;
        if (zProj > 0f && zProj <= .5f) {
            quadrant = xProj > 0 ? 1 : 4;
        }
        else if (zProj > .5f && zProj <= 1f) {
            quadrant = xProj > 0 ? 2 : 3;
        }
        else if (zProj < 0f && zProj >= -.5f) {
            quadrant = xProj > 0 ? 8 : 5;
        }
        else if (zProj < -.5f && zProj >= -1f) {
            quadrant = xProj > 0 ? 7 : 6;
        }
        return LayerMask.NameToLayer("EQ" + quadrant);
    }
}
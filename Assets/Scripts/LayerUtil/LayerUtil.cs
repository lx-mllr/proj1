using UnityEngine;
using System;
using Zenject;

public class LayerUtil {
    public static int GetLayerFromPos (Vector3 position) {
        Vector3 norm = position.normalized;
        float zProj = Vector3.Dot(norm, Vector3.forward);
        float xProj = Vector3.Dot(norm, Vector3.right);

        int quadrant = xProj >= 0 ? 
                                zProj >= 0 ? 1 : 2 
                                : zProj >= 0 ? 4 : 3;

        
        return LayerMask.NameToLayer("EQ" + quadrant);
    }

    public static LayerMask GetLayerMaskFromPos (Vector3 position) {
        Vector3 norm = position.normalized;
        float zProj = Vector3.Dot(norm, Vector3.forward);
        float xProj = Vector3.Dot(norm, Vector3.right);

        int quadrant = xProj >= 0 ? 
                                zProj >= 0 ? 1 : 2 
                                : zProj >= 0 ? 4 : 3;

        
        return LayerMask.GetMask("EQ" + quadrant);
    }
}
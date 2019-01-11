using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MonoDEBUG : MonoBehaviour
{

    [Inject]
    IInputManager _inputMan;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _inputMan.Position3D;
    }
}

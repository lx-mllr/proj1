using UnityEngine;
using Zenject;

public class InputManager : IInputManager {
    
    private Vector3 _3dPos;
    public Vector3 Position3D { get {return _3dPos;} }

    private bool _firing;
    public bool Firing { get {return _firing;} }

    private Camera _cam;
    private Vector3 _centerScreen;

    public InputManager () {
    }

    public void Initialize() {
        _cam = Camera.main;
        _centerScreen = new Vector3(_cam.pixelWidth / 2f, _cam.pixelHeight / 2f, 0f);

        _firing = false;
        _3dPos = Vector3.zero;
    }

    public void Tick () {
        _firing = false;

        if (PollFiring()) {
            _firing = true;

            Ray ray = GetScreenRay();
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                _3dPos = hit.point;
            }
        }
    }

    private bool PollFiring ()  {
        #if UNITY_EDITOR
            return Input.GetMouseButton(0);
        #else
            return Input.touchCount > 0;
        #endif
    }

    private Ray GetScreenRay() { 
        Vector3 pos = _centerScreen;
        #if UNITY_EDITOR
            pos = Input.mousePosition;
        #else
            if (PollFiring()) {
                pos = Input.GetTouch(0).position;
            }
        #endif
        return _cam.ScreenPointToRay(pos);
    }
}
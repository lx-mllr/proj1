using UnityEngine;
using Zenject;

public interface IInputManager : IInitializable, ITickable {
    Vector3 Position3D { get; }
    bool Firing { get; }
}
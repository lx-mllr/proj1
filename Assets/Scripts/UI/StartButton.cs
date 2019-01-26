using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StartButton : MonoBehaviour
{
    [Inject] readonly SignalBus _signalBus;

    public void FireStartGame () {
        _signalBus.Fire<DestroyScreenSignal>();
        _signalBus.Fire<StartGameSignal>();
    }
}

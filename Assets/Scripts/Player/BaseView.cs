using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BaseView : MonoBehaviour
{
    [Inject] readonly ScptGameplayInstaller.Settings _settings;
    [Inject] readonly SignalBus _signalBus;

    public float HP;

    // Start is called before the first frame update
    void Start()
    {
        HP = _settings.BaseHealth;
        _signalBus.Subscribe<AttackSignal>(OnAttackSignal);
    }

    // Update is called once per frame
    void Update()
    {
        // update ui

        if (HP <= 0) {
            _signalBus.Fire<EndGameSignal>();
        }
    }

    public void OnAttackSignal(AttackSignal signal) {
        HP -= signal.damage;
        Debug.Log(signal.damage);
    }

    void OnDestroy () {
        _signalBus.Unsubscribe<AttackSignal>(OnAttackSignal);
    }
}

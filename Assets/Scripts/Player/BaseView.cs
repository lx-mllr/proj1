using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BaseView : MonoBehaviour, IHeroMono
{
    [Inject] readonly ScptGameplayInstaller.Settings _settings;
    [Inject] readonly SignalBus _signalBus;

    public float HP;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
    }

    public void Reset () {
        enabled = true;
        HP = _settings.BaseHealth;
        _signalBus.Subscribe<AttackSignal>(OnAttackSignal);
    }

    // Update is called once per frame
    void Update()
    {
        // update ui

        if (HP <= 0) {
            TriggerEndGame();
        }
    }

    public void TriggerEndGame () {
        enabled = false;
        _signalBus.Fire<EndGameSignal>();
        _signalBus.Unsubscribe<AttackSignal>(OnAttackSignal);
    }

    public void OnAttackSignal(AttackSignal signal) {
        HP -= signal.damage;
    }
}

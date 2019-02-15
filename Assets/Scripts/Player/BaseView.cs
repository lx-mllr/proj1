using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BaseView : MonoBehaviour, IHeroMono
{
    [Inject] readonly ScptGameplayInstaller.Settings _settings;
    [Inject] readonly SignalBus _signalBus;

    public float HP;
    public float UI_Update_Speed = 0.9f;
    public Renderer HealthRenderer;

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
        float old = HealthRenderer.material.GetFloat("_FillPct");
        float updated = Mathf.Lerp(old, HP / _settings.BaseHealth, UI_Update_Speed);
        HealthRenderer.material.SetFloat("_FillPct", updated);

        if (HP <= 0) {
            TriggerEndGame();
        }
    }

    public void TriggerEndGame () {
        enabled = false;
        HealthRenderer.material.SetFloat("_FillPct", 0);
        _signalBus.Fire<EndGameSignal>();
        _signalBus.Unsubscribe<AttackSignal>(OnAttackSignal);
    }

    public void OnAttackSignal(AttackSignal signal) {
        HP -= signal.damage;
    }
}

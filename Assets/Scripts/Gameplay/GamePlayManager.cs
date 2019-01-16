using System;
using UnityEngine;
using Zenject;

public class GamePlayManager : ITickable, IInitializable {
    
    [Inject] readonly ScptGameplayInstaller.Settings _settings;
    [Inject] readonly EnemyView.Factory _enemyFactory;
    [Inject] readonly SignalBus _signalBus;
    
    private int _score;
    private float _lastSpawn;

    public void Initialize () {
        Reset();
    }

    public void Reset () {
        _score = 0;
        _lastSpawn = 0f;
    }

    public void AddScore (AddScoreSignal signal) {
        _score += signal.val;

        int currScoreMod = _score / _settings.RespawnSettingsRebindThreshold;
        int prevScoreMod = (_score - signal.val) / _settings.RespawnSettingsRebindThreshold;

        if (currScoreMod > prevScoreMod) {
            Debug.Log("GPM :: REBIND");
            _signalBus.Fire<RebindEnemySpawnSettings>();
        }
    }

    public void Tick () {
        // per second
        int sRate = 1 / _settings.EnemySpawnRate;
        _lastSpawn += Time.deltaTime;

        if (_lastSpawn > sRate) {
            _enemyFactory.Create();
            _lastSpawn = 0f;
        }
    }
}

public struct AddScoreSignal {
    public int val;
}
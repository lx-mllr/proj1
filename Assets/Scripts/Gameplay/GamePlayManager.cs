using System;
using UnityEngine;
using Zenject;

public class GamePlayManager : ITickable, IInitializable {
    
    [Inject] readonly ScptGameplayInstaller.Settings _settings;
    [Inject] readonly EnemyView.Factory _enemyFactory;
    [Inject] readonly SignalBus _signalBus;
    
    private int _score;
    public int Score { get { return _score; } }
    private float _lastSpawn;
    private float _spawnRate;
    private bool _running;


    public void Initialize () {
        _running = false;
        Reset();
    }

    public void Reset () {
        _score = 0;
        _lastSpawn = 0f;
        _spawnRate = 1 / _settings.EnemySpawnRate;
    }

    public void Start () {
        _running = true;
    }

    public void AddScore (AddScoreSignal signal) {
        _score += signal.val;

        int currScoreMod = _score / _settings.RespawnSettingsRebindThreshold;
        int prevScoreMod = (_score - signal.val) / _settings.RespawnSettingsRebindThreshold;

        if (currScoreMod > prevScoreMod) {
            _signalBus.Fire<RebindEnemySpawnSettings>();
        }
    }

    public void Tick () {
        if (!_running) {
            return;
        }

        _lastSpawn += Time.deltaTime;

        if (_lastSpawn > _spawnRate) {
            _enemyFactory.Create();
            _lastSpawn = 0f;
            _spawnRate = Mathf.Lerp(1, _spawnRate, .9f);
        }
    }
}
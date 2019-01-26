using UnityEngine;
using Zenject;
using System;

[CreateAssetMenu(fileName = "ScptGameplayInstaller", menuName = "Installers/ScptGameplayInstaller")]
public class ScptGameplayInstaller : ScriptableObjectInstaller<ScptGameplayInstaller>
{
    public Settings settings;
    [Serializable]
    public struct Settings {
        public int RespawnSettingsRebindThreshold;
        public float EnemySpawnRate;
    }

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.BindInstance(settings);

        Container.DeclareSignal<AddScoreSignal>().OptionalSubscriber();
        Container.DeclareSignal<StartGameSignal>().OptionalSubscriber();

        Container.BindInterfacesAndSelfTo<GamePlayManager>().AsSingle().NonLazy();
        Container.BindSignal<AddScoreSignal>().ToMethod<GamePlayManager>(s => s.AddScore).FromResolve();
        Container.BindSignal<StartGameSignal>().ToMethod<GamePlayManager>(s => s.Start).FromResolve();
    }
}

public struct AddScoreSignal {
    public int val;
}

public struct StartGameSignal {
}
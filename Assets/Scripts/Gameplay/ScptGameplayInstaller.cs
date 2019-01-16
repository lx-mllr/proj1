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
        public int EnemySpawnRate;
    }

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.BindInstance(settings);

        Container.DeclareSignal<AddScoreSignal>().OptionalSubscriber();
        Container.BindInterfacesAndSelfTo<GamePlayManager>().AsSingle().NonLazy();

        Container.BindSignal<AddScoreSignal>().ToMethod<GamePlayManager>(s => s.AddScore).FromResolve();
    }
}
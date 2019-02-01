using UnityEngine;
using Zenject;
using System;

[CreateAssetMenu(fileName = "ScptGameplayInstaller", menuName = "Installers/ScptGameplayInstaller")]
public class ScptGameplayInstaller : ScriptableObjectInstaller<ScptGameplayInstaller>
{
    public Settings settings;
    [Serializable]
    public struct Settings {
        public int BaseHealth;
        public int RespawnSettingsRebindThreshold;
        public float EnemySpawnRate;
    }

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.BindInstance(settings);

        Container.DeclareSignal<AddScoreSignal>().OptionalSubscriber().RunAsync();
        Container.DeclareSignal<StartGameSignal>().OptionalSubscriber();
        Container.DeclareSignal<EndGameSignal>().OptionalSubscriber();
        Container.DeclareSignal<AttackSignal>().OptionalSubscriber().RunAsync();

        Container.BindInterfacesAndSelfTo<GamePlayManager>().AsSingle().NonLazy();
        Container.BindSignal<AddScoreSignal>().ToMethod<GamePlayManager>(s => s.AddScore).FromResolve();
        Container.BindSignal<StartGameSignal>().ToMethod<GamePlayManager>(s => s.Start).FromResolve();
        Container.BindSignal<EndGameSignal>().ToMethod<GamePlayManager>(s => s.Reset).FromResolve();

        Container.BindSignal<StartGameSignal>().ToMethod<IHeroMono>(s => s.Reset).FromResolve();

        InstallPersistableSystems();
    }


    public void InstallPersistableSystems () {
        Container.Bind<SaveManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<User>().AsSingle().NonLazy();
    }
}

public struct AddScoreSignal {
    public int val;
}

public struct StartGameSignal {
}

public struct EndGameSignal {
}
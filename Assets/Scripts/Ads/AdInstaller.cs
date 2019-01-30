using UnityEngine;
using Zenject;
using System;

public class AdInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<UnityAdManager>().AsSingle().NonLazy();

        Container.DeclareSignal<PlayAdSignal>().OptionalSubscriber().RunAsync();
        Container.BindSignal<PlayAdSignal>().ToMethod<IAdManager>(s => s.OnStartAd).FromResolve();
    }
}

public struct PlayAdSignal {
    public string placement;
    public Action callback;
}
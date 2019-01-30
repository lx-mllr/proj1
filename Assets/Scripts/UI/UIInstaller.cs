using UnityEngine;
using Zenject;
using System;

public class UIInstaller : MonoInstaller
{
    public Canvas canvas;
    public UISettings settings;

    public override void InstallBindings()
    {
        Container.BindInstance(canvas);
        Container.BindInstance(settings);

        Container.DeclareSignal<DestroyScreenSignal>().OptionalSubscriber().RunAsync();

        Container.BindInterfacesAndSelfTo<UIManager>().AsSingle().NonLazy();
        Container.BindSignal<StartGameSignal>().ToMethod<UIManager>(x => x.StartGame).FromResolve();
        Container.BindSignal<EndGameSignal>().ToMethod<UIManager>(x => x.Reset).FromResolve();
        Container.BindSignal<DestroyScreenSignal>().ToMethod<UIManager>(x => x.DestroyScreen).FromResolve();
    }
}

[Serializable]
public struct UISettings {
    public CanvasRenderer startScreen;
    public CanvasRenderer gameScreen;
}

public struct DestroyScreenSignal {
}
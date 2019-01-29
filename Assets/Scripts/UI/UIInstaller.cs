using UnityEngine;
using Zenject;
using System;

public class UIInstaller : MonoInstaller
{
    public Canvas canvas;
    public CanvasRenderer startScreen;
    public CanvasRenderer gameScreen;

    public override void InstallBindings()
    {
        Container.BindInstance(canvas);
        
        UISettings settings = new UISettings() {
            startScreen = startScreen,
            gameScreen = gameScreen
        };
        Container.BindInstance(settings);

        Container.DeclareSignal<DestroyScreenSignal>().OptionalSubscriber().RunAsync();

        Container.BindInterfacesAndSelfTo<UIManager>().AsSingle().NonLazy();
        Container.BindSignal<StartGameSignal>().ToMethod<UIManager>(x => x.StartGame).FromResolve();
        Container.BindSignal<EndGameSignal>().ToMethod<UIManager>(x => x.Reset).FromResolve();
        Container.BindSignal<DestroyScreenSignal>().ToMethod<UIManager>(x => x.DestroyScreen).FromResolve();
    }
}

public class UISettings {
    public CanvasRenderer startScreen;
    public CanvasRenderer gameScreen;
}

public struct DestroyScreenSignal {
}
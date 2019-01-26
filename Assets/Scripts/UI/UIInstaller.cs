using UnityEngine;
using Zenject;
using System;

public class UIInstaller : MonoInstaller
{
    public Canvas canvas;

    public UISettings settings;
    [Serializable]
    public struct UISettings {
        public CanvasRenderer startScreen;
    }

    public override void InstallBindings()
    {
        Container.BindInstance(canvas);
        Container.BindInstance(settings);

        Container.DeclareSignal<CreateScreenSignal>().OptionalSubscriber();
        Container.DeclareSignal<DestroyScreenSignal>().OptionalSubscriber();

        Container.BindInterfacesAndSelfTo<UIManager>().AsSingle().NonLazy();
        Container.BindSignal<CreateScreenSignal>().ToMethod<UIManager>(x => x.CreateScreen).FromResolve();
        Container.BindSignal<DestroyScreenSignal>().ToMethod<UIManager>(x => x.DestroyScreen).FromResolve();
    }
}

public struct CreateScreenSignal {
    public CanvasRenderer toCreate;
}

public struct DestroyScreenSignal {
}
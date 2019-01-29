using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ScreenTapInputManager>().AsSingle().NonLazy();
        Container.BindSignal<StartGameSignal>().ToMethod<IInputManager>(s => s.Enable).FromResolve();
        Container.BindSignal<EndGameSignal>().ToMethod<IInputManager>(s => s.Disable).FromResolve();
    }
}
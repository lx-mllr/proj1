using Zenject;
using System;

public interface IAdManager : IInitializable {
    void OnStartAd(PlayAdSignal signal);
    void StartBanner();
    void EndBanner();
}   
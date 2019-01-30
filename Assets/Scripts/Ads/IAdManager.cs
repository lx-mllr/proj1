using Zenject;
using System;

public interface IAdManager : IInitializable {
    void OnStartAd();
    void StartBanner();
    void EndBanner();
}   
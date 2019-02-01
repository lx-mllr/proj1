using Zenject;
using System;

public interface IAdManager : IInitializable {
    void OnStartAd(PlayAdSignal signal);
    void StartBanner();
    void EndBanner();
}   

public class AdPlacements {
    public const string VIDEO_PLACEMENT = "video";
    public const string REWARD_PLACEMENT = "rewardedVideo";
}
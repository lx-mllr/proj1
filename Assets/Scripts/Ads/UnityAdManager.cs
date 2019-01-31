using UnityEngine.Monetization;
using UnityEngine.Advertisements;
using UnityEngine;
using System;

public class UnityAdManager : IAdManager {

    public const string VIDEO_PLACEMENT = "video";
    public const string REWARD_PLACEMENT = "rewardedVideo";

    private string _gameId
        #if UNITY_ANDROID
            = "3026045";
        #elif UNITY_IOS
            = "3026044";
        #else
            = "1111111";
        #endif

    private Action _callback;

    public void Initialize () {
        Monetization.Initialize(_gameId, Debug.isDebugBuild);
        Advertisement.Initialize(_gameId, Debug.isDebugBuild);
    }
    
    public void OnStartAd (PlayAdSignal signal) {
        _callback = signal.callback;
        PlayAd(signal.placement);
    }

    public void StartBanner () {
        Advertisement.Banner.Show();
    }

    public void EndBanner () {
        Advertisement.Banner.Hide(false);
    }

    private void PlayAd (string placement) {
        if (Monetization.IsReady(placement)) {
            ShowAdPlacementContent ad = Monetization.GetPlacementContent(placement) as ShowAdPlacementContent;
            ad.Show(FinishAd);
        }
    }
    
    private void FinishAd (UnityEngine.Monetization.ShowResult result) {
        switch (result) {
            case UnityEngine.Monetization.ShowResult.Finished:
                if (_callback != null) {
                    _callback();
                    _callback = null;
                }
                break;
            case UnityEngine.Monetization.ShowResult.Failed:
            case UnityEngine.Monetization.ShowResult.Skipped:
                Debug.Log("UnityAdMan - Ad End: " + result);
                break;
        }
    }
}
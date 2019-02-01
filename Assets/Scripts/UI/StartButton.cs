using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartButton : MonoBehaviour
{
    public const int PLAYCOUNT_AD_TRHESHOLD = 5;

    public Image adIcon;

    [Inject] readonly SignalBus _signalBus;
    [Inject] readonly User _user;
    [Inject] readonly IAdManager _adManager;

    private bool playVideoAd;

    void Start () {
        playVideoAd = (_user.state.playCount > 0) && (_user.state.playCount % PLAYCOUNT_AD_TRHESHOLD == 0);
        adIcon.enabled = playVideoAd;
    }

    public void OnStartClick () {
        if (playVideoAd) {
            _signalBus.Fire(new PlayAdSignal() {
                placement = AdPlacements.VIDEO_PLACEMENT, 
                callback = DispatchStart
            });
        }
        else {
            DispatchStart();
        }
    }

    private void DispatchStart () {
        _signalBus.Fire<StartGameSignal>();
    }
}

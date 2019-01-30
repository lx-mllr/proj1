using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameHUD : MonoBehaviour
{
    [Inject] readonly SignalBus _signalBus;

    Text scoreText;
    int _score = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponentInChildren<Text>();
        _signalBus.Subscribe<AddScoreSignal>(UpdateScore);
    }

    private void UpdateScore(AddScoreSignal signal) {
        _score += signal.val;
        if (scoreText) {
            scoreText.text = _score.ToString();
        }
    }

   void OnDestroy () {
       _signalBus.TryUnsubscribe<AddScoreSignal>(UpdateScore);
   }
}

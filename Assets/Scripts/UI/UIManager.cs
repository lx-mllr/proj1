using UnityEngine;
using Zenject;

public class UIManager : IInitializable {

    [Inject] readonly Canvas _canvas;
    [Inject] readonly UISettings _settings;

    private GameObject _activeScreen;

    public void Initialize () {
        Reset();
    }

    public void Reset () {
        CreateScreen(_settings.startScreen);
    }

    public void StartGame (StartGameSignal signal) {
        CreateScreen(_settings.gameScreen);
    }

    public void CreateScreen (CanvasRenderer screen) {
        if (_activeScreen) {
            DestroyScreen();
        }

        _activeScreen = Object.Instantiate(screen, _canvas.transform, false).gameObject;
    }

    public void DestroyScreen () {
        if (_activeScreen) {
            Object.DestroyImmediate(_activeScreen);
            _activeScreen = null;
        }
    }
}
using UnityEngine;
using Zenject;

public class UIManager : IInitializable {

    [Inject] readonly Canvas _canvas;
    [Inject] readonly UIInstaller.UISettings _settings;

    private CanvasRenderer _activeScreen;
    public CanvasRenderer activeScreen { get { return _activeScreen; } }

    public void Initialize () {
        CreateScreen(new CreateScreenSignal() {
            toCreate = _settings.startScreen
        });
    }

    public void CreateScreen (CreateScreenSignal signal) {
        if (_activeScreen) {
            DestroyScreen();
        }

        _activeScreen = CanvasRenderer.Instantiate(signal.toCreate, _canvas.transform, false);
    }

    public void DestroyScreen () {
        if (_activeScreen) {
            GameObject.Destroy(_activeScreen.gameObject);
            _activeScreen = null;
        }
    }
}
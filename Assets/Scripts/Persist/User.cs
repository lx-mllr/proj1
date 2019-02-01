using System;
using Zenject;

public class User : IInitializable {

    [Inject]
    SaveManager _saveMangager;

    private const string STATE_FILENAME = "UserState.dat";
    private UserState _state;
    public UserState state { get {return _state; } }

    private bool _dirty;

    public User () {
    }

    public void Initialize () {
        _state = _saveMangager.Load<UserState>(STATE_FILENAME);
        _dirty = false;
    }

    public void SaveState() {
        if (_dirty) {
            _saveMangager.Save(STATE_FILENAME, _state);
            _dirty = false;
        }
    }

    public void UpdateScore(int roundScore) {
        _dirty = roundScore > _state.highScore;
        _state.highScore = Math.Max(roundScore, _state.highScore);
    }

    public void IncrementPlays() {
        _dirty = true;
        _state.playCount++;
    }
}

[Serializable]
public struct UserState {
    public int highScore;
    public int playCount;
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public interface IScreenState {
    void EnterState();
    void ExitState();

    void BindRouter(IStateRouter router);
}

public interface IStateRouter {
    bool IsCurrentState<T>() where T : class, IScreenState;
    bool HistoryIsEmpty { get; }
    IScreenState CurrentState { get; }
    void ChangeState<T>() where T : class, IScreenState;
    void ReloadState<T>() where T : class, IScreenState;
    IScreenState ReturnBack();
}

public abstract class ScreenStates : IStateRouter {
    
    private readonly List<IScreenState> _states = new();
    private readonly Stack<IScreenState> _history = new();
    
    private IScreenState _currentState;

    public bool IsCurrentState<T>() where T : class, IScreenState => _currentState is T;

    public UnityEvent<IScreenState> StateChanged { get; } = new ();
    public bool HistoryIsEmpty => _history.Count == 0;
    public IScreenState CurrentState => _currentState;

    public void AddState(IScreenState state) {
        state.BindRouter(this);
        _states.Add(state);
    }

    public void ChangeState<T>() where T : class, IScreenState {
        if(_currentState != null) _history.Push(_currentState);
        
        ChangeTo(_states.OfType<T>().FirstOrDefault());
    }

    public void ReloadState<T>() where T : class, IScreenState {
        ChangeTo(_states.OfType<T>().FirstOrDefault());
    }

    private void ChangeTo(IScreenState state, bool reload = false) {
        if (reload || state != _currentState) {
            _currentState?.ExitState();
            _currentState = state;
            _currentState?.EnterState();
            
            StateChanged?.Invoke(_currentState);
        }
    }

    public IScreenState ReturnBack() {
        ChangeTo(_history.Pop());
        return _currentState;
    }

    public void Dispose() => ChangeTo(null);
}

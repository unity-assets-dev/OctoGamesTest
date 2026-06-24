using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public static class StatesExtensions {

    private static MonoBehaviour _player;

    private static MonoBehaviour Player {
        get {
            if (_player == null) _player = Object.FindAnyObjectByType<AppEntry>();
            
            return _player;
        }
    }

    public static (IStateRouter router, IStateScreen screen) CreateBindingWith(this IStateScreen screen, IStateRouter router) => (router, screen);

    public static IButtonCommand AsCommand<TButton, TState>(this (IStateRouter router, IStateScreen screen) binding)
        where TButton : class, ILayoutButton
        where TState : class, IScreenState {
        var command = binding.router.CreateCommand<TState>();
        binding.screen.OnButtonClick<TButton>(command);
        return command;
    }

    public static IButtonCommand InitializeWith(this IButtonCommand command, bool state) {
        command.State.Set(state);
        return command;
    }
    
    public static IButtonCommand TrackCommand<TState>(this IStateRouter router, Action<TState> onExecuted = null) where TState : class, IScreenState {
        if(router == null) throw new ArgumentNullException(nameof(router));
        return IButtonCommand.Create(() => {
            router.ChangeState<TState>();
            onExecuted?.Invoke(router.CurrentState as TState);
        });
    }

    public static IButtonCommand CreateCommand<TState>(this IStateRouter router, Action onExecuted = null) where TState : class, IScreenState {
        if(router == null) throw new ArgumentNullException(nameof(router));
        return IButtonCommand.Create(() => {
            router.ChangeState<TState>();
            onExecuted?.Invoke();
        });
    }

    public static IButtonCommand CreateBackCommand(this IStateRouter router, Action onExecuted = null) =>
        IButtonCommand.Create(() => {
            router.ReturnBack();
            onExecuted?.Invoke();
        });

    public static Coroutine PlayCoroutine(this IEnumerator routine) => Player.StartCoroutine(routine);

    public static Coroutine Replay(this Coroutine coroutine, IEnumerator routine)  {
        coroutine.StopCoroutine();
        return routine.PlayCoroutine();
    }
    
    public static void StopCoroutine(this Coroutine routine) {
        if(routine != null) 
            Player.StopCoroutine(routine);
    }

    private static IEnumerator DelayCommand(float time, Action onDelay) {
        yield return new WaitForSeconds(time);
        onDelay?.Invoke();
    }
    
    public static void Delay<TState>(this IStateRouter router, float delay)  where TState: class, IScreenState {
        DelayCommand(delay, router.ChangeState<TState>).PlayCoroutine();
    }

}

public static class Delay {
    public static Coroutine Execute(float time, Action onComplete) {
        return Run().PlayCoroutine();
        IEnumerator Run() {
            yield return new WaitForSeconds(time);
            
            onComplete?.Invoke();
        }
    }
}
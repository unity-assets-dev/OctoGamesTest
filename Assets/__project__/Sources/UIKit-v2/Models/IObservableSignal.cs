using System;

public interface IObservableSignal<T> {
    T Value { get; set; }
    void Once(Action<T> listener);
}

public class ObservableSignal<T> : IObservableSignal<T> {
    private Action<T> _listeners;
    private T _value;

    public T Value {
        get => _value;
        set {
            _value = value;
            _listeners?.Invoke(value);
        }
    }

    public void Once(Action<T> listener) => _listeners += listener;

    private void ClearListener(Action<T> listener) => _listeners -= listener;

    public void ClearListeners() => _listeners.GetInvocationList().EachNonAlloc(l => ClearListener(l as Action<T>));
}

public interface IObservableSignal {
    void Touch();
    void Once(Action listener);
    void ClearListener(Action listener);
    void ClearListeners();
}

public class ObservableSignal : IObservableSignal {
    private Action _listeners;

    public void Touch() => _listeners?.Invoke();

    public void Once(Action listener) => _listeners += listener;

    public void ClearListeners() => _listeners.GetInvocationList().EachNonAlloc(l => ClearListener(l as Action));

    public void ClearListener(Action listener) => _listeners -= listener;
}
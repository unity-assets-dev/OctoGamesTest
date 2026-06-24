using System;

public interface ILayoutButton : ILayoutElement {
    void AddListener(IButtonCommand command);
    void RemoveListener(IButtonCommand command);
    void RemoveAllListeners();
}

public interface IButtonCommand {
    IObservableField<bool> State { get; }
    
    void Execute();

    public static IButtonCommand Create(Action onExecuted) => new DefaultCommand(onExecuted);

    private class DefaultCommand : IButtonCommand {
        private readonly Action _onExecuted;

        public IObservableField<bool> State { get; } = new ObservableField<bool>(true);

        public DefaultCommand(Action onExecuted) => _onExecuted = onExecuted;

        public void Execute() {
            if (State.Value) _onExecuted();
        }
    }
}
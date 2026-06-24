using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class MenuButton: MonoBehaviour, ILayoutButton {
    [SerializeField] private Button _button;
    private IButtonCommand _command;

    private void OnValidate() {
        _button ??= GetComponent<Button>();
        name = $"[{GetType().Name}]";

        OnValidateNext();
    }

    protected virtual void OnValidateNext() {}

    public void OnLayoutShow() {}
    
    public void OnLayoutHide() => _button.onClick.RemoveAllListeners();

    public void AddListener(IButtonCommand command) {
        _button.onClick.AddListener(() => {
            command?.Execute();
        });
        _command = command;
        SubscribeTo(command);
    }

    private void SubscribeTo(IButtonCommand command) => command?.State.AddListener(OnCommandChangeState);

    private void OnCommandChangeState(bool state) {
        _button.interactable = state;
        OnCommandStateChanged(_button, state);
    }

    protected virtual bool OnCommandStateChanged(Button button, bool toState) => toState;

    private void UnSubscribeFrom(IButtonCommand command) => command?.State.RemoveListener(OnCommandChangeState);

    public void RemoveListener(IButtonCommand command) {
        _button.onClick.RemoveAllListeners();
        UnSubscribeFrom(command);
    }

    public void RemoveAllListeners() {
        _button.onClick.RemoveAllListeners();
        UnSubscribeFrom(_command);
    }

    [NaughtyAttributes.Button]
    private void DebugEvents() {
        var events = _button.onClick.GetPersistentEventCount();
        Debug.Log(events);
    }
    
}
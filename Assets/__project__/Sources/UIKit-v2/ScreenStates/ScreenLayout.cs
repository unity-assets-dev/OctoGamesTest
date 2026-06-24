using System;
using System.Linq;
using UnityEngine;

public abstract class ScreenLayout : MonoBehaviour {
    
    private ILayoutElement[] _elements = Array.Empty<ILayoutElement>();
    
    public void Initialize() {
        _elements = GetComponentsInChildren<ILayoutElement>(true);
        
        _elements.OfType<ILayoutInitialize>().EachNonAlloc(i => i.Initialize());
    }

    protected bool TryGetElement<T>(out T element) where T : class, ILayoutElement {
        element = _elements.OfType<T>().FirstOrDefault();
        
        return element != null;
    }
    
    protected bool TryGetElements<T>(out T[] elements) where T : class, ILayoutElement {
        elements = _elements.OfType<T>().ToArray();
        
        return elements != null && elements.Length > 0;
    }

    public bool TryGetExtension<T>(out T extension) where T : class, ILayoutElement {
        extension = _elements.OfType<T>().FirstOrDefault();
        return extension != null;
    }

    protected void ShowElements() => _elements.EachNonAlloc(e => e.OnLayoutShow());
    protected void HideElements() => _elements.EachNonAlloc(e => e.OnLayoutHide());

    public void OnButtonClick<T>(Action command) where T : class, ILayoutButton {
        OnButtonClick<T>(IButtonCommand.Create(command));
    }

    public void OnButtonClick<T>(IButtonCommand command) where T : class, ILayoutButton {
        if (TryGetElements<T>(out var buttons)) {
            buttons.EachNonAlloc(button => button.AddListener(command));
        }
        
    }
}
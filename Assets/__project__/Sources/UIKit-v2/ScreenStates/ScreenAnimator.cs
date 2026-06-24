using System;
using UnityEngine;

public class ScreenAnimator : MonoBehaviour {
    
    public virtual void Show(Action onComplete) {
        gameObject.SetActive(true);
        onComplete?.Invoke();
    }
    

    public virtual void Hide(Action onComplete) {
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }
    
}
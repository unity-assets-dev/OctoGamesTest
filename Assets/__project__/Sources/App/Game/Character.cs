using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Character : Actor, IPointerClickHandler {
    [SerializeField] private int _health;
    
    public UnityEvent<Character, int> HealthChanged { get; } = new();

    public override void OnCreate() {
        _health = Random.Range(10, 21);
    }

    public void OnPointerClick(PointerEventData _) {
        _health -= Random.Range(1, 4);
        
        HealthChanged?.Invoke(this, _health);

        transform.DOPunchRotation(Vector3.up * 36, .25f);
    }

    public override void Dispose() => HealthChanged.RemoveAllListeners();
    
}
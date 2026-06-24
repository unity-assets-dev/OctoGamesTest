using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour {
    [SerializeField] private ActorFactory _factory;
    
    [SerializeField] private int _minActors = 5;
    [SerializeField] private int _maxActors = 15;

    public UnityEvent<int> CharacterCount { get; } = new ();
    

    public void StartGame(int actorCount) {
        Enumerable.Range(0, actorCount).EachNonAlloc(index => {
            var position = Random.insideUnitSphere.normalized * 5;
            position.y = 0;
            var actor = _factory.CreateActor<Character>(position);
            actor.HealthChanged.AddListener(OnCharacterHealthChanged);
        });
        
        CharacterCount?.Invoke(_factory.Count());
    }

    private void OnCharacterHealthChanged(Character actor, int health) {
        if (health <= 0) {
            actor.HealthChanged.RemoveAllListeners();
            _factory.DisposeActor(actor);
            CharacterCount?.Invoke(_factory.Count());
        }
    }

    public void PauseGame() {
        Time.timeScale = 0;
    }

    public void StopGame() {
        Time.timeScale = 1;
        
        _factory.DisposeAll();
    }

    public void ResumeGame() {
        Time.timeScale = 1;
    }
}
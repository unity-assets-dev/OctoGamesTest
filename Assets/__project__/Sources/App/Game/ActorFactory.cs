using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Actor : MonoBehaviour {
    
    public virtual void OnCreate() {}
    public virtual void Dispose() {}
    
}

public class ActorFactory : MonoBehaviour, IEnumerable<Actor> {
    
    [SerializeField] private Actor[] _prefabs;

    private readonly HashSet<Actor> _actors = new ();

    private bool TryGetPrefab<T>(out T prefab) where T : Actor {
        prefab = _prefabs.OfType<T>().FirstOrDefault();
        
        return prefab != null;
    }
    
    public T CreateActor<T>(Vector3 position) where T : Actor {

        if (TryGetPrefab<T>(out var prefab)) {
            var instance = Instantiate(prefab, position, Quaternion.identity, transform);
            _actors.Add(instance);
            instance.OnCreate();
            return instance;
        }
        
        throw new NullReferenceException("Failed to create actor");
    }

    private void Update() {
        // TODO: You're may iterate actors from here;
        // TODO: to get small boost on control and performance;
        // TODO: This doesn't make sense now;
    }

    public void DisposeActor(Actor actor) {
        _actors.Remove(actor);
        actor.Dispose();
        Destroy(actor.gameObject);
    }

    public void DisposeAll() {
        // TODO: Clean up scene;
        _actors.Each(DisposeActor);
    }

    public IEnumerator<Actor> GetEnumerator() => _actors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

using UnityEngine;
using Zenject;

public class AppEntry : MonoBehaviour {
    
    private AppStates _states;

    [Inject]
    private void Construct(AppStates states) => _states = states;

    private void Start() => _states.ChangeState<BootstrapState>();

    private void OnApplicationQuit() => _states.Dispose();
}

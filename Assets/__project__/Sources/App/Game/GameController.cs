using UnityEngine;

public class GameController : MonoBehaviour {
    public void StartGame() {
        
    }

    public void PauseGame() {
        Time.timeScale = 0;
    }

    public void StopGame() {
        Time.timeScale = 1;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
    }
}

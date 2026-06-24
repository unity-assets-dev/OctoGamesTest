using TMPro;
using UnityEngine;

public interface IPlayerDataHandler {
    void UpdateCoins(int coins);
}

public class MainMenuScreen : MenuScreen, IPlayerDataHandler {

    [SerializeField] private TMP_Text _coinsField;
    
    public void UpdateCoins(int coins) {
        _coinsField.text = coins.ToString();    
    }
}

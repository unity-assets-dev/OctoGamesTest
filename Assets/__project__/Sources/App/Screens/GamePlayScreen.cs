using TMPro;
using UnityEngine;

public class GamePlayScreen : MenuScreen, IPlayerDataHandler {
    
    [SerializeField] private TMP_Text _coinsField;
    
    public void UpdateCoins(int coins) {
        _coinsField.text = coins.ToString();    
    }
}
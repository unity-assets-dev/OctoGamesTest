using PrimeTween;
using TMPro;
using UnityEngine;

public class GamePlayScreen : MenuScreen, IPlayerDataHandler, IPlayerCharactersHandler {
    
    [SerializeField] private TMP_Text _coinsField;
    [SerializeField] private TMP_Text _charactersField;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    public void UpdateCoins(int coins) {
        _coinsField.text = coins.ToString();    
    }

    public void DisplayHints() {
        DOTween
            .Sequence()
            .Append(_canvasGroup.DOFade(0, 0))
            .Append(_canvasGroup.DOFade(1, .5f))
            .AppendInterval(2)
            .Append(_canvasGroup.DOFade(0, .5f))
            .Play();
    }

    public void UpdateCharacterCount(int value) {
        _charactersField.text = value.ToString();
    }
}
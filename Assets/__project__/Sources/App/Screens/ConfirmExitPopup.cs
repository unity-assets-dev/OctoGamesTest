using System;
using UIKit_v2.ScreenStates.Popup;

public class ConfirmExitPopup : MenuPopup<ConfirmExitPopup> {
    
    public static void Choice(Action<bool> onChoice) {
        var handler = PopupHandler.ShowPopup<ConfirmExitPopup>();  
        
        handler.OnButtonClick<ConfirmChoiceButton>(() => ClosePopup(true));
        handler.OnButtonClick<CancelChoiceButton>(() => ClosePopup(false));

        void ClosePopup(bool state) {
            onChoice?.Invoke(state);
            handler.Hide();
        }
        
    }
}
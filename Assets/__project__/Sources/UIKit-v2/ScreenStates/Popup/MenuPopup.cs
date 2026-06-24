using System;
using System.Linq;
using Zenject;

namespace UIKit_v2.ScreenStates.Popup {

    public class PopupHandler: IInitializable {
        private static PopupHandler DefaultInstance { get; set; }
        private readonly IMenuPopup[] _screens;

        public PopupHandler(IMenuPopup[] screens) {
            _screens = screens;
        }

        private bool TryGetInstance<T>(out T instance) where T : IMenuPopup {
            instance = _screens.OfType<T>().FirstOrDefault();
            return instance != null;
        }

        public static bool TryGetPopup<T>(out T popup) where T : IMenuPopup => DefaultInstance.TryGetInstance(out popup);

        public static T ShowPopup<T>(Action<T> onExecute = null) where T : IMenuPopup {
            if (TryGetPopup<T>(out var popup)) {
                popup.Show(() => onExecute?.Invoke(popup));
                
                return popup;
            }
            
            throw new NullReferenceException("Popup Not Found");
        }

        public void Initialize() {
            DefaultInstance = this;
        }
    }

    public interface IMenuPopup : IStateScreen {
        
    }
    
    public abstract class MenuPopup<T> : MenuScreen, IMenuPopup where T: MenuPopup<T> {
        
    }
}
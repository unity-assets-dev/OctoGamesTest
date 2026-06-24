using UIKit_v2.ScreenStates.Popup;

public class UIKitInstaller : KitInstallerBase {
    protected override void BindServices() {
        Container.BindAsSingleFromInstanceMono<GameController>();
        Container.BindAsSingle<PopupHandler>();
    }

    protected override void OnBindTargetInstances() { }
}
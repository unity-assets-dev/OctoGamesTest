public class BootstrapState: ScreenState<BootstrapScreen>, IScreenState {


    public BootstrapState(BootstrapScreen screen) : base(screen) {}

    protected override void OnStateEnter() {
        Screen.Show();
        
        // Wait for loading bootstrap data;
        Router.Delay<MainMenuState>(1f); 
    }

    protected override void OnStateExit() => Screen.Hide();
}

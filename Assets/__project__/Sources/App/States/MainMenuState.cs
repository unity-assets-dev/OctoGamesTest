public class MainMenuState : ScreenState<MainMenuScreen>, IScreenState {
    private readonly PlayerModel _playerData;

    public MainMenuState(PlayerModel playerData, MainMenuScreen screen) : base(screen) {
        _playerData = playerData;
    }

    protected override void OnStateEnter() {
        
    }

    protected override void OnStateExit() {
        
    }
}

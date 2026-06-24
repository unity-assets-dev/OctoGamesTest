public class MainMenuState : ScreenState<MainMenuScreen>, IScreenState {
    private readonly PlayerModel _playerData;
    private readonly IButtonCommand _startGameCommand;

    public MainMenuState(PlayerModel playerData, GameController gameController, MainMenuScreen screen) : base(screen) {
        _playerData = playerData;
        _startGameCommand = IButtonCommand.Create(() => {
            gameController.StartGame();
            Router.ChangeState<GamePlayState>();
        });
    }

    protected override void OnStateEnter() {
        Screen.OnButtonClick<StartGameButton>(_startGameCommand);
    }

    protected override void OnStateExit() {
        
    }
}
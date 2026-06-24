public class GamePlayState : ScreenState<GamePlayScreen>, IScreenState {
    
    private readonly PlayerModel _playerData;
    private readonly IButtonCommand _tryExitGameCommand;

    public GamePlayState(PlayerModel playerData, GameController gameController, GamePlayScreen screen) : base(screen) {
        _playerData = playerData;
        _tryExitGameCommand = IButtonCommand.Create(() => {
            gameController.PauseGame();
            
            ConfirmExitPopup.Choice((exit) => {
                if (exit) {
                    gameController.StopGame();
                    Router.ChangeState<MainMenuState>();
                }
                else {
                    gameController.ResumeGame();
                }
            });
        });
    }

    protected override void OnStateEnter() {
        Screen.OnButtonClick<PauseGameButton>(_tryExitGameCommand);
        Screen.DisplayHints();
    }

    protected override void OnStateExit() {
        
    }
}
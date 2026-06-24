using UnityEngine;

public class GamePlayState : ScreenState<GamePlayScreen>, IScreenState {
    
    private readonly PlayerModel _playerData;
    private readonly GameController _gameController;
    private readonly IButtonCommand _tryExitGameCommand;

    public GamePlayState(PlayerModel playerData, GameController gameController, GamePlayScreen screen) : base(screen) {
        _playerData = playerData;
        _gameController = gameController;
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
        
        _gameController.CharacterCount.AddListener((count) => _playerData.CharactersCount.Set(count));
        _gameController.StartGame(Random.Range(5, 15));
    }

    protected override void OnStateExit() {
        _gameController.CharacterCount.RemoveAllListeners();
    }
}
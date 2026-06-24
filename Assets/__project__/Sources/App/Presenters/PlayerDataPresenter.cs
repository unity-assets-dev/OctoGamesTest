[PresenterOf(typeof(MainMenuState))]
public class PlayerDataPresenter : IPresenter {
    private readonly PlayerModel _playerData;

    public PlayerDataPresenter(PlayerModel playerData) {
        _playerData = playerData;
    }
    
    public void OnEnter(IStateRouter router, IStateScreen screen) {
        if (screen is IPlayerDataHandler handler) {
            _playerData.Coins.AddListener(coins => handler.UpdateCoins(coins));    
        }
    }

    public void BeforeExit(IStateScreen screen) {
        // TODO: Run to cleanup a presenter work;  
        // for example:
        _playerData.Coins.RemoveAllListeners();    
    }
    
}

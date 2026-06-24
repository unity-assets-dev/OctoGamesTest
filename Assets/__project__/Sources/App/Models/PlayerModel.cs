public class PlayerModelData : SerializableData<PlayerModelData> {
    public int coins;
    public bool firstRun;
}

public class PlayerModel: IUIKitModel {

    public IObservableField<int> Coins { get; } = new ObservableField<int>();
    public IObservableField<int> CharactersCount { get; } = new ObservableField<int>();
    public IObservableField<bool> FirstRun { get; } = new  ObservableField<bool>();

    public PlayerModel() {
        // Init with defaults data;
        var data = PlayerModelData.Deserialize(new PlayerModelData {
            coins = 10,
            firstRun = false
        });
        
        Coins.Set(data.coins);
        FirstRun.Set(data.firstRun);
    }

    public void ForceSave() {
        PlayerModelData.Serialize(new PlayerModelData() {
            coins = Coins.Value,
            firstRun = FirstRun.Value
        });
    }
}

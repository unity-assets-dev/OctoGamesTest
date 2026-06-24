##1. Coding Principles (Short Answer)

Describe two coding principles or practices you consider most important when working on real
Unity projects that mix:
● 3D gameplay;
● UI systems;
● iteration by designers.
Explain why they matter and where you apply them.

##2. Save / Load Utility (Production Basics) 

Для универсализации сериализации данных, я давно использую универсальный подход
через создание абстракного класса, который умеет сериализовать\десериализовать класс и инкапсулирует внутри себя всю необходимую дополнительную логику
И впринципе позволяет осуществлять подмену места записи (EasySave3/PlayerPrefs/File/Stream etc)

```
public abstract class SerializableData<T> {

    private static string Key => typeof(T).Name;
    
    public static void Serialize(T data) {
        var source = data.Serialize(); // Serialize into the string (by extension for JSON.net);
        
#if UNITY_EDITOR
        PlayerPrefs.SetString(Key, source);
#else
        ES3.Save(source, Key);
#endif
    }

    public static T Deserialize(T defaults = default) {
        if (!PlayerPrefs.HasKey(Key))
            return defaults;

        var source = PlayerPrefs.GetString(Key);
        
        return source.IsNullOrEmpty() ? defaults : source.Deserialize<T>();
    }
}

public class PlayerModelData : SerializableData<PlayerModelData> {
    public int coins;
    public bool firstRun;
}

public class PlayerModel: IUIKitModel {

    public IObservableField<int> Coins { get; } = new ObservableField<int>();
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


```


##3. Popup / UI System (UI + Architecture)

Я обычно использую для архитектурной базы паттерн состояние, состояния между собой общаются посредством сервисов и\или моделей
В дополнение к обычному подходу, для легкости переиспользования и чистоты кода, я так же реализовываю систему плагинов\презентеров
для экранных представлений, которые подгружаются при переключении состояний и выгружаются, тем самым происходит полный контроль ресурсов (удобно следить за подписками\отписками)
Тем самым, можно изолировать логику в отдельных независимых частях, работать с различными сервисами и держать код в чистоте (по необходимости).
Данный подход позволяет довольно просто переносить и логику и приложение целиком (например, как я поступил здесь).

##4. UI Performance & Refactoring (Core Unity Skill)

```
public class CharactersView : MonoBehaviour {
	// 1. Нужно избегать вызовов GetComponent(); этот вызов довольно тяжелый
	// Если в списке лежат только объекты определенного типа, то поле следует типизировать
	[SerializedField] private List<Transform> _characters;
	
	private void FixedUpdate() {
		float totalValue = 0f;
		
		foreach (Transform characterTransform in _characters) {
			// 3. Ошибка, множественный выбор компонентов - на данном этапе будет ошибка
			// Плюс, вызов тяжелой функции в Update, усугубленный итерированием по коллекции
			Character character = characterTransform.gameObject.GetComponents<Character>();
			totalValue += character != null ? character.Value : 0f;
		}
		
		string text = string.Format(
			"Characters: {0} Avg value: {1}",
			_characters.Length, // 4. Ошибка - Count
			_characters.Length / totalValue // 5. Ошибка вычисления среднего
			);
		// 2. По той же причине, компонент следует закешировать
		gameObject.GetComponent<Text>().text = text;
		Debug.Log(text);
	}
}

```

```
public class CharactersView : MonoBehaviour {
	[SerializedField] private Text _textField;
	[SerializedField] private List<Character> _characters;
	
	private void OnValidate() {
		// Если компонент был неназначен в инспекторе, пробуем его закешировать самостоятельно;
		_textField ??= GetComponent<Text>();
		// Дополнительно, можно выбросить сообщение об ошибке
	}
	
	private void FixedUpdate() {
		int characterCount = _characters.Count;
		float totalValue = 0f;
		
		foreach (Character character in _characters) {
			totalValue += character.Value;
		}
		
		string text = string.Format(
			"Characters: {0} Avg value: {1}",
			characterCount,
			totalValue / characterCount
			);
			
		// alt: string text = $"Characters: {characterCount} Avg value: {totalValue / characterCount}";
			
		_textField = text;
		Debug.Log(text);
	}
	
	// alt: С точки зрения производительности и уместности, гораздо правильнее отслеживать изменение данных в списке, чем итерироваться в пустую.
	private void OnEnable() {
		foreach(var character in _characters) {
			character.ValueChanged.AddListener(OnCharacterValueChanged);
		}
		
		// и\или если нужно отслеживать изменения динамического списка, ссылаться на отдельный компонент (который будет инкапсулировать логику добавления\изменения персонажей):
		// ... _characters.CharacterChanged.AddListener(OnCharacterListUpdated);
		// ну, или вообще использовать реактивные подходы, через UniRx ex.
	}
	
	/*
	
	private void OnCharacterListUpdated(int count, float avg) {
		string text = $"Characters: {count} Avg value: {avg}";
			
		_textField = text;
		Debug.Log(text);
	}
	
	*/
	
	private void OnDisable() {
		foreach(var character in _characters) {
			character.ValueChanged.RemoveListener(OnCharacterValueChanged);
		}
		
		// ... _characters.CharacterChanged.RemoveListener(OnCharacterListUpdated);
	}
	
	private void OnCharacterValueChanged(Character character) {
		int characterCount = _characters.Count;
		float totalValue = 0f;
		
		foreach (Character character in _characters) {
			totalValue += character.Value;
		}
		
		string text = string.Format(
			"Characters: {0} Avg value: {1}",
			characterCount,
			totalValue / characterCount
			);
			
		// alt: string text = $"Characters: {characterCount} Avg value: {totalValue / characterCount}";
			
		_textField = text;
		Debug.Log(text);
	}
}

```

5. Gameplay / State Logic (3D + Systems Thinking)
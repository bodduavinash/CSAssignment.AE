using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsController : SingletonWithMonobehaviour<CardsController>, IGameDataPersistence
{
    [SerializeField] private CardDataManagerSO _cardDataList;
    [SerializeField] private GameObject _cardObjectPrefab;
    [SerializeField] private Sprite _questionSprite;

    [SerializeField] private List<Sprite> _fruitsAndVeggiesSpritesList;
    [SerializeField] private List<CardLayoutObject> _cardLayoutGameObjects;

    private CardLayoutsEnum _currentLayoutSelected;

    private CardDataSO _cardDataProp;

    private List<GameObject> _spawnedCardsInLayout = new List<GameObject>();
    private List<Sprite> _finalSelectedSprites = new List<Sprite>();
    private List<CardObjectProps> _cardClickedList = new List<CardObjectProps>(2);
    private List<int> _matchedCardList = new List<int>();

    private bool _isGameLoaded = false;

    private readonly int CARD_NOT_MATCHED = 0;
    private readonly int CARD_MATCHED = 1;
    private readonly int ADD_SCORE = 1;
    private readonly float SECONDS_TO_WAIT = 0.2f;

    private UIManager _uiManager;
    private GameSceneManager _gameSceneManager;
    private GameScoreManager _gameScoreManager;
    private DataPersistenceManager _dataPersistenceManager;

    private void Start()
    {
        _uiManager = (UIManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.UIManager);
        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);
        _gameScoreManager = GameScoreManager.Instance;
        _dataPersistenceManager = (DataPersistenceManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.DataPersistenceManager);

        _currentLayoutSelected = _uiManager.CurrentLayoutEnumSelected;

        _cardDataProp = _cardDataList.GetCardDataListSO().Find(props => props.layoutEnum == _currentLayoutSelected).cardDataSO;

        PopulateCards();
    }

    private void PopulateCards()
    {
        UpdateGameScore();
        InstantiateCardsInLayout();
        InitCardsMatchedList();
        _finalSelectedSprites = _dataPersistenceManager.IsGameDataLoaded() ? _finalSelectedSprites : GenerateAndRandomizeCardsInLayout();
        ApplyPropsToCardObject();
    }

    private void UpdateGameScore()
    {
        if (!_isGameLoaded)
        {
            _gameScoreManager.ResetScores();
        }
    }

    private void InitCardsMatchedList()
    {
        if (_dataPersistenceManager.IsGameDataLoaded())
        {
            for (int i = 0; i < _matchedCardList.Count; i++)
            {
                if (_matchedCardList[i] == CARD_MATCHED)
                {
                    _spawnedCardsInLayout[i].gameObject.GetComponent<CardObjectProps>().EnableChildGameObjects(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < _cardDataProp.GetNoOfCardsInLayout(); i++)
            {
                _matchedCardList.Add(CARD_NOT_MATCHED);
            }
        }
    }

    private void InstantiateCardsInLayout()
    {
        GameObject _currentLayoutGO = _cardLayoutGameObjects.Find(go => go.cardLayoutEnum == _currentLayoutSelected).CardLayoutGameObject;

        for(int i = 0; i < _cardDataProp.GetNoOfCardsInLayout(); i++)
        {
            var _spawnedCard = Instantiate(_cardObjectPrefab, Vector3.zero, Quaternion.identity);
            _spawnedCard.transform.localScale = Vector3.one * _cardDataProp.GetCardSpriteScaleSize();
            _spawnedCard.name = "CardObject_" + i;
            _spawnedCard.transform.SetParent(_currentLayoutGO.transform);

            var _cardObject = _spawnedCard.GetComponent<CardObjectProps>();
            _cardObject.SetCardImage(_questionSprite);
            _cardObject.PrefabIndex = i;

            _spawnedCardsInLayout.Add(_spawnedCard);
        }
    }

    private List<Sprite> GenerateAndRandomizeCardsInLayout()
    {
        List<Sprite> _fruitsAndVeggiesSpritesClone = new List<Sprite>(_fruitsAndVeggiesSpritesList);
        int _noOfPairs = _cardDataProp.GetNoOfCardsInLayout() / 2;

        for(int i = 0; i < _noOfPairs; i++)
        {
            int _randomElement = Random.Range(0, _fruitsAndVeggiesSpritesClone.Count);

            //Adding two sprites to list to become the pairs
            _finalSelectedSprites.Add(_fruitsAndVeggiesSpritesClone[_randomElement]);
            _finalSelectedSprites.Add(_fruitsAndVeggiesSpritesClone[_randomElement]);

            _fruitsAndVeggiesSpritesClone.Remove(_fruitsAndVeggiesSpritesClone[_randomElement]);
        }

        System.Random _rnd = new System.Random();
        _finalSelectedSprites = _finalSelectedSprites.OrderBy(item => _rnd.Next()).ToList();

        return _finalSelectedSprites;
    }

    private void ApplyPropsToCardObject()
    {
        for(int i = 0; i < _spawnedCardsInLayout.Count; i++)
        {
            var _cardObjectProps = _spawnedCardsInLayout[i].GetComponent<CardObjectProps>();
            _cardObjectProps.RevealSprite = _finalSelectedSprites[i];
            _cardObjectProps.SpriteIndex = int.Parse(_finalSelectedSprites[i].name.Split('_')[1]);
        }
    }

    public void CheckIfCardsMatching(CardObjectProps _cardObject)
    {
        if (_cardClickedList.Count < 2)
        {
            _cardClickedList.Add(_cardObject);
        }

        if(_cardClickedList.Count >= 2)
        {
            StartCoroutine(CheckIfCardsMatchingEnumerator());
        }
    }

    private IEnumerator CheckIfCardsMatchingEnumerator()
    {
        int _spriteIndex = _cardClickedList[0].SpriteIndex;

        if (_cardClickedList.TrueForAll(_card => _card.SpriteIndex == _spriteIndex))
        {
            yield return new WaitForSeconds(SECONDS_TO_WAIT);

            _gameScoreManager.UpdateMatchedScore(ADD_SCORE);
            _gameScoreManager.UpdateTurnsScore(ADD_SCORE);

            _cardClickedList.ForEach(_co => {
                _co.EnableChildGameObjects(false);

                int index = int.Parse(_co.gameObject.name.Split('_')[1]);
                _matchedCardList[index] = CARD_MATCHED;
            });
        }
        else
        {
            yield return new WaitForSeconds(SECONDS_TO_WAIT);

            _gameScoreManager.UpdateTurnsScore(ADD_SCORE);

            _cardClickedList.ForEach(_co => {
                _co.FlipCard(false);
            });
        }

        _cardClickedList.Clear();
        CheckIfAllCardsAreMatched();

        StopCoroutine(CheckIfCardsMatchingEnumerator());
        yield return null;
    }

    private void CheckIfAllCardsAreMatched()
    {
        if(_matchedCardList.TrueForAll(value => value == CARD_MATCHED))
        {
            _dataPersistenceManager.SaveGameData();
            _gameSceneManager.LoadScene(GameScenesEnum.GameOverScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }

    public void LoadGameData(GameData gameData)
    {
        if (_uiManager == null)
        {
            _uiManager = (UIManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.UIManager);
        }

        if (gameData.currentLayout != CardLayoutsEnum.None)
        {
            _uiManager.CurrentLayoutEnumSelected = gameData.currentLayout;
        }

        _finalSelectedSprites = gameData.finalSelectedList;
        _matchedCardList = gameData.matchedCardList;

        _isGameLoaded = true;
    }

    public void SaveGameData(ref GameData gameData)
    {
        gameData.currentLayout = _uiManager.CurrentLayoutEnumSelected;
        gameData.finalSelectedList = _finalSelectedSprites;
        gameData.matchedCardList = _matchedCardList;
    }
}
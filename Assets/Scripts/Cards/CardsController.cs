using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardsController : SingletonWithMonobehaviour<CardsController>
{
    [SerializeField] private CardDataManagerSO _cardDataList;
    [SerializeField] private GameObject _cardObjectPrefab;
    [SerializeField] private Sprite _questionSprite;

    [SerializeField] private List<Sprite> _fruitsAndVeggiesSpritesList;
    [SerializeField] private List<CardLayoutObject> _cardLayoutGameObjects;

    [SerializeField] private CardLayoutsEnum _currentLayoutSelected = CardLayoutsEnum.Layout2x2;

    private List<GameObject> _spawnedCardsInLayout = new List<GameObject>();
    private List<Sprite> _finalSelectedSprites = new List<Sprite>();
    private List<CardObjectProps> _cardClickedList = new List<CardObjectProps>(2);
    private List<int> _matchedCardList = new List<int>();

    private readonly int CARD_NOT_MATCHED = 0;
    private readonly int CARD_MATCHED = 1;
    private readonly float SECONDS_TO_WAIT = 0.2f;

    private void Start()
    {
        PopulateCards();
    }

    private void PopulateCards()
    {
        InstantiateCardsInLayout();
        _finalSelectedSprites = GenerateAndRandomizeCardsInLayout();
        ApplyPropsToCardObject();
    }

    private void InstantiateCardsInLayout()
    {
        GameObject _currentLayoutGO = _cardLayoutGameObjects.Find(go => go.cardLayoutEnum == _currentLayoutSelected).CardLayoutGameObject;
        var _cardDataProp = _cardDataList.GetCardDataListSO().Find(props => props.layoutEnum == _currentLayoutSelected).cardDataSO;        

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

            _matchedCardList.Add(CARD_NOT_MATCHED);
        }
    }

    private List<Sprite> GenerateAndRandomizeCardsInLayout()
    {
        List<Sprite> _fruitsAndVeggiesSpritesClone = new List<Sprite>(_fruitsAndVeggiesSpritesList);
        var _cardDataProp = _cardDataList.GetCardDataListSO().Find(props => props.layoutEnum == _currentLayoutSelected).cardDataSO;
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

            for (int i = 0; i < 2; i++)
            {
                _matchedCardList[i] = CARD_MATCHED;
                _cardClickedList[i].EnableChildGameObjects(false);
            }
        }
        else
        {
            //yield return new WaitUntil( () => !_cardClickedList.TrueForAll(_card => _card.IsCardAnimating()));
            yield return new WaitForSeconds(SECONDS_TO_WAIT);

            _cardClickedList.ForEach(_co => {
                _co.FlipCard(false);
            });
        }

        _cardClickedList.Clear();
        StopCoroutine(CheckIfCardsMatchingEnumerator());
        yield return null;
    }
}
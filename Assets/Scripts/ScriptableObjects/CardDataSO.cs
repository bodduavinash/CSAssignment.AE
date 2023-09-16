using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardDataSO", order = 2)]
public class CardDataSO : ScriptableObject
{
    [SerializeField] private int _noOfCardsInLayout = 2;
    [SerializeField] private float _cardSpriteScaleSize = 1.0f;

    public float GetCardSpriteScaleSize()
    {
        return _cardSpriteScaleSize;
    }

    public int GetNoOfCardsInLayout()
    {
        return _noOfCardsInLayout;
    }
}

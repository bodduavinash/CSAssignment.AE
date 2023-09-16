using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataManager", menuName = "ScriptableObjects/CardDataManagerSO", order = 1)]
public class CardDataManagerSO : ScriptableObject
{
    [SerializeField] private List<CardDataObject> _cardDataList;

    public List<CardDataObject> GetCardDataListSO()
    {
        return _cardDataList;
    }
}

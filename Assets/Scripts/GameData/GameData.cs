using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public CardLayoutsEnum currentLayout;
    public List<string> layoutSpritesList;
    public List<int> matchedCardList;

    public int noOfTurns;
    public int noOfMatched;
}

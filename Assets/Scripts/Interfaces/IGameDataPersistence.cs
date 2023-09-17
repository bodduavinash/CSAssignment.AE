using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameDataPersistence
{
    void LoadGameData(GameData gameData);
    void SaveGameData(ref GameData gameData);
}

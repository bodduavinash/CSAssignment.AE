using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonWithMonobehaviour<UIManager>
{
    private CardLayoutsEnum layoutSelected = CardLayoutsEnum.None;

    public CardLayoutsEnum CurrentLayoutEnumSelected { set => layoutSelected = value; get => layoutSelected; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

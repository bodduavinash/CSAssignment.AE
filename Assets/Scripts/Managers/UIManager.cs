using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonWithMonobehaviour<UIManager>
{
    public CardLayoutsEnum CurrentLayoutEnumSelected { set => layoutSelected = value; get => layoutSelected; }

    private CardLayoutsEnum layoutSelected = CardLayoutsEnum.None;
    
    private void Awake()
    {
        if (FindObjectOfType<UIManager>().gameObject.scene.name.CompareTo(DontDestroyOnLoadObjects.DONT_DESTROY_ON_LOAD) == 1)
        {
            DontDestroyOnLoad(this);
        }
    }
}

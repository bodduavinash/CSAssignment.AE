using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObjectProps : MonoBehaviour, ISetCardImage
{
    [SerializeField] private Sprite _questionSprite;
    [SerializeField] private Image _cardImage;

    public Sprite RevealSprite { get; set; }
    public int PrefabIndex { get; set; }
    public int SpriteIndex { get; set; }

    private Button _cardButton;

    private bool _shouldStartFlipAnimation = false;
    private bool _isCardAnimating = false;

    private float _angle = 0f;
    private Quaternion _rotation;

    private readonly int ROTATION_SPEED = 1;
    private readonly int TARGET_ANGLE = 180;

    private void Start()
    {
        _rotation = transform.rotation;

        _cardButton = GetComponent<Button>();
        _cardButton.onClick.AddListener(OnButtonClicked);
    }

    public void SetCardImage(Sprite _cardSprite)
    {
        _cardImage.sprite = _cardSprite;
    }

    public void FlipCard(bool _shouldFlip)
    {
        ResetFlip();
        EnableButton(!_shouldFlip);
        PlayFlipAnimation(_shouldFlip);
        SwapSprite(_shouldFlip);
    }

    public void EnableChildGameObjects(bool _shouldEnable)
    {
        Image[] _childImages = GetComponentsInChildren<Image>();

        foreach (var _child in _childImages)
        {
            _child.gameObject.SetActive(_shouldEnable);
        }
    }

    private void OnButtonClicked()
    {
        FlipCard(true);
        CardsController.Instance.CheckIfCardsMatching(this);
    }

    public bool IsCardAnimating()
    {
        return _isCardAnimating;
    }

    private void SwapSprite(bool _shouldSwap)
    {
        _cardImage.sprite = _shouldSwap ? RevealSprite : _questionSprite;        
    }

    private void EnableButton(bool _shouldEnable)
    {
        _cardButton.interactable = _shouldEnable;
    }

    private void PlayFlipAnimation(bool _shouldFlip)
    {
        _shouldStartFlipAnimation = _shouldFlip;
    }

    private void ResetFlip()
    {
        _shouldStartFlipAnimation = false;
        _isCardAnimating = false;

        _angle = 0;
        _rotation.y = 0;

        transform.localRotation = _rotation;
    }

    private void Update()
    {
        if(_shouldStartFlipAnimation && _angle <= TARGET_ANGLE)
        {
            _isCardAnimating = true;

            _angle += ROTATION_SPEED;
            _rotation.y = _angle;

            if(_angle > TARGET_ANGLE - ROTATION_SPEED)
            {
                ResetFlip();
            }

            transform.localRotation = _rotation;
        }
    }
}
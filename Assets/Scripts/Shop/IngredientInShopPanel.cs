using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientInShopPanel : MonoBehaviour
{
    public IngredientSO ingredient { get; private set; }
    private ShopContent _shopContent;

    [Space]
    [Header("Components")]
    [SerializeField] private Image _ingredientPanelImage;
    [SerializeField] private Image _ingredientImage;
    [SerializeField] private Text _name;
    [SerializeField] private GameObject _checkBox;

    [Space]
    [Header("Sprites")]
    [SerializeField] private Sprite _lockSprite;
    [SerializeField] private Sprite _unlockSprite;

    [SerializeField] private Animation _clickAnimation;

    private bool _isSelected;


    public void SetIngredient(ShopContent shopContent, IngredientSO ingredientSO)
    {
        _shopContent = shopContent;


        ingredient = ingredientSO;
        _name.text = ingredientSO.name;
        _isSelected = ingredientSO.isSelected;


        _ingredientImage.sprite = ingredientSO.sprite;
        _ingredientImage.gameObject.SetActive(true);

        if (ingredientSO.isAlwaysActive)
        {
            _isSelected = true;
            ingredientSO.isSelected = true;
            ingredientSO.isLock = false;
        }


        if (ingredientSO.isLock)
        {
            _ingredientPanelImage.sprite = _lockSprite;
            _ingredientImage.gameObject.SetActive(false);
        }

        _checkBox.SetActive(_isSelected);
    }


    public void UnblockThis()
    {
        ingredient.isLock = false;
        ingredient.isSelected = true;

        _isSelected = true;
        _checkBox.SetActive(_isSelected);
        _ingredientImage.gameObject.SetActive(true);

        _ingredientPanelImage.sprite = _unlockSprite;
    }

    public void Click()
    {
        print("CLICK");

        if (!ingredient.isLock && !ingredient.isAlwaysActive)
        {
            _clickAnimation.Play();
            SoundManager.instance.ClickSelect();

            if (_isSelected && _shopContent.allSelectedIngredients <= 3) // Don`t UNSELECTED this ingredient, if selected 3 ingredients in the shop
                return;


            _isSelected = !_isSelected;
            _checkBox.SetActive(_isSelected);
            ingredient.isSelected = _isSelected;

            if (ingredient.isSelected)
            {
                _shopContent.SelectedIngredient();
            }
            else
            {
                _shopContent.UnselectedIngredient();
            }


            ingredient.SaveData();
        }
    }
}

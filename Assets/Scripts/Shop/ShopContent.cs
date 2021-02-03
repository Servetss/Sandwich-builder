using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    private const int _ingredientCostDefault = 20;
    private const int _differenceBetweenIngredientCost = 10;
    

    public int allSelectedIngredients { get; private set; }
    public int allLocketIngredients { get; private set; }


    [SerializeField] private GameObject _shopPanel;

    [SerializeField] private GameObject _ingredientUIPrefab;
    private IngredientInShopPanel[] _ingredientInShopList;


    [SerializeField] private IngredientSO[] _ingredientInfo;
    [SerializeField] private int[] _lockIngredientIndexes;


    [Space]
    [Header("Random purchase")]
    [SerializeField] private GameObject _randomPurchasePanel;
    [SerializeField] private Button _randomPurchaseButton;


    [Space]
    [SerializeField] private Text _ingredientsCountText;
    [SerializeField] private Text _ingredientCostText;

    private void Awake()
    {
        allSelectedIngredients = 0;
        _ingredientInShopList = new IngredientInShopPanel[_ingredientInfo.Length];
        LoadIngredientsSO();

        for (int i = 0; i < _ingredientInfo.Length; i++)
        {
            GameObject ingredientUI = Instantiate(_ingredientUIPrefab, transform);
            _ingredientInShopList[i] = ingredientUI.GetComponent<IngredientInShopPanel>();
            _ingredientInShopList[i].SetIngredient(this, _ingredientInfo[i]);


            if (_ingredientInfo[i].isSelected)
            {
                SelectedIngredient();
            }

            if (_ingredientInfo[i].isLock)
            {
                allLocketIngredients++;
            }
        }

        // Set All lock
        int lockCount = 0;
        _lockIngredientIndexes = new int[allLocketIngredients];
        for (int i = 0; i < _ingredientInfo.Length; i++)
        {
            if (_ingredientInfo[i].isLock)
            {
                _lockIngredientIndexes[lockCount] = i;
                lockCount++;
            }
        }

        _randomPurchaseButton.interactable = allLocketIngredients > 0;
    }




    private void Start()
    {
        IngredientCountRender();
        _ingredientCostText.text = GetIngredientCost().ToString();

        _shopPanel.SetActive(false);
    }


    #region Save / Load
    private void LoadIngredientsSO()
    {
        if (GameManager.instance.IsFirstStart())
        {
            for (int i = 0; i < _ingredientInfo.Length; i++)
            {
                _ingredientInfo[i].DefaultSettings();
            }
        }
        else
        {
            for (int i = 0; i < _ingredientInfo.Length; i++)
            {
                _ingredientInfo[i].LoadData();
            }
        }
    }

    private void SaveIngredientsSO()
    {
        for (int i = 0; i < _ingredientInfo.Length; i++)
        {
            _ingredientInfo[i].SaveData();
        }
    }
    #endregion



    public void SelectedIngredient()
    {
        allSelectedIngredients++;
    }

    public void UnselectedIngredient()
    {
        allSelectedIngredients--;
    }


    public IngredientSO UnblockIngredient(int ingredientindex, int lockIndex)
    {
        allLocketIngredients--;
        _lockIngredientIndexes[lockIndex] = -1;
        _ingredientInShopList[ingredientindex].UnblockThis();

        _randomPurchaseButton.interactable = allLocketIngredients > 0;

        SaveIngredientsSO();

        return _ingredientInShopList[ingredientindex].ingredient;
    }


    public void BuyRandomIngredient()
    {
        if (allLocketIngredients > 0 && Wallet.passiveMoney >=  GetIngredientCost())
        {
            Wallet.Buy(GetIngredientCost());
            _randomPurchasePanel.SetActive(true);
            IngredientCountRender();

            _ingredientCostText.text = GetIngredientCost().ToString();
        }
        else
        {
            // no money
        }
    }

    private void IngredientCountRender()
    {
        _ingredientsCountText.text = allSelectedIngredients + " / " + _ingredientInfo.Length;
    }

    #region Ingredient lists
    public IngredientSO[] GetAllIngredient()
    {
        return _ingredientInfo;
    }

    public IngredientSO GetIngredientByIndex(int index)
    {
        return _ingredientInfo[index];
    }

    public int GetLockIngredientByIndex(ref int index)
    {
        while (_lockIngredientIndexes[index] == -1)
        {
            if (_lockIngredientIndexes.Length <= index)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }

        return _lockIngredientIndexes[index];
    }


    private int GetIngredientCost()
    {
        int unlockIngredientsCount = _ingredientInfo.Length - allLocketIngredients - 3; // 3 - trhree first unlock ingredients from start

        return _ingredientCostDefault + (_differenceBetweenIngredientCost * unlockIngredientsCount);  // GetUnlockIngredientCount() + 1 - Start from 1, not 0
    }

    #endregion
}

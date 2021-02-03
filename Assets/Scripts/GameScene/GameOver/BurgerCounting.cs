using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurgerCounting : MonoBehaviour
{
    [SerializeField] private StartMenu _startMenu;
    [SerializeField] private GameObject _burgerNameObject;
    [SerializeField] private Text _burgerCountText;
    [SerializeField] private Text _heightText;


    [Space]
    [SerializeField] private Transform _burgerUI;
    [SerializeField] private Transform _upperBread;
    [SerializeField] private Transform _ingredientImage;
    [SerializeField] private Transform _pointForUpperBread;


    private Vector3 _startUIBurgerVector;
    private Vector3 _startIngredientImageVector;


    private Animator _burgerNameAnimator;
    private Text _burgerNameText;

    private int _ingredientCounts;

    private float _heightScore;
    private float _heightForChangeScale;

    private float _growthPower;
    private int _numberForNextGrowth;

    private float _growthIngredientImage;


    private const int _growthPowerDivide = 4;
    private const int _startGrowthPower = 10;

    // ~~ Upper Bread Lerp ~~
    private bool _isSetUpperBread;
    private float _lerpUpperBread;
    private Vector3 _startUpperBreadVector;



    private void Start()
    {
        _startUIBurgerVector = _burgerUI.localScale;
        _startIngredientImageVector = _ingredientImage.position;

        ResetData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SkipCounting();
        }

        SetUpperBread();
    }

    public void StartCounting()
    {
        _growthIngredientImage = Burger.instance.GetBurgerStats().height / Burger.instance.GetIngredientsLength();
        InvokeRepeating("ShowIngredientInBurger", 0, 0.05f);
    }


    private void ShowIngredientInBurger()
    {
        ShowNextIngredient();
        SetIngredientHigher(_growthIngredientImage * 10);

        if (_ingredientCounts >= Burger.instance.GetIngredientsLength())
        {
            // End
            CancelInvoke("ShowIngredientInBurger");
            _upperBread.gameObject.SetActive(true);
            _startUpperBreadVector = _upperBread.position;
            _isSetUpperBread = true;
        }
    }



    private void ShowNextIngredient()
    {
        _ingredientCounts++;
        _burgerCountText.text = "X " + _ingredientCounts;
    }


    private void SetIngredientHigher(float height)
    {
        _heightScore += height;
        _heightText.text = Mathf.Round(_heightScore) + " cm.";


        if (_heightScore >= _heightForChangeScale)
        {
            _heightForChangeScale *= _growthPowerDivide;

            float newScaleBurgerScale = Mathf.Clamp(_burgerUI.localScale.x - 0.2f, 0.3f, 1);
            _burgerUI.localScale = new Vector3(newScaleBurgerScale, newScaleBurgerScale, _burgerUI.localScale.z);


            _growthPower /= _growthPowerDivide;
        }


        _upperBread.position = new Vector3(_upperBread.position.x, _upperBread.position.y + (height * _growthPower), _upperBread.position.z);
        _ingredientImage.position = new Vector3(_ingredientImage.position.x, _ingredientImage.position.y + (height * _growthPower), _ingredientImage.position.z);
    }



    private void SetUpperBread()
    {
        if (_isSetUpperBread)
        {
            _upperBread.position = Vector3.Slerp(_startUpperBreadVector, _pointForUpperBread.position, _lerpUpperBread);
            _lerpUpperBread += Time.deltaTime;


            if (_lerpUpperBread >= 1)
            {
                _isSetUpperBread = false;
            }
        }
    }


    public void SkipCounting()
    {
        CancelInvoke("ShowIngredientInBurger");

        _burgerCountText.text = "X " + Burger.instance.GetIngredientsLength();
        _heightText.text = Mathf.Round((Burger.instance.GetBurgerStats().height * 10)) + " cm.";
        CalculateHeight();
    }


    private void CalculateHeight()
    {
        int numberOfHeightLevel = 0;
        int minimalHeight = _startGrowthPower / _growthPowerDivide;

        float balanceAfterDivide = (Burger.instance.GetBurgerStats().height * 10) / _growthPowerDivide;
        float totallyGrowthPower = 1;
        float changedGrowthPower = _startGrowthPower;

        while (balanceAfterDivide > minimalHeight)
        {
            balanceAfterDivide /= _growthPowerDivide;
            changedGrowthPower /= _growthPowerDivide;
            totallyGrowthPower += changedGrowthPower;

            numberOfHeightLevel++;
        }

        totallyGrowthPower /= numberOfHeightLevel;

        float newScaleBurgerScale = Mathf.Clamp(_burgerUI.localScale.x - (0.2f * numberOfHeightLevel), 0.3f, 1);
        _burgerUI.localScale = new Vector3(newScaleBurgerScale, newScaleBurgerScale, _burgerUI.localScale.z);


        _ingredientImage.position = new Vector3(_ingredientImage.position.x, _ingredientImage.position.y + (Burger.instance.GetBurgerStats().height * 10 * totallyGrowthPower), _ingredientImage.position.z);


        _upperBread.position = _pointForUpperBread.position;
        _upperBread.gameObject.SetActive(true);
    }


    public void ResetData()
    {
        CancelInvoke("ShowIngredientInBurger");

        _startMenu.SetNewHeight((int)(Burger.instance.GetBurgerStats().height * 10));

        _growthPower = _startGrowthPower;
        _heightForChangeScale = 10;
        _numberForNextGrowth = 0;
        _ingredientCounts = 0;
        _lerpUpperBread = 0;
        _heightScore = 0;


        // Reset texts
        _burgerCountText.text = "X 0";
        _heightText.text = "0 cm.";

        

        // Reset UI Burger Positions
        _burgerUI.localScale = _startUIBurgerVector;
        _upperBread.position = _startUpperBreadVector;
        _ingredientImage.GetComponent<RectTransform>().localPosition = new Vector3(0, -491, 0);


        _upperBread.gameObject.SetActive(false);
    }
}

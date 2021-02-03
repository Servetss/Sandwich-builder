using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calorie : MonoBehaviour
{
    [SerializeField] private GameObject _calorieObject;

    private Text _calorieText;
    private Animator _calorieAnimator;
    private float _calorieCount;

    private int _ingredientCount;
    private int _changeCalorieTextLine;
    private int _maxCalorie;

    private void Awake()
    {
        _calorieText = _calorieObject.GetComponent<Text>();
        _calorieAnimator = _calorieObject.GetComponent<Animator>();
    }

    private void Start()
    {
        _maxCalorie = 0;
        _calorieCount = 0;
        _ingredientCount = 0;
        _changeCalorieTextLine = 200;

    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _maxCalorie = (int)Burger.instance.GetBurgerStats().kcal;


            InvokeRepeating("CountingCalorieText", 0, 0.05f);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SkipCounting();
        }
    }


    public void StartCounting()
    {
        for (int i = 0; i < Burger.instance.GetIngredientsLength(); i++)
        {
            _maxCalorie += (int)Burger.instance.GetIngredientByIndex(i).kcal;
        }


        InvokeRepeating("CountingCalorieText", 0, 0.05f);
    }


    private void CountingCalorieText()
    {
        _calorieCount += 15;


        _calorieText.text = _calorieCount + " ccal";

        if (_calorieCount > _changeCalorieTextLine)
        {
            _changeCalorieTextLine += 200;
            _calorieAnimator.Play("CalorieText_GameOver");
        }

        if (_calorieCount >= _maxCalorie)
        {
            CancelInvoke("CountingCalorieText");
        }
    }

    public void SkipCounting()
    {
        CancelInvoke("CountingCalorieText");
        _calorieText.text = _maxCalorie + " ccal";
    }


    public void ResetCalories()
    {
        CancelInvoke("CountingCalorieText");
        _maxCalorie = 0;
        _calorieCount = 0;
        _ingredientCount = 0;
        _changeCalorieTextLine = 200;
        _calorieText.text = "0 ccal";
    }
}

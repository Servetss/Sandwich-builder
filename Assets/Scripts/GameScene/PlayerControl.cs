using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl instance;

    [Space]
    [Header("Scripts")]
    [SerializeField] private IngredientsBuffer _ingredientBuffer;
    private ThrowDirection _throwDirectionComp;
    private LevelManager _levelManagerComp;
    

    [Space]
    [Header("Throw")]
    [SerializeField] private Transform _handPosition;
    [SerializeField] private Transform _throwVector;
    [SerializeField] private float _throwStrength;
    private Ingredient _ingredient;
    private Ingredient _lastIngredient;
    private Animator _animator;

    [HideInInspector] public int throwCount;
    private bool _isIngredientInHand;


    // ~~ MOVE HIGHER ~~
    [Space]
    [SerializeField] private Transform _staggerPoint;
    private float _startPlayerHeightPosition;
    private float _heigherPosition;
    private float _moveUpLerp;
    private float _heightLerpPower;
    private bool _isGetHigher;

    [SerializeField] private bool _autoThrow;
    private void Awake()
    {
        instance = this;
        _throwDirectionComp = GetComponent<ThrowDirection>();
        _animator = GetComponent<Animator>();
        _levelManagerComp = GetComponent<LevelManager>();
    }

    public void StartGame()
    {
        throwCount = 0;
        _moveUpLerp = 0;
        
        _startPlayerHeightPosition = transform.position.y;
        _heigherPosition = transform.position.y;
        _saveStaggerY = _staggerPoint.position.y;

        Time.timeScale = 2f;
        SetObjectInHand();
    }

    private void Update()
    {
        if ((Input.GetButtonDown("Throw") || _autoThrow) && _isIngredientInHand && !GameOver.instance.isGameOver)
        {
            throwCount++;
            _isIngredientInHand = false;
            _lastIngredient = _ingredient;
            _heightLerpPower = 0.1f / _lastIngredient.GetIngredientInfo().height * 2; // 0.1 default height;

            _ingredient.rigidBody.isKinematic = false;
            _ingredient.rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            _ingredientBuffer.AddIngredient(_ingredient);

            Vector3 directionShoot = _throwDirectionComp.GetDirectionShoot();
            _throwVector.localEulerAngles = directionShoot;
            _ingredient.rigidBody.AddForce(_throwVector.forward * _throwStrength, ForceMode.VelocityChange);


            _ingredient.transform.SetParent(null);
            if (throwCount == 1)
            {
                FirstThrow();
            }
        }


        GetHigher();
    }

    private void FirstThrow()
    {
        NextStep(true);
        _ingredient.SetIngredientOnTheBurger();
        _throwDirectionComp.FirstShoot();
    }


    public void NextStep(bool isIngredientTargeted)
    {
        if (_isIngredientInHand == false && _isGetHigher == false)
        {
            if (isIngredientTargeted)
            {
                _isGetHigher = true;
            }
            else
            {
                SetObjectInHand();
            }
        }
    }



    public void SetObjectInHand()
    {
        Invoke("WaitForObjcectInHand", 0.2f);
    }

    private void WaitForObjcectInHand()
    {
        _ingredient = _ingredientBuffer.GetLastIngredient();
        _ingredient.SetIngredientInfo();

        _ingredient.transform.SetParent(_handPosition);
        _ingredient.gameObject.SetActive(true);
        _ingredient.SetIngredientInHand(_handPosition.position);
        _isIngredientInHand = true;
    }

    private float _saveStaggerY;
    private void GetHigher()
    {
        if (_isGetHigher)
        {
            float up = Mathf.Lerp(0, _lastIngredient.GetIngredientInfo().height + Burger.instance.burgerCorrection, _moveUpLerp);
            transform.position = new Vector3(transform.position.x, _heigherPosition + up, transform.position.z);
            if (throwCount > 70) _staggerPoint.position = new Vector3(_staggerPoint.position.x, _saveStaggerY + up, _staggerPoint.position.z);

            BackgroundColor.instance.ChangeColor();


            if (_moveUpLerp >= 1)
            {
                _isGetHigher = false;
                _moveUpLerp = 0;
                transform.position = new Vector3(transform.position.x, _heigherPosition + up, transform.position.z);
                _heigherPosition = transform.position.y;
                Burger.instance.ResetBurgerCorrection();

                if (throwCount > 70)
                {
                    _staggerPoint.position = new Vector3(_staggerPoint.position.x, _saveStaggerY + up, _staggerPoint.position.z);
                    _saveStaggerY = _staggerPoint.position.y;
                }


                SetObjectInHand();
            }
            else
            {
                _moveUpLerp += Time.deltaTime * _heightLerpPower;
            }
        }
    }


    public void HitOnTheBurger()
    {
        _levelManagerComp.ChangeSpeed();
        _levelManagerComp.ChangeDegreeArrwoThrow();
    }

    public void ResetData()
    {
        throwCount = 0;
        throwCount = 0;
        _moveUpLerp = 0;
        transform.position = new Vector3(0, _startPlayerHeightPosition, transform.position.z);
        _heigherPosition = transform.position.y;
    }
}

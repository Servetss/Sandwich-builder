using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Hit { Perfect, Normal, Fall }


public class Burger : MonoBehaviour
{
    public static Burger instance;

    [SerializeField] private MissionManager _missionManager;


    [Space]
    [SerializeField] private List<IngredientSO> _ingredientListToRead = new List<IngredientSO>();
    [SerializeField] private int _maxBurgerDegreeOfStagger;
    [SerializeField] private float _degreeOfStagger;
    [SerializeField]private float _sinusValue;
    private float _oldEulerAngles;


    private Hit ingredientHit;
    private BurgerStats _burgerStats;
    private Transform _lastIngredient;
    private Vector3 _burgerStartPosition;
    private Quaternion _burgerStartRotation;

    private int _allIngredientsOnTheBurger;

    // ~~ LERP ~~
    [Space]
    [SerializeField] private Transform _staggerpoint;
    private Vector3 _startStaggerPointPosition;
    private Quaternion _startStaggerRotation;
    private bool _isChangeStagger;
    private float _oldStaggerValue;
    private float _newStaggerValue;
    private float _lerpTime;


    
    private bool _isBurgerStable;

    [Space]
    [SerializeField] private int _perfectHitToStaggerZero;
    private int _countPerfectHIt;




    // ~~ Burger heigher correction ~~
    private int _ingredientsBeforeCorrection = 4;
    private int _ingredientCount;
    private float _ingredientsHeight;
    private Vector3 _startPointToMeasureCorrection;
    public float burgerCorrection { get; private set; }

    private void Awake()
    {
        instance = this;
        _burgerStats = new BurgerStats();
    }

    private void Start()
    {
        _degreeOfStagger = 0;
        _sinusValue = 0;
        _lerpTime = 0;
        _countPerfectHIt = 0;
        _isBurgerStable = false;


        _startStaggerPointPosition = _staggerpoint.position;
        _startStaggerRotation = _staggerpoint.rotation;

        _burgerStartPosition = transform.position;
        _burgerStartRotation = transform.rotation;

        _ingredientCount = 0;
        _ingredientsHeight = 0;

        _allIngredientsOnTheBurger = 0;
    }


    public void BurgerStableActive(bool active)
    {
        _isBurgerStable = active;
    }

    private void FixedUpdate()
    {
        if (_isBurgerStable)
        {
            _sinusValue += Time.fixedDeltaTime * (GameOver.instance.isGameOver ? 1 : 0.5f);
            float angleRotateZ = Mathf.Sin(_sinusValue) * _degreeOfStagger;

            float angle = Vector3.SignedAngle(_staggerpoint.position, _staggerpoint.position + (Vector3.right * angleRotateZ), Vector3.up);

            transform.localEulerAngles = new Vector3(0,0,angle);


            if (_isChangeStagger)
            {
                _lerpTime += Time.fixedDeltaTime * 0.5f;
                _degreeOfStagger = Mathf.Clamp(Mathf.Lerp(_oldStaggerValue, _newStaggerValue, _lerpTime), -_maxBurgerDegreeOfStagger, _maxBurgerDegreeOfStagger);


                _isChangeStagger = _lerpTime <= 1;
            }
        }
    }




    public void ChangeDegreeOfStagger(float angle)
    {
        if (GameManager.instance.CheckIsPerfectHit(angle))
        {
            _oldStaggerValue = _degreeOfStagger;
            _newStaggerValue = _degreeOfStagger;
        }
        else
        {
            _oldStaggerValue = _degreeOfStagger;
            _newStaggerValue = _degreeOfStagger +  angle/ 4;
        }


        _lerpTime = 0;
        _isChangeStagger = true;
    }


    public void ChangeStagger(float value)
    {
        _degreeOfStagger = Mathf.Clamp(value, -_maxBurgerDegreeOfStagger, _maxBurgerDegreeOfStagger);
    }

    public float GetStaggerDegree()
    {
        return _degreeOfStagger;
    }

    public int GetIngredientsLength()
    {
        return _ingredientListToRead.Count;
    }

    public void AddIngredientToBurger(Ingredient ingredient)
    {
        _burgerStats.AddHeight(ingredient.GetIngredientInfo().height);
        _burgerStats.AddKcal(ingredient.GetIngredientInfo().kcal);

        _ingredientListToRead.Add(ingredient.GetIngredientInfo());
    }

    public IngredientSO GetIngredientByIndex(int index)
    {
        return _ingredientListToRead[index];
    }

    public BurgerStats GetBurgerStats()
    {
        return _burgerStats;
    }

    public float GetBurgerKcal()
    {
        float maxKcal = 0;

        for (int i = 0; i < _ingredientListToRead.Count; i++)
        {
            maxKcal += _ingredientListToRead[i].kcal;
        }

        return maxKcal;
    }

    public void PerfectHit(IngredientSO ingredient, Transform lastIngredient)
    {
        SetLastIngredient(lastIngredient);

        _allIngredientsOnTheBurger++;
        _countPerfectHIt++;

        //SoundManager.instance.BurgerHitSound();

        if (_countPerfectHIt == _perfectHitToStaggerZero)
        {
            Bonuses.StopStagger();
            GameManager.instance.SetPerfektHitFX(lastIngredient.position + Vector3.up / 6);
            SoundManager.instance.PerfectHitSound();

            _countPerfectHIt = 0;
        }
        else
        {
            SoundManager.instance.PerfectHitTypesSound();
        }

        ingredientHit = Hit.Perfect;
        Score.instance.ChangeScoreWhenHit(GameManager.instance.GetPerfectHitScore(), ingredientHit);
        PlayerControl.instance.HitOnTheBurger();
        

        _missionManager.MissionDo(Hit.Perfect, ingredient);

        IngredientOnTheBurger(ingredient);
    }

    public void NormalHit(IngredientSO ingredient, Transform lastIngredient)
    {
        SetLastIngredient(lastIngredient);

         _countPerfectHIt = 0;
        _allIngredientsOnTheBurger++;

        ingredientHit = Hit.Normal;
        Score.instance.ChangeScoreWhenHit(GameManager.instance.GetNormalHitScore(), ingredientHit);
        PlayerControl.instance.HitOnTheBurger();
        SoundManager.instance.BurgerHitSound();

        _missionManager.MissionDo(Hit.Normal, ingredient);

        IngredientOnTheBurger(ingredient);
    }

    public void FallHit(IngredientSO ingredient)
    {
        _countPerfectHIt = 0;
        GameManager.instance.DepriveHealth();

        ingredientHit = Hit.Fall;
        Score.instance.ChangeScoreWhenHit(GameManager.instance.GetFallHitScore(), ingredientHit);

        SoundManager.instance.BurgerFallSound();
        SoundManager.instance.Vibrate();

        _missionManager.MissionDo(Hit.Fall, null);
    }

    public void ResetBurgers()
    {
        _ingredientListToRead.Clear();
        _burgerStats.Clear();

        _degreeOfStagger = 0;
        _sinusValue = 0;
        _lerpTime = 0;
        _countPerfectHIt = 0;
        _isBurgerStable = false;
        

        _staggerpoint.position = _startStaggerPointPosition;
        _staggerpoint.rotation = _startStaggerRotation;

        transform.position = _burgerStartPosition;
        transform.rotation = _burgerStartRotation;

        _allIngredientsOnTheBurger = 0;
        _ingredientCount = 0;
        _ingredientsHeight = 0;
        _lastIngredient = null;
    }

    private void SetLastIngredient(Transform lastIngredient)
    {
        if (_lastIngredient == null)
        {
            _startPointToMeasureCorrection = lastIngredient.position;
            print("_lastIngredient  == null, then VECTOR3 = " + _startPointToMeasureCorrection);
        }


        _lastIngredient = lastIngredient;
    }


    private void IngredientOnTheBurger(IngredientSO ingredient)
    {
        _ingredientCount++;
        _ingredientsHeight += ingredient.height;


        if (_allIngredientsOnTheBurger % 10 == 0)
        {
            Wallet.AddActiveMoney();
        }

        // Camera correction by burger height
        if (_ingredientCount == _ingredientsBeforeCorrection)
        {
            Vector3 positionUnderLastIngredient = new Vector3(_lastIngredient.position.x, _startPointToMeasureCorrection.y, _startPointToMeasureCorrection.z); 
            float diff = Vector3.Distance(_lastIngredient.position, positionUnderLastIngredient);


            burgerCorrection = diff - _ingredientsHeight;

            _ingredientsHeight = 0;
            _ingredientCount = 0;

            _startPointToMeasureCorrection = new Vector3(positionUnderLastIngredient.x, positionUnderLastIngredient.y + diff, positionUnderLastIngredient.z);
        }
    }

    public void ResetBurgerCorrection()
    {
        burgerCorrection = 0;
    }

}


public class BurgerStats
{
    public float height { get; private set; }
    public float kcal { get; private set; }


    public BurgerStats()
    {
        height = 0;
        kcal = 0;
    }

    public void AddHeight(float ingredientHeight)
    {
        height += ingredientHeight;
    }


    public void AddKcal(float ingredientKcal)
    {
        kcal += ingredientKcal;
    }

    public void Clear()
    {
        height = 0;
        kcal = 0;
    }

}
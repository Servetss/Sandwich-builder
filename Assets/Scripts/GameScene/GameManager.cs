using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static string isFirstStart = "IsFirstStart";


    private IngredientSO[] _ingredientsSO;
    private IngredientSO[] _unlockIngredients;
    private IngredientSO _lastIngredient;

    // ~~ SCRIPTS ~~
    [Space]
    [Header("Scripts")]
    [SerializeField] private MissionManager _missionManager; 
    [SerializeField] private ShopContent _shopContent;
    [SerializeField] private Score _scoreComponent;
    [SerializeField] private HealtPoint _healthComponent;
    [SerializeField] private PlayerControl _playerController;
    [SerializeField] private ThrowDirection _throwDirection;
    [SerializeField] private IngredientsBuffer _ingredientBuffer;
    [SerializeField] private Burger _burger;


    [Space]
    [Header("Canvas")]
    [SerializeField] private GameObject _gameCanvas;
    [SerializeField] private GameObject _mainMenuCanvas;


    [Space]
    [Header("Game objects")]
    [SerializeField] private GameObject _gameArrow;
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _smokeFX;
    [SerializeField] private GameObject _perfectHitFX;

    [Space]
    [Header("Ingredient hited parametrs")]
    [SerializeField] private float _prefectTargeted;
    [SerializeField] private float _normalTargeted;


    [Space]
    [Header("Levels")]
    [SerializeField] private int _hiitingOnTheBurgerForNextLevel;
    [SerializeField] private float _speedChangeInNewLevel;
    [SerializeField] private float _arrowdegreeChangeInNewLevel;



    [Space]
    [Header("Point for hit")]
    [SerializeField] private int _scoreForPerfectHit;
    [SerializeField] private int _scoreForNormalHit;
    [SerializeField] private int _scoreForFalltHit;



    // ~~ Camera settings ~~
    private Vector3 _defaultCameraPosition;
    private Quaternion _defaultCameraRotation;


    private void Awake()
    {
        instance = this;
        SetIngredientsToTab();
    }


    private void Start()
    {
        Invoke("SetFirstStart", 0.2f);
    }

    private void SetFirstStart()
    {
        PlayerPrefs.SetInt(isFirstStart, 1);
    }

    private void SetIngredientsToTab()
    {
        _ingredientsSO = _shopContent.GetAllIngredient();
        _unlockIngredients = new IngredientSO[GetUnlockIngredientsNumber()];
        int unlockIndex = 0;
        for (int i = 0; i < _ingredientsSO.Length; i++)
        {
            if (!_ingredientsSO[i].isLock && _ingredientsSO[i].isSelected)
            {
                _unlockIngredients[unlockIndex] = _ingredientsSO[i];
                unlockIndex++;
            }
        }

         // ResetIngredientsSaveData();
    }


    public bool IsFirstStart()
    {
        return PlayerPrefs.GetInt(isFirstStart) == 0;
    }


    private void ResetIngredientsSaveData()
    {
        for (int i = 0; i < _ingredientsSO.Length; i++)
        {
            _ingredientsSO[i].DefaultSettings();
        }
    }

    public void StartGame()
    {
        TinySauce.OnGameStarted();

        SetIngredientsToTab();
        _playerController.StartGame();

        _defaultCameraPosition = _mainCamera.transform.position;
        _defaultCameraRotation = _mainCamera.transform.rotation;
        

        _burger.BurgerStableActive(true);
        _scoreComponent.gameObject.SetActive(true);
        _gameArrow.SetActive(true);
    }

    private int GetUnlockIngredientsNumber()
    {
        int unlockCount = 0;
        for (int i = 0; i < _ingredientsSO.Length; i++)
        {
            if (!_ingredientsSO[i].isLock && _ingredientsSO[i].isSelected)
                unlockCount++;
        }

        return unlockCount;
    }

    public IngredientSO GetRandomIngredient()
    {
        if (_lastIngredient == null)
        {
            // First throw
            _lastIngredient = _unlockIngredients[0];
            return _lastIngredient;
        }
        else
        {
            int indexIngredient = Random.Range(0, _unlockIngredients.Length);

            if (GetSpecialIngredient(indexIngredient) == _lastIngredient)
            {
                indexIngredient = indexIngredient + 1 == _unlockIngredients.Length ? 0 : indexIngredient + 1;
            }

            _lastIngredient = _unlockIngredients[indexIngredient];
            return _unlockIngredients[indexIngredient];
        }
    }


    #region FX
    public void SetSmoke(Vector3 position)
    {
        _smokeFX.transform.position = position;
        _smokeFX.GetComponent<ParticleSystem>().Play();
    }

    public void SetPerfektHitFX(Vector3 position)
    {
        _perfectHitFX.transform.position = position;
        _perfectHitFX.GetComponent<ParticleSystem>().Play();
    }

    #endregion

    public IngredientSO GetRandomUnlockIngredient()
    {
        int indexIngredient = Random.Range(0, _unlockIngredients.Length);
        return _unlockIngredients[indexIngredient];
    }

    public IngredientSO GetSpecialIngredient(int ingredientIndex)
    {
        return _unlockIngredients[ingredientIndex];
    }


    public bool CheckIsPerfectHit(float angle)
    {
        return angle >= -_prefectTargeted && _prefectTargeted >= angle;
    }

    public bool CheckIsNormaltHit(float angle)
    {
        return CheckIsPerfectHit(angle) == false && (angle >= -_normalTargeted && _normalTargeted >= angle);
    }



    public int GetPerfectHitScore()
    {
        return _scoreForPerfectHit;
    }

    public int GetNormalHitScore()
    {
        return _scoreForNormalHit;
    }

    public int GetFallHitScore()
    {
        return _scoreForFalltHit;
    }


    public int GetHitsForNextLevel()
    {
        return _hiitingOnTheBurgerForNextLevel;
    }

    public float GetSpeedChangePerLevel()
    {
        return _speedChangeInNewLevel;
    }

    public float GetArrowDegreeChangePerLevel()
    {
        return _arrowdegreeChangeInNewLevel;
    }

    public int GetScore()
    {
        return _scoreComponent.GetScore();
    }

    public void ChangeScore(int scoreToChange)
    {
        _scoreComponent.ChangeScoreWhenHit(scoreToChange, Hit.Normal);
    }

    public void AddHealth(int pointAdd)
    {
        _healthComponent.AddHealth(pointAdd);
    }

    public void DepriveHealth()
    {
        _healthComponent.DepriveHealth();
    }


    public void GameOverFunc()
    {
        _scoreComponent.gameObject.SetActive(false);
        _gameArrow.SetActive(false);

        _missionManager.EndGame();
        
    }

    public void GameRestart()
    {
        ClearAll();

       _playerController.SetObjectInHand();

        Time.timeScale = 2;

        
        _scoreComponent.gameObject.SetActive(true);
        _burger.BurgerStableActive(true);
        _gameArrow.SetActive(true);
    }

    public void GoToMainMenu()
    {
        ClearAll();


        _playerController.enabled = false;
        _throwDirection.enabled = false;

        _mainMenuCanvas.SetActive(true);
        _gameCanvas.SetActive(false);


        GameTime.instance.SaveTime();
    }

    public void ClearAll()
    {
        _lastIngredient = null;
        _healthComponent.SetStartHealth();
        _scoreComponent.ResetScore();
        _scoreComponent.gameObject.SetActive(false);

        GameOver.instance.ResetData();
        _playerController.ResetData();
        _throwDirection.ResetData();
        _ingredientBuffer.HideIngredients();
        _burger.ResetBurgers();


        // default Camera position
        _mainCamera.transform.position = _defaultCameraPosition;
        _mainCamera.transform.rotation = _defaultCameraRotation;


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomPurchases : MonoBehaviour
{
    [SerializeField] private Image _randomIngredientImage;
    [SerializeField] private ShopContent _shopContent;
    
    private Random _random;

    [SerializeField] private GameObject _shopPanel;


    [Space]
    [SerializeField] private GameObject _buttonClose;
    [SerializeField] private Transform _giftFX;
    private Animation _animation;


    private IngredientSO unlockIngredient;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    private void OnEnable()
    {
        if (_shopContent.allLocketIngredients > 0)
        {
            int randomLockIndex = GetRandomLock();
            int ingredientSOIndex = _shopContent.GetLockIngredientByIndex(ref randomLockIndex);


            unlockIngredient = _shopContent.UnblockIngredient(ingredientSOIndex, randomLockIndex);

            _shopContent.SelectedIngredient();

            _shopPanel.SetActive(false);
            _animation.Play();

            _randomIngredientImage.sprite = unlockIngredient.sprite;
        }
        else
        {
            CloseRandomPurchasesPanel();
        }
    }


    private int GetRandomLock()
    {
        return Random.RandomRange(0, _shopContent.allLocketIngredients);
    }



    public void GiftFallSound()
    {
        SoundManager.instance.GiftFall();
    }

    public void GiftHitSound()
    {
        SoundManager.instance.GiftHit();
    }


    public void SetGiftFX()
    {
        Camera cam = Camera.main;

        Vector3 point = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, cam.nearClipPlane));
        point += cam.transform.forward * 2;
        point -= cam.transform.up / 5;

        _giftFX.GetComponent<ParticleSystem>().Play();
        _giftFX.transform.position = point;

        SoundManager.instance.GiftOpen();
    }


    public void ShowButtonClose()
    {
        _buttonClose.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = unlockIngredient.name;
        _buttonClose.SetActive(true);
    }

    public void CloseRandomPurchasesPanel()
    {
        _giftFX.GetComponent<ParticleSystem>().Stop();
        _buttonClose.SetActive(false);
        _shopPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}

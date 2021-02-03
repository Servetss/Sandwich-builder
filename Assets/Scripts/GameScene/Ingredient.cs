using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{

    [HideInInspector] public Rigidbody rigidBody;
    public Collider collider;


    // ~~ Ingredient mesh ~~
    [SerializeField] private GameObject _ingredientMesh;
    private IngredientRenderer _ingredientRenderer;




    // ~~ Burger stats
    [Space]
    [SerializeField] private IngredientSO _info;
    [SerializeField] private bool _isOnTheBurger;
    [HideInInspector] public bool isIngredientFall;

    private float _randomRotationPos = 0;

    private const float _standartIngredientTrigger = 0.1f;
    private const float _standartTriggerMesh = 0.04f;

    private void Awake()
    {
        _ingredientRenderer = new IngredientRenderer();
        _ingredientRenderer.meshFilter = _ingredientMesh.GetComponent<MeshFilter>();
        _ingredientRenderer.meshRenderer = _ingredientMesh.GetComponent<MeshRenderer>();
    }

    public void SetIngredientInfo()
    {
        _info = GameManager.instance.GetRandomIngredient();
    }

    public void SetIngredientInHand(Vector3 posiiton)
    {
        isIngredientFall = false;
        _isOnTheBurger = false;
        collider.enabled = false;

        rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rigidBody.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _randomRotationPos = UnityEngine.Random.RandomRange(-70, 70);
        transform.Rotate(new Vector3(0, _randomRotationPos, 0));

        _ingredientRenderer.ingredientObjectFromAssets = (GameObject)Resources.Load("Models/" + _info.modelsName);
        _ingredientRenderer.meshFilter.mesh = _ingredientRenderer.ingredientObjectFromAssets.GetComponent<MeshFilter>().sharedMesh;
        _ingredientRenderer.meshRenderer.materials = _ingredientRenderer.ingredientObjectFromAssets.GetComponent<MeshRenderer>().sharedMaterials;


        GetComponent<BoxCollider>().size = new Vector3(1, _info.height, 1);
        float proc = (_info.height / _standartIngredientTrigger * 100);
        collider.transform.localPosition = new Vector3(0, proc / 100 * _standartTriggerMesh, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.Equals(gameObject) && other.GetComponent<Ingredient>() != null)
        {
            Vector3 pivotPositionFallIngredient = other.attachedRigidbody.position - new Vector3(0, other.GetComponent<BoxCollider>().size.y / 2, 0);
            
            float angle = Vector3.SignedAngle(transform.up, ((transform.position + transform.up) - pivotPositionFallIngredient).normalized, Vector3.forward);

            
            if (GameManager.instance.CheckIsPerfectHit(angle))
            {
                print("PERFECTO!!!! \nSet zero position");

                other.transform.position = new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z);
                IngredientCatched(other, angle);


                Burger.instance.PerfectHit(other.GetComponent<Ingredient>().GetIngredientInfo(), transform);
            }
            else if (GameManager.instance.CheckIsNormaltHit(angle))
            {
                print("NORMAL - Angle " + angle);
                IngredientCatched(other, angle);


                Burger.instance.NormalHit(other.GetComponent<Ingredient>().GetIngredientInfo(), transform);
            }
            else
            {
                print("W MOLOKO !!!");

                GameManager.instance.SetSmoke(other.transform.position);
                other.attachedRigidbody.AddForce(other.transform.right * angle * 0.17f, ForceMode.Impulse);  // Remove from BurgerTower
                
                // FallHit action make by BottomTrigger.cs
                
            // Set NextStep in BottomTrigger
            }
        }
    }

    IEnumerator IngredientCatch(Collider other, float angle)
    {
        yield return new WaitForSeconds(0.05f);
        other.attachedRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        other.attachedRigidbody.velocity = Vector3.zero;
        other.attachedRigidbody.isKinematic = true;


        other.GetComponent<Ingredient>().SetIngredientOnTheBurger();


        Burger.instance.ChangeDegreeOfStagger(angle);
        PlayerControl.instance.NextStep(true);

        DisabledIngredient();

    }


    private void IngredientCatched(Collider other, float angle)
    {
        other.attachedRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        other.attachedRigidbody.velocity = Vector3.zero;
        other.attachedRigidbody.isKinematic = true;


        other.GetComponent<Ingredient>().SetIngredientOnTheBurger();


        Burger.instance.ChangeDegreeOfStagger(angle);
        PlayerControl.instance.NextStep(true);

        DisabledIngredient();
    }



    public void SetIngredientOnTheBurger()
    {
        _isOnTheBurger = true;
        collider.enabled = true;

        transform.SetParent(Burger.instance.transform);

        transform.localEulerAngles = new Vector3(0, _randomRotationPos, 0);

        Burger.instance.AddIngredientToBurger(this);
    }

    public bool GetIsOnTheBurger()
    {
        return _isOnTheBurger;
    }

    public IngredientSO GetIngredientInfo()
    {
        return _info;
    }

    public void DisabledIngredient()
    {
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rigidBody.isKinematic = true;
        collider.enabled = false;
    }
}


public class IngredientRenderer
{
    public GameObject ingredientObjectFromAssets;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
}
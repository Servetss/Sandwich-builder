using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsBuffer : MonoBehaviour
{
    public Queue<Ingredient> ingredientList = new Queue<Ingredient>();
    private GameObject[] _allIngredientinScene;

    private void Awake()
    {
        _allIngredientinScene = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            AddIngredient(transform.GetChild(i).GetComponent<Ingredient>());
            transform.GetChild(i).gameObject.SetActive(false);
            _allIngredientinScene[i] = transform.GetChild(i).gameObject;
        }
    }


    public Ingredient GetLastIngredient()
    {
        return ingredientList.Dequeue();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ingredientList.Enqueue(ingredient);
    }


    public void HideIngredients()
    {
        for (int i = 0; i < _allIngredientinScene.Length; i++)
        {
            _allIngredientinScene[i].SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomTrigger : MonoBehaviour
{
    [SerializeField] private IngredientsBuffer _ingredientBuffer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ingredient>() != null && !other.GetComponent<Ingredient>().isIngredientFall && !other.GetComponent<Ingredient>().GetIsOnTheBurger())
        {
            Burger.instance.FallHit(other.GetComponent<Ingredient>().GetIngredientInfo());

            if (GameOver.instance.isGameOver == false)
            {
                PlayerControl.instance.NextStep(false);
                other.GetComponent<Ingredient>().isIngredientFall = true;
            }
        }
    }

}

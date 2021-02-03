using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameOver.instance.isGameOver == false && other.GetComponent<Ingredient>() != null && !other.GetComponent<Ingredient>().isIngredientFall && !other.GetComponent<Ingredient>().GetIsOnTheBurger())
        {
            PlayerControl.instance.NextStep(false);
            other.GetComponent<Ingredient>().isIngredientFall = true;
        }
        else if (GameOver.instance.isGameOver == false && other.GetComponent<Ingredient>() != null && !other.GetComponent<Ingredient>().isIngredientFall && other.GetComponent<Ingredient>().GetIsOnTheBurger())
        {
            SoundManager.instance.FirstHit();
        }
    }
}

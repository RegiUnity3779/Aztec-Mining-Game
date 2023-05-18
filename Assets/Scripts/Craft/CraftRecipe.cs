using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftRecipe", menuName ="NewCraftRecipe")]
public class CraftRecipe : ScriptableObject
{
    Recipe[] recipeCost;
    ItemData craftItem;
    int craftAmount;

}

public class Recipe
{
    ItemData item;
    int amountNeeded;
}



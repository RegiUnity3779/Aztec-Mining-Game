using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting", menuName = "NewCrafting")]
public class Crafting : ScriptableObject
{
    List<CraftRecipe> recipesList = new List<CraftRecipe>();
}

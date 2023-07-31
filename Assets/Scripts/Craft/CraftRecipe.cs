using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftRecipe", menuName ="NewCraftRecipe")]
public class CraftRecipe : ScriptableObject
{
    public ItemData[] item;
    public int[] amountNeeded;
    public ItemData craftItem;
    public int craftAmount;

}




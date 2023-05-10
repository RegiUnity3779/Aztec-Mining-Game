using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Rock Type", menuName = "Rock Type/ New Rock Type")]
public class RockType : ScriptableObject
{
   public string rockName;
   public Material rockColour;
   public int spawnChance;
   public Item[] spawnItem;
   public int[] spawnProability;

    
}

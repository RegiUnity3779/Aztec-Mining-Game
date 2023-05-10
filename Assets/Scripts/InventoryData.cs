using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName =("InventoryData"))]
public class InventoryData : ScriptableObject
{
    
    public List<InventorySlot> inventory = new List<InventorySlot>();
        
}

[System.Serializable]
public class InventorySlot
{

    public ItemData item;
    public int amount;
    public bool hasItem;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Resource,
    Equipment,
    Consumable
}
public enum EquipmentType
{
    None,
    Pickaxe,
    Torch,
    Shovel,
    Hammer
    
}
public enum Rating
{
    E,
    D,
    C,
    B,
    A

}

[CreateAssetMenu(fileName = "Item", menuName = "Items/ New Item")]
public class ItemData : ScriptableObject
{

    public int indexNumber;
    public GameObject itemObject;
    public GameObject equipObject;
    public Sprite itemSprite;
    public string itemName;
    public string itemDescription;
    public ItemType type;
    public EquipmentType equipment;
    public Rating rating;
    public int value;



    [Header("Stackable")]

    public bool canStack;
    public int maxStackAmount;

    [Header("Consumerable")]

    public int staminaValue;


}

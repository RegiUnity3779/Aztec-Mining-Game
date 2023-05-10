using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    //public GameObject itemObject;
    //public Sprite itemSprite;
    //public string itemName;
    //public string itemDescription;
    //public ItemType type;
    //public int value;


    //[Header("Stackable")]

    //public bool canStack;
    //public int maxStackAmount;

    // Start is called before the first frame update
    void Start()
    {
        //ItemSetUp();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void ItemSetUp()
    //{
    //    itemObject = itemData.itemObject;
    //    itemSprite = itemData.itemSprite;
    //    itemName = itemData.itemName;
    //    itemDescription = itemData.itemDescription;
    //    type = itemData.type;
    //    canStack = itemData.canStack;
    //    maxStackAmount = itemData.maxStackAmount;
    //    value = itemData.value;
    //}

    public void CollectItem()
    {
        Destroy(gameObject);
        EventsManager.AddToInventory(this.itemData);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedItem : MonoBehaviour
{
    public bool itemEquiped;
    public ItemData item;
    private GameObject equipedObject;

    private void OnEnable()
    {
        EventsManager.EquipableItem += EquipableItem;
        EventsManager.EquipItem += ItemEquiped;
        EventsManager.UnEquipItem += ItemUnEquiped;
    }

    private void OnDisable()
    {
        EventsManager.EquipableItem -= EquipableItem;
        EventsManager.EquipItem -= ItemEquiped;
        EventsManager.UnEquipItem -= ItemUnEquiped;
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EquipableItem (ItemData data)
    {
        item = data;
        
    }
     void ItemEquiped()
    {
        
        if(itemEquiped == true && item.equipObject != equipedObject)
        {
            ItemUnEquiped();
        }

        if (item != null)
        {
            itemEquiped = true;
            equipedObject = Instantiate(item.equipObject, transform);
        }

        else
        {
            itemEquiped = false;
        }
    }

     void ItemUnEquiped()
    {
        if (itemEquiped == true)
        {
            Destroy(equipedObject);
            itemEquiped = false;
            
        }
    }
}

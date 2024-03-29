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
        EventsManager.RemoveEquipableItem += RemoveEquipableItem;
    }

    private void OnDisable()
    {
        EventsManager.EquipableItem -= EquipableItem;
        EventsManager.EquipItem -= ItemEquiped;
        EventsManager.UnEquipItem -= ItemUnEquiped;
        EventsManager.RemoveEquipableItem -= RemoveEquipableItem;
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
        if (data != null)
        {
            item = data;
        }

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
            EventsManager.EquipButton(false);
            EventsManager.UnEquipButton(true);
            EventsManager.EquipToggleInteractable(itemEquiped);
            EventsManager.EquipToggle(itemEquiped, item);
        }

        else
        {
            itemEquiped = false;
            EventsManager.EquipToggleInteractable(itemEquiped);
            EventsManager.EquipToggle(itemEquiped, item);
        }
       
    }

     void ItemUnEquiped()
    {
        if (itemEquiped == true)
        {
            Destroy(equipedObject);
            itemEquiped = false;
            EventsManager.EquipButton(true);
            EventsManager.UnEquipButton(false);
            EventsManager.EquipToggle(itemEquiped, item);
        }

    }
    void RemoveEquipableItem()
    {
        if(itemEquiped == true)
        {
            Destroy(equipedObject);
            itemEquiped = false;
        }
        item = null;
        EventsManager.EquipButton(false);
        EventsManager.UnEquipButton(false);
        EventsManager.EquipToggle(itemEquiped, null);

    }
}

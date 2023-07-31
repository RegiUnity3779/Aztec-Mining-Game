using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public InventoryData data;

    public Slot[] inventory;
    public Slot[] craftInventory;
    public Slot selectedSlot = null;
    public GameObject playerInteractor;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemType;
    public TextMeshProUGUI itemValue;
    public TextMeshProUGUI itemRating;


    public Button equipButton;
    public Button unEquipButton;
    public Button dropButton;
    private void OnEnable()
    {
        EventsManager.AddToInventory += AddItemInventory;
        EventsManager.EquipButton += EquipButton;
        EventsManager.UnEquipButton += UnEquipButton;
        EventsManager.FindPlayerInteractor += FindPlayer;
        EventsManager.RemoveFromInventory += RemoveItemInventory;
    }

    private void OnDisable()
    {
        EventsManager.AddToInventory -= AddItemInventory;
        EventsManager.EquipButton -= EquipButton;
        EventsManager.UnEquipButton -= UnEquipButton;
        EventsManager.FindPlayerInteractor -= FindPlayer;
        EventsManager.RemoveFromInventory -= RemoveItemInventory;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        InventoryUpdate();
        UpdateInventoryText();

    }

    public void FindPlayer(GameObject obj)
    {
        playerInteractor = obj;
        
    }
    public void AddItem()
    {
        AddItemInventory(selectedSlot.slotItem);
    }


    public void AddItemInventory(ItemData item)
    {

        if (InventoryFull())
        {
            DropItem(item);
            return;
        }
        for (int i = 0; i < data.inventory.Count; i++)
        {
           
         
            if (data.inventory[i].item == item && item.canStack && data.inventory[i].amount < item.maxStackAmount)
            {
                data.inventory[i].amount++;
                InventoryUpdate();
                return;
            }
            if (!data.inventory[i].item)
         {
                

                data.inventory[i].item = item;
                data.inventory[i].amount++;
                data.inventory[i].hasItem = true;
                InventoryUpdate();
                   return;
         
         }
            
        }

    }

    public void RemoveItemInventory(ItemData item)
    {

        for (int i = 0; i < data.inventory.Count; i++)
        {


            if (data.inventory[i].item == item)
            {

                data.inventory[i].amount--;
                if (data.inventory[i].amount <= 0)
                {
                    data.inventory[i].item = null;
                    data.inventory[i].hasItem = false;
                }
                InventoryUpdate();
                
                return;
            }
         

        }

    }

    public void DropItem(ItemData item)
    {
        Instantiate(item.itemObject, playerInteractor.transform.position, Quaternion.identity);
        
    }

    public void DropItem()
    {
        
            for(int i = 0; i <inventory.Length; i++)
            {
                if(inventory[i] == selectedSlot)
                {

                if(data.inventory[i].hasItem == false)
                {
                    return;
                }

                data.inventory[i].amount--;
                Instantiate(selectedSlot.slotItem.itemObject, playerInteractor.transform.position, Quaternion.identity);
                if (data.inventory[i].amount <= 0)
                    {
                    data.inventory[i].item = null;
                    data.inventory[i].hasItem = false;
                    
                    }
                InventoryUpdate();
                SlotSelected(inventory[i]);
                UpdateInventoryText();
                return;
                }
            }

    }

    public void InventoryUpdate()
    {

        for (int i = 0; i < inventory.Length; i++)

        {

                if (data.inventory[i].item && data.inventory[i].amount > 0)
                {
                inventory[i].slotItem = data.inventory[i].item;
                inventory[i].slotAmount = data.inventory[i].amount;
                data.inventory[i].hasItem = true;
                inventory[i].UpdateItem(inventory[i].slotItem,inventory[i].slotAmount);
                }
            else
            {
                inventory[i].slotItem = null;
                inventory[i].slotAmount = 0;
                data.inventory[i].hasItem = false;
                inventory[i].UpdateItem(inventory[i].slotItem, inventory[i].slotAmount);
            }
           

        }

        for (int i = 0; i < craftInventory.Length; i++)

        {

            if (data.inventory[i].item)
            {
                craftInventory[i].slotItem = data.inventory[i].item;
                craftInventory[i].slotAmount = data.inventory[i].amount;
                craftInventory[i].UpdateItem(inventory[i].slotItem, inventory[i].slotAmount);
            }
            else
            {
                craftInventory[i].slotItem = null;
                craftInventory[i].slotAmount = 0;
                data.inventory[i].hasItem = false;
                craftInventory[i].UpdateItem(inventory[i].slotItem, inventory[i].slotAmount);
            }


        }

    }
    public  bool InventoryFull()
{
    foreach(InventorySlot slot in data.inventory)
        {
            if(!slot.item)
            {
                return false;
            }
        }

    return true;


    }
    public void SlotSelected(Slot slot)
    {
        selectedSlot = null;
        
        selectedSlot = slot;
        UpdateButtonOptions();
        UpdateInventoryText();

    }
    public void UpdateButtonOptions()
    {
        if (selectedSlot.slotItem == null)
        {
            DropButton(false);
            EquipButton(false);
            return;
        }
        if (selectedSlot.slotItem.type == ItemType.Equipment)
        {


            DropButton(false);
            EquipButton(true);


            EventsManager.EquipableItem(selectedSlot.slotItem);


        }
        else if (selectedSlot.slotItem.type == ItemType.Resource)
        {
            DropButton(true);
            EquipButton(false);
        }
        else
        {
            DropButton(true);
        }
    }
    public void EquipButton(bool equip)
    {

        equipButton.gameObject.SetActive(equip);
        
    }

    public void UnEquipButton(bool equip)
    {

        unEquipButton.gameObject.SetActive(equip);

    }

    public void DropButton(bool drop)
    {
        
        dropButton.gameObject.SetActive(drop);
        
    }

    public void UpdateInventoryText()
    {
        if (selectedSlot != null)
        {
            if (selectedSlot.slotItem != null)
            {
                itemName.text = selectedSlot.slotItem.itemName;
                itemDescription.text = selectedSlot.slotItem.itemDescription;
                itemType.text = $"Type: {selectedSlot.slotItem.type}";
                if (selectedSlot.slotItem.value != 0)
                {
                    itemValue.gameObject.SetActive(true);
                    itemValue.text = $"Value: {selectedSlot.slotItem.value}";
                }
                else
                {
                    itemValue.gameObject.SetActive(false);
                }

                itemRating.text = $"Rating: {selectedSlot.slotItem.rating}";
            }
            else
            {
                itemName.text = "";
                itemDescription.text = "";
                itemType.text = "";
                itemValue.text = "";
                itemRating.text = "";
            }
        }

        else
        {
            itemName.text = "";
            itemDescription.text = "";
            itemType.text = "";
            itemValue.text = "";
            itemRating.text = "";
        }
    }
}


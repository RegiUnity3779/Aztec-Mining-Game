using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public InventoryData data;

    public Slot[] inventory;
    public Slot selectedSlot;
    public GameObject playerInteractor;

    int amountOfSlots;
    public Button equipButton;
    public Button unEquipButton;
    public Button dropButton;
    private void OnEnable()
    {
        EventsManager.AddToInventory += AddItemInventory;
        EventsManager.EquipItem += EquipButtons;
        EventsManager.UnEquipItem += EquipButtons;
        EventsManager.FindPlayerInteractor += FindPlayer;
    }

    private void OnDisable()
    {
        EventsManager.AddToInventory -= AddItemInventory;
        EventsManager.EquipItem -= EquipButtons;
        EventsManager.UnEquipItem -= EquipButtons;
        EventsManager.FindPlayerInteractor -= FindPlayer;
    }
    // Start is called before the first frame update
    void Start()
    {
        InventoryUpdate();

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
                
                return;
                }
            }
    }

    public void InventoryUpdate()
    {

        for (int i = 0; i < inventory.Length; i++)

        {

                if (data.inventory[i].item)
                {
                inventory[i].slotItem = data.inventory[i].item;
                inventory[i].slotAmount = data.inventory[i].amount;
                data.inventory[i].hasItem = data.inventory[i].hasItem;
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
        if(selectedSlot.slotItem == null)
        {
            return;
        }
        if (selectedSlot.slotItem.type == ItemType.Equipment)
        {
            if (dropButton.gameObject.activeInHierarchy)
            {
                DropButton();
            }
            if (!equipButton.gameObject.activeInHierarchy)
            {
                equipButton.gameObject.SetActive(true);
            }

            EventsManager.EquipableItem(selectedSlot.slotItem);

        }
        else
        {
            if (!dropButton.gameObject.activeInHierarchy)
            {
                DropButton();
            }


        }
        
        
    }
    
    public void EquipButtons()
        {
        if (equipButton.gameObject.activeInHierarchy)
        {
            equipButton.gameObject.SetActive(false);
        }
        else
        {
            equipButton.gameObject.SetActive(true);
        }

        if (unEquipButton.gameObject.activeInHierarchy)
        {
            unEquipButton.gameObject.SetActive(false);
        }
        else
        {
            unEquipButton.gameObject.SetActive(true);
        }
        
    }

    public void DropButton()
    {
        if (dropButton.gameObject.activeInHierarchy)
        {
            dropButton.gameObject.SetActive(false);
        }
        else
        {
            dropButton.gameObject.SetActive(true);
        }

    }
}


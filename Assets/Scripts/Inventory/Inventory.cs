using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public ItemDatabase database;
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
    public TextMeshProUGUI itemStaminaValue;

    public Button equipButton;
    public Button unEquipButton;
    public Button dropButton;
    public Button eatButton;
    private void OnEnable()
    {
        EventsManager.AddToInventory += AddItemInventory;
        EventsManager.EquipButton += EquipButton;
        EventsManager.UnEquipButton += UnEquipButton;
        EventsManager.RemoveFromInventory += RemoveItemInventory;
        EventsManager.EatButton += EatConsumerable;
        EventsManager.Fainted += PlayerFainted;
    }

    private void OnDisable()
    {
        EventsManager.AddToInventory -= AddItemInventory;
        EventsManager.EquipButton -= EquipButton;
        EventsManager.UnEquipButton -= UnEquipButton;
        EventsManager.RemoveFromInventory -= RemoveItemInventory;
        EventsManager.EatButton -= EatConsumerable;
        EventsManager.Fainted -= PlayerFainted;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        InventoryUpdate();
        UpdateInventoryText();

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
                    if (data.inventory[i].item.itemSprite == GameManager.instance.GetComponent<GameManager>().equipImage.sprite)
                {
                    EventsManager.RemoveEquipableItem();
                }
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

                data.inventory[i].item = null;
                inventory[i].slotItem = null;
                data.inventory[i].amount = 0;
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
            EatButton(false);
            return;
        }
        if (selectedSlot.slotItem.type == ItemType.Equipment)
        {


            DropButton(false);
            EquipButton(true);
            EatButton(false);


            EventsManager.EquipableItem(selectedSlot.slotItem);


        }
        else if (selectedSlot.slotItem.type == ItemType.Resource)
        {
            if (selectedSlot.slotItem.itemObject)
            {
                DropButton(true);
            }
            else 
            {
                DropButton(false);
            }
            
            EquipButton(false);
            EatButton(false);
        }

        else if (selectedSlot.slotItem.type == ItemType.Consumable)
        {
            DropButton(false);
            EquipButton(false);
            EatButton(true);
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

    public void EatButton(bool eat)
    {

        eatButton.gameObject.SetActive(eat);

    }

    public void EatConsumerable()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == selectedSlot)
            {

                if (data.inventory[i].hasItem == false)
                {
                    return;
                }

                data.inventory[i].amount--;
                EventsManager.EatConsumerable(selectedSlot.slotItem.staminaValue);
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

    public void PlayerFainted()
    {
        for (int i = 0; i < data.inventory.Count; i++)
        {
            if (data.inventory[i].hasItem)
            {
                if(data.inventory[i].item.equipment == EquipmentType.Pickaxe)
                {
                    int rate = ((int)data.inventory[i].item.rating);
                    if (rate != 0) 
                    {
                        EventsManager.UnEquipItem();
                        RemoveItemInventory(data.inventory[i].item);

                        for (int j = 0; j < database.equipmentItemsList.Count; j++)
                        {
                            if (database.equipmentItemsList[j].equipment == EquipmentType.Pickaxe && ((int)database.equipmentItemsList[j].rating) == (rate-1))
                            {
                                AddItemInventory(database.equipmentItemsList[j]);
                            }
                        }

                            
                        

                        //for(int j =0; j< database.item.Length; j++)
                        //{
                        //    if (database.item[i].equipment == EquipmentType.Pickaxe && ((int)database.item[i].rating) == rate)
                        //    {
                        //        AddItemInventory(database.item[i]);
                        //    }
                        //}


                    }

                }

                else if(data.inventory[i].amount > 0)
                {
                    data.inventory[i].amount /= 2;

                    // to be contiuned
                }

            }
            

        }

        
        EventsManager.UpStairs();
        InventoryUpdate();
        UpdateInventoryText();
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

                if(selectedSlot.slotItem.staminaValue != 0)
                {
                    itemStaminaValue.text = $"Stamina Value: {selectedSlot.slotItem.staminaValue}";
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
                itemStaminaValue.text = "";
            }
        }

        else
        {
            itemName.text = "";
            itemDescription.text = "";
            itemType.text = "";
            itemValue.text = "";
            itemRating.text = "";
            itemStaminaValue.text = "";
        }
    }
}


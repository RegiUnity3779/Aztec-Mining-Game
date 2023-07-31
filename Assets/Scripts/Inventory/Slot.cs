using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public Image icon;
    Sprite sprite;
    public TextMeshProUGUI amount;

    public ItemData slotItem;
    public int slotAmount;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSlot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSlot()
    {
        if (slotItem == null)
        {
            sprite = null;
            icon.sprite = sprite;
            icon.gameObject.SetActive(false);
            amount.gameObject.SetActive(false);
        }
        else if (slotItem != null && slotAmount == 0)
        {
            slotItem = null;
            sprite = null;
            icon.sprite = sprite;
            icon.gameObject.SetActive(false);
            amount.gameObject.SetActive(false);
        }
        else
        {
            if (slotAmount > 1)
            {
                amount.gameObject.SetActive(true);
                amount.text = $"{slotAmount}";
            }
            else
            {
                amount.gameObject.SetActive(false);
            }
            sprite = slotItem.itemSprite;
            icon.sprite = sprite;
            icon.gameObject.SetActive(true);
        }

    }

    public void UpdateItem(ItemData item, int amount)
    {

        slotItem = item;
        slotAmount = amount;
        UpdateSlot();
    }

    public bool IsSlotSelected()
    {
        SlotSelected();
        return true;
    }
    public void SlotSelected()
    {
        
       // gameObject.GetComponent<Image>().color = new Color(Color.red.r, Color.red.g, Color.red.b);

    }
    public void AddSlotItem(Inventory inventory)
    {
        Slot selectedslot = inventory.selectedSlot;
        
        
        if (slotItem != null)
        {
            while (slotAmount > 0)
            {
                EventsManager.AddToInventory(slotItem);
                slotAmount--;
            }

            slotItem = null;
            

        }
        
            slotItem = selectedslot.slotItem;
            slotAmount= selectedslot.slotAmount;

            int a = selectedslot.slotAmount;
            while (a > 0)
            {
                EventsManager.RemoveFromInventory(selectedslot.slotItem);
                a--;
            }

        UpdateSlot();
    }
}

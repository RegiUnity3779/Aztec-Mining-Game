using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image icon;
    Sprite sprite;

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
        }
        else
        {

            sprite = slotItem.itemSprite;

            icon.sprite = sprite;
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
        
        gameObject.GetComponent<Image>().color = new Color(Color.red.r, Color.red.g, Color.red.b);

    }
}

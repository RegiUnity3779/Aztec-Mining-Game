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
            amount.gameObject.SetActive(false);
        }
        else
        {
            if (!amount.gameObject.activeInHierarchy && slotAmount >= 2)
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

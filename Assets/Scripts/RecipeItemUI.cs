using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeItemUI : MonoBehaviour
{
   public  Image itemImage;
   public  TextMeshProUGUI quanityText;
    private void Start()
    {
        
    }

    public void Setup(ItemData data, int quanity)
    {
        itemImage.sprite = data.itemSprite;
        quanityText.text = $"{ quanity}";
    }

}

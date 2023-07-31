using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RecipeCard : MonoBehaviour
{
    public CraftRecipe recipe;
    public GameObject sequence;
    public TextMeshProUGUI title;
    public GameObject recipeItemUI;
    public TextMeshProUGUI equationText;

    // Start is called before the first frame update
    void Start()
    {
        SetUpRecipeCard();



    }

    void SetUpRecipeCard()
    {
        if (recipe != null)
        {
            GameObject craft = Instantiate(recipeItemUI.gameObject, sequence.transform);

            craft.GetComponent<RecipeItemUI>().Setup(recipe.craftItem, recipe.craftAmount);

            TextMeshProUGUI text = Instantiate(equationText, sequence.transform);
            text.text = "=";

            for (int i = 0; i < (recipe.item.Length); i++)
            {
                GameObject g = Instantiate(recipeItemUI.gameObject, sequence.transform);
                g.GetComponent<RecipeItemUI>().Setup(recipe.item[i], recipe.amountNeeded[i]);

                if (i != recipe.item.Length -1)
                {
                    TextMeshProUGUI plus = Instantiate(equationText, sequence.transform);
                    plus.text = "+";
                }
            }

            title.text = recipe.name;

           // GameObject h = Instantiate(recipeItemUI.gameObject, sequence.transform);
           // h.GetComponent<RecipeItemUI>().Setup(recipe.item[recipe.item.Length], recipe.amountNeeded[recipe.amountNeeded.Length]);
        }
    }
}

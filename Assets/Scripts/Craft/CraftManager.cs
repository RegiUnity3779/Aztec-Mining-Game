using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{

    public List<CraftRecipe> recipesList = new List<CraftRecipe>();
    private List<bool> currentCriteria = new List<bool>();
    public Slot[] craftSlot;
    private CraftRecipe craftRecipe;
    public Slot resultCraftSlot;
    public GameObject recipeCard;
    public GameObject recipeManager;

    // Start is called before the first frame update
    void Start()
    {
        if(recipeManager.transform.childCount > 0)
        {
            foreach(Transform child in recipeManager.transform)
            {
                Destroy(child.gameObject);
            }
            
        }

        for(int i = 0; i < recipesList.Count; i++)
        {
            GameObject card = Instantiate(recipeCard, new Vector3(recipeManager.transform.position.x, recipeManager.transform.position.y,recipeManager.transform.position.z), Quaternion.identity);
            card.transform.SetParent(recipeManager.transform);
            card.GetComponent<RecipeCard>().recipe = recipesList[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CanCraft()
    {
        foreach (CraftRecipe recipe in recipesList)
        {

            if (RecipeCraftable(recipe) == true)
            {
                craftRecipe = recipe;
                PotenitalCraftableItemSlot();
                Debug.Log($"{recipe}");
                return;
            }
        }
        craftRecipe = null;
        Debug.Log("Can't craft");
        PotenitalCraftableItemSlot();
    }
    public void CraftRecipe()
    {


        if (craftRecipe != null)
        {

            for (int j = 0; j < craftRecipe.item.Length; j++)
            {
                int amount = craftRecipe.amountNeeded[j];
                Debug.Log(amount);

                for (int i = 0; i < craftSlot.Length; i++)
                {
                    if (craftSlot[i].slotItem != null)
                    {
                        if (craftSlot[i].slotItem == craftRecipe.item[j])
                        {
                            if (amount >= craftSlot[i].slotAmount)
                            {
                                amount -= craftSlot[i].slotAmount;
                                RemoveCraftCostItem(craftSlot[i], craftSlot[i].slotAmount);
                            }
                            else if (amount < craftSlot[i].slotAmount)
                            {
                                RemoveCraftCostItem(craftSlot[i], amount);
                            }

                        }
                    }

                }
 
            }
            EventsManager.AddToInventory(craftRecipe.craftItem);
            
        }

    }

    public void RemoveCraftCostItem(Slot slot , int i)
    {

        slot.slotAmount -= i;
        slot.UpdateSlot();
       
    }
    public bool RecipeCraftable( CraftRecipe recipe)
    {
        currentCriteria.Clear();
        for(int b = 0; b < recipe.item.Length; b++)
        {
            currentCriteria.Add(false);
        }
        
        for(int j = 0; j < recipe.item.Length; j++)
      {
            int amount = 0;
        for (int i = 0; i < craftSlot.Length; i++)
        {
            
            if (craftSlot[i].slotItem != null)
            {
                
                    
                    if (craftSlot[i].slotItem == recipe.item[j])
                    {
                        amount += craftSlot[i].slotAmount;
                        
                    }

                    if (amount >= recipe.amountNeeded[j])
                    {
                        currentCriteria[j] = true;

                    }
                }

            }

        }
        foreach(bool b in currentCriteria)
        {
            if (b == false)
            {
                return false;
            }
        }

        return true;
    }

    public void PotenitalCraftableItemSlot()
    {
        if(craftRecipe != null)
        {
            resultCraftSlot.slotItem = craftRecipe.craftItem;
            resultCraftSlot.slotAmount = craftRecipe.craftAmount;
            resultCraftSlot.UpdateSlot();

        }
        else
        {
            resultCraftSlot.slotItem = null;
            resultCraftSlot.slotAmount = 0;
            resultCraftSlot.UpdateSlot();

        }
    }

    public void AddToCraftSlot(Slot slot)
    {
        

        for(int i = 0; i < craftSlot.Length; i++)
        {
            if(craftSlot[i].slotItem == null)
            {
                craftSlot[i].slotItem = slot.slotItem;
                craftSlot[i].slotAmount = slot.slotAmount;

                int a = slot.slotAmount;
                while (a > 0)
                {
                    EventsManager.RemoveFromInventory(slot.slotItem);
                    a--;
                }

                craftSlot[i].UpdateSlot();
                return;
            }
            
        }

        Debug.Log("Craft slots are full");


    }
    public void RemoveFromCraftSlot(Slot slot)
    {
        if (slot.slotItem != null)
        {
            while (slot.slotAmount > 0)
            {
                EventsManager.AddToInventory(slot.slotItem);
                slot.slotAmount--;
            }

            slot.UpdateSlot();
        }
    }

    public void CraftPanelClose()
    {
        for (int i = 0; i < craftSlot.Length; i++)
        {
            RemoveFromCraftSlot(craftSlot[i]);

        }
    }

}

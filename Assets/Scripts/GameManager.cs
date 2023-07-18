using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] floorLevel;
    public bool autoEquipMode;
    private int autoEquipModeStaminaPenalty;
    public int stamina;
    public int staminaMax;
    public Toggle equipToggle;
    public Image equipImage;
    public Sprite offSprite;
    public Sprite onSprite;
    //public static GameObject inventoryInstance;
    //public static GameObject instance;
    //public FloorManager floorManager;


    private void OnEnable()
    {
        EventsManager.EquipToggle += EquipToggle;
        EventsManager.EquipToggleInteractable += EquipInteractable;
    }

    private void OnDisable()
    {
        EventsManager.EquipToggle -= EquipToggle;
        EventsManager.EquipToggleInteractable -= EquipInteractable;
    }

    // Start is called before the first frame update
    void Start()
    {
        EquipToggle(false, null);
        //instance = this.gameObject;
        //if (instance != null)
        //{
        //    Destroy(this);

        //}
        //DontDestroyOnLoad(instance.gameObject);



        //if(inventoryInstance == null)
        //    {
        //    DontDestroyOnLoad(inventoryInstance);


        //}
        //else
        //{
        //    Destroy(inventoryInstance.gameObject);
        //}

        if (SceneManager.GetActiveScene().name == "UnderGround")
        {
            EventsManager.DownStairs();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
   
   public void EquipButton()
    {
        EventsManager.EquipItem();
    }
   public void UnEquipButton()
    {
        EventsManager.UnEquipItem();
    }

    public void EquipToggleButton()
    {
       if(equipToggle.isOn == true && equipToggle.interactable)
        {
            UnEquipButton();
        }
        else
        {
            EquipButton();
        }
    }
    private void EquipToggle(bool b, ItemData data)
    {
        if(data == null)
        {
            equipImage.sprite = null;
            equipToggle.image.sprite = offSprite;
            equipImage.gameObject.SetActive(false);
            equipToggle.interactable = false;
        }
        else
        {
            equipToggle.interactable = true;
            if (b == true)
            {
                equipImage.sprite = data.itemSprite;
                equipToggle.image.sprite = onSprite;
            }
            else
            {
                equipToggle.image.sprite = offSprite;
            }
            equipImage.gameObject.SetActive(true);

        }
    }
    private void EquipInteractable(bool b)
    {
        equipToggle.interactable = b;
    }

}

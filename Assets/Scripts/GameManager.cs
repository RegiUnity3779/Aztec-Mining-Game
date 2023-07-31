using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] floorLevel;
    public bool autoEquipMode;
    private int autoEquipModeStaminaPenalty = 1;
    private int staminaCost = 2;
    private float staminaPercentage;
    public int staminaCurrent;
    public int staminaMax;
    public Image staminaBar;
    public TextMeshProUGUI staminaBarText;
    public TextMeshProUGUI floorLevelText;

    public bool zenMode = false;
    public Toggle equipToggle;
    public Image equipImage;
    public Sprite offSprite;
    public Sprite onSprite;
    public  GameObject inGameUICanvas;
    private static GameObject inGameUIInstance;
   private static GameObject instance;
    //public FloorManager floorManager;


    private void OnEnable()
    {
        EventsManager.EquipToggle += EquipToggle;
        EventsManager.EquipToggleInteractable += EquipInteractable;
        EventsManager.Stamina += Stamina;
        EventsManager.FloorChange += FloorChange;
        EventsManager.SceneChange += SceneChange;
        EventsManager.UnderGroundCheck += IsUnderGround;
    }

    private void OnDisable()
    {
        EventsManager.EquipToggle -= EquipToggle;
        EventsManager.EquipToggleInteractable -= EquipInteractable;
        EventsManager.Stamina -= Stamina;
        EventsManager.FloorChange -= FloorChange;
        EventsManager.SceneChange -= SceneChange;
        EventsManager.UnderGroundCheck -= IsUnderGround;
    }

    // Start is called before the first frame update
    void Awake()
    {


        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(inGameUICanvas);
        
        if (instance == null)
        {
            instance = this.gameObject;
            
        }
        else
        {
            Destroy(this.gameObject);
        }


        if (inGameUIInstance == null)
        {
            inGameUIInstance = inGameUICanvas.gameObject;


        }
        
        else
        {
            Destroy(inGameUICanvas.gameObject);
        }
        
        SceneChange("EntranceFloor");
        EquipToggle(false, null);
        StaminaBar();
        FloorChange(0);
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

    public void Stamina()
    {
        if (staminaCurrent > 0 && !zenMode)
        {

            staminaCurrent -= staminaCost;
            if (autoEquipMode)
            {
                staminaCurrent -= autoEquipModeStaminaPenalty;
            }
            if (staminaCurrent <= 0)
            {
                staminaCurrent = 0;
                Debug.Log("Fainted");
            }

            StaminaBar();
        }

    }
    public void StaminaBar()
    {
        

        if (!zenMode)
        {
            staminaBarText.text = $"{staminaCurrent}/{staminaMax}";

            //To convert stamina to a float from int
            float staminaPercentage = (float)staminaCurrent/(float)staminaMax;
            staminaBar.fillAmount = staminaPercentage;
        }

        else
        {
            staminaBarText.text = "Infinity";
            staminaBar.fillAmount = 1;
        }
    }

    private void FloorChange(int floor)
    {

        if (floor == 0)
        {
            floorLevelText.gameObject.SetActive(false);
        }

        else
        {   
            floorLevelText.text = $"Floor {floor}";
            floorLevelText.gameObject.SetActive(true);
        }
    }

    public void SceneChange(string name)
    {
        
        if(SceneManager.GetActiveScene().name != name)
        {
            SceneManager.LoadScene(name);
        }
    }
    private void GoDownStairs()
    {
        EventsManager.DownStairs();
    }
   
    private void IsUnderGround()
    {
        if (SceneManager.GetActiveScene().name == "Underground")
        {
            if (gameObject.GetComponent<Inventory>().playerInteractor != null)
            {
                EventsManager.IsUnderGround(true);
            }

        }
        else
        {
            if (gameObject.GetComponent<Inventory>().playerInteractor != null)
            {
                EventsManager.IsUnderGround(false);
            }
            

        }
    }
}

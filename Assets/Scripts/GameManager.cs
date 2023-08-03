using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject[] floorLevel;
    public TextMeshProUGUI floorLevelText;

    private int staminaCost = 2;
    
    public int staminaCurrent;
    public int staminaMax;
    private float staminaPercentage;
    public Image staminaBar;
    public TextMeshProUGUI staminaBarText;

    public bool zenMode = false;
    public Toggle zenModeToggle;
    
    public Toggle equipToggle;
    public Image equipImage;

    public bool autoEquipMode;
    public Toggle autoEquipModeToggle;
    private int autoEquipModeStaminaPenalty = 1;

    public Sprite offSprite;
    public Sprite onSprite;

    public  GameObject inGameUICanvas;
    public static GameObject inGameUIInstance;
    public static GameObject instance;
    public static GameObject playerInstance;
    public static GameObject cameraInstance;


    public GameObject loadingScreen;
    //public Image loadingImage;

    private void OnEnable()
    {
        EventsManager.EquipToggle += EquipToggle;
        EventsManager.EquipToggleInteractable += EquipInteractable;
        EventsManager.Stamina += Stamina;
        EventsManager.FloorChange += FloorChange;
        EventsManager.SceneChange += SceneChange;
        EventsManager.UnderGroundCheck += IsUnderGround;
        EventsManager.EatConsumerable += EatConsumerable;
        EventsManager.StaminaRestored += StaminaRestored;
        


    }

    private void OnDisable()
    {
        EventsManager.EquipToggle -= EquipToggle;
        EventsManager.EquipToggleInteractable -= EquipInteractable;
        EventsManager.Stamina -= Stamina;
        EventsManager.FloorChange -= FloorChange;
        EventsManager.SceneChange -= SceneChange;
        EventsManager.UnderGroundCheck -= IsUnderGround;
        EventsManager.EatConsumerable -= EatConsumerable;
        EventsManager.StaminaRestored -= StaminaRestored;

    }

    // Start is called before the first frame update
    void Awake()
    {


        if (instance == null)
        {
            instance = this.gameObject;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        if (SceneManager.GetActiveScene().name != "EntranceFloor")
        {
            SceneChange("EntranceFloor");
        }
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

    public void EatButton()
    {
        EventsManager.EatButton();
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
                EventsManager.Fainted();
                Debug.Log("Fainted");
            }

            StaminaBar();
        }

    }

    public void StaminaRestored()
    {
        if (!zenMode)
        {

            staminaCurrent = staminaMax;

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
            floorLevelText.transform.parent.gameObject.SetActive(false);
            floorLevelText.text = "Floor: Entrance";
        }

        else
        {   
            floorLevelText.text = $"Floor: {floor}";
            floorLevelText.transform.parent.gameObject.SetActive(true);
        }
    }

    public void SceneChange(string name)
    {
        
        if(SceneManager.GetActiveScene().name != name)
        {
            //StartCoroutine(LoadingSceneScreen());
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

    public void EatConsumerable(int i)
    {
        if (!zenMode)
        {
            if (staminaCurrent == staminaMax)
            {
                staminaCurrent -= i;
            }
            else
            {
                staminaCurrent += i;
                if (staminaCurrent > staminaMax)
                {
                    staminaCurrent = staminaMax;
                }
            }

            StaminaBar();
        }
    }
  

    public void AutoEquipMode()
    {
        autoEquipMode = autoEquipModeToggle.isOn;
        if (autoEquipMode)
        {
            autoEquipModeToggle.image.sprite = onSprite;
        }
        else
        {
            autoEquipModeToggle.image.sprite = offSprite;
        }

    }

    public void ZenMode()
    {
        zenMode = zenModeToggle.isOn;
        if (zenMode)
        {
            zenModeToggle.image.sprite = onSprite;

        }
        else
        {
            zenModeToggle.image.sprite = offSprite;
        }
        StaminaBar();
    }

    public IEnumerator LoadingSceneScreen()
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        loadingScreen.SetActive(false);
    }
}

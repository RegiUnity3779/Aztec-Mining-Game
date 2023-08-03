using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    private GameObject gameManager;
    private float playerSpeed = 4.0f;
    private float playerTurnSpeed = 90.0f;

    private float forwardInput;
    private float horizontalInput;
    private bool isUnderGround;
    public GameObject playerInteractor;
    public EquipedItem playerEquip;
    private bool canInteract;
    private GameObject interactableObject;
    public GameObject playerIndicator;


    private void OnEnable()
    {
        EventsManager.Interactable += Interactable;
        EventsManager.PlayerMarker += PlayerMarker;
        EventsManager.IsUnderGround += IsUnderGround;
        EventsManager.UpdatePlayerLocation += UpdatePlayerLocation;
    }

    private void OnDisable()
    {
        EventsManager.Interactable -= Interactable;
        EventsManager.PlayerMarker -= PlayerMarker;
        EventsManager.IsUnderGround -= IsUnderGround;
        EventsManager.UpdatePlayerLocation -= UpdatePlayerLocation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if (GameManager.playerInstance == null)
        {
            GameManager.playerInstance = this.gameObject;
            DontDestroyOnLoad(this);

        }

        else
        {
            Destroy(this.gameObject);
            return;
        }
        gameManager = GameManager.instance;
        EventsManager.UnderGroundCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

       transform.Translate(Vector3.forward * forwardInput * playerSpeed * Time.deltaTime);
       transform.Rotate(Vector3.up * horizontalInput * playerTurnSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Interaction();
        }
    }

    void Interaction()
    {
     
        if (canInteract && interactableObject != null) 
        {
            
                    if (gameManager.GetComponent<GameManager>().autoEquipMode == false)
                    {
                        if (interactableObject.CompareTag("Rock") && playerEquip.itemEquiped && playerEquip.item.equipment == EquipmentType.Pickaxe)
                        {

                            Interact(interactableObject.GetComponent<Rock>());
                            EventsManager.Stamina();

                        }

                        if (interactableObject.CompareTag("Item") && !playerEquip.itemEquiped)
                        {

                            Interact(interactableObject.GetComponent<Item>());

                        }

                        if (interactableObject.CompareTag("StairsDown"))// interactableObject.CompareTag("StairsDown") && interactableObject.GetComponentInChildren<StairsDown>().active == true
                        {
                            EventsManager.UnderGroundCheck();
                            if (isUnderGround)
                            {
                                EventsManager.DownStairs();
                                
                            }
                            else
                            {
                                EventsManager.SceneChange("Underground");
                    }

                        }

                        if (interactableObject.CompareTag("StairsUp"))
                        {
                            EventsManager.UnderGroundCheck();

                            if (isUnderGround)
                            {
                                EventsManager.UpStairs();
                               // EventsManager.UpdateCamera();
                            }


                        }

                        else
                        {

                        }
                    }
                    else
                    {

                        if (interactableObject.CompareTag("Rock"))

                        {
                            Inventory inventory = gameManager.GetComponent<Inventory>();
                    if (playerEquip.item == null)
                    {
                        for (int i = 0; i < inventory.data.inventory.Count; i++)
                        {
                            if (inventory.data.inventory[i].hasItem)
                            {
                                if (inventory.data.inventory[i].item.type == ItemType.Equipment)
                                {
                                    if (inventory.data.inventory[i].item.equipment == EquipmentType.Pickaxe)
                                    {
                                        inventory.SlotSelected(inventory.inventory[i]);

                                    }
                                }
                            }


                        } 
                    }

                    else if (playerEquip.item.type != ItemType.Equipment && playerEquip.item.equipment != EquipmentType.Pickaxe)
                    {

                        for (int i = 0; i < inventory.data.inventory.Count; i++)
                        {
                            if (inventory.data.inventory[i].item.equipment == EquipmentType.Pickaxe)
                            {
                                inventory.SlotSelected(inventory.inventory[i]);

                            }
                        }
                    }
                            if (playerEquip.itemEquiped == false)
                            {
                                EventsManager.EquipItem();
                            }

                            Interact(interactableObject.GetComponent<Rock>());
                            EventsManager.Stamina();

                        }

                        if (interactableObject.CompareTag("Item"))
                        {
                            if (playerEquip.itemEquiped == true)
                            {
                                EventsManager.UnEquipItem();
                            }

                            Interact(interactableObject.GetComponent<Item>());

                        }

                        if (interactableObject.CompareTag("StairsDown"))
                        {
                            EventsManager.UnderGroundCheck();
                            if (isUnderGround)
                            {
                                EventsManager.DownStairs();
                            }
                            else
                            {
                                EventsManager.SceneChange("Underground");
                                
                            }


                        }

                        if (interactableObject.CompareTag("StairsUp"))
                        {
                            EventsManager.UnderGroundCheck();

                            if (isUnderGround)
                            {
                                EventsManager.UpStairs();
                        
                            }

                        }

                        else
                        {

                        }
                    }

        }
       
    }

    void Interact(Rock rock)
    {
        EventsManager.StairsRevealed(rock.transform.position);
        rock.RockDestroyed();

    }

    void Interact(Item item)
    {
        item.CollectItem();

    }

    void Interactable(bool interact, GameObject gameObject)
    {
        if (gameObject != null)
        {
            canInteract = interact;
            if (canInteract == false)
            {  
                interactableObject = null;

            }
            else
            {
               
                interactableObject = gameObject;
            }
        }
        else
        {
            canInteract = false;
            interactableObject = null;
        }
    }
    public void PlayerMarker()
    {
        StartCoroutine("PlayerMarkerTimer");
    }

    public IEnumerator PlayerMarkerTimer()
    {
        playerIndicator.SetActive(true);
        yield return new WaitForSeconds(3);
        playerIndicator.SetActive(false);
    }



    private void IsUnderGround(bool underground)
    {
        isUnderGround = underground;
    }


    private void UpdatePlayerLocation(Vector3 pos)
    {
        this.gameObject.transform.position = pos;
        this.gameObject.transform.rotation = new Quaternion(0, 0, 0,0);
        EventsManager.UpdateCamera();
    }
}






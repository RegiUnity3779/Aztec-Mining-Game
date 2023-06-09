using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{

    private float playerSpeed = 5.0f;
    private float playerTurnSpeed = 75.0f;

    private float forwardInput;
    private float horizontalInput;

    public GameObject playerInteractor;
    public EquipedItem playerEquip;
    private bool canInteract;
    private GameObject interactableObject;


    private void OnEnable()
    {
        EventsManager.Interactable += Interactable;
    }

    private void OnDisable()
    {
        EventsManager.Interactable -= Interactable;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
            if (interactableObject.CompareTag("Rock") && playerEquip.itemEquiped && playerEquip.item.equipment == EquipmentType.Pickaxe)
            {
               
                Interact(interactableObject.GetComponent<Rock>());
                
            }

            if (interactableObject.CompareTag("Item") && !playerEquip.itemEquiped)
            {
               
                Interact(interactableObject.GetComponent<Item>());

            }

            if (interactableObject.CompareTag("StairsDown") && interactableObject.GetComponentInChildren<StairsDown>().active == true)
            {
                EventsManager.DownStairs();

            }

            if (interactableObject.CompareTag("StairsUp"))
            {
                EventsManager.UpStairs();

            }

            else
            {

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
    
}

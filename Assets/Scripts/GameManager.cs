using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] floorLevel;
    //public static GameObject inventoryInstance;
    //public static GameObject instance;
    //public FloorManager floorManager;

 
    // Start is called before the first frame update
    void Start()
    {
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

}

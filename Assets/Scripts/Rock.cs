using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rock : MonoBehaviour
{
    public RockType type;
    private GameObject stairsD;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.gameObject.CompareTag("StairsUp"))// other.gameObject.CompareTag("Player")
        {
            EventsManager.GroundObjectRemoved(this.gameObject);

        }
        if (collision.gameObject.CompareTag("StairsDown"))
        {
            // stairsD = other.gameObject.transform.parent.gameObject;
            stairsD = collision.collider.gameObject;
            stairsD.SetActive(false);



        }
    }


    //private void OnTriggerStay(Collider other)
    //{

    //    if (other.gameObject.CompareTag("StairsUp"))// other.gameObject.CompareTag("Player")
    //    {
    //        EventsManager.GroundObjectRemoved(this.gameObject);
    //        Debug.Log("U");
    //    }
    //    if (other.gameObject.CompareTag("StairsDown"))
    //    {
    //        // stairsD = other.gameObject.transform.parent.gameObject;
    //        stairsD = other.gameObject;
    //        stairsD.SetActive(false);
    //        Debug.Log("D off");


    //    }
    //}
    public void RockDestroyed()
    {
        if (stairsD != null)
        {
            if (gameObject.transform.position.x == stairsD.transform.position.x && gameObject.transform.position.z == stairsD.transform.position.z)
            {
                stairsD.SetActive(true);
                //stairsD.GetComponent<StairsDown>().active = true;
                stairsD = null;
                Destroy(this.gameObject);
               
                return;
            }
        }


        int a = Random.Range(0, 100);
        
        if (a < type.spawnChance)
        {
            GenerateItem();
            
        }
        Destroy(this.gameObject);
    }
    void GenerateItem()
    {
        int t = 0;
        for (int i = 0; i < type.spawnProability.Length; i++)
        {
            
            t += type.spawnProability[i];
        }

        int a = Random.Range(0, t);
        for (int i = 0; i < type.spawnProability.Length; i++)
        {
            a -= type.spawnProability[i];
            if(a <= 0)
            {
                Instantiate(type.spawnItem[i].gameObject, new Vector3(transform.position.x, (transform.position.y + (gameObject.transform.localScale.y/2)), transform.position.z), Quaternion.identity);
                
               
                return;
            }
            
        }

    }

    
}

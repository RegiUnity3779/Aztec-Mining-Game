using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rock : MonoBehaviour
{
    public RockType type;

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("StairsUp"))
        {
            EventsManager.GroundObjectRemoved(this.gameObject);

        }
        else if (other.gameObject.CompareTag("StairsDown"))
        {
            other.gameObject.SetActive(false);
        }
    }
    public void RockDestroyed()
    {
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
                Instantiate(type.spawnItem[i].gameObject, transform.position, Quaternion.identity);
                
               // Debug.Log(type.spawnItem[i]);
                return;
            }
            
        }

    }

    
}

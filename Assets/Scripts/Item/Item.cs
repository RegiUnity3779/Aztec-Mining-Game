using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.ItemInScene(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectItem()
    {
        Destroy(gameObject);
        EventsManager.AddToInventory(this.itemData);

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.CompareTag("StairsUp") || collision.gameObject.CompareTag("StairsDown"))
        {
            EventsManager.GroundObjectRemoved(this.gameObject);

        }
        
    }
}

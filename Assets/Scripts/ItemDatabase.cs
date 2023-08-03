using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item Database",menuName ="Item Database/New Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> equipmentItemsList = new List<ItemData>();
    public List<ItemData> resourceItemsList = new List<ItemData>();
    public List<ItemData> consumerableItemsList = new List<ItemData>();
    public List<ItemData> legendaryItemsList = new List<ItemData>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

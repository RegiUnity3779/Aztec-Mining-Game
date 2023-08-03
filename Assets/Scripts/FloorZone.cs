using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FloorZone", menuName = "FloorZone/ New Zone")]
public class FloorZone : ScriptableObject
{
    
    public int[] mapSizeXMinAndMax;
    public int[] mapSizeZMinAndMax;

    public GameObject[] groundTiles;
    public GameObject[] groundObject;

    public int[] groundTilesProbability;
    public int[] landTilesProbability;
    public int groundObjectSpawning;
    public int[] groundObjectProbability;

    

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] List<GameObject> floorLayout = new List<GameObject>();
    [SerializeField] List<GameObject> walkableFloor = new List<GameObject>();
    [SerializeField] List<GameObject> stairSite = new List<GameObject>();
    [SerializeField] List<GameObject> floorObjects = new List<GameObject>();

    public FloorZone[] zone;
    private FloorZone curentZone;
    public int[] zoneChange;

    public Vector3[] stairTileLocations;
    private Vector3 upStairTile;
    public GameObject stairsUp;
    public GameObject stairsDown;
    private GameObject player;
    private int floorLevel = 0;

    private ItemData curPlayerEquipItem;
    private bool curPlayerItemEquiped = false;


    int floorMapSizeX;
    int floorMapSizeZ;

    private void OnEnable()
    {
        EventsManager.GroundObjectRemoved += GroundObjectRemoved;
        EventsManager.StairsRevealed += StairsRevealed;
        EventsManager.DownStairs += GoDownStairs;
        EventsManager.UpStairs += GoUpStairs;
        EventsManager.ItemInScene += ItemInScene;
    }

    private void OnDisable()
    {
        EventsManager.GroundObjectRemoved -= GroundObjectRemoved;
        EventsManager.StairsRevealed -= StairsRevealed;
        EventsManager.DownStairs -= GoDownStairs;
        EventsManager.UpStairs -= GoUpStairs;
        EventsManager.ItemInScene -= ItemInScene;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.playerInstance;
        GoDownStairs();

    }
    private void Update()
    {

        //Debug.DrawRay(upStairTile, Vector3.forward, Color.red, 0.5f);
        //Debug.DrawRay(upStairTile, -Vector3.forward, Color.red, 0.5f);
        //Debug.DrawRay(upStairTile, Vector3.right, Color.red, 0.5f);
        //Debug.DrawRay(upStairTile, -Vector3.right, Color.red, 0.5f);
    }
    public void GoDownStairs()
    {
        
        
        floorLevel++;
        FloorZone();
        NewFloor();
        EventsManager.FloorChange(floorLevel);
        EventsManager.PlayerMarker();

       
    }

    public void GoUpStairs()
    {
        floorLevel = 0;
        
        EventsManager.FloorChange(floorLevel);
        EventsManager.SceneChange("EntranceFloor");
        EventsManager.StaminaRestored();
        EventsManager.UpdatePlayerLocation(new Vector3(-2.3f,0.6f,-4.3f));
        

    }


    public void FloorZone()
    {
        
            for (int i = 0; i < zoneChange.Length; i++)
            {
                if (floorLevel < zoneChange[i])
                {
                    curentZone = zone[i];
                    return;
                }
            }

            curentZone = zone[zoneChange.Length];
 
    }
    public void NewFloor()
    {

        if (player != null)
        {
            curPlayerEquipItem = player.GetComponent<PlayerController>().playerEquip.item;
            curPlayerItemEquiped = player.GetComponent<PlayerController>().playerEquip.itemEquiped;
        }

        //curPlayer = null;
        //while (!curPlayer)
        //{
        DestroyFloor();
        GenerateFloor();
        //}
        if (curPlayerEquipItem != null && curPlayerItemEquiped == true)
        {
            EventsManager.EquipableItem(curPlayerEquipItem);
            EventsManager.EquipItem();
        }
    }
    public void GenerateFloor()
    {
            GroundLayout();
            GenerateStairs();
            UpdatePlayerLocation();
            GroundObjectLayoutProbability();

    }
    void ItemInScene(GameObject obj)
    {
        floorObjects.Add(obj);
    }
    public void DestroyFloor()
    {
        

        foreach (GameObject obj in floorObjects)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in stairSite)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in floorLayout)
        {
            Destroy(obj);
        }

        floorObjects.Clear();
        stairSite.Clear();
        floorLayout.Clear();
        walkableFloor.Clear();

    }

    void GroundLayout()
    {
        floorMapSizeX = Random.Range(curentZone.mapSizeXMinAndMax[0] , curentZone.mapSizeXMinAndMax[1]);
        floorMapSizeZ = Random.Range(curentZone.mapSizeZMinAndMax[0], curentZone.mapSizeZMinAndMax[1]);
        for (int x = 0; x < floorMapSizeX; x++)
        {
            for (int z = 0; z < floorMapSizeZ; z++)
            {
                if(x == 0 || z == 0 || x == (floorMapSizeX - 1) || z == (floorMapSizeZ - 1)) 
                { 
                    BoundaryLayout(x, z); 
                }
                else 
                {
                    if (GroundLayoutCompare(x, z) == true)
                    {

                        LandGroundLayoutProbability(x, z);
                    }
                    else
                    {
                        
                        StandardGroundLayoutProbability(x, z);
                    }
                }
                

            }
        }
       
        foreach(GameObject tile in floorLayout)
        {
            if (tile.CompareTag("Walkable"))
            {
                walkableFloor.Add(tile);
            }
        }
    }
    void BoundaryLayout(int x, int z)
    {
        if(x == floorMapSizeX - 1 || z == floorMapSizeZ - 1)
        {
            GameObject floorTileH = Instantiate(curentZone.groundTiles[2], new Vector3(x, curentZone.groundTiles[2].transform.localScale.y / 2, z), Quaternion.identity);
            floorTileH.transform.SetParent(this.transform);
            floorLayout.Add(floorTileH);
        }
        else
        {
            GameObject floorTileL = Instantiate(curentZone.groundTiles[1], new Vector3(x, curentZone.groundTiles[1].transform.localScale.y / 2, z), Quaternion.identity);
            floorTileL.transform.SetParent(this.transform);
            floorLayout.Add(floorTileL);
        }
        
        return;
    }
    bool GroundLayoutCompare(int x, int z)
    {

        foreach (GameObject tile in floorLayout)
        {

            int AdjacentX = x - Mathf.RoundToInt(tile.transform.position.x);
            int AdjacentZ = z - Mathf.RoundToInt(tile.transform.position.z);
            if (AdjacentX == 1 || AdjacentX == -1 && AdjacentZ == 0)
            {
                if (tile.CompareTag("Walkable"))
                {
                    return true;
                }
 
            }

            if (AdjacentZ == 1 || AdjacentZ == -1 && AdjacentX == 0)
            {
                if (tile.CompareTag("Walkable"))
                {
                    return true;
                }
            }
           
        } return false;

    }
    void StandardGroundLayoutProbability(int x, int z)
    {
        foreach (GameObject tile in floorLayout)
        {
        int AdjacentX = x - Mathf.RoundToInt(tile.transform.position.x);
        int AdjacentZ = z - Mathf.RoundToInt(tile.transform.position.z);

            if (AdjacentX == 1 || AdjacentX == -1 && AdjacentZ == 1 || AdjacentZ == -1)
            {
                if (tile.CompareTag("Walkable"))
                {
                    GameObject floorTile = Instantiate(curentZone.groundTiles[1], new Vector3(x, curentZone.groundTiles[1].transform.localScale.y / 2, z), Quaternion.identity);
                    floorTile.transform.SetParent(this.transform);
                    floorLayout.Add(floorTile);
                    return;
                }

            }

            if (AdjacentZ == 1 || AdjacentZ == -1 && AdjacentX == 1 || AdjacentX == -1)
            {
                if (tile.CompareTag("Walkable"))
                {
                    floorLayout.Add(Instantiate(curentZone.groundTiles[1], new Vector3(x, curentZone.groundTiles[1].transform.localScale.y / 2, z), Quaternion.identity)); 
                    return;
                }
            }
        }

        int total = SumOfArray(curentZone.groundTilesProbability);
            int a = Random.Range(0, total);

            for (int i = 0; i < curentZone.groundTilesProbability.Length; i++)
            {


                a -= curentZone.groundTilesProbability[i];

                if (a < 0)
                {
                GameObject floorTile =Instantiate(curentZone.groundTiles[i], new Vector3(x, curentZone.groundTiles[i].transform.localScale.y / 2, z), Quaternion.identity);
                floorTile.transform.SetParent(this.transform);
                floorLayout.Add(floorTile);
                return;
                }

        }
    }
    void LandGroundLayoutProbability(int x, int z)
    {
        int total = SumOfArray(curentZone.landTilesProbability);
        int a = Random.Range(0, total);

        for (int i = 0; i < curentZone.landTilesProbability.Length; i++)
        {


            a -= curentZone.landTilesProbability[i];

            if (a < 0)
            {
                GameObject floorTile = Instantiate(curentZone.groundTiles[i], new Vector3(x, curentZone.groundTiles[i].transform.localScale.y / 2, z), Quaternion.identity);
                floorTile.transform.SetParent(this.transform);
                floorLayout.Add(floorTile);
                return;
            }

        }
    }

    void GenerateStairs()
    {

        int u = Random.Range(0, walkableFloor.Count);
        stairSite.Add(Instantiate(stairsUp, walkableFloor[u].transform.position + new Vector3(0, (curentZone.groundTiles[0].transform.localScale.y / 2), 0), Quaternion.identity));

        stairTileLocations[0] = walkableFloor[u].transform.position;

        int d = Random.Range(0, walkableFloor.Count);
        while(u == d)
        {
            d = Random.Range(0, walkableFloor.Count);
        }

        stairSite.Add(Instantiate(stairsDown, walkableFloor[d].transform.position + new Vector3(0, (curentZone.groundTiles[0].transform.localScale.y / 2), 0), Quaternion.identity));

        stairTileLocations[1] = walkableFloor[d].transform.position;


        // stairLocations = stairSite.ToArray();
        //Instantiate(stairs, stairLocations[Random.Range(0, stairLocations.Length)].transform.position, Quaternion.identity);
        //+ new Vector3 (0,(stairs.transform.localScale.y / 2),0)

    }

    void UpdatePlayerLocation()
    {
        
        RaycastHit hit;
        
        foreach(GameObject obj in walkableFloor)
        {
            if(obj.transform.position.x == stairSite[0].transform.position.x && obj.transform.position.z == stairSite[0].transform.position.z)
            {
                upStairTile = obj.transform.position;
            }
        }
       
        if (Physics.Raycast(upStairTile, -transform.forward, out hit, 0.5f))
            {
            if (hit.collider != null)
            {
                    if (hit.collider.gameObject.CompareTag("Walkable"))
                    {
                    EventsManager.UpdatePlayerLocation(new Vector3(hit.collider.gameObject.transform.position.x, ((player.transform.localScale.y) + (curentZone.groundTiles[0].transform.localScale.y / 2)), hit.collider.gameObject.transform.position.z));
                    //player.transform.position = new Vector3(0, ((player.transform.localScale.y) + (groundTiles[0].transform.localScale.y / 2)), 0);

                    return;
                }
            }
        }

        if (Physics.Raycast(upStairTile, transform.forward, out hit, 0.5f))
            {
                if (hit.collider != null)
                {
                if(hit.collider.gameObject.CompareTag("Walkable")) 
                    {
                    EventsManager.UpdatePlayerLocation(new Vector3(hit.collider.gameObject.transform.position.x, ((player.transform.localScale.y) + (curentZone.groundTiles[0].transform.localScale.y / 2)), hit.collider.gameObject.transform.position.z));
                    //player.transform.position = new Vector3(0, ((player.transform.localScale.y) + (groundTiles[0].transform.localScale.y / 2)), 0);
              
                    return;
                }
                    }
                
            }

            

            if (Physics.Raycast(upStairTile, transform.right, out hit, 0.5f))
            {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Walkable"))
                {
                    EventsManager.UpdatePlayerLocation(new Vector3(hit.collider.gameObject.transform.position.x, ((player.transform.localScale.y) + (curentZone.groundTiles[0].transform.localScale.y / 2)), hit.collider.gameObject.transform.position.z));
                    //player.transform.position = new Vector3(0, ((player.transform.localScale.y) + (groundTiles[0].transform.localScale.y / 2)), 0);

                    return;
                }
            }
        }

             if (Physics.Raycast(upStairTile, -transform.right, out hit, 0.5f))
            {
            if (hit.collider != null)
            {
                    if (hit.collider.gameObject.CompareTag("Walkable"))
                    {
                    EventsManager.UpdatePlayerLocation(new Vector3(hit.collider.gameObject.transform.position.x, ((player.transform.localScale.y) + (curentZone.groundTiles[0].transform.localScale.y / 2)), hit.collider.gameObject.transform.position.z));
                    //player.transform.position = new Vector3(0, ((player.transform.localScale.y) + (groundTiles[0].transform.localScale.y / 2)), 0);

                    return;
                    }
            }

        }

        else
        {
            DestroyFloor();
            GenerateFloor();
            return;
        }

    }
    void GroundObjectLayoutProbability()

    { foreach (GameObject tile in walkableFloor)
        {
            if (tile.transform.position.x == player.transform.position.x && tile.transform.position.z == player.transform.position.z)
            {
                
            }

            else
            {
                int a = Random.Range(0, 100);


                a -= curentZone.groundObjectSpawning;

                if (a < 0)
                {
                    GroundObjectProbability(tile);

                }
            }
            
        }
    }
    void GroundObjectProbability(GameObject tile)
    {
        int sum = SumOfArray(curentZone.groundObjectProbability);
        int b = Random.Range(0, sum);

        for (int i = 0; i < curentZone.groundObjectProbability.Length; i++)
        {


            b -= curentZone.groundObjectProbability[i];


            if (b < 0)
            {

                //floorObjects.Add(Instantiate(groundObject[i], new Vector3(tile.transform.position.x, (tile.transform.position.y + ((tile.transform.localScale.y / 2) + (groundObject[i].transform.localScale.y / 2))), tile.transform.position.z), Quaternion.identity));
                GameObject floorObj = Instantiate(curentZone.groundObject[i], new Vector3(tile.transform.position.x, (tile.transform.position.y + (tile.transform.localScale.y / 2)), tile.transform.position.z), Quaternion.identity);
               // floorObj.transform.SetParent(this.transform);
                floorObjects.Add(floorObj);
                return;
            }



        }
    }
    int SumOfArray(int[] a)
    {
        int sum = 0;
        for(int i = 0; i < a.Length; i++)
        {
           sum += a[i];
        }

        return sum;
    }

   void StairsRevealed(Vector3 vec)
    {
        if (stairsDown.transform.position.x == vec.x && stairsDown.transform.position.z == vec.z)
        {
            stairsDown.SetActive(true);
        }
    }

   void GroundObjectRemoved(GameObject obj)
    {
        floorObjects.Remove(obj);
        Destroy(obj);
    }
}

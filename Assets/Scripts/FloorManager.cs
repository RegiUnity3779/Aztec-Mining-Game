using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] List<GameObject> floorLayout = new List<GameObject>();
    [SerializeField] List<GameObject> walkableFloor = new List<GameObject>();
    [SerializeField] List<GameObject> stairSite = new List<GameObject>();
    [SerializeField] List<GameObject> floorObjects = new List<GameObject>();

    public GameObject[] groundTiles;
    public GameObject[] groundObject;
    public Vector3[] stairTileLocations;
    private Vector3 upStairTile;
    public GameObject stairsUp;
    public GameObject stairsDown;
    public GameObject player;
    private int floorLevel = 0;

    private GameObject curPlayer;
    private GameObject curPlayerInteractor;
    private ItemData curPlayerEquipItem;
    private bool curPlayerItemEquiped = false;

    public int[] groundTilesProbability;
    public int[] landTilesProbability;
    public int groundObjectSpawning;
    public int[] groundObjectProbability;

    int mapSizeX;
    int mapSizeZ;


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
        NewFloor();
        EventsManager.FloorChange(floorLevel);
        EventsManager.PlayerMarker();

       
    }

    public void GoUpStairs()
    {
        floorLevel = 0;
        EventsManager.FloorChange(floorLevel);
        EventsManager.SceneChange("EntranceFloor");
        
        

    }

    public void NewFloor()
    {

        if (curPlayer != null)
        {
            curPlayerEquipItem = curPlayer.GetComponent<PlayerController>().playerEquip.item;
            curPlayerItemEquiped = curPlayer.GetComponent<PlayerController>().playerEquip.itemEquiped;
        }
        
        curPlayer = null;
        while (!curPlayer)
        {
            DestroyFloor();
            GenerateFloor();
        }
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
            GeneratePlayer();
            GroundObjectLayoutProbability();
            FindPlayer();

    }
    void ItemInScene(GameObject obj)
    {
        floorObjects.Add(obj);
    }
    void FindPlayer()
    {
        curPlayerInteractor = curPlayer.GetComponentInChildren<Interaction>().gameObject;
        EventsManager.FindPlayerInteractor(curPlayerInteractor);
        

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
        mapSizeX = Random.Range(8, 15);
        mapSizeZ = Random.Range(8, 15);
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                if(x == 0 || z == 0 || x == (mapSizeX-1) || z == (mapSizeZ-1)) 
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
        GameObject floorTile = Instantiate(groundTiles[1], new Vector3(x, groundTiles[1].transform.localScale.y / 2, z), Quaternion.identity);
        floorTile.transform.SetParent(this.transform);
        floorLayout.Add(floorTile);
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
                    GameObject floorTile = Instantiate(groundTiles[1], new Vector3(x, groundTiles[1].transform.localScale.y / 2, z), Quaternion.identity);
                    floorTile.transform.SetParent(this.transform);
                    floorLayout.Add(floorTile);
                    return;
                }

            }

            if (AdjacentZ == 1 || AdjacentZ == -1 && AdjacentX == 1 || AdjacentX == -1)
            {
                if (tile.CompareTag("Walkable"))
                {
                    floorLayout.Add(Instantiate(groundTiles[1], new Vector3(x, groundTiles[1].transform.localScale.y / 2, z), Quaternion.identity)); 
                    return;
                }
            }
        }

        int total = SumOfArray(groundTilesProbability);
            int a = Random.Range(0, total);

            for (int i = 0; i < groundTilesProbability.Length; i++)
            {


                a -= groundTilesProbability[i];

                if (a < 0)
                {
                GameObject floorTile =Instantiate(groundTiles[i], new Vector3(x, groundTiles[i].transform.localScale.y / 2, z), Quaternion.identity);
                floorTile.transform.SetParent(this.transform);
                floorLayout.Add(floorTile);
                return;
                }

        }
    }
    void LandGroundLayoutProbability(int x, int z)
    {
        int total = SumOfArray(landTilesProbability);
        int a = Random.Range(0, total);

        for (int i = 0; i < landTilesProbability.Length; i++)
        {


            a -= landTilesProbability[i];

            if (a < 0)
            {
                GameObject floorTile = Instantiate(groundTiles[i], new Vector3(x, groundTiles[i].transform.localScale.y / 2, z), Quaternion.identity);
                floorTile.transform.SetParent(this.transform);
                floorLayout.Add(floorTile);
                return;
            }

        }
    }

    void GenerateStairs()
    {

        int u = Random.Range(0, walkableFloor.Count);
        stairSite.Add(Instantiate(stairsUp, walkableFloor[u].transform.position + new Vector3(0, (groundTiles[0].transform.localScale.y / 2), 0), Quaternion.identity));

        stairTileLocations[0] = walkableFloor[u].transform.position;

        int d = Random.Range(0, walkableFloor.Count);
        while(u == d)
        {
            d = Random.Range(0, walkableFloor.Count);
        }

        stairSite.Add(Instantiate(stairsDown, walkableFloor[d].transform.position + new Vector3(0, (groundTiles[0].transform.localScale.y / 2), 0), Quaternion.identity));

        stairTileLocations[1] = walkableFloor[d].transform.position;


        // stairLocations = stairSite.ToArray();
        //Instantiate(stairs, stairLocations[Random.Range(0, stairLocations.Length)].transform.position, Quaternion.identity);
        //+ new Vector3 (0,(stairs.transform.localScale.y / 2),0)

    }

    void GeneratePlayer()
    {
        
        RaycastHit hit;
        
        foreach(GameObject obj in walkableFloor)
        {
            if(obj.transform.position.x == stairSite[0].transform.position.x && obj.transform.position.z == stairSite[0].transform.position.z)
            {
                upStairTile = obj.transform.position;
            }
        }
        

        if (Physics.Raycast(upStairTile, transform.forward, out hit, 0.5f))
            {
                if (hit.collider != null)
                {
                if(hit.collider.gameObject.CompareTag("Walkable")) 
                    {
                        curPlayer = Instantiate(player, hit.transform.position + new Vector3(0, ((player.transform.localScale.y) + (groundTiles[0].transform.localScale.y / 2)), 0), Quaternion.identity);
                        floorObjects.Add(curPlayer);
                    return;
                }
                    }
                
            }

            if (Physics.Raycast(upStairTile, -transform.forward, out hit, 0.5f))
            {
            if (hit.collider != null)
            {
                    if (hit.collider.gameObject.CompareTag("Walkable"))
                    {
                        curPlayer = Instantiate(player, hit.transform.position + new Vector3(0, ((player.transform.localScale.y) + (groundTiles[0].transform.localScale.y / 2)), 0), Quaternion.identity);
                        floorObjects.Add(curPlayer);
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
                        curPlayer = Instantiate(player, hit.transform.position + new Vector3(0, ((player.transform.localScale.y) + (groundTiles[0].transform.localScale.y / 2)), 0), Quaternion.identity);
                        floorObjects.Add(curPlayer);
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
                        curPlayer = Instantiate(player, hit.transform.position + new Vector3(0, ((player.transform.localScale.y) + (groundTiles[0].transform.localScale.y / 2)), 0), Quaternion.identity);
                        floorObjects.Add(curPlayer);
                    return;
                    }
            }

        }

    }
    void GroundObjectLayoutProbability()

    { foreach (GameObject tile in walkableFloor)
        {
            if (tile.transform.position.x == curPlayer.transform.position.x && tile.transform.position.z == curPlayer.transform.position.z)
            {
                
            }

            else
            {
                int a = Random.Range(0, 100);


                a -= groundObjectSpawning;

                if (a < 0)
                {
                    GroundObjectProbability(tile);

                }
            }
            
        }
    }
    void GroundObjectProbability(GameObject tile)
    {
        int sum = SumOfArray(groundObjectProbability);
        int b = Random.Range(0, sum);

        for (int i = 0; i < groundObjectProbability.Length; i++)
        {


            b -= groundObjectProbability[i];


            if (b < 0)
            {

                //floorObjects.Add(Instantiate(groundObject[i], new Vector3(tile.transform.position.x, (tile.transform.position.y + ((tile.transform.localScale.y / 2) + (groundObject[i].transform.localScale.y / 2))), tile.transform.position.z), Quaternion.identity));
                GameObject floorObj = Instantiate(groundObject[i], new Vector3(tile.transform.position.x, (tile.transform.position.y + (tile.transform.localScale.y / 2)), tile.transform.position.z), Quaternion.identity);
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

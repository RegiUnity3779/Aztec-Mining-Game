using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] List<GameObject> floorLayout = new List<GameObject>();
    [SerializeField] List<GameObject> walkableFloor = new List<GameObject>();
    [SerializeField] List<GameObject> stairSite = new List<GameObject>();
    List<GameObject> floorObjects = new List<GameObject>();

    public GameObject[] groundTiles;
    public GameObject[] groundObject;
    public Vector3[] stairTileLocations;
    public GameObject stairsUp;
    public GameObject stairsDown;
    public GameObject player;

    public int[] groundTilesProbability;
    public int[] landTilesProbability;
    public int groundObjectSpawning;
    public int[] groundObjectProbability;

    int mapSizeX = 10;
    int mapSizeZ = 10;


    private void OnEnable()
    {
        EventsManager.GroundObjectRemoved += GroundObjectRemoved;
        EventsManager.StairsRevealed += StairsRevealed;
    }

    private void OnDisable()
    {
        EventsManager.GroundObjectRemoved -= GroundObjectRemoved;
        EventsManager.StairsRevealed -= StairsRevealed;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateFloor();



    }
    public void GenerateFloor()
    {
        GroundLayout();
        GenerateStairs();
        GeneratePlayer();
        GroundObjectLayoutProbability();
    }

    public void DestroyFloor()
    {
        for(int i =0; i < floorObjects.Count; i++)
        {   
            Destroy(floorObjects[i].gameObject);
            floorObjects.Remove(floorObjects[i]);
            
            
        }

        //foreach(GameObject obj in floorObjects)
        //{
        //    floorObjects.Remove(obj);
        //    Destroy(obj);
        //}
        //foreach (GameObject obj in stairSite)
        //{
        //    stairSite.Remove(obj);
        //    Destroy(obj);
        //}
        //foreach(GameObject obj in floorLayout)
        //{
        //    floorLayout.Remove(obj);
        //    Destroy(obj);
        //}
        

    }

    void GroundLayout()
    {
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
            if (tile.CompareTag("Ground"))
            {
                walkableFloor.Add(tile);
            }
        }
    }
    void BoundaryLayout(int x, int z)
    {
        floorLayout.Add(Instantiate(groundTiles[1], new Vector3(x, -groundTiles[1].transform.localScale.y / 2, z), Quaternion.identity));
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
                if (tile.CompareTag("Ground"))
                {
                    return true;
                }
 
            }

            if (AdjacentZ == 1 || AdjacentZ == -1 && AdjacentX == 0)
            {
                if (tile.CompareTag("Ground"))
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
                if (tile.CompareTag("Ground"))
                {
                    floorLayout.Add(Instantiate(groundTiles[1], new Vector3(x, groundTiles[1].transform.localScale.y / 2, z), Quaternion.identity));
                    return;
                }

            }

            if (AdjacentZ == 1 || AdjacentZ == -1 && AdjacentX == 1 || AdjacentX == -1)
            {
                if (tile.CompareTag("Ground"))
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
                floorLayout.Add(Instantiate(groundTiles[i], new Vector3(x, -groundTiles[i].transform.localScale.y / 2, z), Quaternion.identity));
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
                floorLayout.Add(Instantiate(groundTiles[i], new Vector3(x, -groundTiles[i].transform.localScale.y / 2, z), Quaternion.identity));
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


            if (Physics.Raycast(stairTileLocations[0], transform.forward, out hit, 0.5f))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("Ground"))
                    {
                       floorObjects.Add(Instantiate(player, hit.transform.position + new Vector3(0, ((player.transform.localScale.y / 2) + (groundTiles[0].transform.localScale.y / 2)), 0), Quaternion.identity));
                        return;
                    }
                }
            }

            if (Physics.Raycast(stairTileLocations[0], -transform.forward, out hit, 0.5f))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("Ground"))
                    {
                    floorObjects.Add(Instantiate(player, hit.transform.position + new Vector3(0,((player.transform.localScale.y / 2)+ (groundTiles[0].transform.localScale.y / 2)), 0), Quaternion.identity));
                        return;
                    }
                }
            }

            if (Physics.Raycast(stairTileLocations[0], transform.right, out hit, 0.5f))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("Ground"))
                    {
                    floorObjects.Add(Instantiate(player, hit.transform.position + new Vector3(0, ((player.transform.localScale.y / 2) + (groundTiles[0].transform.localScale.y / 2)), 0), Quaternion.identity));
                        return;
                    }
                }
            }

            if (Physics.Raycast(stairTileLocations[0], -transform.right, out hit, 0.5f))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("Ground"))
                    {
                    floorObjects.Add(Instantiate(player, hit.transform.position + new Vector3(0,((player.transform.localScale.y / 2)+ (groundTiles[0].transform.localScale.y / 2)), 0), Quaternion.identity));
                        return;
                    }
                }
            }
     


            
    }
    void GroundObjectLayoutProbability()

    { foreach (GameObject tile in walkableFloor)
        {   
                    
            if (tile.tag == "Ground")
            {
            int a = Random.Range(0, 100);
           
                
                a -= groundObjectSpawning;

                if (a < 0)
                {
                    if(tile.transform.position.x == player.transform.position.x && tile.transform.position.z == player.transform.position.z) 
                    {
                    }
                    else
                    {
                        GroundObjectProbability(tile);
                    }
                    
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
                floorObjects.Add(Instantiate(groundObject[i], new Vector3(tile.transform.position.x, (tile.transform.position.y + ((tile.transform.localScale.y / 2) + (groundObject[i].transform.localScale.y / 2))), tile.transform.position.z), Quaternion.identity));
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
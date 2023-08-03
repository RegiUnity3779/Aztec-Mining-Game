using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
   public float offSetZ = 5f;
    public float smoothing = 2f;
    public GameObject player;


    private void OnEnable()
    {
       
        EventsManager.UpdateCamera += PlayerStartPosition;

    }

    private void OnDisable()
    {
       EventsManager.UpdateCamera -= PlayerStartPosition;

    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.cameraInstance == null)
        {
            GameManager.cameraInstance = this.gameObject;
            DontDestroyOnLoad(this);

        }

        else
        {
            Destroy(this.gameObject);
            return;
        }
        player = FindObjectOfType<PlayerController>().gameObject;
        PlayerStartPosition();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // Position the camera should be in

        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - offSetZ);
        //Vector3 targetRotate = 

        //Set the position accordingly

        // transform.position = targetPosition;

        // to smooth the camera more
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

    }
    void PlayerStartPosition()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - offSetZ);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkerIcon : MonoBehaviour
{
    private GameObject _gameObject;
    private void Start()
    {
        UpdateCamera();
    }


    // Update is called once per frame
 
    private void UpdateCamera()
    {
        // _gameObject = GameObject.Find("Main Camera");
        _gameObject = null;
        _gameObject = GameManager.cameraInstance;
        //_gameObject =Camera.main.gameObject;


    }
    void Update()
    {
        
        transform.rotation = Quaternion.LookRotation(transform.position - _gameObject.transform.position);
    }
}

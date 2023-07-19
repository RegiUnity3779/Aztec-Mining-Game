using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkerIcon : MonoBehaviour
{
    GameObject _gameObject;

    // Start is called before the first frame update
    void Start()
    {
        _gameObject = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _gameObject.transform.position);
    }
}

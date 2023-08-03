using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCanvasInstance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
 
        if (GameManager.inGameUIInstance == null)
        {
            GameManager.inGameUIInstance = this.gameObject;
            DontDestroyOnLoad(this);

        }

        else
        {
            Destroy(this.gameObject);
            return;
        }
    }




}

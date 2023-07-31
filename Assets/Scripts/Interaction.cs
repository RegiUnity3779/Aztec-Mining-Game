using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    bool coolDownFinished = true;

 
    //private void OnTriggerEnter(Collider other)
    //{

    //    EventsManager.Interactable(true, other.gameObject);

    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    EventsManager.Interactable(false, other.gameObject);

    //}

    //this needs to be updated as it currently keep up to date with what is in front of it when null. Perhaps change to raycast.
    private void OnTriggerStay(Collider other)
    {
        if (coolDownFinished)
        {
            EventsManager.Interactable(true, other.gameObject);
            StartCoroutine(DetectInteractable());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        EventsManager.Interactable(false, null);
    }

    IEnumerator DetectInteractable()
    {
        coolDownFinished = false;

        yield return new WaitForSeconds(1f);

        coolDownFinished = true;

    }
}

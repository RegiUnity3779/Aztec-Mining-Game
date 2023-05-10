using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EventsManager.Interactable(true, other.gameObject);

    }
    private void OnTriggerExit(Collider other)
    {
        EventsManager.Interactable(false, other.gameObject);

    }
}

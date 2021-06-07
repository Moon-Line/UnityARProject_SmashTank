using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crushing_Up : MonoBehaviour
{
    public GameObject buildingManager;

    public void OnCollisionEnter(Collision collision) {
        string otherLayer = LayerMask.LayerToName(collision.gameObject.layer);
        if (otherLayer == "Ground") {
            //buildingManager.gameObject.GetComponent<BuildingManager>().upCrush = true;
        }
    }
}
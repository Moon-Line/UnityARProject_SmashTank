using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager_Up : MonoBehaviour {
    public GameObject upBefore;
    public GameObject upAfter;
    public bool objectChange;

    // Start is called before the first frame update
    void Start() {
        upBefore.SetActive(true);
        upAfter.SetActive(false);

    }

    // Update is called once per frame
    void Update() {
        if (objectChange == true) {
            Invoke("TurnOff", 1f);

        }
    }
    void TurnOff() {
        upBefore.SetActive(false);
        if (upAfter != null) {
            upAfter.transform.position = upBefore.transform.position;
            upAfter.transform.rotation = upBefore.transform.rotation;
            upAfter.SetActive(true);
        }
    }
}

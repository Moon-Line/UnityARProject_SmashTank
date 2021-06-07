using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreenPosition : MonoBehaviour
{
    Vector3 screenPosition;

    void Start()
    {
        screenPosition = new Vector3(0, 0, 10);
    }

    void Update()
    {
        transform.localScale = new Vector3(Screen.width * 2, Screen.height * 2, 1);

        transform.localPosition = screenPosition;
    }
}

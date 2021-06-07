using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPosition : MonoBehaviour
{
    Vector3 textPosition;

    // Start is called before the first frame update
    void Start()
    {
        textPosition = new Vector3(0, 0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = textPosition;
    }
}

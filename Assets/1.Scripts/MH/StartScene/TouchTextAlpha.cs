using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchTextAlpha : MonoBehaviour
{
    Text text;

    float currentTime;
    float startTime = 2;
    public AnimationCurve ac;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        
        text = GetComponent<Text>();

        Color c = text.color;
        c.a = 0;
        text.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime < startTime) return;

        Color c =  text.color;
        c.a = ac.Evaluate(Time.time);
        text.color = c;
    }
}

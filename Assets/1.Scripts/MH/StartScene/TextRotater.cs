using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타이틀을 돌리고 싶다.

// 160 에 도착하면 왼쪽으로 돌아 200까지 가고
// 200 까지 가면 오른쪽으로 돌아 160까지 돌아가는 것을 반복한다.

public class TextRotater : MonoBehaviour
{
    Vector3 rotY;
    bool isLeftTurn;
    public float speed;

    float currentTime;
    float startTime = 1.3f;

    // Start is called before the first frame update
    void Start()
    {
        isLeftTurn = false;
        rotY = new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime < startTime) return;

        if (transform.localEulerAngles.y > 200f)
        {
            isLeftTurn = !isLeftTurn;
        }
        else if (transform.localEulerAngles.y < 160)
        {
            isLeftTurn = !isLeftTurn;
        }


        if (isLeftTurn)
        {
            transform.localEulerAngles += rotY * speed * Time.deltaTime;
        }
        else
        {
            transform.localEulerAngles -= rotY * speed * Time.deltaTime;
        }
    }
}

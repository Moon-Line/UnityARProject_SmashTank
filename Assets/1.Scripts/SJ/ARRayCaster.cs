using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

//Main Camera의 앞 방향으로 Ray를 쏴서 부딪힌 곳에 Indicator를 가져다 놓고 싶다.
//

public class ARRayCaster : MonoBehaviour {
    public GameObject indicator;
    GameObject foxFactory;
    ARRaycastManager arRayCastManager;

    // Start is called before the first frame update
    void Start() {
        foxFactory = Resources.Load<GameObject>("Fox");
        arRayCastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update() {
        //UpdateIndicator();
        UpdateARIndicator();
        UpdateTouch();
    }

    private void UpdateARIndicator() {
        Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
        //AR의 Raycast 기능을 이용해서 인디케이터를 그 위치에 가져다 놓고싶다.
        if(arRayCastManager.Raycast(center, hitResults)) {
            var hitInfo = hitResults[0].pose;
            indicator.transform.position = hitInfo.position;
        } else {
            indicator.SetActive(false);
        }
    }

    private void UpdateTouch() {
        //1. 만약 Touch를 했는데 
        if (Input.GetMouseButtonDown(0)) {
            //2. 터치한 곳에서 카메라의 앞방향으로 Ray를 만들고
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            int indicatorLayer = 1 << LayerMask.NameToLayer("Indicator");
            //3. 만약 부딪힌 Indicator가 있다면
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, indicatorLayer)) {
                //4. 그곳에 물체공장에서 물체를 하나 만들어서 배치하고 싶다.
                var fox = Instantiate(foxFactory);
                fox.transform.position = hitInfo.point;
            }
        }
    } 

    private void UpdateIndicator() {
        //1. 메인카메라의 앞방향으로 Ray를 만들고
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        int indicatorLayer = 1 << LayerMask.NameToLayer("Indicator");
        //2. 만약 바라봤는데 부딪힌 곳이 있다면, 단, Indicator는 바라보는 대상에서 제외해야 한다.
        RaycastHit hitInfo;
        //3. Indicator를 가져다 놓고싶다.
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, ~indicatorLayer)) {
            indicator.transform.position = hitInfo.point + hitInfo.normal * 0.001f;
        }
    }
}

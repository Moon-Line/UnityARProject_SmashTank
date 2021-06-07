using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

//Main Camera의 앞 방향으로 Ray를 쏴서 부딪힌 곳에 Indicator를 가져다 놓고 싶다.
//

public class ARRayCast_MH : MonoBehaviour
{
    public GameObject indicator;
    public GameObject field;
    ARRaycastManager arRayCastManager;

    Vector3 indicatorOrigin;

    float kAdjust = 1f;
    public Text text;

    public Scrollbar scroll;

    public bool isAR;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        //field = Resources.Load<GameObject>("Fox");
        arRayCastManager = GetComponent<ARRaycastManager>();
        indicatorOrigin = indicator.transform.localScale;
        scroll.value = 0.5f;
        isAR = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (indicator == null) return;
        if (isAR)
        {
            UpdateARIndicator();
        }
        else
        {
            UpdateIndicator();
        }
        UpdateTouch();

        kAdjust = scroll.value * 2;
        //text.text = $"{Math.Truncate(kAdjust * 100)/100}";

    }

    private void UpdateARIndicator()
    {
        Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

        //AR의 Raycast 기능을 이용해서 인디케이터를 그 위치에 가져다 놓고싶다.
        if (arRayCastManager.Raycast(center, hitResults))
        {
            var hitInfo = hitResults[0].pose;
            indicator.SetActive(true);
            indicator.transform.position = hitInfo.position;
            indicator.transform.localScale = indicatorOrigin * hitResults[0].distance * kAdjust;
        }
        else
        {
            indicator.SetActive(false);
        }
    }

    private void UpdateTouch()
    {
        // 1. 만약 화면을 터치를 했을 때
        if (Input.GetMouseButtonDown(0)) // AR 화면 터치까지 가능하다
        {
            // UI가 있으면 밑에는 실행을 안함
            if (EventSystem.current.IsPointerOverGameObject()) return; // PC에서 클릭 시

            if (Input.touchCount > 0)
            {    //터치가 1개 이상이면.
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (EventSystem.current.IsPointerOverGameObject(i)) return;
                }
            }

            // 2. 터치한 곳에서 카메라의 앞방향으로 Ray를 만들고
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;

            // 3. 그 시선을 이용해서 바라본 곳에 인디케이터가 있다면
            int indicatorLayer = 1 << LayerMask.NameToLayer("Indicator");

            if (Physics.Raycast(ray, out hitinfo, float.MaxValue, indicatorLayer))
            {
                // 4. 선택된 그 지점에 물체공장에서 물체를 1개 만들어서
                var obj = Instantiate(field);

                // 5. 그 위치에 물체를 가져다 놓고 싶다.
                //Vector3 objOrigin = indicatorOrigin;
                obj.transform.localScale = indicator.transform.localScale;
                obj.transform.position = indicator.transform.position;
                StartGame();
            }
        }
    }

    private void UpdateIndicator()
    {
        //1. 메인카메라의 앞방향으로 Ray를 만들고
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        //2. 만약 바라봤는데 부딪힌 곳이 있다면, 단, Indicator는 바라보는 대상에서 제외해야 한다.
        int indicatorLayer = 1 << LayerMask.NameToLayer("Indicator");
        //3. Indicator를 가져다 놓고싶다.
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, ~indicatorLayer))
        {
            indicator.transform.position = hitInfo.point + hitInfo.normal * 0.001f;
            indicator.SetActive(true);
            indicator.transform.localScale = indicatorOrigin;
            indicator.transform.localScale = indicator.transform.localScale * hitInfo.distance * kAdjust;
        }
        else
        {
            indicator.SetActive(false);
        }
    }

    //void OnClickMinus()
    //{
    //    kAdjust -= 0.1f;
    //}

    //void OnClickPlus()
    //{
    //    kAdjust += 0.1f;
    //}

    void StartGame()
    {
        Destroy(indicator.gameObject);
        scroll.gameObject.SetActive(false);
    }

}

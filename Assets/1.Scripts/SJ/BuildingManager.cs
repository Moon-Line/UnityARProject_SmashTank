using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    //public GameObject field;

    public GameObject building;
    public GameObject downBefore;
    public GameObject DownAfter;
    public bool downCrush;


    //public GameObject upBefore;
    //public GameObject upAfter;
    //public bool upCrush;

    void Start()
    {
        downBefore.SetActive(true);
        //upBefore.SetActive(true);
    }

    void Update()
    {
        if (downCrush == true)
        {
            GameObject downBuilding = Instantiate(DownAfter);
            downBuilding.transform.parent = building.transform;
            downBuilding.transform.localScale = downBefore.transform.localScale;
            downBuilding.transform.position = downBefore.transform.position;
            downCrush = false;
            downBefore.SetActive(false);
        }
        //if (upCrush == true)
        //{
        //    //떨어지는 효과를 위해 Invoke 사용
        //    Invoke("TurnOff", 0.5f);
        //    upCrush = false;
        //}
    }
    //void TurnOff()
    //{
    //    GameObject upBuilding = Instantiate(upAfter);
    //    upBuilding.transform.parent = building.transform;
    //    upBuilding.transform.localScale = upBefore.transform.localScale;
    //    upBuilding.transform.position = upBefore.transform.position;

    //    upBefore.SetActive(false);
    //}
}

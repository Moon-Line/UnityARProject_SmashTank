using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCrush_Up : MonoBehaviour {
    //자식 오브젝트들의 rigidbody를 가져와서
    //iskinematic을 True로 하고 
    //충돌시
    //iskine를 false로 바꾸고 싶다.
    public List<Rigidbody> rigidList = new List<Rigidbody>();
    public PaticleSy PS_buildingManager;
    void Start() {

        for (int i = 0; i < transform.childCount; i++) {
            rigidList.Add(transform.GetChild(i).gameObject.GetComponent<Rigidbody>());
            rigidList[i].isKinematic = true;
        }
    }

    //bool readyCrush = false;
    private void OnTriggerEnter(Collider other) {
        string otherLayer = LayerMask.LayerToName(other.gameObject.layer);
        if (otherLayer == "Player") {
            //충돌한 위치 정보를 가지고 오고 싶다.
            Ray ray = new Ray(other.transform.position, transform.position - other.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                //파티클을 실행해라
                PS_buildingManager.ParticleStart(hit.point);
            }

            for (int i = 0; i < rigidList.Count; i++) {
                rigidList[i].isKinematic = false;
            }

            gameObject.GetComponent<BoxCollider>().enabled = false;
        
            GameManager.instance.buildingCount++;

            Destroy(gameObject, 2);
        }
    }
}

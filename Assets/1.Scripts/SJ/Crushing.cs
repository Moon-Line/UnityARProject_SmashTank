using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crushing : MonoBehaviour
{
    public GameObject buildingManager;
    //public GameObject afterBuilding;

    public Quaternion rot;
    public Vector3 pos;
    ContactPoint contact;

    public void OnCollisionEnter(Collision collision) {
        string otherLayer = LayerMask.LayerToName(collision.gameObject.layer);
        if (otherLayer == "Player") {

            //충돌한 위치 정보를 가지고 오고 싶다.
            contact = collision.contacts[0];
            rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
            pos = contact.point;
            //충돌한 방향으로 물체를 회전하고 싶다.
            //afterBuilding.transform.rotation = rot; 
            //파티클을 실행해라
            buildingManager.GetComponent<PaticleSy>().ParticleStart(rot, pos);
            
            buildingManager.gameObject.GetComponent<BuildingManager>().downCrush = true;
        }
    }
}
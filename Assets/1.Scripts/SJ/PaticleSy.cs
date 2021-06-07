using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaticleSy : MonoBehaviour
{

    public GameObject particleFactory;
    public GameObject buildingBefore;
    public AudioSource bombSound;
    public GameObject fieldScale;

    public void ParticleStart(Quaternion crushRot, Vector3 crushPos) {
        GameObject pa = Instantiate(particleFactory, crushPos, crushRot);
        pa.transform.localScale = fieldScale.transform.localScale;
        pa.GetComponent<ParticleSystem>().Stop();
        pa.GetComponent<ParticleSystem>().Play();
        bombSound.Stop();
        bombSound.Play();
    }

    public void ParticleStart(Vector3 crushPos) {
        GameObject pa = Instantiate(particleFactory);
        pa.transform.position = crushPos;
        pa.transform.localScale = fieldScale.transform.localScale;
        pa.GetComponent<ParticleSystem>().Stop();
        pa.GetComponent<ParticleSystem>().Play();
        bombSound.Stop();
        bombSound.Play();
    }
}

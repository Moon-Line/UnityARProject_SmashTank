using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public Collider touchScreen;

    bool isGoNextScene = false;

    float currentTime;
    float startTime = 1.5f;

    void Start()
    {
        Time.timeScale = 1f;
        touchScreen.enabled = false;
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime < startTime) return;
        touchScreen.enabled = true;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            int layer = 1 << LayerMask.NameToLayer("TouchScreen");
            if (Physics.Raycast(ray, out hitinfo, float.MaxValue, layer))
            {
                if (!isGoNextScene)
                {
                    isGoNextScene = true;
                    Load_Scene();
                }
            }
        }
    }

    #region #LoadScene
    void Load_Scene()
    {
        FadeOut();
        StartCoroutine("IELoadScene");
    }

    IEnumerator IELoadScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("ARScene_MH");
    }

    #endregion

    public Image image_Fader;

    #region #FadeIn
    float time_FadeIn;
    public void FadeIn()
    {
        StartCoroutine("IEFadeIn");
    }

    IEnumerator IEFadeIn()
    {
        time_FadeIn = 1f;
        Color c = image_Fader.color;
        c.a = 1;
        image_Fader.color = c;

        float add = 0.01f;
        c = image_Fader.color;
        for (float a = 0; a <= time_FadeIn; a += add)
        {
            c.a = 1 - (a / time_FadeIn);
            image_Fader.color = c;
            yield return new WaitForSeconds(add);
        }
        c.a = 0;
        image_Fader.color = c;
    }
    #endregion
    #region #FadeOut
    float time_FadeOut;
    public void FadeOut()
    {
        StartCoroutine("IEFadeOut");
    }

    IEnumerator IEFadeOut()
    {
        time_FadeOut = 1f;
        Color c = image_Fader.color;
        c.a = 0;
        image_Fader.color = c;

        float add = 0.02f;
        c = image_Fader.color;
        for (float a = 0; a <= time_FadeOut; a += add)
        {
            c.a = a / time_FadeOut;
            image_Fader.color = c;
            yield return new WaitForSeconds(add);
        }
        c.a = 1;
        image_Fader.color = c;
    }
    #endregion
}

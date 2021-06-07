using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void OnClickStart() {
        SceneManager.LoadScene("StartScene");
    }
    public void OnClickQuit() {
        Application.Quit();
    }
}

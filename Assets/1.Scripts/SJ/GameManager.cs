using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public void Awake() {
        instance = this;
    }
    //빌딩 건물 관련 변수
    public int buildingCount;
    //public int targetCount;
    //PlayingUI
    public GameObject playingUI;
    public Text scoreText;
    public Text bestScore;
    int high;

    //FinishUI
    public GameObject finishUI;
    public Text finishScoreText;
    public Text finishBestScore;
    //타이머 관련 변수
    public int finishTime = 60;
    public Text timeText;
    float currentTime;

    void Start() {
        //targetCount = GameObject.Find("Building").transform.childCount;
        finishUI.SetActive(false);
        MinuteSecond();
        Score();

        //최고 점수를 가져와서 화면에 출력한다.
        high = PlayerPrefs.GetInt("BestScore");
        bestScore.text = $"Best : {high.ToString("00")}";

    }

    void Update() {
        Score();
        Timer();
        //GameFinish();
    }

    void Score() {
        scoreText.text = $"SCORE : {buildingCount.ToString("00")}";
        //최고 기록을 갱신했다면
        if (buildingCount > high) {
            PlayerPrefs.SetInt("BestScore", buildingCount);
            bestScore.text = $"BEST : {buildingCount.ToString("00")}";
        }

    }
    void Timer() {
        currentTime += Time.deltaTime;
        //타이머
        if (currentTime >= 1) {
            finishTime--;
            MinuteSecond();
            currentTime = 0;
        }
        //시간이 다 되면 게임을 멈추고 싶다.
        if (finishTime == 0) {
            Invoke("TimeStop", 1);
        }
    }
    void MinuteSecond() {
        int minute = finishTime / 60;
        int second = finishTime % 60;
        timeText.text = $"{minute}:{second.ToString("00")}";
    }
    //void GameFinish() {
    //    //빌딩이 다 부서지면 게임을 끝내고 싶다.
    //    if (buildingCount >= targetCount) {
    //        Invoke("TimeStop", 1);
    //    }
    //}
    void TimeStop() {
        Time.timeScale = 0;

        finishUI.SetActive(true);
        finishScoreText.text = scoreText.text;
        finishBestScore.text = bestScore.text;
    }
}

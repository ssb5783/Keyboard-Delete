using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Loading : MonoBehaviour
{
    // 비디오 객체
    public VideoPlayer videoPlayer;
    // 병렬 작업 활성화 변수 생성
    AsyncOperation asyncOperation;
    void Start()
    {
        //비동기 씬 로딩 시작 다음
        asyncOperation = SceneManager.LoadSceneAsync("SSB_Scene");

        //씬이 완전히 로드된 후 자동으로 전화되지 않도록 설정
        asyncOperation.allowSceneActivation = false;

    }

    void Update()
    {
        //비디오가 멈춘다면
        if (videoPlayer.isPaused)
        {
            // 다음씬 전환 허용
            asyncOperation.allowSceneActivation = true;

        }
    }
}

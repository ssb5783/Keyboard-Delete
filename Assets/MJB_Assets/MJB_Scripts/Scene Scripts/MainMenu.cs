using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // 게임 플레이 화면으로 이동하고 싶다
    public void PlayGame()
    {
        // 카메라 씬을 1초 뒤에 시작하고 싶다.
        SceneManager.LoadSceneAsync(1);
    }

    // 게임을 종료하고 싶다.
    public void QuitGame()
    {
        Application.Quit();
    }
}

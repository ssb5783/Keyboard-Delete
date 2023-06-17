using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임을 제 리스폰하고 싶다.
public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject completeGameUI;
    public static GameManager instance;
    public float restartTimeDelay = 4f;

    // 초기게임이 끝났는가?
    bool hasEndedGame = false;
    private void Awake()
    {
        instance = this;
    }

    //게임을 시작할 때 Active를 비활성화 하고싶다.
    void Start()
    {
        gameOverUI.SetActive(false);
        completeGameUI.SetActive(false);

    }

    //게임을 완료했을 때
    public void CompleteGame()
    {
        completeGameUI.SetActive(true);
    }

    //게임이 종료되었을 때
    public void GameOver()
    {
        // 게임이 종료된다면 
        if (hasEndedGame == false)
        {
            // 게임을 끝나지 않은 것으로 바꾸고
            hasEndedGame = true;
            // 게임 UI를 활성화 한다.
            gameOverUI.SetActive(true);

            // 죽은 횟수를 증가시킨다.
            DeathManager.instance.DEATH++;
            // 게임을 2초뒤에 재시작한다.
            Invoke("Respawn", restartTimeDelay);
        }
    }

    void Respawn()
    {
        // 씬매니저로 재시작하고 활성화 한다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 게임을 재시작하고 싶다.
    public void Restart()
    {
        // 현재 로드 씬을 다시 로드하고싶다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // 게임을 종료하고 싶다.
    public void Quit()
    {
        // 유니티 에디터가 플레이가 끝났다면 어플리케이션을 종료한다.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

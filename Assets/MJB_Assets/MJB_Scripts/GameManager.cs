using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}

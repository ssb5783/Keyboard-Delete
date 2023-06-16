using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 플레이어가 죽는 스코어를 0으로 시작해서 만들고 싶다.

public class DeathManager : MonoBehaviour
{
    // 키를 만든다,
    readonly string deathCountKey = "DEATH_COUNT_KEY";
    //유저의 그래픽을 관리하기 위해서 UGUI pro를 가져온다.
    public TextMeshProUGUI deathCount;
    //죽는 관리자를 패턴화한다.
    public static DeathManager instance;
    // 죽음 횟수를 만든다.
    int death;

    private void Awake()
    {
        //디자인 패턴화 한 것을 컴포넌트에 대입한다.
        instance = this;
    }
    void Start()
    {
        // 이 컴퓨터에서 죽은 횟수를 저장한 것을 가져오고싶다.
        DEATH = PlayerPrefs.GetInt(deathCountKey, 0);
    }

    //죽는 횟수 관리를 프로퍼티 한다.
    public int DEATH
    {
        // 죽은 횟수 값, 텍스처에 값을 넣는다.
        set
        {
            death = value;
            deathCount.text = "Your " + death + " th knight";
            // 데이터를 저장하고 싶다.
            PlayerPrefs.SetInt(deathCountKey, DEATH);

        }
        get
        {
            //죽은 횟수를 반환한다.
            return death;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 HP 파일을 하나로 지정해서 만들고 싶다.
public class PlayerHP : MonoBehaviour
{
    // 싱글톤 만들기
    public static PlayerHP instance;
    // hp 값
    int hp;
    // 최대 체력을 만들고 싶다.
    public int maxHP = 2;

    private void Awake()
    {
        instance = this;
    }

    // 게임을 시작할 때 체력을 넣고 싶다.
    void Start()
    {
        HP = maxHP;
    }

    // 체력 프로퍼티를 만들고싶다.
    public int HP
    {
        set
        {
            // 체력을 가져오고 싶다.
            hp = value;
            // 최대체력을 세팅한다
        }

        get
        {
            // 체력을 반환한다.
            return hp;
        }
    }

}

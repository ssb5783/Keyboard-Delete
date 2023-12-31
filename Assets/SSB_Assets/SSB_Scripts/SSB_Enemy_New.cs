﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB_Enemy_New : MonoBehaviour
{
    //state 구성 
    //Idle - 초기 상태
    //Move - 일정거리안에 있다면 플레이어 쪽으로 달려간다
    //Attack - 발차기를 한다 
    //Die - 래그돌

    //죽을 때 폭발효과 발생시키고 싶다.
    //필요속성 : 폭발효과 공장
    public GameObject explosionFactory;

    //무브 사운드를 넣고싶다
    AudioSource audiosource;

    //enemy가 죽었다면
    bool isdead = false;
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        FallDown,
        SetStateMove,
        Die,
        Roar,

    };

    public EnemyState m_state = EnemyState.Idle; //초기값 셋팅

    //애니메이션 넣기 -> 필요한 것 애니메이터 컨트롤러 -> 애니메이터
    Animator anim;

    private void Awake()
    {
        //오디오 소스 가져오기
        audiosource = GetComponent<AudioSource>();
    }
    void Start()
    {
        //캐릭터콘트롤러
        cc = GetComponent<CharacterController>();
        //애니메이터는 에너미의 자식 컴포넌트에 있다 그 중 젤 위에 애를 가져오는것
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //목차 만들기
        switch (m_state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.FallDown:
                FallDown();
                break;
            case EnemyState.SetStateMove:
                SetStateMove();
                break;
            case EnemyState.Die:
                Die();
                break;
            case EnemyState.Roar:
                Roar();
                break;
        }

        //현재 state print
        //print("state :" + m_state); 
    }
    //Idle에서 Move로 전환되는 애니메이션 처리를 하고싶다 -> 애니메이션 컨트롤러 필요
    //필요속성 : Animator Controller

    //플레이어와 일정거리 이상 가까워지면 상태를 이동으로 전환하고 싶다.
    //필요속성 : 타겟과의 거리, 일정거리, 타겟
    public Transform target;
    public float moveRange = 10;

    private void Idle()
    {
        //Idle - 초기 상태
        //플레이어와 일정거리 이상 가까워지면 상태를 이동으로 전환
        //1. 방향이 필요
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        if (distance < moveRange)
        {
            m_state = EnemyState.Move;

            //애니메이션 상태도 이동으로 전환
            anim.SetTrigger("Move");
        }


    }

    public float speed = 5; //이동속도
    CharacterController cc; //캐릭터 콘트롤러
    public float attackRange = 2; //공격범위

    private void Move()
    {
        //Move - 일정거리안에 있다면 플레이어 쪽으로 달려간다 -> 타겟이 공격범위안에 들어오면 상태를 공격으로 전환

        //타겟쪽 방향
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;//너와 나의 거리를 미리 저장해 둠
        dir.Normalize();
        dir.y = 0; //y값을 0으로 만들어서 눕지않도록 함
        //이동하고싶다
        cc.SimpleMove(dir * speed); //SimpleMove하면 점프불가
        //타겟방향으로 회전하고 싶다. -> Enemy의 방향을 dir로 하자
        //transform.forward = dir;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime); //10은 회전속도
                                                                                                                     //타겟이 공격범위 안에 들어오면 상태를 공격으로 전환

        if (distance < attackRange)
        {
            speed = 10; //속도를  빠르게 변화시키고
            cc.SimpleMove(dir * speed);
            m_state = EnemyState.Attack;
        }
    }

    //일정시간에 한번씩 공격하고싶다
    //필요속성 : 공격대기시간
    public float attackDelayTime = 2;
    float currentTime = 0;
    //float animTime = 3;

    public void Attack()
    {
        //일정시간에 한번씩 공격
        currentTime += Time.deltaTime;
        if (attackDelayTime > currentTime)
        {
            currentTime = 0;
            //print("attack!");
            //플레이어 방향으로 힘
            //애니메이션 넣기
            anim.SetTrigger("Attack");

            m_state = EnemyState.FallDown;
            anim.SetTrigger("FallDown");

            Vector3 dir = target.gameObject.transform.position - transform.position;
            dir.Normalize();
            target.gameObject.GetComponent<Rigidbody>().AddForce(dir * 3, ForceMode.Impulse);
            PlayerHP.instance.HP--;

        }

    }

    private void FallDown()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 4)
        {
            //print("FalllDown check");
            m_state = EnemyState.SetStateMove;
            anim.SetTrigger("Idle");
            currentTime = 0;
        }
    }

    void SetStateMove()
    {
        //2초 동안 누워있다가 일어난다
        currentTime += Time.deltaTime;
        if (currentTime > 4)
        {
            m_state = EnemyState.Move;
            anim.SetTrigger("Move");
            currentTime = 0;

        }
    }

    //2초뒤에 래그돌
    private void Die()
    {
        //Die - 래그돌
        //print("Die!");
        Vector3 dir = target.gameObject.transform.position - transform.position;
        dir.Normalize();
        gameObject.GetComponent<Rigidbody>().AddForce(-dir * 5, ForceMode.Impulse);
        GameObject explosion = Instantiate(explosionFactory);
        explosion.transform.position = transform.position;
        Destroy(explosion, 2);
        Destroy(gameObject, 3);
        //isdead = true;
        //if(this.isdead)
        //{
        //    audiosource = GetComponent<AudioSource>();
        //    audiosource.Stop();
        //}



    }

    private void Roar()
    {
        //Roar - 성공 시
        //print("Roar!");
        anim.SetTrigger("Roar");
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject);
        //충돌된게 치킨불렛이라면 래그돌 -> 죽음
        if (collision.gameObject.layer == LayerMask.NameToLayer("ChickenBullet"))
        {
            //사운드를 재생한다
            audiosource.Play();
            m_state = EnemyState.Die;
        }
        //충돌된게  Player라면 FallDown animation 재생 -> Move
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("충돌성공");
            PlayerHP.instance.HP--;//플레이어의 HP를 깎는다
            //상태를 Move로 전환한다
            m_state = EnemyState.FallDown;
            anim.SetTrigger("FallDown");
            Invoke("SetStateMove", 3);
            //Move 애니메이션
        }
        //충돌된게 Enemy라면 roar animation 재생 -> Move
        if (collision.gameObject.CompareTag("Enemy"))
        {

            print("Enemy끼리 부딪힘");
            anim.SetTrigger("FallDown");
            //상태를 Move로 전환한다
            m_state = EnemyState.FallDown;
        }
    }
}



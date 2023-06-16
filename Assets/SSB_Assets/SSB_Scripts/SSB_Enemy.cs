using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적이 유한한 상태를 갖도록 하고싶다.
//필요속성 : 상태정의
//애니메이션을 상태를 Move상태로 전환하고 싶다.
//필요속성 : Animator
public class SSB_Enemy : MonoBehaviour
{
    //필요속성 : 상태정의
    //필요속성 : 중력 계수
    public float gravity = -9.81f;

    enum EnemyState
    {
        Idle,
        Move,
        Attack1,
        FallDown1,
        Move2,
        Attack2,
        FallDown2,
        Roar,
        Bump,
        Die

    };
    EnemyState m_State = EnemyState.Idle; //초기값은 Idle이다.

    #region Idle 속성
    //필요속성 : 대기시간 경과시간
    public float idleDelayTime = 2;
    public float currentTime = 0;
    #endregion

    #region Move 속성
    //필요속성 : 이동속도 , 타겟, CharacterController
    public float speed = 5;
    public Transform target;
    CharacterController cc;
    #endregion

    //필요속성 : Animator
    Animator anim;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() //업데이트를 목차로 활용한다
    {
       

        print("state :" + m_State);
         switch(m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack1:
                Attack1();
                break;
            case EnemyState.FallDown1:
                FallDown1();
                break;
            case EnemyState.Move2:
                Move2();
                break;
            case EnemyState.Attack2:
                Attack2();
                break;
            case EnemyState.FallDown2:
                FallDown2();
                break;
            case EnemyState.Roar:
                Roar();
                break;
            case EnemyState.Bump:
                Bump();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }
    //일정시간이 흐르면 상태를 이동으로 전환하고 싶다.
   
    private void Idle()
    {
        //1.시간이 흐르고
        currentTime += Time.deltaTime;
        //2.경과시간이 대기시간을 초과하면
        if(currentTime > idleDelayTime)
        {
            //3.상태를 이동으로 전환한다
            m_State = EnemyState.Move;
            //시간 초기화
            currentTime = 0;
            //애니메이션의 상태를 Move로 전환하고 싶다
            //anim.SetTrigger("Move");
        }
    }
    //타겟쪽으로 이동하고싶다
    //타겟쪽으로 회전하고싶다
    //공격범위에 타겟이 들어온 상태오면 상태를 공격으로 전환하고 싶다
    //필요속성 : 공격범위
    public float attackRange = 2;
    private void Move()
    {
        //타겟이 일정거리 안에 들어왔을 때 타겟쪽으로 이동하고 싶다
        //1.방향이 필요
        //target - me
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        dir.y = 0; //하늘로 떠오르지 않게 하기
        //타겟쪽으로 회전하고 싶다
        transform.rotation = Quaternion.Lerp(transform.rotation , Quaternion.LookRotation(dir), 5 * Time.deltaTime); //부드럽게 회전하도록(from,to,%)
        //2.이동하고싶다
        cc.SimpleMove(dir * speed );

        //공격범위에 타겟이 들어오면 상태를 공격으로 전환하고싶다.
        if (distance < attackRange )
        {
            m_State = EnemyState.Attack1;
           // currentTime = attackDelayTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //HP를 깎는다.
        //Destroy(collision.gameObject);
        //Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    //일정시간에 한번씩 공격하고 싶다.
    //필요속성 : 공격대기시간
    //타겟이 공격범위를 벗어나면 상태를 이동으로 전환하고 싶다
    public float attackDelayTime = 2;
    private void Attack1()
    {
        //타겟쪽으로 이동하고 
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        dir.y = 0; //하늘로 떠오르지 않게 하기
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        //이동하고싶다
        cc.SimpleMove(dir * speed);
        currentTime = attackDelayTime;

        currentTime += Time.deltaTime;
        //현재시간이 공격타임을 초과하면 타겟쪽으로 이동하고 공격하고 싶다
        if(currentTime > attackDelayTime)
        {
            currentTime = 0;
          
            print("attack!!!");
            //공격 애니메이션을 재생한다.
            //anim.SetTrigger("Attack");
            //상태를 FalllDown1으로 전환한다.
            m_State = EnemyState.FallDown1;
        }
      
    }


    //멈추는 시간
    public float FallDownIdleTime;

    private void FallDown1()
    {
        //FallDown애니메이션을 재생하고싶다
        //anim.SetTrigger("FallDown");
        //시간이 흘러서
        currentTime += Time.deltaTime;
        //idleTime이 지나면
        if(currentTime > FallDownIdleTime)
        {
            //상태를 Move2로 전환하고 싶다.
            m_State = EnemyState.Move2;
            //시간을 0으로 바꾼다  
            currentTime = 0;
        }
    }

    private void Move2()
    {
        //타겟이 일정거리 안에 들어왔을 때 타겟쪽으로 이동하고 싶다
        //1.방향이 필요
        //target - me
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        dir.y = 0; //하늘로 떠오르지 않게 하기
        //타겟쪽으로 회전하고 싶다
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime); //부드럽게 회전하도록(from,to,%)
        //2.이동하고싶다
        cc.SimpleMove(dir * speed);
        //공격범위에 타겟이 들어오면 상태를 공격으로 전환하고싶다.
        if (distance < attackRange)
        {
           m_State = EnemyState.Attack2;
        }
    }

    private void Attack2()
    {
        //타겟쪽으로 이동하고
        Vector3 dir = target.transform.position - transform.position;
        //거리 미리 구해놓기
        float distance = dir.magnitude;
        //하늘로 떠오르지 않게 하기 
        dir.y = 0;
        //타겟쪽으로 회전하고 싶다
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);
        //이동하게 한다
        cc.SimpleMove(dir * speed);
        currentTime = attackDelayTime;

        currentTime += Time.deltaTime;
        //현재시간이 공격타임을 초과하면 타겟쪽으로 이동하고 공격하고 싶다
        if (currentTime > attackDelayTime)
        {
            currentTime = 0;

            print("attack!!!");
            //공격 애니메이션을 재생한다.
            //anim.SetTrigger("Attack");
            //상태를 FalllDown1으로 전환한다.
            m_State = EnemyState.FallDown2;
        }

    }

    private void FallDown2()
    {
        //FallDown애니메이션을 재생하고싶다
        //anim.SetTrigger("FallDown");
        //시간이 흘러서
        currentTime += Time.deltaTime;
        //idleTime이 지나면
        if (currentTime > FallDownIdleTime)
        {
            //상태를 Move2로 전환하고 싶다.
            m_State = EnemyState.Roar;
            //시간을 0으로 바꾼다  
            currentTime = 0;
        }
    }

    private void Roar()
    {
       
    }

    private void Bump()
    {
       
    }

    //피격은 언제든지 일어날 수 있다.
    //피격당했을 때 호출되는 이벤트 함수
    //3대 맞으면 죽도록 처리하자
    public int hp = 3;
    public void OnDamageProcess()
    {
        //3대 맞으면
        hp--;
        if (hp <= 0)
        {
            //죽도록 처리하자
            m_State = EnemyState.Die;
            //충돌체 꺼버리자
            cc.enabled = false;
        }
        //시간 초기화
        currentTime = 0;
    }

    public float dieSpeed = 0.5f;
    private void Die()
    {

        //아래로 사라지도록 하자
        transform.position += Vector3.down * dieSpeed * Time.deltaTime;
        //완전히 사라지면 제거 -3
        if(transform.position.y < -3)
        {
            Destroy(gameObject);
        }
    }
}

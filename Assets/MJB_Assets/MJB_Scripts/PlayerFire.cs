using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용자 발사 방향으로 불렛을 날리고 싶다.
// 불렛 공장을 만든다.
// 불렛을 발사 방향의 앞으로 만든다.
// 발사 속도를 제어하고 싶다.
// 3인칭 이동과 카메라를 만들고 싶다.
public class PlayerFire : MonoBehaviour
{
    //불렛 공장
    public GameObject chickenBulletFactory;
    //발사 물체
    public Transform firePosition;
    // 에니메이션 컴포넌트를 가져오고싶다.
    private Animator animator;

    // 발사 간격을 2초로 하고 싶다.
    // 현재 시간
    float currentTime;
    // 발사 지연 시간
    public float fireDelayTime = 1f;

    // 발사를 했는가?
    bool isFiring;

    private void Awake()
    {
        // 상위 애니메이션 컴포넌트를 가져온다.
        animator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        // 처음에는 발사할 수 있게 한다.
        currentTime = fireDelayTime;
    }

    // Update is called once per frame
    void Update()
    {
        initInput();
        Fire();
    }

    void initInput()
    {
        // 발사를 했을 때 변수를 초기화한다.
        isFiring = Input.GetButtonDown("Fire1");
    }

    void Fire()
    {
        // 2초 시간이 지나면 불렛을 발사하도록 하고싶다.
        currentTime += Time.deltaTime;
        // 2초가 넘으면 불렛을 생성하고 발사한다. 그리고 발사하지 않았을 때
        if (currentTime > fireDelayTime)
        {
            // 왼쪽 마우스를 누르면
            if (isFiring)
            {
                MakeChickenBullet();

                // 던질때 애니메이션이 발생한다.
                animator.SetTrigger("isThrowing");

                // 발사 시간을 초기화한다.
                currentTime = 0;
            }
        }
    }

    // 불렛을 2초 마다 생성해서 딜레이를 주어 생성하고 싶다.
    void MakeChickenBullet()
    {
        // 1.불렛을 게임오브젝트로 만든다
        GameObject chickenBullet = Instantiate(chickenBulletFactory);

        // 2.치킨 불렛을 발사 위치로 지정한다.
        chickenBullet.transform.position = firePosition.transform.position;
    }
}

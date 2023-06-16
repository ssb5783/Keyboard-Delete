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
    // 발사를 했는가?
    bool isFiring;

    private void Awake()
    {
        // 상위 애니메이션 컴포넌트를 가져온다.
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        initInput();
        // 발사 간격을 2초로 하고싶다.
        Fire();
    }

    void initInput()
    {
        // 발사를 했을 때 변수를 초기화한다.
        isFiring = Input.GetButtonDown("Fire1");
    }

    void Fire()
    {
        // 왼쪽 마우스를 누르면
        if (isFiring)
        {
            // 1.불렛을 게임오브젝트로 만든다
            GameObject chickenBullet = Instantiate(chickenBulletFactory);

            // 2.치킨 불렛을 발사 위치로 지정한다.
            chickenBullet.transform.position = firePosition.transform.position;

            // 3.생성된 bullet의 물체를 가져온다.
            Rigidbody bulletRigid = chickenBullet.GetComponent<Rigidbody>();

            // 던질때 애니메이션이 발생한다.
            animator.SetTrigger("isThrowing");

            // 불렛의 속도를 발사 앞 방향의 속도로 한다.
            bulletRigid.velocity = firePosition.forward * 25;
        }
    }
}

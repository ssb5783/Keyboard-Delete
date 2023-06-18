using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 불렛이 땅에 있거나 벽에 부딪히면 삭제시키고 싶다.
// 불렛이 다으면 그 힘 만큼 힘을주고 싶다.
// 불렛이 생성되면 앞으로 힘을주고 싶다.
public class ChickenBullet : MonoBehaviour
{
    // 불렛으로 얼마나 데미지를 주는가
    public int damage;
    public float power = 10000f;
    // 불렛의 속도
    public float speed = 2f;

    // 불렛의 속도 값을 가져오고 싶다.
    private Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
    }
    private void Start()
    {
        // 카메라 방향으로 던지고 싶다.
        rigid.AddForce(Camera.main.gameObject.transform.forward * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 땅의 테그 충돌이 발생시키면 3초뒤에 물체를 삭제한다.
        // 1. floor 태그라면
        if (collision.gameObject.tag == "Floor")
        {
            // 2. 3초뒤에 물체를 삭제한다.
            Destroy(gameObject, 4);
        }

        // 충돌 물체에게 불렛의 속도 만큼 파워를 줘서 넛백 시키고 싶다.
        collision.rigidbody.AddForce(rigid.velocity * power, ForceMode.VelocityChange);
    }
}

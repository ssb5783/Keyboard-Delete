using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB_JumpTrapFloor : MonoBehaviour
{
    //리지드바디
    Rigidbody rb;
    //스피드
    public float speed = 10;
    //현재시간
    //float currentTime = 0;
    //특정시간
    public float creatTime = 3;
    //올라갔는지
    bool isUp;
    //타겟점프파워
    public float JumpPower = 30;

    Quaternion targetRotation;

    Vector3 origin;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = Quaternion.identity; //identity는 0,0,0
        origin = transform.position;//첫번째 position
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = origin;//첫번째 포지션을 기억한다
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * 5);

        if (isUp && transform.localEulerAngles.x >= 29.5f)
        {
            targetRotation = Quaternion.identity;
            isUp = false;
        }
    }

    void MoterOff()
    {
    }

    //닿았을 때 x축 방향으로 회전한다
    private void OnCollisionEnter(Collision collision)
    {

        collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * JumpPower;  
        isUp = true;
        targetRotation = Quaternion.Euler(30, 0, 0);

        ////시간이 흐르고
        //currentTime += Time.deltaTime;
        ////앞 방향으로 계속 가게 하기
        ////rb.AddForce(transform.forward* speed, ForceMode.Impulse);
        ////x축 방향으로 날리기
        //rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(80,0,0) * speed * Time.deltaTime));
        ////현재시간이 특정시간을 지나면

        ////충돌한 맞은 지점
        ////맞은 오브젝트의 레이어가 Plane이 아니라면
        //if(collision.gameObject.layer !=20)
        //{
        //Vector3 hitpoint = collision.contacts[0].point;
        //rb.AddForce(hitpoint , ForceMode.Impulse);

        //}
    }
}

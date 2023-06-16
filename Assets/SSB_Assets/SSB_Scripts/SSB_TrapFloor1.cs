using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB_TrapFloor1 : MonoBehaviour
{
    //리지드바디
    Rigidbody rg;
    //콜라이더
    Collider co;
    //현재시간
    float currentTime = 0;
    //특정시간
    public float creatTime = 2;
    //닿았는지
    bool touch = false;
    // Start is called before the first frame update
    void Start()
    {

        co = GetComponentInChildren<Collider>();
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //만약 닿으면
        if (touch)
        {
            //회전을 시킨다
            transform.Rotate(Vector3.right * Time.deltaTime * 50);
        }

        //print(transform.rotation.x);

        if (transform.rotation.x < -0.8f)
        {
            //닿는걸 false로 만든다
            touch = false;
            currentTime += Time.deltaTime;
        }
        
        //3초가 지나면
        if (currentTime > 3)
        {   //각도가 변한다.
            transform.Rotate(-Vector3.right * Time.deltaTime * 50);

            //만약 각도가 0도가 되면
            if(transform.rotation.x >= 0)
            {
                //시간을 0으로 만든다
                currentTime = 0;
               
            }
        }
    }


    //충돌하면 현재시간이 특정시간을 초과했을때


    //충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        {
            //만약에 충돌한 물체가 플레이어라면
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "enemy")
            {
                //플레이어와 닿았다
                touch = true;
                //x축을 50도로 돌린다
                //rg.MoveRotation(rg.rotation * Quaternion.Euler(new Vector3(-50,0,0)*50*Time.deltaTime)); 

            }

            
        }
    }
}

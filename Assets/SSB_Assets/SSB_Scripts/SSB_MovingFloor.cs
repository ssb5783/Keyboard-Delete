using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//3초 뒤에 x축 방향으로 10만큼 이동하고 싶다
public class SSB_MovingFloor : MonoBehaviour
{

    //필요속성 : 이동속도 특정시간 현재시간
    public float speed = 10;
    public float FirstTime = 3;
    public float SecondTime = 6;
    float currentTime;

    public Transform lower;
    public Transform upper;

    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //3초 뒤에 x축 방향으로 10만큼 이동하고 싶다
        //1.시간이 흐르고
        currentTime += Time.deltaTime;
        
        //2.현재시간이 3초보다 작으면
        if(currentTime < FirstTime)
        {
            //3.LowerPosition까지 Lerp로 이동한다.
            transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, lower.localPosition, 0.05f);
        }
        //4.현재시간이 두번째시간을 지나면
        else if(currentTime < SecondTime)
        {
            //5.처음 자리로 이동한다.
            transform.localPosition = Vector3.Lerp(transform.localPosition, upper.localPosition, 0.05f);
        }
        else
        {
            currentTime = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>();
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}

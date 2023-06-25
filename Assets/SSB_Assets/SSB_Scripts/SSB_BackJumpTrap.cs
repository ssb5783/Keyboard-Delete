using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB_BackJumpTrap : MonoBehaviour
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

    Quaternion targetRotation;

    Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = Quaternion.identity;
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = origin;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * 5);

        if(isUp && transform.localEulerAngles.z >= -29.5f)
        {
            targetRotation = Quaternion.identity;
            isUp = false;
        }
    }

  
    
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 10 + Vector3.right * 9; //맞은 상대방
        isUp = true;
        targetRotation = Quaternion.Euler(0, 0, -30);
        
    }
}

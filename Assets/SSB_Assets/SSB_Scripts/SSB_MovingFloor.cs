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
    public Transform MovingFloorReach;
    Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //3초 뒤에 x축 방향으로 10만큼 이동하고 싶다
        //1.시간이 흐르고
        currentTime += Time.deltaTime;
        //2.현재시간이 첫번째시간을 지나면
        if(FirstTime < currentTime)
        {
            //3.MovingFloorReach까지 Lerp로 이동한다.
            transform.position = Vector3.Lerp(gameObject.transform.position, MovingFloorReach.position, 0.05f);
        }
        //4.현재시간이 두번째시간을 지나면
        if(SecondTime < currentTime)
        {
            //5.처음 자리로 이동한다.
            transform.position = Vector3.Lerp(MovingFloorReach.position, origin, 0.05f);

        }
        
    }
}

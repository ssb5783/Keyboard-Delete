using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB_RockGOD : MonoBehaviour
{
    //일정시간이 지나면 프리팹 ROCK을 불러와서 배치하고싶다
    //현재시간
    float currentTime;
    //일정시간
    float creatTime;
    //바위
    public GameObject rockFactory;
    
    void Start()
    {
        creatTime = Random.Range(1, 6);
        
    }

    // Update is called once per frame
    void Update()
    {
        //print(creatTime);
        currentTime += Time.deltaTime;
        //일정시간이 지나면 프리팹 ROCK을 불러와서 배치하고싶다
        if(currentTime > creatTime)
        {
             GameObject rock = Instantiate(rockFactory);
            rock.transform.position = transform.position;
            currentTime = 0;
            creatTime = Random.Range(1, 6);
        }
    }
}

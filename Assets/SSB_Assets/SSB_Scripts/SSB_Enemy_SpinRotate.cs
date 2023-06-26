using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB_Enemy_SpinRotate : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    public float followDistance = 5; //따라가는 거리
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 타겟과의 거리 계산
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= followDistance)
        {
            Vector3 dir = target.position - transform.position;
            dir.Normalize();

            transform.position += dir * speed * Time.deltaTime;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB_Rock : MonoBehaviour
{
    //없어질 때 폭발효과 발생시키고 싶다.
    //필요속성 : 폭발효과 공장
    public GameObject explosionFactory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject explosion = Instantiate(explosionFactory);

        currentTime += Time.deltaTime;
        if (currentTime > 2.5f)
        {
            explosion.transform.position = transform.position;
        }
    }

    float currentTime = 0;
    private void OnCollisionEnter(Collision collision)
    {
         
        Destroy(gameObject, 3);
    }
}

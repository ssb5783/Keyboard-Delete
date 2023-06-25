using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//부딪힌 상대를 3초 뒤에 없앤다.
public class SSB_FirstState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other);
        Destroy(other.gameObject);
    }
}

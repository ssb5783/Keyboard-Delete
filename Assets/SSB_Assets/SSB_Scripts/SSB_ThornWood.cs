using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSB_ThornWood : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 500;
    Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        origin = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        print(rb.velocity);
        transform.position = origin;//첫번째 포지션을 기억한다
        rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision);
        Destroy(collision.gameObject);
        
    }
}

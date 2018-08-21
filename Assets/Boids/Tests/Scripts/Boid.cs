using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boid : MonoBehaviour {
    Rigidbody rb;
    public float moveSpeed = 10.0f; 

    private Vector3 pos = new Vector3(0, 0, 0);

    

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        RandomDirection();
        //InvokeRepeating("ChangeDirection", 1, 1);
    }
    void RandomDirection()
    {
        var euler = transform.eulerAngles;
        euler.x = Random.Range(euler.x - 360, euler.x + 360);
        euler.y = Random.Range(euler.y - 360, euler.y + 360);
        euler.z = Random.Range(euler.z - 360, euler.z + 360);
        transform.eulerAngles = euler;
    }
    void FixedUpdate()
    {
        rb.velocity = transform.forward * moveSpeed;
    }
}

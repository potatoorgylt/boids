﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    // The target marker.
    public Transform target;

    // Speed in units per sec.
    public float speed;

    PointToObject point;
    private void Start()
    {
        point = Camera.main.GetComponent<PointToObject>();
    }
    void Update()
    {
        //target = point;
        // The step size is equal to speed times frame time.
        float step = speed * Time.deltaTime;

        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
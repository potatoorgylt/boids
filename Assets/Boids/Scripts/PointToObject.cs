using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToObject : MonoBehaviour
{
    //[HideInInspector]
    public Vector3 targetPos;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                Debug.DrawLine(ray.origin, hit.point);
                targetPos = hit.point;
                Debug.Log("targetPos: " + targetPos);
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.GetPoint(100), Color.red);
                targetPos = ray.GetPoint(100);
                Debug.Log("targetPos: " + targetPos);
            }
        }
    }
}

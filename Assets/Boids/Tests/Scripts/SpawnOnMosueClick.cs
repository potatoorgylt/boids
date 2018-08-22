using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnMosueClick : MonoBehaviour {

    public GameObject spawnObject;
    public float spawnOffset;
    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(0, 0, spawnOffset);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            GameObject clone;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                Debug.DrawLine(ray.origin, hit.point);
                clone = Instantiate(spawnObject, hit.point, transform.rotation);
            }
            else
            {
                Debug.Log("Empty field");
                Debug.DrawLine(ray.origin, ray.GetPoint(100), Color.red);
                clone = Instantiate(spawnObject, ray.GetPoint(100), transform.rotation);
            }
        }
    }
}

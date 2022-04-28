using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    public float moveSpeed;
    public float orbitDist;

    public GameObject render;

    public Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector3(0, 0.75f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        pos.x = Mathf.Cos(Time.time*moveSpeed);
        pos.z = Mathf.Sin(Time.time*moveSpeed);

        gameObject.transform.position = pos * orbitDist/2;

        gameObject.transform.LookAt(Vector3.zero);
    }
}

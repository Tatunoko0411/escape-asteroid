using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour
{
    Rigidbody rb;
    Vector3 force = new Vector3(-100.0f, 100.0f, 0);
    // Start is called before the first frame update
    void Start()
    {
         rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 80.0f * Time.deltaTime, 0f);
    }

    public void GameStart()
    {
        rb.AddForce(force);
    }
}

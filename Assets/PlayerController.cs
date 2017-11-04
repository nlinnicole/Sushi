﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    GameObject mainCamera;

    [SerializeField]
    float speed = 0.0f;

    private Vector3 forward;
    private Vector3 right;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
            Move();
        }
	}

    private void Move()
    {
        if (mainCamera)
        {
            forward = mainCamera.transform.forward;
            forward.y = 0.0f;
            right = Quaternion.Euler(new Vector3(0,90,0)) * forward;
        }

        Vector3 rightMov = right * speed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 forwardMov = forward * speed * Time.deltaTime * Input.GetAxis("Vertical");
        Vector3 movement = rightMov + forwardMov;

        transform.forward = Vector3.Normalize(movement);

        Debug.DrawRay(transform.position, transform.forward, Color.green, 0.2f);
        transform.position += movement;
    }
}
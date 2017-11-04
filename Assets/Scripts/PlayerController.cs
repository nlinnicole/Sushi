using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    GameObject mainCamera;

    [SerializeField]
    float speed = 0.0f;

    private Vector3 forward;
    private Vector3 right;

    private float speedBuff;
    private float speedBuffTimer;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
            Move();
        }
	}

    private void FixedUpdate()
    {
        UpdateSpeed();
    }

    void IncreaseSpeed(float increment, float timer)
    {
        speedBuff = increment;
        speedBuffTimer = timer;
    }

    private void Move()
    {
        if (mainCamera)
        {
            forward = mainCamera.transform.forward;
            forward.y = 0.0f;
            right = Quaternion.Euler(new Vector3(0,90,0)) * forward;
        }

        float actualSpeed = speed + speedBuff;

        Vector3 rightMov = right * actualSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 forwardMov = forward * actualSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        Vector3 movement = rightMov + forwardMov;

        transform.forward = Vector3.Normalize(movement);

        Debug.DrawRay(transform.position, transform.forward, Color.green, 0.2f);
        transform.position += movement;
    }

    private void UpdateSpeed()
    {
        if (speedBuffTimer <= 0.0f)
        {
            speedBuff = 0.0f;
            speedBuffTimer = 0.0f;
        }

        if (speedBuff > 0.0f && speedBuffTimer > 0.0f)
        {
            speedBuffTimer -= Time.deltaTime;
        }
    }
}

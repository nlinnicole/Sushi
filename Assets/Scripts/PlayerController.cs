using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject mainCamera;
    [SerializeField]
    GameObject field;
    [SerializeField]
    GameObject BuffController;

    [SerializeField]
    float speed = 0.0f;

    private Vector3 _forward;
    private Vector3 _right;
    private float _speedBuff;
    private float _speedBuffTimer;
    private BuffController _buffController;
    private bool _allowInput = true;

    // Use this for initialization
    void Start()
    {
        _buffController = BuffController.GetComponent<BuffController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        UpdateSpeed();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "WasabiBuff":
                _speedBuff = _buffController.buffSpeed;
                _speedBuffTimer = _buffController.wasabiBuffSpeedTimer;
                break;
            case "SoySauceBuff":
                _speedBuff = _buffController.buffSpeed;
                _speedBuffTimer = Mathf.Infinity;
                _allowInput = false;
                break;
            default:
                return;
        }
    }

    private void CheckBounds(Vector3 movement)
    {
        Vector3 newPosition = transform.position + movement;

        if (field.GetComponent<Renderer>().bounds.Contains(new Vector3(newPosition.x, field.transform.position.y, newPosition.z)))
        {
            transform.position = newPosition;
        }
        else
        {
            _allowInput = true;
            _speedBuffTimer = 0.0f;
        }
    }

    private void Move()
    {
        if (mainCamera)
        {
            _forward = mainCamera.transform.forward;
            _forward.y = 0.0f;
            _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
        }

        float actualSpeed = speed + _speedBuff;
        Vector3 movement = Vector3.zero;

        if (!_allowInput)
        {
            movement = transform.forward * actualSpeed * Time.deltaTime;
        }
        else if (_allowInput && Input.anyKey)
        {
            Vector3 rightMov = _right * actualSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
            Vector3 forwardMov = _forward * actualSpeed * Time.deltaTime * Input.GetAxis("Vertical");
            movement = rightMov + forwardMov;
        }

        if (Vector3.SqrMagnitude(movement) != 0.0f)
        {
            transform.forward = Vector3.Normalize(movement);
            CheckBounds(movement);
        }
    }

    private void UpdateSpeed()
    {
        if (_speedBuffTimer <= 0.0f)
        {
            _speedBuff = 0.0f;
            _speedBuffTimer = 0.0f;
        }

        if (_speedBuff > 0.0f && _speedBuffTimer > 0.0f)
        {
            _speedBuffTimer -= Time.deltaTime;
        }
    }
}

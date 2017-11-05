using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopstickSpearController : MonoBehaviour
{
    [SerializeField]
    float chopstickSpeed = 0.2f;
    [SerializeField]
    float lifetime = 3f;
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(DestroyCountDown());
    }

    void FixedUpdate()
    {
        transform.position += transform.up * chopstickSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        player.GetComponent<PlayerController>().CollectItem(other.gameObject.tag);
    }

    private IEnumerator DestroyCountDown()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}

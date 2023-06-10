using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    Vector3 camOffset;
    Transform player;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        camOffset = player.position - transform.position;
    }
    void Update()
    {
        transform.position = player.position - camOffset;
    }
}

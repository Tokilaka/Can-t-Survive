using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, 1f);
    }
}

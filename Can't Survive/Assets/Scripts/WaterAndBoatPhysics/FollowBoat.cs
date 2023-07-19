using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBoat : MonoBehaviour
{
    [SerializeField] Transform boat;
    void Update()
    {
        transform.SetPositionAndRotation(boat.position, boat.rotation); 
    }
}

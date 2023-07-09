using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    [SerializeField] Transform[] floaters;
    [SerializeField] float underWaterDrag = 3f;
    [SerializeField] float underWaterAngularDrag = 1f;
    [SerializeField] float airDrag = 0f;
    [SerializeField] float airAngularDrag = 0.05f;
    [SerializeField] float floatingPower = 50f;
    [SerializeField] Transform waterHeight;

    Rigidbody rb;
    bool underWater;
    int floatersUnderWater;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        floatersUnderWater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float difference = floaters[i].position.y - waterHeight.position.y;
            if (difference < 0)
            {
                rb.AddForceAtPosition(floatingPower * Mathf.Abs(difference) * Vector3.up, floaters[i].position, ForceMode.Force);
                floatersUnderWater++;

                if (!underWater)
                {
                    underWater = true;
                    SwitchState(true);
                }
            }
        }

        if (underWater && floatersUnderWater == 0)
        {
            underWater = false;
            SwitchState(false);
        }
    }

    void SwitchState(bool underWater)
    {
        if (underWater)
        {
            rb.drag = underWaterDrag;
            rb.angularDrag = underWaterAngularDrag;
        }
        else
        {
            rb.drag = airDrag;
            rb.angularDrag = airAngularDrag;
        }
    }
}

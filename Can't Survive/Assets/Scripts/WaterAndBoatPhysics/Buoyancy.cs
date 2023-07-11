using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    [SerializeField] Transform[] Floaters;
    [SerializeField] float UnderWaterDrag = 3f;
    [SerializeField] float UnderWaterAngularDrag = 1f;
    [SerializeField] float AirDrag = 0f;
    [SerializeField] float AirAngularDrag = 0.05f;
    [SerializeField] float FloatingPower = 15f;

    WaterManager Water;
    Rigidbody Rb;
    bool Underwater;
    int FloatersUnderWater;
    // Start is called before the first frame update
    void Start()
    {
        Rb = this.GetComponent<Rigidbody>();
        Water = FindObjectOfType<WaterManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FloatersUnderWater = 0;
        for (int i = 0; i < Floaters.Length; i++)
        {
            float diff = Floaters[i].position.y - Water.WaterAtHeightPosition(Floaters[i].position);
            if (diff < 0)
            {
                Rb.AddForceAtPosition(Vector3.up * FloatingPower * Mathf.Abs(diff), Floaters[i].position, ForceMode.Force);
                FloatersUnderWater += 1;
                if (!Underwater)
                {
                    Underwater = true;
                    SwitchState(true);
                }
            }
        }
        if (Underwater && FloatersUnderWater == 0)
        {
            Underwater = false;
            SwitchState(false);
        }
    }
    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            Rb.drag = UnderWaterDrag;
            Rb.angularDrag = UnderWaterAngularDrag;
        }
        else
        {
            Rb.drag = AirDrag;
            Rb.angularDrag = AirAngularDrag;
        }
    }
}

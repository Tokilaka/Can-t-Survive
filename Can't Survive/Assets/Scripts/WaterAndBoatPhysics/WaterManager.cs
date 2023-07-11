using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class WaterManager : MonoBehaviour
{
    [SerializeField] float waveHeight = 1.0f;
    [SerializeField] float waveFrequency = 2.1111f;
    [SerializeField] float waveSpeed = 0.5f;
    [SerializeField] Transform water;

    Material waterMaterial;
    Texture2D waterTexture;

    private void Start()
    {
        SetVariables();
    }

    void SetVariables()
    {
        waterMaterial = water.GetComponent<Renderer>().sharedMaterial;
        waterTexture = (Texture2D)waterMaterial.GetTexture("_WavesDisplacement");
    }

    public float WaterAtHeightPosition(Vector3 position)
    {
        return water.position.y + waterTexture.GetPixelBilinear(position.x * waveFrequency * water.localScale.x, (position.z * waveFrequency + Time.time * waveSpeed) * water.localScale.z).g * waveHeight;
    }


    private void OnValidate()
    {
        if (!waterMaterial)
            SetVariables();
        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        waterMaterial.SetFloat("_Waves_Height", waveHeight);
        waterMaterial.SetFloat("_Waves_Speed", waveSpeed);
        waterMaterial.SetFloat("_WavesFrequency", waveFrequency);
    }
}

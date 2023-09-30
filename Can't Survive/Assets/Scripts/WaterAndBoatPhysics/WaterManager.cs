using UnityEngine;
public class WaterManager : MonoBehaviour
{
    [SerializeField] float waveFrequency = 1f;
    [SerializeField] float waveHeight = 4f;
    [SerializeField] float waveSpeed = 2f;
    [SerializeField] Transform water;

    Material waterMat;
    Texture2D waveDisplacement;

    private void Start()
    {
        SetVariables();
    }

    void SetVariables()
    {
        waterMat = water.GetComponent<Renderer>().sharedMaterial;
        waveDisplacement = (Texture2D)waterMat.GetTexture("_WaveDisplacement");
    }

    public float WaterAtHeightPosition(Vector3 position)
    {
        return water.position.y + waveDisplacement.GetPixelBilinear(position.x * (waveFrequency/10) * water.localScale.x, (position.z * (waveFrequency/10) + Time.time * (waveSpeed/10)) * water.localScale.z).g * waveHeight;
    }


    private void OnValidate()
    {
        if(!waterMat)
            SetVariables();

        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        waterMat.SetFloat("_Waves_Speed", waveSpeed/10);
        waterMat.SetFloat("_WavesFrequency", waveFrequency/10);
        waterMat.SetFloat("_WavesHeight", waveHeight);
    }
}

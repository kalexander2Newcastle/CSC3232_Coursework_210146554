using UnityEngine;

public class DynamicGlow : MonoBehaviour
{
    public Color glowColor = Color.yellow;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    public float pulseSpeed = 2f;

    private Material starMaterial;
    private float currentIntensity;
    private bool increasing = true;

    /// <summary>
    /// The start method gets the material of the star and enables emission to allow the star to glow
    /// </summary>
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null || renderer.material == null)
        {
            Debug.LogError("No Renderer or Material found on the star GameObject.");
            return;
        }
        starMaterial = renderer.material;

        starMaterial.EnableKeyword("_EMISSION");
        currentIntensity = minIntensity;
    }

    /// <summary>
    /// The update method transitions between the minimum and maximum intensity of the glow and applies the colour to the Star
    /// </summary>
    void Update()
    {
        if (increasing)
        {
            currentIntensity += Time.deltaTime * pulseSpeed;
            if (currentIntensity >= maxIntensity)
                increasing = false; 
        }
        else
        {
            currentIntensity -= Time.deltaTime * pulseSpeed;
            if (currentIntensity <= minIntensity)
                increasing = true; 
        }

        starMaterial.SetColor("_EmissionColor", glowColor * currentIntensity);
    }
}

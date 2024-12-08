using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] public float dayLengthInSeconds = 120f; 
    [SerializeField] public Light sunLight;               
    [SerializeField] public Material skyboxMaterial;        
    [SerializeField] public AnimationCurve lightIntensityCurve; 

    private float timeOfDay = 0f; 

    /// <summary>
    /// The Start method sets up the initial settings of the sunlight during day time
    /// timeOfDay = 0.4 begins the day at daytime
    /// </summary>
    void Start()
    {
        if (sunLight != null)
        {
            sunLight.intensity = 1f;
            sunLight.color = Color.white;
        }

        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_Exposure", 1.2f); 
            skyboxMaterial.SetFloat("_SunSize", 0.05f); 
            skyboxMaterial.SetFloat("_SunSizeConvergence", 1f); 
        }

        timeOfDay = 0.4f;
    }

    /// <summary>
    /// The update method updates the time of day, rotating the sun around the skybox as the day progresses
    /// </summary>
    void Update()
    {
        timeOfDay += Time.deltaTime / dayLengthInSeconds;
        if (timeOfDay > 1f) timeOfDay = 0f;

        float sunAngle = Mathf.Lerp(0f, 360f, timeOfDay);
        sunLight.transform.rotation = Quaternion.Euler(new Vector3(sunAngle - 90f, 170f, 0f));

        if (lightIntensityCurve != null)
        {
            sunLight.intensity = lightIntensityCurve.Evaluate(timeOfDay);
        }

        if (skyboxMaterial != null)
        {
            float exposure = Mathf.Lerp(1.2f, 0.1f, timeOfDay);
            skyboxMaterial.SetFloat("_Exposure", exposure);
        }
    }
}

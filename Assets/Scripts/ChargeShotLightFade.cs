using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class ChargeShotLightFade : MonoBehaviour
{
    public Color inactiveColor = Color.red;
    public Color activeColor = new Color(0.6f, 0f, 0.8f); // Purple
    public float fadeSpeed = 2f;

    private Light2D pointLight;

    void Start()
    {
        pointLight = GetComponent<Light2D>();
        if (pointLight == null)
        {
            Debug.LogError("ChargeShotLightFade requires a Light2D component.");
            enabled = false;
        }
    }

    void Update()
    {
        Color targetColor = GameManager.IsChargeShotActive ? activeColor : inactiveColor;
        pointLight.color = Color.Lerp(pointLight.color, targetColor, Time.deltaTime * fadeSpeed);
    }
}

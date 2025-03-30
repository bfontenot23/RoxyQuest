using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
public class ChargeShotSpriteFade : MonoBehaviour
{
    public Color inactiveColor = Color.red;
    public Color activeColor = new Color(0.6f, 0f, 0.8f); // Purple
    public float fadeSpeed = 2f;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (sprite == null)
        {
            Debug.LogError("ChargeShotSpriteFade requires a SpriteRenderer component.");
            enabled = false;
        }
    }

    void Update()
    {
        Color targetColor = GameManager.IsChargeShotActive ? activeColor : inactiveColor;
        sprite.color = Color.Lerp(sprite.color, targetColor, Time.deltaTime * fadeSpeed);
    }
}

using UnityEngine;

public class StaffFollowFlip : MonoBehaviour
{
    public Transform player;
    public Vector2 offsetRight = new Vector2(0.5f, 0f);
    public Vector2 offsetLeft = new Vector2(-0.5f, 0f);
    public float positionLerpSpeed = 10f;
    public float rotationLerpSpeed = 10f;

    public float revolutionThreshold = 10f;
    public float inactivityDuration = 2f;

    private SpriteRenderer playerRenderer;
    private float accumulatedAngle = 0f;
    private float lastAngle;
    private float inactivityTimer;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("StaffFollowFlip requires a reference to the player transform.");
            return;
        }

        playerRenderer = player.GetComponent<SpriteRenderer>();
        if (playerRenderer == null)
        {
            Debug.LogError("Player does not have a SpriteRenderer.");
        }

        lastAngle = GetAngleToMouse();
    }

    void LateUpdate()
    {
        if (player == null || playerRenderer == null) return;

        // Position Lerp
        Vector3 currentLocalPos = transform.localPosition;
        float targetX = playerRenderer.flipX ? offsetLeft.x : offsetRight.x;
        Vector3 targetLocalPos = new Vector3(targetX, currentLocalPos.y, currentLocalPos.z);
        transform.localPosition = Vector3.Lerp(currentLocalPos, targetLocalPos, Time.deltaTime * positionLerpSpeed);

        // Rotation Toward Mouse
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPos - transform.position;
        direction.z = 0f;

        if (direction.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationLerpSpeed);
        }

        float currentAngle = GetAngleToMouse();
        float angleDelta = Mathf.DeltaAngle(lastAngle, currentAngle);

        // Update total rotation (for partial revolution tracking)
        accumulatedAngle += angleDelta;

        lastAngle = currentAngle;

        // Count full revolutions using sign crossing
        if(Input.GetMouseButton(0))
        {
            if (Mathf.Abs(accumulatedAngle) >= 360f)
            {
                accumulatedAngle = 0f;
                GameManager.CurrentRevolutions++;
                inactivityTimer = inactivityDuration;
            }

            // Charge shot when revolutions hit threshold
            if (GameManager.CurrentRevolutions >= revolutionThreshold)
            {
                GameManager.IsChargeShotActive = true;
            }
        }

        // Inactivity logic - only starts counting down if no full revolutions are made
        if (inactivityTimer > 0f)
        {
            inactivityTimer -= Time.deltaTime;
        }
        else
        {
            GameManager.IsChargeShotActive = false;
            GameManager.CurrentRevolutions = 0;
        }
    }

    private float GetAngleToMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mouseWorldPos - player.position;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}

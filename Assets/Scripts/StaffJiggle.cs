using UnityEngine;

public class StaffJiggle : MonoBehaviour
{
    public Transform staffTransform;
    public float jiggleAmount = 0.15f;
    public float damping = 5f;
    public float stiffness = 50f;

    private Vector3 originalLocalPosition;
    private float lastStaffRotation;
    private Vector3 velocity;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
        lastStaffRotation = staffTransform.eulerAngles.z;
    }

    void Update()
    {
        float currentRotation = staffTransform.eulerAngles.z;
        float rotationDelta = Mathf.DeltaAngle(lastStaffRotation, currentRotation);

        // Apply a jiggle force based on how much the rotation has changed
        Vector3 jiggleOffset = new Vector3(rotationDelta * jiggleAmount, 0, 0);

        // Simulate spring-damping motion toward the original position + offset
        Vector3 targetPosition = originalLocalPosition + jiggleOffset;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, 1f / damping, stiffness);

        lastStaffRotation = currentRotation;
    }
}

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SortableObject : MonoBehaviour
{
    [Header("Sorting Options")]
    public float pivotOffset = 0f;
    public bool sortAgainstPlayer = true;

    [Header("Player Tracking (Optional)")]
    public bool isPlayer = false;

    [HideInInspector] public SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Register with the manager
        if (YSortManager.InstanceExists)
        {
            YSortManager.Instance.Register(this);

            if (isPlayer)
                YSortManager.Instance.SetPlayerReference(this);
        }
        else
        {
            Debug.LogWarning($"{name}: YSortManager not found in scene.");
        }
    }

    void OnDestroy()
    {
        if (YSortManager.InstanceExists)
        {
            YSortManager.Instance.Unregister(this);

            if (isPlayer)
                YSortManager.Instance.ClearPlayerReference(this);
        }
    }

    public float GetSortY()
    {
        return transform.position.y + pivotOffset;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isPlayer ? Color.green : Color.cyan;

        Vector3 center = new Vector3(transform.position.x, GetSortY(), transform.position.z);
        Vector3 left = center + Vector3.left * 0.5f;
        Vector3 right = center + Vector3.right * 0.5f;

        Gizmos.DrawLine(left, right);
        Gizmos.DrawSphere(center, 0.025f);
    }
}

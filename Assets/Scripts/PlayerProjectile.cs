using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public enum OrbState { CHARGING, STANDARD }

    public OrbState state;
    private Vector2 direction;
    public float speed;

    private Animator animator;
    private Animator glintAnimator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        glintAnimator = transform.GetChild(0).GetComponent<Animator>();
    }


}

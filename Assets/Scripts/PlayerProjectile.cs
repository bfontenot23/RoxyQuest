using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public enum OrbState { CHARGING, STANDARD }

    public OrbState state;
    public float speed = 5;

    private Animator animator;
    private Animator glintAnimator;

    private Rigidbody2D rb;

    private bool shot = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        glintAnimator = transform.GetChild(0).GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !shot)
        {
            rb.simulated = true;
            transform.SetParent(null);
            rb.linearVelocity = transform.right * speed;
            shot = true;
            StartCoroutine(ProjLifespan(5));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bounds"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    System.Collections.IEnumerator ProjLifespan(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }


}

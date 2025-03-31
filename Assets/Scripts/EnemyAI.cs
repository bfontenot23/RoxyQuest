using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyType { SLIME, SKELETON }

    public EnemyType enemyType;
    [SerializeField, Range(0f, 3f)] private float speed = 5f;
    [SerializeField, Range(0f, 1f)] private float speedVariance = 1f;

    [SerializeField, Range(1f, 15f)] private float jumpDistance = 3f;
    [SerializeField, Range(1f, 4f)] private float jumpDistanceVariance = 1f;
    [SerializeField] private float jumpDuration = 0.5f;


    [SerializeField, Range(0f, 10f)] private float minWaitTime = 1f;
    [SerializeField, Range(0f, 10f)] private float maxWaitTime = 3f;

    private Animator animator;
    [SerializeField] private string jumpTriggerName = "Jump";
    [SerializeField] private string attackTriggerName = "Attack";


    [SerializeField] private Vector2 attackBoxSize = new Vector2(1.5f, 2f);
    [SerializeField] private Vector2 attackBoxOffset = new Vector2(0f, 0f);
    [SerializeField] private float attackCooldown = 1f;

    private float attackTimer = 0f;

    private Transform player;
    [SerializeField] private LayerMask playerLayer;

    private float waitTimer;
    private bool isJumping = false;
    private Vector2 jumpTarget;
    private float actualSpeed;

    private bool isMovementPaused = false;


    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();

        // Apply speed variance once on spawn
        actualSpeed = speed + Random.Range(-speedVariance, speedVariance);

        if (enemyType == EnemyType.SLIME)
        {
            ResetWaitTimer();
        }
    }

    void FixedUpdate()
    {
        switch (enemyType)
        {
            case EnemyType.SLIME:
                SlimeUpdate();
                break;
            case EnemyType.SKELETON:
                SkeletonUpdate();
                break;
        }
    }

    void SlimeUpdate()
    {
        if (!isJumping)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                float actualJumpDistance = jumpDistance + Random.Range(-jumpDistanceVariance, jumpDistanceVariance);

                jumpTarget = (Vector2)transform.position + direction * actualJumpDistance;

                if (animator != null && !string.IsNullOrEmpty(jumpTriggerName))
                {
                    animator.SetTrigger(jumpTriggerName);
                }

                StartCoroutine(JumpTowards(jumpTarget));
                isJumping = true;
            }

        }
    }

    System.Collections.IEnumerator JumpTowards(Vector2 target)
    {
        Vector2 start = transform.position;
        float elapsed = 0f;

        float x = target.x - start.x;
        if (x != 0) GetComponent<SpriteRenderer>().flipX = x < 0;

        while (elapsed < jumpDuration)
        {
            float t = elapsed / jumpDuration;
            transform.position = Vector2.Lerp(start, target, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        ResetWaitTimer();
        isJumping = false;
    }


    void SkeletonUpdate()
    {
        bool playerInRange = IsPlayerInAttackBox();

        if (playerInRange)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                if (direction.x != 0) GetComponent<SpriteRenderer>().flipX = direction.x < 0;

                if (animator != null && !string.IsNullOrEmpty(attackTriggerName))
                {
                    animator.SetTrigger(attackTriggerName);
                }

                attackTimer = attackCooldown;
            }
        }
        else if (!isMovementPaused)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction.x != 0) GetComponent<SpriteRenderer>().flipX = direction.x < 0;

            transform.position += (Vector3)(direction * actualSpeed * Time.deltaTime);
        }
    }

    bool IsPlayerInAttackBox()
    {
        Vector2 boxCenter = (Vector2)transform.position + attackBoxOffset;
        Collider2D hit = Physics2D.OverlapBox(boxCenter, attackBoxSize, 0f, playerLayer);

        return hit != null;
    }

    private void OnDrawGizmosSelected()
    {
        // Attack box
        Gizmos.color = Color.red;
        Vector2 boxCenter = (Vector2)transform.position + attackBoxOffset;
        Gizmos.DrawWireCube(boxCenter, attackBoxSize);
    }




    void ResetWaitTimer()
    {
        waitTimer = Random.Range(minWaitTime, maxWaitTime);
    }

    public void PauseMovement()
    {
        isMovementPaused = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void ResumeMovement()
    {
        StartCoroutine(ResumeMovementAfterDelay(0.1f));
    }

    private System.Collections.IEnumerator ResumeMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isMovementPaused = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }


}

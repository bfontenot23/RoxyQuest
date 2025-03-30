using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyType { SLIME, SKELETON }

    [SerializeField] private EnemyType enemyType;
    [SerializeField, Range(0f, 3f)] private float speed = 5f;
    [SerializeField, Range(0f, 1f)] private float speedVariance = 1f;

    [SerializeField, Range(1f, 15f)] private float jumpDistance = 3f;
    [SerializeField, Range(1f, 4f)] private float jumpDistanceVariance = 1f;
    [SerializeField] private float jumpDuration = 0.5f;


    [SerializeField, Range(0f, 10f)] private float minWaitTime = 1f;
    [SerializeField, Range(0f, 10f)] private float maxWaitTime = 3f;

    private Animator animator;
    [SerializeField] private string jumpTriggerName = "Jump";


    private Transform player;
    private float waitTimer;
    private bool isJumping = false;
    private Vector2 jumpTarget;
    private float actualSpeed;

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

    void Update()
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
        Vector2 direction = (player.position - transform.position).normalized;
        if (direction.x!=0) GetComponent<SpriteRenderer>().flipX = direction.x < 0;
        transform.position += (Vector3)(direction * actualSpeed * Time.deltaTime);
    }

    void ResetWaitTimer()
    {
        waitTimer = Random.Range(minWaitTime, maxWaitTime);
    }
}

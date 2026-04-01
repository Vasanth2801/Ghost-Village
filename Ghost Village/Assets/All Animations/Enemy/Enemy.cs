using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement for the Enemy")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private int facingDirection = 1;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCoolDown;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private bool isAttacking;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if(distanceFromPlayer <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            if(!isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }

            return;
        }
        else
        {
            rb.linearVelocity = new Vector2(speed, 0f);
        }

        if(player.position.x < transform.position.x && facingDirection == 1 || player.position.x > transform.position.x && facingDirection == -1)
        {
            FLip();
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while(true)
        {
            if(player == null)
            {
                break;
            }

            float distance = Vector2.Distance(player.position, transform.position);
            if(distance > attackRange)
            {
                break;
            }

            Attack();

            yield return new WaitForSeconds(attackCoolDown);
        }
        isAttacking = false;
    }

    void Attack()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerMask);
        foreach(Collider2D player in hitPlayers)
        {
            Debug.Log("Attacking the Player");
        }
    }

    void FLip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        attackPoint.Rotate(0, 180f, 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        speed = -speed;
        FlipEnemy();
    }

    void FlipEnemy()
    {
        transform.localScale = new Vector2(-Mathf.Sign(rb.linearVelocity.x), 1f);
    }
}

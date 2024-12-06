using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Speed = 1f;
    public float collisionOffset = 0.05f;
    public float damage = 10f;
    public GameObject defeatPanel;
    public HordeManager hordeManager;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    public float initialHealth = 100f;
    public float health;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;

    public HealthBar healthBar;

    private bool canDash = true;
    public float dashingPower = 5f;
    public float dashingTime = 0.1f;
    public float dashingCooldown = 3f;

    [SerializeField] private TrailRenderer tr;

    public Text cooldownText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadPlayerData(this);
        }
        else
        {
            health = initialHealth;
        }

        healthBar.SetMaxHealth(initialHealth);
        healthBar.SetHealth(health / initialHealth);

        if (cooldownText != null)
        {
            cooldownText.text = "Dash Ready";
        }
    }

    private void Update()
    {
        if (!canMove)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (cooldownText != null && canDash)
        {
            cooldownText.text = "Dash Ready";
        }
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }

            animator.SetBool("isMoving", success);
        }
        else
        {
            animator.SetBool("isMoving", false);
            StopMovement();
        }

        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.SetHealth(health / initialHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void IncreaseSpeed(float amount)
    {
        Speed += amount;
    }

    public void IncreaseDamage(float amount)
    {
        damage += amount;
    }

    public void IncreaseHealth(int amount)
    {
        initialHealth += amount;
        health += amount;
        healthBar.SetMaxHealth(initialHealth);
        healthBar.SetHealth(health / initialHealth);
    }

    private void Die()
    {
        defeatPanel.SetActive(true);
        hordeManager.ResetHordes();
    }

    public void RestartGame()
    {
        initialHealth = 100;
        Speed = 1.0f;
        damage = 10.0f;
        defeatPanel.SetActive(false);
        hordeManager.RestartHordeCoroutine();
        health = initialHealth;
        healthBar.SetHealth(1f);
    }

    public void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    public void OnFire()
    {
        animator.SetTrigger("swordatack");
    }

    public void SwordAttack()
    {
        LockMovement();

        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            Speed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            TakeDamage(2);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Maplimit"))
        {
            // Manejar colisión con el límite del mapa
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("colisionobjects"))
        {
            Debug.Log("Collision with Tilemap detected");
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        canMove = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector2 dashDirection = movementInput.normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        }

        Vector2 originalPosition = rb.position;
        Vector2 targetPosition = originalPosition + dashDirection * dashingPower;

        tr.emitting = true;
        float elapsedTime = 0f;
        while (elapsedTime < dashingTime)
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, dashDirection, dashingPower * Time.deltaTime, LayerMask.GetMask("Limit"));
            if (hit.collider != null || (hit.collider != null && hit.collider.CompareTag("Limit")))
            {
                targetPosition = hit.point - dashDirection * 0.01f;
                break;
            }

            rb.MovePosition(Vector2.Lerp(originalPosition, targetPosition, elapsedTime / dashingTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(targetPosition);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        StopMovement();
        canMove = true;

        float cooldownTimer = dashingCooldown;
        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownText != null)
            {
                cooldownText.text = $"Dash Cooldown: {cooldownTimer:F1}";
            }
            yield return null;
        }

        if (cooldownText != null)
        {
            cooldownText.text = "Dash Ready";
        }
        canDash = true;
    }
}
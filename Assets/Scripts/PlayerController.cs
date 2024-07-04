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
    public GameObject defeatPanel; // Referencia al panel de derrota
    public HordeManager hordeManager; // Referencia al HordeManager
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    public float health;
    public HealthBar healthBar;
    public float initialHealth = 100f;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollsions = new List<RaycastHit2D>();
    bool canMove = true;

    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage called with damage: " + damage);
        initialHealth -= damage;
        if (initialHealth <= 0)
        {
            initialHealth = 0;
            Die();
        }
        healthBar.SetHealth(initialHealth / 100.0f); // Asumiendo que la salud máxima es 100
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
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
            }

            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }

        if (initialHealth <= 0)
        {
            Die();
        }
    }

    public bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollsions,
            Speed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0) // **Modificación: Verificar si no hay colisiones**
        {
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            TakeDamage(2);
            if (enemy != null)
            {
                Debug.Log("Enemy detected: " + enemy.name);
            }
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
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
        if (health > initialHealth) // Si quieres un límite máximo de salud
        {
            health = initialHealth;
        }
    }

    private void Die()
    {
        // Mostrar la pantalla de derrota
        defeatPanel.SetActive(true);
        // Detener el juego o reiniciar las hordas
        hordeManager.ResetHordes();
    }

    public void RestartGame()
    {
        initialHealth = 100; // Restablecer la salud
        Speed = 1.0f; // Restablecer la velocidad
        damage = 10.0f; // Restablecer el daño
        defeatPanel.SetActive(false); // Ocultar la pantalla de derrota
        hordeManager.RestartHordeCoroutine();
    }
}
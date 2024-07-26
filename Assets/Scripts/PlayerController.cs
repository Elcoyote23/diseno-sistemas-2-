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
    public float initialHealth = 100f;
    private float health;
    private float originalSpeed; // Para almacenar la velocidad original
    private float lastDamageTime; // Para controlar el intervalo de daño

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;

    public HealthBar healthBar; // Asigna la HealthBar que actualiza la barra de vida visual

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = initialHealth;
        healthBar.SetMaxHealth(initialHealth);
        originalSpeed = Speed; // Almacenar la velocidad original
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

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        healthBar.SetHealth(health / initialHealth);
        Debug.Log("Daño recibido por PlayerController");

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

    public void IncreaseSpeed(float percentage)
    {
        Speed += Speed * (percentage / 100f);
        originalSpeed = Speed; // Actualizar la velocidad original
    }

    public void IncreaseDamage(float percentage)
    {
        damage += damage * (percentage / 100f);
    }

    public void IncreaseHealth(float percentage)
    {
        float increaseAmount = initialHealth * (percentage / 100f);
        initialHealth += increaseAmount;
        health += increaseAmount;
        healthBar.SetMaxHealth(initialHealth);
        healthBar.SetHealth(health / initialHealth);
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

        bool canMove = true;

        foreach (var hit in castCollisions)
        {
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("EnemyBlock"))
            {
                canMove = false;
                break;
            }
        }

        if (canMove)
        {
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (Time.time - lastDamageTime > 1f) // Intervalo de 1 segundo
            {
                TakeDamage(2); // Ejemplo de daño constante al colisionar con un enemigo
                lastDamageTime = Time.time;
            }
            Speed *= 0.2f; // Reduce la velocidad del jugador al 80%
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Speed = originalSpeed; // Restaurar la velocidad original del jugador
        }
    }

    public IEnumerator RestoreSpeed()
    {
        yield return new WaitForSeconds(1f);
        Speed = originalSpeed; // Restaura la velocidad del jugador
    }
}

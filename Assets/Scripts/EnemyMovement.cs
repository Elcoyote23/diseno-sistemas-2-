using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform objetivo;
    public float speed;
    public float distancia;

    private Rigidbody2D rb;
    private Vector2 movementDirection;

    void Start()
    {
        objetivo = GameObject.Find("player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PerseguirJugador();
    }

    private void PerseguirJugador()
    {
        Vector2 targetPosition = objetivo.position;
        Vector2 directionToPlayer = (targetPosition - rb.position).normalized;

        // Verificar colisiones con obstáculos
        RaycastHit2D hit = Physics2D.Raycast(rb.position, directionToPlayer, distancia);

        // Visualizar el raycast principal
        Debug.DrawRay(rb.position, (directionToPlayer).normalized * distancia, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("EnemyBlock"))
        {
            // Intentar rodear el obstáculo
            Vector2 perpendicularDirection = Vector2.Perpendicular(directionToPlayer);
            Vector2 leftCheck = rb.position + perpendicularDirection * distancia;
            Vector2 rightCheck = rb.position - perpendicularDirection * distancia;

            // Verificar si hay espacio libre a la izquierda o derecha
            RaycastHit2D leftHit = Physics2D.Raycast(rb.position, leftCheck - rb.position, distancia);
            RaycastHit2D rightHit = Physics2D.Raycast(rb.position, rightCheck - rb.position, distancia);

            // Visualizar los raycasts laterales
            Debug.DrawRay(rb.position, (leftCheck - rb.position).normalized * distancia, Color.green);
            Debug.DrawRay(rb.position, (rightCheck - rb.position).normalized * distancia, Color.blue);

            if (leftHit.collider == null)
            {
                movementDirection = (leftCheck - rb.position).normalized;
            }
            else if (rightHit.collider == null)
            {
                movementDirection = (rightCheck - rb.position).normalized;
            }
            else
            {
                // Si ambos lados están bloqueados, retroceder
                movementDirection = -directionToPlayer;
            }
        }
        else
        {
            // Si no hay obstáculos, moverse hacia el jugador
            movementDirection = directionToPlayer;
        }

        // Mover al enemigo
        rb.MovePosition(rb.position + movementDirection * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(10); // Inflige daño al jugador
                player.Speed *= 0.8f; // Reduce la velocidad del jugador al 80%
                StartCoroutine(player.RestoreSpeed());
            }
        }
        else if (collision.gameObject.CompareTag("Limites"))
        {
            // Permitir que los enemigos atraviesen los límites
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
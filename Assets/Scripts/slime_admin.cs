using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime_admin : MonoBehaviour
{
    public Transform objetivo;
    public float speed;
    public bool debePerseguir;
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
        Vector2 targetPosition = objetivo.position;
        Vector2 directionToPlayer = (targetPosition - rb.position).normalized;

        // Verificar colisiones con obstáculos
        RaycastHit2D hit = Physics2D.Raycast(rb.position, directionToPlayer, speed * Time.deltaTime);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("EnemyBlock"))
            {
                // Intentar rodear el obstáculo
                if (directionToPlayer.x != 0)
                {
                    // Moverse verticalmente si hay un obstáculo horizontal
                    movementDirection = new Vector2(0, directionToPlayer.y > 0 ? 1 : -1);
                }
                else
                {
                    // Moverse horizontalmente si hay un obstáculo vertical
                    movementDirection = new Vector2(directionToPlayer.x > 0 ? 1 : -1, 0);
                }
            }
            else
            {
                // Si no hay obstáculos, moverse hacia el jugador
                movementDirection = directionToPlayer;
            }
        }
        else
        {
            // Si no hay colisiones, moverse hacia el jugador
            movementDirection = directionToPlayer;
        }

        // Mover al enemigo
        rb.MovePosition(rb.position + movementDirection * speed * Time.deltaTime);
    }
}
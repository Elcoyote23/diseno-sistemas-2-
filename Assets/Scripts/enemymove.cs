using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemymove : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public int direccion;
    public float speed_walk;
    public float speed_run;
    public GameObject target;
    public bool atacando;

    private Rigidbody2D rb;
    private Vector2 movementDirection;

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Comportamientos();
    }

    public void Comportamientos()
    {
        ani.SetBool("run", false);
        cronometro += 1 * Time.deltaTime;
        if (cronometro >= 4)
        {
            rutina = Random.Range(0, 2);
            cronometro = 0;
        }

        switch (rutina)
        {
            case 0:
                ani.SetBool("walk", false);
                break;
            case 1:
                direccion = Random.Range(0, 2);
                rutina++;
                break;

            case 2:
                DetermineMovement();
                ani.SetBool("walk", true);
                break;
        }
    }

    private void DetermineMovement()
    {
        Vector2 targetPosition = target.transform.position;
        Vector2 directionToPlayer = (targetPosition - rb.position).normalized;

        // Verificar colisiones con obstáculos
        RaycastHit2D hit = Physics2D.Raycast(rb.position, directionToPlayer, speed_walk * Time.deltaTime);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("EnemyBlock")) // Cambia "EnemyBlock" por la etiqueta que uses
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
        rb.MovePosition(rb.position + movementDirection * speed_walk * Time.deltaTime);
    }
}

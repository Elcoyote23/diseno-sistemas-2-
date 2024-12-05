using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime_admin : MonoBehaviour
{
    public Transform objetivo;
    public float speed;

    private Vector2 movementDirection;

    void Start()
    {
        objetivo = GameObject.Find("player").GetComponent<Transform>();
    }

    void Update()
    {
        Vector2 targetPosition = objetivo.position;
        Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;

        // Verificar colisiones con obstáculos
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, speed * Time.deltaTime);

        

        // Mover al enemigo
        transform.Translate(movementDirection * speed * Time.deltaTime);
        movementDirection = directionToPlayer;
    }
}
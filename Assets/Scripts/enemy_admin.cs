using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_admin : MonoBehaviour
{
    public Transform objetivo;
    public float speed;
    public bool debePerseguir;
    public float distancia;

    private Rigidbody2D rb;

    void Start()
    {
        objetivo = GameObject.Find("player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (debePerseguir)
        {
            Vector2 direction = (objetivo.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (debePerseguir)
        {
            Vector2 direction = (objetivo.position - transform.position).normalized;
            TryMove(direction);
        }
    }

    public bool TryMove(Vector2 direction)
    {
        List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
        ContactFilter2D movementFilter = new ContactFilter2D().NoFilter();
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            speed * Time.fixedDeltaTime + 0.05f);

        if (count == 0) // **Modificación: Verificar si no hay colisiones**
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Evitar superposición de enemigos
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            rb.MovePosition(rb.position + pushDirection * speed * Time.deltaTime);
        }
    }
}

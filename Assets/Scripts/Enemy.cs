using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    private EnemyMovement enemyMovement; //referencia al script unificado
    private AudioSource audioSource;
    public int damage = 2;
    private bool isAttacking = false; // Para controlar el intervalo de daño

    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    public float health = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>(); //obtengo la referencia de los componentes
        audioSource = GetComponent<AudioSource>(); // Asegúrate de que el componente AudioSource esté presente
    }

    public void Defeated()
    {
        animator.SetTrigger("Defeated");
        if (enemyMovement != null)
        {
            enemyMovement.speed = 0; // Establecer la velocidad a 0
        }
    }

    public void OnDefeatedAnimation()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null && !isAttacking)
            {
                StartCoroutine(ApplyDamage(player));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines(); // Detener el daño continuo cuando deje de colisionar
            isAttacking = false;
        }
    }

    private IEnumerator ApplyDamage(PlayerController player)
    {
        isAttacking = true;
        while (isAttacking)
        {
            player.TakeDamage(damage); // Inflige daño al jugador
            yield return new WaitForSeconds(1f); // Intervalo de 1 segundo
        }
    }
}


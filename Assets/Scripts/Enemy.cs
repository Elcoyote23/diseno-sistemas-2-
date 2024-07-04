using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    private slime_admin slimeAdmin; //referencia al script 
    private enemy_admin enemyAdmin; //referencia al script 
    public void Attack(PlayerController player)
    {
        player.TakeDamage(2); // Inflige 10 puntos de daño
    }

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
        slimeAdmin = GetComponent<slime_admin>(); //obtengo la referencia de los componentes
        enemyAdmin = GetComponent<enemy_admin>(); //obtengo la referencia de los componentes
    }

    public void Defeated()
    {
        animator.SetTrigger("Defeated");
        if (slimeAdmin != null || enemyAdmin != null)
        {
            slimeAdmin.speed = 0; // Establecer la velocidad a 0
            enemyAdmin.speed = 0; // Establecer la velocidad a 0
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
            if (player != null)
            {
                Attack(player);
            }
        }
    }
}

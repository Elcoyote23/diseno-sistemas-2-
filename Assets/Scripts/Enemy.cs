using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   
    Animator animator;
    public void Attack(PlayerController player)
    {
        player.TakeDamage(10); // Inflige 10 puntos de da�o
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

        get {
                return health;
            }
            
            
        
    }

    public float health = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

   

    public void Defeated()
    {
        animator.SetTrigger("Defeated");
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }
}

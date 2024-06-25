using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{



    public float damage = 0;
    Vector2 rightAttackOffSet;

     public Collider2D swordCollider;

    private void Start() {

        
        rightAttackOffSet = transform.localPosition;

    }

    

    public void AttackRight()
    {
        
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffSet;
        

    }

    public void AttackLeft()
    {
        
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffSet.x * - 1, rightAttackOffSet.y);
    }

    public void StopAttack()
    {
        swordCollider.enabled = false; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.Health -= damage;
            }
        }
    }
}



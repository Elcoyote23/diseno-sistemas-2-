using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private EnemyMovement enemyMovement;
    private AudioSource audioSource;

    public float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Defeated");
        enemyMovement.StopMovement();
        audioSource.Play();

        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
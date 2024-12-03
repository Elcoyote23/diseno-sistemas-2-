using UnityEngine;

public class Pinchos : MonoBehaviour
{
    public GameManager gameManager; // Referencia al GameManager
    public int damage = 2;          // Daño que infligen los pinchos

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto con el que colisiona tiene el tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.playerHealth -= damage;
                Debug.Log("Vida del jugador: " + gameManager.playerHealth);

                if (gameManager.playerHealth <= 0)
                {
                    Debug.Log("El jugador ha muerto!");
                    // Lógica de muerte o respawn
                }
            }
        }
    }
}



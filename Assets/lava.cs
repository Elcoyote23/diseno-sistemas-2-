using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint; // Punto donde el jugador respawneará
    [SerializeField] private AudioClip respawnSound; // Sonido que se reproducirá al respawnear
    private AudioSource audioSource;
    public GameObject defeatPanel;
   public HordeManager hordeManager;
    private void Start()
    {
        if (respawnSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = respawnSound;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Verifica si el objeto es el Player
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // También verifica si el Player entra en un trigger
        {
            KillPlayer(other.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
        // Aquí puedes añadir animaciones o efectos para la muerte si lo necesitas
        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.position; // Mueve al jugador al punto de respawn
        }

        // Reproduce el sonido de respawn
        if (audioSource != null && respawnSound != null)
        {
            audioSource.Play();
        }

        defeatPanel.SetActive(true);

        // Detener el juego o reiniciar las hordas
        hordeManager.ResetHordes();
    }
}

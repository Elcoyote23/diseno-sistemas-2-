using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentHorde;
    public float playerHealth;
    public int playerDamage;
    public float playerSpeed;
    public int enemiesPerHorde;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Asegúrate de que GameManager persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para guardar datos del jugador
    public void SavePlayerData(PlayerController player)
    {
        playerHealth = player.health;
        playerDamage = (int)player.damage;
        playerSpeed = player.Speed;
    }

    // Método para cargar datos del jugador
    public void LoadPlayerData(PlayerController player)
    {
        player.health = playerHealth;
        player.damage = playerDamage;
        player.Speed = playerSpeed;
    }
}

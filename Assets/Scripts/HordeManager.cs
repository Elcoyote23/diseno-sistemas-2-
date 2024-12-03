using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HordeManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo básico
    public GameObject enemyType2Prefab; // Enemigo que aparece a partir de la horda 3
    public GameObject enemyType3Prefab; // Enemigo que aparece a partir de la horda 5
    public int initialEnemiesPerHorde = 10; // Cantidad inicial de enemigos por horda
    public float spawnInterval = 1.0f; // Intervalo de tiempo entre spawns
    public float pauseBetweenHordes = 5.0f; // Duración de la pausa entre hordas
    public Transform[] spawnPoints; // Puntos de spawn
    public Text hordeMessage; // Referencia al texto de la UI
    public GameObject selectionPanel; // Panel de selección
    public PlayerController player; // Referencia al controlador del jugador

    // Variables para botones
    public Button speedButton;
    public Button damageButton;
    public Button healthButton;

    public int speedIncreaseAmount = 1; // Cantidad de incremento de velocidad
    public int damageIncreaseAmount = 5; // Cantidad de incremento de daño
    public int healthIncreaseAmount = 20; // Cantidad de incremento de vida

    private int currentHorde = 0; // Contador de hordas
    private int enemiesPerHorde; // Cantidad actual de enemigos por horda
    private Coroutine hordeCoroutine; // Referencia a la coroutine de las hordas
    private bool gameRunning = true; // Estado del juego

    void Start()
    {
        enemiesPerHorde = initialEnemiesPerHorde;

        // Cargar el estado del juego si se está continuando desde una escena anterior
        if (GameManager.Instance != null)
        {
            currentHorde = GameManager.Instance.currentHorde;
            enemiesPerHorde = GameManager.Instance.enemiesPerHorde;
        }

        selectionPanel.SetActive(false); // Panel comienza inactivo

        // Configuración de botones
        speedButton.onClick.AddListener(() =>
        {
            player.IncreaseSpeed(speedIncreaseAmount);
            selectionPanel.SetActive(false); // Cerrar panel
        });

        damageButton.onClick.AddListener(() =>
        {
            player.IncreaseDamage(damageIncreaseAmount);
            selectionPanel.SetActive(false); // Cerrar panel
        });

        healthButton.onClick.AddListener(() =>
        {
            player.IncreaseHealth(healthIncreaseAmount);
            selectionPanel.SetActive(false); // Cerrar panel
        });

        hordeCoroutine = StartCoroutine(SpawnHorde());
    }

    private IEnumerator SpawnHorde()
    {
        while (gameRunning)
        {
            // Mostrar mensaje de inicio de horda
            hordeMessage.text = "Horda " + (currentHorde + 1) + " comenzando...";

            yield return new WaitForSeconds(2.0f); // Mostrar el mensaje por 2 segundos

            int totalEnemies = enemiesPerHorde;
            while (totalEnemies > 0)
            {
                // Spawn enemigos básicos
                SpawnEnemy(enemyPrefab);
                totalEnemies--;

                // Spawn enemigos tipo 2 a partir de la horda 3
                if (currentHorde >= 3 && totalEnemies > 0)
                {
                    SpawnEnemy(enemyType2Prefab);
                    totalEnemies--;
                }

                // Spawn enemigos tipo 3 a partir de la horda 5
                if (currentHorde >= 5 && totalEnemies > 0)
                {
                    SpawnEnemy(enemyType3Prefab);
                    totalEnemies--;
                }

                yield return new WaitForSeconds(spawnInterval);
            }


            currentHorde++;
            enemiesPerHorde *= 2; // Multiplicar la cantidad de enemigos para la próxima horda

            // Guardar el estado del juego
            SaveGameData();

            // Mostrar opciones de mejora
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0); // Espera a que no haya enemigos

            // Mostrar opciones de mejora después de 2 segundos
            yield return new WaitForSeconds(2.0f);
            selectionPanel.SetActive(true);
            hordeMessage.text = "Selecciona una mejora antes de la próxima horda";

            yield return new WaitForSeconds(pauseBetweenHordes); // Pausa entre hordas

            // Cambiar de escena si aplica
            if (currentHorde >= 3)
            {
                ChangeScene("LEVEL2");
                yield break; // Salir del ciclo para evitar conflictos en la nueva escena
            }
        }
        
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void SaveGameData()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentHorde = currentHorde;
            GameManager.Instance.enemiesPerHorde = enemiesPerHorde;

            if (player != null)
            {
                GameManager.Instance.SavePlayerData(player);
            }
            else
            {
                Debug.LogError("El objeto player no está asignado en HordeManager.");
            }
        }
        else
        {
            Debug.LogError("GameManager.Instance no está disponible.");
        }
    }

    public void ChangeScene(string sceneName)
    {
        SaveGameData();
        SceneManager.LoadScene(sceneName);
    }

    public void ResetHordes()
    {
        currentHorde = 0;
        enemiesPerHorde = initialEnemiesPerHorde;

        StopCoroutine(hordeCoroutine);

        // Reinicia la escena al nivel inicial
        if (SceneManager.GetActiveScene().name != "LEVEL1")
        {
            ChangeScene("LEVEL1");
        }
        else
        {
            hordeCoroutine = StartCoroutine(SpawnHorde());
        }
    }
    public void RestartHordeCoroutine()
    {
        // Reiniciar la coroutine si el jugador reinicia el juego
        StopCoroutine(hordeCoroutine);
        hordeCoroutine = StartCoroutine(SpawnHorde());
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;


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
    public float speedIncreaseAmount = 1.0f; // Cantidad de incremento de velocidad
    public int damageIncreaseAmount = 5; // Cantidad de incremento de daño
    public int healthIncreaseAmount = 20; // Cantidad de incremento de vida

    private int currentHorde = 0; // Contador de hordas
    private int enemiesPerHorde; // Cantidad actual de enemigos por horda
    private Coroutine hordeCoroutine; // Referencia a la coroutine de las hordas
    private bool gameRunning = true; // Estado del juego

    void Start()
    {
        enemiesPerHorde = initialEnemiesPerHorde;
        hordeCoroutine = StartCoroutine(SpawnHorde());
    }

    private IEnumerator SpawnHorde()
    {
        while (gameRunning)
        {
            float currentSpeed = 1.0f; // Velocidad base de los enemigos (puedes ajustar esto según tus necesidades)

            // Mostrar mensaje de inicio de horda
            hordeMessage.text = "Horda " + (currentHorde + 1) + " comenzando...";

            yield return new WaitForSeconds(2.0f); // Mostrar el mensaje por 2 segundos

            int totalEnemies = enemiesPerHorde;
            while (totalEnemies > 0)
            {
                // Spawn base enemies
                SpawnEnemy(enemyPrefab);
                totalEnemies--;

                // Spawn type 2 enemies starting from horde 3
                if (currentHorde >= 3 && totalEnemies > 0)
                {
                    SpawnEnemy(enemyType2Prefab);
                    totalEnemies--;
                }

                // Spawn type 3 enemies starting from horde 5
                if (currentHorde >= 5 && totalEnemies > 0)
                {
                    SpawnEnemy(enemyType3Prefab);
                    totalEnemies--;
                }

                yield return new WaitForSeconds(spawnInterval);
            }

            currentHorde++;
            enemiesPerHorde *= 2; // Multiplicar la cantidad de enemigos por 2 para la próxima horda

            yield return new WaitForSeconds(pauseBetweenHordes); // Pausa entre hordas

            // Verificar si quedan enemigos
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

            // Mostrar pantalla de selección
            ShowSelectionPanel();
            yield return new WaitUntil(() => selectionPanel.activeSelf == false); // Esperar hasta que el jugador haga una elección

            hordeMessage.text = ""; // Limpiar mensaje después de la pausa
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        enemy.tag = "Enemy"; // Asignar etiqueta "Enemy" a cada enemigo instanciado
    }

    private void ShowSelectionPanel()
    {
        PauseGame(); // Pausar el juego
        selectionPanel.SetActive(true);
    }

    public void OnIncreaseSpeedButton()
    {
        player.IncreaseSpeed(speedIncreaseAmount);
        ResumeGame(); // Reanudar el juego
        selectionPanel.SetActive(false);
    }

    public void OnIncreaseDamageButton()
    {
        player.IncreaseDamage(damageIncreaseAmount);
        ResumeGame(); // Reanudar el juego
        selectionPanel.SetActive(false);
    }

    public void OnIncreaseHealthButton()
    {
        player.IncreaseHealth(healthIncreaseAmount);
        ResumeGame(); // Reanudar el juego
        selectionPanel.SetActive(false);
    }

    public void ResetHordes()
    {
        gameRunning = false; // Detener la generación de hordas
        if (hordeCoroutine != null)
        {
            StopCoroutine(hordeCoroutine);
        }
        currentHorde = 0;
        enemiesPerHorde = initialEnemiesPerHorde;

        // Eliminar todos los enemigos existentes
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }

    public void RestartHordeCoroutine()
    {
        gameRunning = true;
        hordeCoroutine = StartCoroutine(SpawnHorde());
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pausar el tiempo del juego
        player.enabled = false; // Deshabilitar el movimiento del jugador
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().enabled = false; // Deshabilitar los scripts de los enemigos
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Reanudar el tiempo del juego
        player.enabled = true; // Habilitar el movimiento del jugador
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().enabled = true; // Habilitar los scripts de los enemigos
        }
    }
}


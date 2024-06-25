using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HordeManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public int initialEnemiesPerHorde = 10; // Cantidad inicial de enemigos por horda
    public float spawnInterval = 1.0f; // Intervalo de tiempo entre spawns
    public float pauseBetweenHordes = 5.0f; // Duración de la pausa entre hordas
    public Transform[] spawnPoints; // Puntos de spawn
    public Text hordeMessage; // Referencia al texto de la UI
    public GameObject selectionPanel; // Panel de selección
    private int currentHorde = 0; // Contador de hordas
    private int enemiesPerHorde; // Cantidad actual de enemigos por horda
    public float speedIncreaseAmount = 1.0f; // Cantidad de incremento de velocidad
    public float damageIncreaseAmount = 5.0f; // Cantidad de incremento de daño
    public PlayerController player;

    void Start()
    {
        enemiesPerHorde = initialEnemiesPerHorde;
        StartCoroutine(SpawnHorde());
    }

    private IEnumerator SpawnHorde()
    {
        while (true)
        {
            float currentSpeed = 1.0f; // Velocidad base de los enemigos (puedes ajustar esto según tus necesidades)

            // Mostrar mensaje de inicio de horda
            hordeMessage.text = "Horda " + (currentHorde + 1) + " comenzando...";

            yield return new WaitForSeconds(2.0f); // Mostrar el mensaje por 2 segundos

            for (int i = 0; i < enemiesPerHorde; i++)
            {
                SpawnEnemy(currentSpeed);
                yield return new WaitForSeconds(spawnInterval);
            }

            currentHorde++;
            enemiesPerHorde *= 2; // Multiplicar la cantidad de enemigos por 2 para la próxima horda

            yield return new WaitForSeconds(pauseBetweenHordes); // Pausa entre hordas

            ShowSelectionPanel();
            yield return new WaitUntil(() => selectionPanel.activeSelf == false);

            hordeMessage.text = ""; // Limpiar mensaje después de la pausa
        }
    }

    private void SpawnEnemy(float speed)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
    }

    private void ShowSelectionPanel()
    {
        selectionPanel.SetActive(true);
    }

    public void OnIncreaseSpeedButton()
    {
        player.IncreaseSpeed(speedIncreaseAmount);
        selectionPanel.SetActive(false);
    }

    public void OnIncreaseDamageButton()
    {
        player.IncreaseDamage(damageIncreaseAmount);
        selectionPanel.SetActive(false);
    }
}
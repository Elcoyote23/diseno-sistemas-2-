using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public PlayerController player;
    public bool isGameStarted = false;

    private bool isPaused = false;

    void Start()
    {
        // Asegurarse de que el menú de pausa esté activado al inicio del juego
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pausar el tiempo del juego al inicio
    }

    void Update()
    {
        if (isGameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
        Resume();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Reanudar el tiempo del juego
        player.enabled = true; // Habilitar el movimiento del jugador
        SetEnemiesActive(true); // Reanudar el movimiento de los enemigos
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pausar el tiempo del juego
        player.enabled = false; // Deshabilitar el movimiento del jugador
        SetEnemiesActive(false); // Pausar el movimiento de los enemigos
        isPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit(); // Salir del juego
    }

    private void SetEnemiesActive(bool isActive)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.enabled = isActive;
            }
        }
    }
}
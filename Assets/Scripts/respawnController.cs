using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class RespawnManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo que respawnea
    public Transform[] spawnPoints; // Puntos de spawn de los enemigos

    public float respawnTime = 3f; // Tiempo de respawn en segundos

    void Start()
    {
        // Inicia el proceso de respawn
        StartCoroutine(RespawnEnemies());
    }

    IEnumerator RespawnEnemies()
    {
        while (true)
        {
            // Espera el tiempo de respawn
            yield return new WaitForSeconds(respawnTime);

            // Elige un punto de spawn aleatorio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instancia un nuevo enemigo en el punto de spawn
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
           
        }
    }
}

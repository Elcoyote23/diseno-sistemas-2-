using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyRespawn : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo a respawnear
    public Transform spawnPoint; // Punto de respawn
    public bool shouldRespawn = true; // Condici�n para respawn, inicialmente true

    void Update()
    {
        // Verificar si la condici�n de respawn es true y no hay enemigos en escena
        if (shouldRespawn && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            // Respawnear enemigo
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Desactivar la condici�n de respawn para evitar respawn continuo
            shouldRespawn = false;
        }
    }
}
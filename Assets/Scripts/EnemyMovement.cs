using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 1.0f;

    void Update()
    {
        // Movimiento básico hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill; // Asegúrate de que esta línea esté presente y sea pública

    public void SetHealth(float healthPercentage)
    {
        Debug.Log("SetHealth called with percentage: " + healthPercentage);
        healthBarFill.fillAmount = healthPercentage;
    }
}


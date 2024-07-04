using UnityEngine;
using UnityEngine.UI;



public class HealthBar : MonoBehaviour
{
    public Slider slider; // Referencia al slider de la barra de salud

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(float health)
    {
        slider.value = health * slider.maxValue;
    }
}




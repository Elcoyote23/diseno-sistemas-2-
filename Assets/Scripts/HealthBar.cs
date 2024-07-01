using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill; // Aseg�rate de que esta l�nea est� presente y sea p�blica

    public void SetHealth(float healthPercentage)
    {
        Debug.Log("SetHealth called with percentage: " + healthPercentage);
        healthBarFill.fillAmount = healthPercentage;
    }
}


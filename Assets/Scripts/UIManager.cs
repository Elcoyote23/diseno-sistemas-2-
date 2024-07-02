using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIManager : MonoBehaviour
{
    public GameObject defeatScreen;

    public void ShowDefeatScreen()
    {
        defeatScreen.SetActive(true);
    }

    public void HideDefeatScreen()
    {
        defeatScreen.SetActive(false);
    }
}

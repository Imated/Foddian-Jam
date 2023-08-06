using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    public bool timerActive = true;

    float timer = 0;

    private void Update()
    {
        if (timerActive)
        {
            timer += Time.deltaTime;

            textMeshProUGUI.text = timer.ToString("0.0");
        }
    }
}
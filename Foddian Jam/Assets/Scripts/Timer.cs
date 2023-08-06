using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    public bool timerActive = true;
    public float timer = 0;

    private void Update()
    {
        if (timerActive)
        {
            timer += Time.deltaTime;

            textMeshProUGUI.text = timer.ToString("F2");
        }
    }
}

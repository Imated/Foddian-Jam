using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    float timer = 0;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    private void Update()
    {
        timer += Time.deltaTime;

        textMeshProUGUI.text = timer.ToString("0.0");
    }
}

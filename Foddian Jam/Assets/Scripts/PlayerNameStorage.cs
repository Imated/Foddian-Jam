using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameStorage : MonoBehaviour
{
    public static string playerName;

    public void readStringInput(string s)
    {
        playerName = s;
        Debug.Log(s);
    }
}

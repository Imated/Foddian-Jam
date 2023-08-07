using System;
using LootLocker.Requests;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class NameSelectionUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    
    private void Awake()
    {
        if(PlayerPrefs.GetInt("SetUsername", 0) == 1)
            gameObject.SetActive(true);
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += StateChange;
#endif
    }
#if UNITY_EDITOR
    void StateChange(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            OnApplicationQuit();
        }
    }
#endif
    
    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) || Input.GetKey(KeyCode.F4))
            OnApplicationQuit();
    }

    public void SetName()
    {
        LootLockerSDKManager.SetPlayerName(nameField.text, (response) =>
        {
            PlayerPrefs.SetInt("SetUsername", 1);
        });
    }
    
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("SetUsername", 0);
    }
}

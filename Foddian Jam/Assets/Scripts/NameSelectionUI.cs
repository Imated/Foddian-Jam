using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class NameSelectionUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    
    public void SetName()
    {
        LootLockerSDKManager.SetPlayerName(nameField.text, (response) =>
        {
            
        });
    }
}

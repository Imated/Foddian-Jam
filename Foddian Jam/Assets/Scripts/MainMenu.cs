using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        TransitionManager.Instance.LoadSceneFade("My Scene DOnt TOuch");
    }

    public void QuitGame()
    {
        TransitionManager.Instance.Exit();
    }
}

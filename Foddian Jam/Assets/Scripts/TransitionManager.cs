using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject loadingStuff;
    [SerializeField] private float fadeDuration;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(transform.root);
    }

    public void LoadSceneFade(string sceneName)
    {
        fadeImage.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            loadingStuff.SetActive(true);
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            StartCoroutine(WaitForSceneLoad(asyncLoad));
        });
    }

    private IEnumerator WaitForSceneLoad(AsyncOperation asyncLoad)
    {
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        fadeImage.DOFade(0f, fadeDuration + 0.25f).OnComplete(() =>
        {
            loadingStuff.SetActive(false);
        });
    }

    public void Exit()
    {
        fadeImage.DOFade(1f, fadeDuration).OnComplete(Application.Quit);
    }
}

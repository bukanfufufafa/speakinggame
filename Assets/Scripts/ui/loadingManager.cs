using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    //[SerializeField] private TMP_Text loadingText;
    [SerializeField] private float fakeLoadDelay = 2f;

    private void Start()
    {
        StartCoroutine(LoadMainSceneAsync());
    }

    private IEnumerator LoadMainSceneAsync()
    {
        yield return new WaitForSeconds(fakeLoadDelay);

        AsyncOperation operation = SceneManager.LoadSceneAsync("tes");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            progressBar.value = progress;
            //loadingText.text = $"Loading... {Mathf.FloorToInt(progress * 100)}%";

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{

    public GameObject loadScreen;
    public Image loadBarFill;
    public string sceneName;

    private void Start()
    {
        StartCoroutine(LoadScene(sceneName));
    }

    public IEnumerator LoadScene(string sceneId)
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(LoadSceneAsync(sceneId));

    }

    IEnumerator LoadSceneAsync(string sceneId)
    {
        AsyncOperation operation =  SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            loadBarFill.fillAmount = progressValue;

            yield return null;
        }
    }
 
}

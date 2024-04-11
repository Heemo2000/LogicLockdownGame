using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]private Image loadPercent;
    private Coroutine _levelCoroutine;
    public void LoadFirstLevel()
    {
        if(_levelCoroutine == null)
        {
            _levelCoroutine = StartCoroutine(LoadLevel("SampleScene"));
        }
    }

    private IEnumerator LoadLevel(string levelName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);

        while(!asyncLoad.isDone)
        {
            loadPercent.fillAmount = asyncLoad.progress;
            yield return null;
        }

        _levelCoroutine = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public string sceneName;
    public int TransisionNumber;
    public bool isTransisionScene = false;

    public void LoadNextScene()
    {
        if (isTransisionScene)
        {
            PlayerPrefs.SetInt("SceneIndex", TransisionNumber);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }

    }
}

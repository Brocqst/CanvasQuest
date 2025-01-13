using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class SceneLoader : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneTransisions(int TransisionNumber)
    {
        if (TransisionNumber < 1 || TransisionNumber > 4)
        { Debug.LogWarning("Transision Number does not exist"); }
        else {
            PlayerPrefs.SetInt("SceneIndex", TransisionNumber);
            SceneManager.LoadScene("TransisionScene");
        }

    }
}

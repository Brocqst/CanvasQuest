using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadSceneTransition : MonoBehaviour
{
    int sceneIndex;
    [SerializeField] Animator transitionAnim;

    private void Start()
    {
        sceneIndex = PlayerPrefs.GetInt("SceneIndex");

        OnPlaySceneAnim(sceneIndex);
    }

    private void OnPlaySceneAnim(int TransisionNumber)
    {
        switch (TransisionNumber)
        {
            case 1:
                transitionAnim.Play("BoatRoute1");
                StartCoroutine(LoadSceneAfterAnim());
                break;
            case 2:
                transitionAnim.Play("BoatRoute2");
                StartCoroutine(LoadSceneAfterAnim());
                break;
            case 3:
                transitionAnim.Play("BoatRoute3");
                StartCoroutine(LoadSceneAfterAnim());
                break;
            case 4:
                transitionAnim.Play("BoatRoute4");
                StartCoroutine(LoadSceneAfterAnim());
                break;
        }
    }

    private System.Collections.IEnumerator LoadSceneAfterAnim()
    {
        yield return new WaitForSeconds(transitionAnim.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("Transision Done");

        if (sceneIndex == 1)
        {
            SceneManager.LoadScene("Dialogue1");
        }
        if (sceneIndex == 2)
        {
            SceneManager.LoadScene("Dialogue2");
        }
        if (sceneIndex == 3)
        {
            SceneManager.LoadScene("Dialogue3");
        }
        if (sceneIndex == 4)
        {
            SceneManager.LoadScene("Dialogue4");
        }

    }

}

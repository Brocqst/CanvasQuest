using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateHandler : MonoBehaviour
{
    public static StateHandler instance;
    public GameObject fadeAnim;
    [SerializeField] Animator animator;
    [SerializeField] GameObject dieSound;
    [SerializeField] GameObject gameWonSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        Instantiate(dieSound);
    }

    public void Win()
    {
        animator.SetTrigger("Win");
        Instantiate(gameWonSound);
    }

    public void Drown()
    {
        animator.SetTrigger("Drown");
        fadeAnim.GetComponent<SceneManagement>().sceneName = "EasterEgg";
        fadeAnim.GetComponent<Animator>().SetTrigger("Out");
    }

    public void NextSceneWin()
    {
        fadeAnim.GetComponent<SceneManagement>().sceneName = "TransisionScene";
        fadeAnim.GetComponent<Animator>().SetTrigger("Out");
    }

    public void NextSceneDie()
    {
        fadeAnim.GetComponent<SceneManagement>().sceneName = "MainMenu";
        fadeAnim.GetComponent<Animator>().SetTrigger("Out");
    }
}

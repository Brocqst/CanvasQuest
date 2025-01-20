using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMainMenu : MonoBehaviour
{
    [SerializeField] Animator sceneTransitions;

    public void BackToMainMenu()
    {
        sceneTransitions.SetTrigger("Out");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPages : MonoBehaviour
{
    [SerializeField] GameObject[] pages;
    int currentPage = 0;

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryItemManager : MonoBehaviour
{
    [SerializeField] GameObject inactivePreview;
    [SerializeField] GameObject activePreview;

    private void Start()
    {
        inactivePreview.SetActive(true);
        activePreview.SetActive(false);
    }

    public void SwitchToActive()
    {
        inactivePreview.SetActive(false);
        activePreview.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public List<GameObject> inventoryItems = new List<GameObject>();
    public List<GameObject> requiredItems = new List<GameObject>();

    public static int numberOfFoundItems = 0;

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

    private void Start()
    {
        numberOfFoundItems = 0;
    }

    public static void AddItem(GameObject item)
    {
        instance.inventoryItems.Add(item);
    }

    private void Update()
    {
        int numberOfFoundItemsTemp = 0;
        foreach (GameObject item in requiredItems)
        {
            if (inventoryItems.Contains(item))
            {
                numberOfFoundItemsTemp++;
                Debug.Log("Got the GameObject");
            }
            else
            {
                Debug.Log("Missing GameObject");
            }
        }
        numberOfFoundItems = numberOfFoundItemsTemp;
    }
}

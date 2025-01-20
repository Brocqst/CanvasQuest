using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] GameObject Vfx;
    [SerializeField] GameObject pickupSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            InventoryManager.AddItem(collision.gameObject);
            collision.gameObject.GetComponent<Item>().targetIcon.SwitchToActive();
            Instantiate(Vfx, collision.gameObject.transform.position, Quaternion.identity);
            Instantiate(pickupSound, collision.gameObject.transform.position, Quaternion.identity);
            collision.gameObject.SetActive(false);
        }
    }
}

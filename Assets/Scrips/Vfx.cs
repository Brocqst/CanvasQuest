using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vfx : MonoBehaviour
{
    private void Start()
    {
        Invoke("DestroyVfx", 5f);
    }

    void DestroyVfx()
    {
        Destroy(gameObject);
    }
}

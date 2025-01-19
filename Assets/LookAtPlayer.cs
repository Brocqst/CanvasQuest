using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] GameObject target;
    void Update()
    {
        Vector3 dir = transform.position - target.transform.position;
        transform.LookAt(dir);
    }
}

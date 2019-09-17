using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    BoxCollider col;
    bool held = false;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!held)
        {
            transform.SetParent(other.transform);
            transform.Translate(new Vector3(0, 1, 0));
            Physics.IgnoreCollision(col, other);
            held = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    BoxCollider col;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.SetParent(other.transform);
        transform.Translate(new Vector3(0, 1, 0));
        Physics.IgnoreCollision(col, other);
    }
}

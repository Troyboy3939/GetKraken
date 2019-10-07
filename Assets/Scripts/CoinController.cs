using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    BoxCollider col;
    [HideInInspector] public bool m_bHeld = false;

    public void SetHeld(bool bHeld)
    {
        m_bHeld = bHeld;
    }
    private void Start()
    {
        col = GetComponent<BoxCollider>();
        Rigidbody rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        if (!m_bHeld)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            if (!m_bHeld)
            {
                
                PlayerController p = other.rigidbody.GetComponentInParent<PlayerController>();
                p.SetHasCoin(true);
                transform.SetParent(other.transform);
                transform.Translate(new Vector3(0, 1, 0));
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.freezeRotation = true;
                rb.constraints = RigidbodyConstraints.FreezeAll;

                BoxCollider bc = GetComponent<BoxCollider>();
                bc.enabled = false;
               
               

                Physics.IgnoreCollision(col, other.rigidbody.GetComponent<Collider>());
                m_bHeld = true;
            }
        }
    }
}


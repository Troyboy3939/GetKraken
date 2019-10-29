using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    BoxCollider col;
    [SerializeField] private float m_fGravityMultiplier;
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

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(m_fGravityMultiplier * Physics.gravity);
    }

    private void OnCollisionEnter(Collision other)
    {
        // If touching a player that is not stunned
        if (other.transform.tag == "Player")
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();

            if (!m_bHeld && !pc.m_bHasCoin && !pc.GetStunned() && pc.m_bCanPickUpCoin)
            {
                pc.SetHasCoin(true);
                transform.SetParent(other.transform);
                transform.Translate(new Vector3(0, 1, 0));


                Rigidbody rb = GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                BoxCollider bc = GetComponent<BoxCollider>();
                bc.enabled = false;
                Physics.IgnoreCollision(col, other.rigidbody.GetComponent<Collider>());
                m_bHeld = true;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_fSpeed = 10;
    Rigidbody m_Controller;
    [SerializeField] int m_nPlayerID = 1;
    [SerializeField] float m_fSphereCastDist = 10;
    [SerializeField] float m_fSphereCastRadius = 1;
    [SerializeField] float m_fShovePower = 10;
    [SerializeField] float m_fStunTime = 10;
    bool m_bStunned = false;
    float m_fTimeWhenStunned = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<Rigidbody>();
    }

    public void Stun()
    {
        if (!m_bStunned)
        {
            m_bStunned = true;
            m_fTimeWhenStunned = Time.time;
        }
    }

    public bool GetStunned()
    {
        return m_bStunned;
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_bStunned)
        {
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Movement
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------

            //If your controller is plugged in
            if (XCI.IsPluggedIn(m_nPlayerID))
            {
                //And if you are using the left or right stick
                if (XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)m_nPlayerID) != 0 || XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)m_nPlayerID) != 0)
                {
                    Vector3 v3InputDir = new Vector3(XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)m_nPlayerID), 0, XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)m_nPlayerID));
                    v3InputDir.Normalize();

                    m_Controller.transform.localRotation = Quaternion.LookRotation(v3InputDir, Vector3.up);
                    m_Controller.velocity = transform.forward * m_fSpeed;
                }
            }
            else //else if controller not connected, use WASD instead
            {
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    Vector3 v3InputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    v3InputDir.Normalize();

                    m_Controller.transform.localRotation = Quaternion.LookRotation(v3InputDir, Vector3.up);
                    m_Controller.velocity = transform.forward * m_fSpeed;


                }
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Shoving
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------

            RaycastHit hit;

            //If button being pressed
            if (XCI.GetButtonDown(XboxButton.B, (XboxController)m_nPlayerID))
            {
                //Do a raycast
                if (Physics.SphereCast(transform.position - (transform.forward * 2), m_fSphereCastRadius, transform.forward, out hit, m_fSphereCastDist))
                {
                    Debug.Log("Hit");

                    Rigidbody hitController = hit.transform.GetComponent<Rigidbody>();

                        PlayerController p = hit.transform.GetComponent<PlayerController>();
                    //hitController.velocity += transform.forward * m_fShovePower;
                    if (!p.GetStunned())
                    {
                        hitController.AddForce(transform.forward * m_fShovePower, ForceMode.VelocityChange);
                        p.Stun();
                    }

                }


            }
        }
        else //If stunned
        {
            if(Time.time - m_fTimeWhenStunned > m_fStunTime)
            {
                m_bStunned = false;
                m_fTimeWhenStunned = 0;
            }
        }
    }
}
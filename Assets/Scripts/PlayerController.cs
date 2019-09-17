using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_fSpeed = 10;
    CharacterController m_Controller;
    [SerializeField] int m_nPlayerID = 1;
    [SerializeField] float m_fRaycastDist;
     // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
    }
    
    // Update is called once per frame
    void Update()
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
                m_Controller.SimpleMove(m_Controller.transform.forward * m_fSpeed);
            }
        }
        else //else if controller not connected, use WASD instead
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector3 v3InputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                v3InputDir.Normalize();

                m_Controller.transform.localRotation = Quaternion.LookRotation(v3InputDir, Vector3.up);
                m_Controller.SimpleMove(m_Controller.transform.forward * m_fSpeed);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Shoving
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        //RaycastHit hit;
        //LayerMask layerMask = LayerMask.GetMask(,);
        
        ////If button being pressed
        //if (XCI.GetButtonDown(XboxButton.B))
        //{
        //    //Do a raycast
        //    if (Physics.Raycast(transform.position, transform.forward,out hit, m_fRaycastDist, layerMask))
        //    {

        //    }
        //}


    }
}

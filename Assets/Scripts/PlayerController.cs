using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_fSpeed = 10;
    CharacterController m_Controller;
    // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    { 
        if(XCI.GetAxis(XboxAxis.LeftStickX) != 0 || XCI.GetAxis(XboxAxis.LeftStickY) != 0)
        {
            Vector3 v3InputDir = new Vector3(XCI.GetAxis(XboxAxis.LeftStickX),0, XCI.GetAxis(XboxAxis.LeftStickY));
            v3InputDir.Normalize();

            m_Controller.transform.localRotation = Quaternion.LookRotation(v3InputDir,Vector3.up);
            m_Controller.SimpleMove(m_Controller.transform.forward * m_fSpeed);
        }
    }
}

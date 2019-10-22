using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{

    //[SerializeField] float m_fLerpStart = 0.0f;
    float m_fT = 0.0f;
    bool m_bSwitch = false;
    [SerializeField] float m_fSpeed = 0.005f;
    [SerializeField] float m_fStartY = 0.0f;
    [SerializeField] float m_fPositionSinModifier1 = 0.0f;
    [SerializeField] float m_fPositionSinModifier2 = 0.0f;
    [SerializeField] float m_RotationStart = 82.0f;
    [SerializeField] float m_RotationEnd = 101.0f;
    float Lerp(float fStart, float fEnd, float fT)
    {
        return fStart * (1 - fT) + fEnd * fT;
    }



    // Start is called before the first frame update
    void Start()
    {
        
        m_fT = Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        float fRot = Lerp(m_RotationStart, m_RotationEnd, m_fT);

        




        
        
       
        transform.position = new Vector3( transform.position.x, m_fPositionSinModifier1 * Mathf.Sin(m_fT / m_fPositionSinModifier2) + m_fStartY, transform.position.z);
        transform.rotation = Quaternion.Euler(0,0,fRot);

        if(m_fT > 1)
        {
            m_bSwitch = true;
        }
        else if(m_fT < 0)
        {
            m_bSwitch = false;
        }

        if(m_bSwitch)
        {
            m_fT -= m_fSpeed * Time.deltaTime;
        }
        else
        {
            m_fT += m_fSpeed * Time.deltaTime;
        }

    }
}

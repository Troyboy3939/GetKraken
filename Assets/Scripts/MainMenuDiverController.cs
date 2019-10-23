using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuDiverController : MonoBehaviour
{
    bool m_bGo = false;
    [SerializeField] Vector3 m_v3Start;
    [SerializeField] Vector3 m_v3End;
    float m_fT;
    [SerializeField] float m_fSpeed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_bGo)
        {
            transform.position = Vector3.Lerp(m_v3Start, m_v3End, m_fT);

            m_fT += m_fSpeed * Time.deltaTime;
        }
    }

    public void Go()
    {
        m_bGo = true;
    }
}

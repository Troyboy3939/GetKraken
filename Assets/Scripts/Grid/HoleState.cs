using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleState : State
{
    GameObject m_Plane;
    Vector3 m_v3Position;
    public HoleState(ref GameObject plane)
    {
        m_Plane = plane;
        OnEnter();
    }

    public override void OnEnter()
    {
        if (!m_Plane)
        {
            if (m_Plane.activeInHierarchy)
            {
                m_Plane.SetActive(false);
            }
        }
    }

    public override void OnExit()
    {

    }

    public override void Update()
    {

    }
}
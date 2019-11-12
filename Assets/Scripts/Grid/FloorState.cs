using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  public class FloorState : State
{
    GameObject m_Plane;
    Vector3 m_v3Position;

    public FloorState(ref GameObject plane)
    {
        m_Plane = plane;
        OnEnter();
        m_v3Position = m_Plane.transform.position;
    }
    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }
    public override void Update()
    {

    }

}




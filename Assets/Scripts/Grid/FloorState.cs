using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  public class FloorState : State
{
    GameObject m_Plane;
    Vector3 m_v3Position;
    bool m_bHasTentacle = false;
    bool m_bHasChest = false;

    public FloorState(ref GameObject plane)
    {
        m_Plane = plane;
        OnEnter();
        m_v3Position = m_Plane.transform.position;
    }
    public override void OnEnter()
    {
        m_Plane.SetActive(true);
    }

    public override void OnExit()
    {
        m_Plane.SetActive(false);
    }
    public override void Update()
    {

    }

    public bool GetHasTentacle()
    {
        return m_bHasTentacle;
    }

    public bool GetHasChest()
    {
        return m_bHasChest;
    }

    public void SetHasTentacle(bool b)
    {
        m_bHasTentacle = b;
    }

    public void SetHasChest(bool b)
    {
        m_bHasChest = b;
    }
}


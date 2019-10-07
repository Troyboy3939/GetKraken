using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleState : State
{
    GameObject m_Tentacle;
    GameObject m_Plane;
    Vector3 m_v3Position;
    public TentacleState(Vector3 pos,  GameObject plane)
    {
        m_v3Position = pos;
        m_Plane = plane;
        m_Tentacle = GameObject.Instantiate<GameObject>(Blackboard.GetInstance().GetTentacle(),m_v3Position,new Quaternion(0,0,0,0));
        m_Tentacle.SetActive(false);
  
    }

    public override void OnEnter()
    {
        m_Tentacle.SetActive(true);
    }

    public override void OnExit()
    {
        m_Tentacle.SetActive(false);
    }

    public override void Update()
    {

    }
}


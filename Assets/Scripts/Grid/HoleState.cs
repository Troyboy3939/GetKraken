using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleState : State
{
    GameObject m_Plane = null;
    Vector3 m_v3Position;
    bool m_bFirst = true;
    GameObject m_Hole;
   // Material m_OldMat = null;
    public HoleState(GameObject plane)
    {
        m_Plane = plane;
        

    }

    public override void OnEnter()
    {
        if (m_bFirst)
        {
            GameObject.FindGameObjectWithTag("Hole");
            m_Hole = GameObject.Instantiate<GameObject>(GameObject.FindGameObjectWithTag("Hole"),m_Plane.transform.position,new Quaternion(0,0,0,0));
        }
        MeshRenderer m = m_Hole.GetComponent<MeshRenderer>();
        m_Plane.SetActive(true);

        if (m != null)
        {
            m.enabled = true;
          
        }
    }

    public override void OnExit()
    {
       
    }

    public override void Update()
    {

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TentacleState : State
{
    GameObject m_Tentacle;
    GameObject m_Plane;
    Vector3 m_v3Position;
    bool m_bFirstTime = true;

    public Animator anim;

    public TentacleState(Vector3 pos,  GameObject plane)
    {
        m_v3Position = pos;
        m_Plane = plane;
        //m_Tentacle.SetActive(false);
  
    }

    public override void OnEnter()
    {
        //Code that is done the first time a tentacle is spawned / state switched into . This first branch of the if statement is basically a constructor
        if (m_bFirstTime)
        {
            //Creation of the actual tentacle
            m_Tentacle = GameObject.Instantiate<GameObject>(Blackboard.GetInstance().GetTentacle(), m_v3Position, new Quaternion(0, 0, 0, 0));
            anim = m_Tentacle.GetComponent<Animator>();
            

            //No longer go through this branch
            m_bFirstTime = false;


            //anim.bodyRotation = new Quaternion(0, , 0, 0);
            m_Tentacle.transform.Rotate(Vector3.up, Random.Range(1, 4) * 90.0f);
            anim.Play("Rise Up");
            
        }
        else
        {
            m_Tentacle.SetActive(true);
            anim.Play("Rise Up");       
        }
    }

    public override void OnExit()
    {
        m_Tentacle.SetActive(false);
    }

    public override void Update()
    {
        
    }
}


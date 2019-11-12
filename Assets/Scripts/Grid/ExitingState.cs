using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitingState : State
{
    bool m_bFirstTime = true;
    StateMachine m_StateMachine;
    GameObject m_Tentacle;
    Vector2 m_v2NodePos;
    public Animator m_Anim;
    public ExitingState(StateMachine stateMachine, Vector2 v2NodePos)
    {
        m_StateMachine = stateMachine;
        m_v2NodePos = v2NodePos;
    }
    public override void OnEnter()
    {
        //Debug.Log("Exit: OnEnter()");
        if (m_bFirstTime)
        {
            TentacleState tentacleState = m_StateMachine.GetTentacleState();
            m_Tentacle = tentacleState.GetTentacle();
            m_bFirstTime = false;
            m_Anim = tentacleState.GetAnimator();
        }
        else
        {

        }

        MeshCollider m = m_Tentacle.GetComponent<MeshCollider>();

        if(m != null)
        {
            m.enabled = true;
        }
        
        m_Anim.SetTrigger("Exit");
        
       

    }


    public override void OnExit()
    {
        m_Anim.ResetTrigger("Exit");
        m_Anim.ResetTrigger("Attack");
        MeshCollider m = m_Tentacle.GetComponent<MeshCollider>();

        if (m != null)
        {
            m.enabled = false;
        }

        //m_Tentacle.transform.Translate(new Vector3(0, -1, 0), Space.Self);
    }

    public override void Update()
    {
        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Exit") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Exit Vertical"))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !m_Anim.IsInTransition(0))
            {
                // if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Vertical Idle") && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Lay Down Idle"))
                // {
               // Debug.Log("Exit: Exiting");
                m_StateMachine.ChangeState(StateMachine.ESTATE.HOLE);
                List<Vector2> l = Blackboard.GetInstance().GetGrid().GetTentaclePos();
                if (l.Contains(m_v2NodePos))
                {
                    l.Remove(m_v2NodePos);
                }

                Blackboard.GetInstance().GetGrid().SetTentaclePos(l);
                //  }
            }
        }

        //if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Exit Vertical"))
        //{

        //}

    }


}

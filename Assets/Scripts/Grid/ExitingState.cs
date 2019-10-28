using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitingState : State
{
    bool m_bFirstTime = true;
    StateMachine m_StateMachine;
    GameObject m_Tentacle;
    public Animator m_Anim;
    public ExitingState(StateMachine stateMachine)
    {
        m_StateMachine = stateMachine;

    }
    public override void OnEnter()
    {
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

        m_Anim.SetTrigger("Exit");
        
       

    }


    public override void OnExit()
    {
        m_Anim.ResetTrigger("Exit");
    }

    public override void Update()
    {
        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Exit") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Exit Vertical"))
        {
            if(m_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                m_StateMachine.ChangeState(StateMachine.ESTATE.HOLE);
            }
        }
    }


}

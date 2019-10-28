using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StateMachine
{
    //-----------------------------------------------------------
    //ESTATE
    //
    //-----------------------------------------------------------
    public enum ESTATE
    {
        FLOOR,
        HOLE,
        TENTACLE,
        EXITING
    };

    private FloorState m_FloorState;
    private HoleState m_HoleState;
    private TentacleState m_TentacleState;
    private ExitingState m_ExitingState;
    ESTATE m_eState = ESTATE.FLOOR;
    public Vector2 m_v2NodePos;

    private Vector3 m_v3Position;
    private GameObject m_BasePlane;

    public StateMachine(Vector3 pos, ref GameObject plane, Vector2 vec)
    {
        m_TentacleState = new TentacleState(pos, plane, vec);
        m_FloorState =  new FloorState(ref plane);
        m_HoleState = new HoleState(ref plane);
        m_ExitingState = new ExitingState(this);
    }

   public ref FloorState GetFloorState()
    {
        return ref m_FloorState;
    }

    public TentacleState GetTentacleState()
    {
        return m_TentacleState;
    }

    public void ChangeState(StateMachine.ESTATE nextState)
    {
        if (nextState == m_eState)
            Debug.LogError("Changing to same state: " + nextState);

        //Exit the state you are currently and enter the next state
        switch (m_eState)
        {
            case ESTATE.FLOOR:
                m_FloorState.OnExit();
                break;
            case ESTATE.HOLE:
                m_HoleState.OnExit();
                break;
            case ESTATE.TENTACLE:
                m_TentacleState.OnExit();
                break;
            case ESTATE.EXITING:
                m_ExitingState.OnExit();
                break;
        }

        //Enter
        switch(nextState)
        {
            case ESTATE.FLOOR:
                m_FloorState.OnEnter();
                m_eState = ESTATE.FLOOR;
                break;
            case ESTATE.HOLE:
                m_HoleState.OnEnter();
                m_eState = ESTATE.HOLE;
                break;
            case ESTATE.TENTACLE:
                m_TentacleState.OnEnter();
                m_eState = ESTATE.TENTACLE;
                break;
            case ESTATE.EXITING:
                m_ExitingState.OnEnter();
                m_eState = ESTATE.EXITING;
                break;
        }
    }

    public ESTATE GetState()
    {
        return m_eState;
    }

    public void Update()
    {
        switch (m_eState)
        {
            case ESTATE.FLOOR:
                m_FloorState.Update();
                break;

            case ESTATE.HOLE:
                m_HoleState.Update();
                break;
            case ESTATE.TENTACLE:
                m_TentacleState.Update();
                break;
            case ESTATE.EXITING:
                m_ExitingState.Update();
                break;
        }
    }
}
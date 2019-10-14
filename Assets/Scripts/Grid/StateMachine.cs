using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        TENTACLE
    };




    private FloorState m_FloorState;
    private HoleState m_HoleState;
    private TentacleState m_TentacleState;
    ESTATE m_eState = ESTATE.FLOOR;


    private Vector3 m_v3Position;
    private GameObject m_BasePlane;


    public StateMachine(Vector3 pos, ref GameObject plane, Vector2 vec)
    {
        m_TentacleState = new TentacleState(pos, plane, vec);
        m_FloorState =  new FloorState(ref plane);
        m_HoleState = new HoleState(ref plane);
    }
   public ref FloorState GetFloorState()
    {
        return ref m_FloorState;
    }

    public TentacleState GetTentacleState()
    {
        return m_TentacleState;
    }

    public void ChangeState()
    {

        //Exit the state you are currently and enter the next state
        switch (m_eState)
        {
            case ESTATE.FLOOR:
                m_FloorState.OnExit();
                m_HoleState.OnEnter();
                m_eState = ESTATE.HOLE;
                break;
            case ESTATE.HOLE:
                m_HoleState.OnExit();
                m_TentacleState.OnEnter();
                m_eState = ESTATE.TENTACLE;
                break;
            case ESTATE.TENTACLE:
                m_TentacleState.OnExit();
                m_HoleState.OnEnter();
                m_eState = ESTATE.HOLE;
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
        }
    }
}
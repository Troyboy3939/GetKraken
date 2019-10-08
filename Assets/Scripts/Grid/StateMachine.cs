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
    private HoleState m_HoleState = new HoleState();
    private TentacleState m_TentacleState;
    ESTATE m_eState = ESTATE.FLOOR;


    private Vector3 m_v3Position;
    private GameObject m_BasePlane;


    public StateMachine(Vector3 pos, ref GameObject plane)
    {
        m_TentacleState = new TentacleState(pos, plane);
        m_FloorState =  new FloorState(ref plane);
    }
   public ref FloorState GetFloorState()
    {
        return ref m_FloorState;
    }
    public void ChangeState(ESTATE eState)
    {

        //Exit the state you are currently in
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
        }



        switch (eState)
        {
            case ESTATE.FLOOR:
                m_FloorState.OnEnter();
                break;
            case ESTATE.HOLE:
                m_HoleState.OnEnter();
                break;
            case ESTATE.TENTACLE:
                m_TentacleState.OnEnter();
                break;
        }

        //Change State
        m_eState = eState;
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
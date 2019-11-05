using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tentacle should spawn in random positions on the grid, in two different states.
// If player collides with tentacle, player will be killed and tentacle will be hidden (handle player killing and respawning in PlayerController).
// After a short time, tentacle should also move to a different spot on the grid.
public class TentacleController : MonoBehaviour
{
    TentacleState m_TentacleState = null;
    StateMachine m_StateMachine;

    void Start()
    {
  
    }

    public TentacleState GetTentacleState()
    {
        return m_TentacleState;
    }

    public void SetTentacleState(TentacleState state)
    {
        m_TentacleState = state;
    }


    public StateMachine GetStateMachine()
    {
        return m_StateMachine;
    }

    public void SetStateMachine(StateMachine state)
    {
        m_StateMachine = state;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tentacle should spawn in random positions on the grid, in two different states.
// If player collides with tentacle, player will be killed and tentacle will be hidden (handle player killing and respawning in PlayerController).
// After a short time, tentacle should also move to a different spot on the grid.
public class TentacleController : MonoBehaviour
{
    [SerializeField] float m_fRespawnTime = 2;
    ESTATE m_eTentacleState;

    private enum ESTATE
    {
        VERTICAL,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    void Start()
    {
        // TODO: Set up a loop that checks whether or not the current state is possible,
        // eg. randomise again if the tentacle is ESTATE.DOWN, but there's already a tentacle
        // in the way. Detect this with a raycast maybe

        // Tentacle will be in a random state when spawned
        m_eTentacleState = (ESTATE)Random.Range(0, 4);

        switch(m_eTentacleState)
        {
            case ESTATE.VERTICAL:
                // tentacle sticks straight up
                break;
            case ESTATE.UP:
                // tentacle makes a wall upwards
                break;
            case ESTATE.DOWN:
                // tentacle makes a wall downwards
                break;
            case ESTATE.LEFT:
                // tentacle makes a wall to the left
                break;
            case ESTATE.RIGHT:
                // tentacle makes a wall to the right
                break;
        }
    }

    // Hide the tentacle and start a timer
    // Check that you're not trying to do the same thing across different scripts
    private void Hide()
    {

    }
}

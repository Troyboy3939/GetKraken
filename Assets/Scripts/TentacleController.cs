using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tentacle should spawn in random positions on the grid, in two different states.
// If player collides with tentacle, player will be killed (handle player killing and respawning in PlayerController)
// When this happens, tentacle should also move to a different spot on the grid
public class TentacleController : MonoBehaviour
{
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // kill player, should be a full animation in the full game but
            // for alpha just despawn both player and tentacle
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    // This should match the ID of the player that can deposit coins into this chest
    // Should be manually set in the inspector
    public int m_nPlayerID = 0;
    [SerializeField] GameObject m_bGrid;
    void Start()
    {
        Debug.Assert(m_nPlayerID > 0, "Player ID has not been set on " + gameObject.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

            // Destroy the coin and update score
            if (pc.m_bHasCoin && pc.m_nPlayerID == m_nPlayerID)
            {
                FloorGrid grid = m_bGrid.GetComponent<FloorGrid>();
                grid.SetCoinCount(grid.GetCoinCount() - 1);
                Destroy(pc.gameObject.GetComponentInChildren<CoinController>().gameObject);
                pc.m_bHasCoin = false;
                UIController uic = Blackboard.GetInstance().GetCanvas().GetComponent<UIController>();
                switch (m_nPlayerID)
                {
                    // add to score based on which player it is
                    case 1:
                        uic.m_nP1Score++;
                        break;
                    case 2:
                        uic.m_nP2Score++;
                        break;
                    default:
                        Debug.LogError("Player ID is out of currently defined range.");
                        break;
                }

                uic.UpdateScore();
            }
        }
    }
}

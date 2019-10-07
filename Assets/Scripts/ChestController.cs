using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    // This should match the ID of the player that can deposit coins into this chest
    // Should be manually set in the inspector
    public int m_nPlayerID = 0;

    void Start()
    {
        Debug.Assert(m_nPlayerID > 0, "Player ID has not been set on " + gameObject.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

        if (pc.m_bHasCoin && pc.m_nPlayerID == m_nPlayerID)
        {
            Destroy(pc.gameObject.GetComponentInChildren<CoinController>().gameObject);
            pc.m_bHasCoin = false;
            // Add to the score when we get to that
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    // This should match the ID of the player that can deposit coins into this chest
    // Should be manually set in the inspector
    public int m_nPlayerID = 0;
    [SerializeField] GameObject m_bGrid;
    private Animation chestAnimation;
    [SerializeField] string m_szColour = "";

    public ParticleSystem coinSplash;
    private AudioSource[] m_AudioSources;

    void Start()
    {
        Debug.Assert(m_nPlayerID > 0, "Player ID has not been set on " + gameObject.name);
        chestAnimation = GetComponentInChildren<Animation>();

        GameObject calOb = GameObject.FindGameObjectWithTag("Calibration");
        m_AudioSources = GetComponents<AudioSource>();

        Calibration c = null;
        if (calOb != null)
        {
            c = calOb.GetComponent<Calibration>();
        }

        coinSplash = GetComponentInChildren<ParticleSystem>();


            MeshRenderer m = GetComponent<MeshRenderer>();
            if(c != null)
            {
                if (m_szColour == "Blue")
                {
                    m_nPlayerID = c.GetBlueID();
                   // m.material = Blackboard.GetInstance().GetBlueChestMat();
                }
                else if(m_szColour == "Green")
                {
                    m_nPlayerID = c.GetGreenID();
                   // m.material = Blackboard.GetInstance().GetGreenChestMat();
                }
                else if (m_szColour == "Yellow")
                {
                    m_nPlayerID = c.GetYellowID();
                    //m.material = Blackboard.GetInstance().GetYellowChestMat();
                }
                else if (m_szColour == "Orange")
                {
                    m_nPlayerID = c.GetOrangeID();
                   // m.material = Blackboard.GetInstance().GetOrangeChestMat();
                }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();


            Animator a = collision.gameObject.GetComponent<Animator>();
            
            if(a != null)
            {
                a.SetTrigger("DropCoin");
            }

            // Destroy the coin and update score
            if (pc.m_bHasCoin && pc.m_nPlayerID == m_nPlayerID)
            {
                FloorGrid grid = m_bGrid.GetComponent<FloorGrid>();
                grid.SetCoinCount(grid.GetCoinCount() - 1);

                
                Destroy(pc.gameObject.GetComponentInChildren<CoinController>().gameObject);
                pc.m_bHasCoin = false;
                pc.m_bCanPickUpCoin = true;
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
                    case 3:
                        uic.m_nP3Score++;
                        break;
                    case 4:
                        uic.m_nP4Score++;
                        break;
                    default:
                        Debug.LogError("Player ID is out of currently defined range.");
                        break;
                }

                foreach (AudioSource source in m_AudioSources) source.Play();
                coinSplash.Play();
                chestAnimation.Play("ChestOpen");
                uic.UpdateScore();
            }
        }
    }
}

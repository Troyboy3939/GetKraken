using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TentacleState : State
{
    GameObject m_Tentacle;
    GameObject m_Plane;
    Vector3 m_v3Position;
    bool m_bFirstTime = true;
    Vector2 m_v2NodePos;
    List<Vector2> m_AdjacentNodes = new List<Vector2>();
    public Animator anim;
    ETENTACLESTATE m_eState;
    enum ETENTACLESTATE
    {
        VERTICAL,
        HORIZONTAL
    }

    public TentacleState(Vector3 pos,  GameObject plane, Vector2 vecPos)
    {
        m_v3Position = pos;
        m_Plane = plane;
        //m_Tentacle.SetActive(false);
        m_v2NodePos = vecPos;
    }

    public override void OnEnter()
    {
        //Code that is done the first time a tentacle is spawned / state switched into . This first branch of the if statement is basically a constructor
        if (m_bFirstTime)
        {
            //Creation of the actual tentacle
            m_Tentacle = GameObject.Instantiate<GameObject>(Blackboard.GetInstance().GetTentacle(), m_v3Position, new Quaternion(0, 0, 0, 0));
            anim = m_Tentacle.GetComponent<Animator>();

            AnimationPlayer();

            //No longer go through this branch
            m_bFirstTime = false;
        }
        else
        {
            m_Tentacle.SetActive(true);

            AnimationPlayer();

            if(Blackboard.GetInstance().GetNode(m_v2NodePos) != null)
            {
                Blackboard.GetInstance().GetNode(m_v2NodePos).SetHasTentacle(true);
            }
            else
            {
                ////Debug.Log("On Enter set own node has tentacle true returned null. Line 43");
            }
        }
    }

    void AnimationPlayer()
    {
        //Pick random state based on percentage on blackboard

        int n = Random.Range(1, 100);

        Blackboard instance = Blackboard.GetInstance();

        if (n < instance.GetTentacleVerticalPercentage())
        {
            m_eState = ETENTACLESTATE.VERTICAL;
        }
        else
        {
            m_eState = ETENTACLESTATE.HORIZONTAL;
        }

        switch (m_eState)
        {
            case ETENTACLESTATE.HORIZONTAL:
                StartUpRotation();
                break;
            case ETENTACLESTATE.VERTICAL:
                anim.Play("Rise Up Vertical");
                break;
        }
    }

    private void StartUpRotation()
    {
        float tentacleRotation = Random.Range(1, 4) * 90.0f;

        bool bValid = false;
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < m_AdjacentNodes.Count; i++)
            {
                Blackboard.GetInstance().GetNode(m_AdjacentNodes[i]).SetHasTentacle(false);
            }
            m_AdjacentNodes.Clear();

            bool bBreak = false;
            for (int i = 1; i < 3; i++)
            {
                Vector2 vec;
                if (tentacleRotation == 0) //if you are facing forwards
                {
                    vec = new Vector2(m_v2NodePos.x, m_v2NodePos.y + i);
                }
                else if (tentacleRotation == 90)
                {
                    vec = new Vector2(m_v2NodePos.x + i, m_v2NodePos.y);
                }
                else if (tentacleRotation == 180)
                {
                    vec = new Vector2(m_v2NodePos.x, m_v2NodePos.y - i);
                }
                else
                {
                    vec = new Vector2(m_v2NodePos.x - i, m_v2NodePos.y);
                }

                if (vec.x < 0 || vec.y < 0)
                {
                    break;
                }
                if (Blackboard.GetInstance().GetNode(vec) != null)
                {
                    if (!Blackboard.GetInstance().GetNode(vec).GetHasTentacle()) // check if node has tentacle on it
                    {
                        if (!Blackboard.GetInstance().GetNode(vec).GetHasChest()) //check if node has a chest on top
                        {
                            if (!Blackboard.GetInstance().GetNode(vec).GetIsBuffer()) //check if node is a buffer zone
                            {

                                Blackboard.GetInstance().GetNode(vec).SetHasTentacle(true);
                                m_AdjacentNodes.Add(vec);
                                bBreak = true;
                                bValid = true;

                            }
                            else
                            {
                                bBreak = false;
                                break;
                            }
                        }
                        else
                        {
                            bBreak = false;
                            break;
                        }
                    }
                    else
                    {
                        bBreak = false;
                        break;
                    }
                }
            }

            if (bBreak)
            {
                break;
            }

            tentacleRotation += 90;

        }

        if (!bValid)
        {
            m_eState = ETENTACLESTATE.VERTICAL;
            anim.Play("Rise Up Vertical");
            m_Tentacle.SetActive(false);
        }
        else
        {
            m_Tentacle.transform.rotation = Quaternion.AngleAxis(tentacleRotation, Vector3.up);
            anim.Play("Rise Up");
        }
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
    }

    public void Reset()
    {
        if (m_Tentacle)
        {
            m_Tentacle.SetActive(false);

            for (int i = 0; i < m_AdjacentNodes.Count; i++)
            {
                if (Blackboard.GetInstance().GetNode(m_AdjacentNodes[i]) != null)
                {
                    Blackboard.GetInstance().GetNode(m_AdjacentNodes[i]).SetHasTentacle(false);
                }
                else
                {
                    //Debug.Log("Reset tentacle state adjacent node returned null. Line 164");
                }

                if (Blackboard.GetInstance().GetNode(m_v2NodePos) != null)
                {
                    Blackboard.GetInstance().GetNode(m_v2NodePos).SetHasTentacle(false);
                }
                else
                {
                    //Debug.Log("Reset tentacle state own node returned null. Line 173");
                }
            }
            m_AdjacentNodes.Clear();
        }
    }
}


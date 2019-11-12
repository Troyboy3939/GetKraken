using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FloorGrid : MonoBehaviour
{
    //-----------------------------------------------------------
    //Variables
    //-----------------------------------------------------------
    [SerializeField] int m_nGridWidth = 10;
    [SerializeField] int m_nGridHeight = 10;
    [SerializeField] GameObject m_Plane = null;
    [SerializeField] GameObject m_Tentacle = null;
    [SerializeField] GameObject m_Coin = null;
    [SerializeField] float m_fDropHeight = 30;
    [SerializeField] float m_fTentacleSwitchTime = 3;
    [SerializeField] float m_fCoinSpawnTime = 3;
    [SerializeField] List<Vector2> m_HolePositions = new List<Vector2>();
    [SerializeField] List<Vector2> m_CoinSpawnBlacklist = new List<Vector2>();
    int m_nRandomSeed = 0;
    List<Vector2> m_TentaclePositions = new List<Vector2>();
    
    float m_fTentacleTimer = 9.0f;
    float m_fCoinTimer = 0.0f;
    bool m_bFirstTime = true;
    bool m_bSwitch = false;
    int m_nCoinCount = 0;
    Node[,] m_Nodes;

    private Node posChest0;
    private Node posChest1;
    private Node posChest2;
    private Node posChest3;

    // Only use this field for debugging! Should always be true when testing the game.
    [SerializeField] private bool m_bLimitCoinCount = true;


    //-----------------------------------------------------------
    //Functions
    //-----------------------------------------------------------



    // Start is called before the first frame update
    void Start()
    {
        
        Random.InitState(System.DateTime.Now.Millisecond);

        m_Nodes = new Node[m_nGridWidth,m_nGridHeight];

        for(int x = 0; x < m_nGridWidth; x++)
        {
            for(int y = 0;  y < m_nGridHeight; y++)
            {

                
                Vector3 size = m_Plane.GetComponent<Collider>().bounds.size;

                
                m_Nodes[x, y] = new Node(new Vector3(x * size.x, 2.25f, y * size.z) ,m_Plane,new Vector2(x,y));
            
                
            }
        }

        Blackboard.GetInstance().SetNodes(ref m_Nodes);
        Blackboard.GetInstance().SetGrid(this);

        for(int i = 0; i < Blackboard.GetInstance().GetChestCount(); i++)
        {
            GetNodeByPosition(Blackboard.GetInstance().GetChest(i).transform.position).SetHasChest(true);

        }
       
        for (int i = 0; i < m_HolePositions.Count; i++)
        {
           m_Nodes[Mathf.FloorToInt(m_HolePositions[i].x), Mathf.FloorToInt(m_HolePositions[i].y)].ChangeState(StateMachine.ESTATE.HOLE);
        }

       
    }


    public List<Vector2> GetTentaclePos()
    {
        return m_TentaclePositions;
    }

    public void SetTentaclePos(List<Vector2> l)
    {
        m_TentaclePositions = l;
    }

    void PlayRiseUp()
    {
        for(int i = 0;  i < m_TentaclePositions.Count; i++)
        {
            //m_Nodes[m_TentaclePositions[i].x, m_TentaclePositions[i].y].OnEnter()
        }
    }

    public void SetCoinCount(int nNumber)
    {
        m_nCoinCount = nNumber;
    }

    public int GetCoinCount()
    {
        return m_nCoinCount;
    }



    public Node GetNodeByPosition(Vector3 v3Pos)
    {

        Collider col = m_Plane.GetComponent<Collider>();
        float x = col.bounds.size.x;

       


        float fW = (v3Pos.x + 1)  /x;
        if (fW < 0)
        {
            fW *= -1;
        }
        int nWidth = Mathf.FloorToInt(fW);
        

        

        //float fH = toPos.z / col.bounds.size.x;

        float fH = (v3Pos.z + 1)  /x;
        if(fH < 0)
        {
            fH *= -1;
        }

        int nHeight = Mathf.FloorToInt(fH);

        if (nWidth > m_Nodes.GetLength(0) || nWidth < 0 || nHeight > m_Nodes.GetLength(1) || nHeight < 0)
        {
            Debug.Log("Index of array out of bounds");
            return m_Nodes[0,0];
        }
        return m_Nodes[nWidth, nHeight]; 
    }

    //----------------------------------------------------------------------------------------------------
    //DropObjectAtNode
    //Will move gameobject above the node to be dropped. Object must have physics for gravity so that it falls
    //----------------------------------------------------------------------------------------------------
    public void DropObjectAtNode(Node node, GameObject gameObject)
    {
        Vector3 v3Pos = node.GetPosition();
        gameObject.transform.position = v3Pos;
        gameObject.transform.Translate(Vector3.up * m_fDropHeight, Space.World);

    }

    public void DropObjectAtNode(Node node,  Transform transform)
    {
        Vector3 v3Pos = node.GetPosition();
        transform.position = v3Pos;
        transform.Translate(Vector3.up * m_fDropHeight, Space.World);
    }





    //----------------------------------------------------------------------------------------------------
    //DropNewObjectAtNode
    //Object is cloned and dropped at the node. Object must have physics for gravity so that it falls
    //----------------------------------------------------------------------------------------------------
    public void DropNewObjectAtNode(Node node, GameObject gameObject)
    {
        Vector3 v3Pos = node.GetPosition();
        GameObject go = Instantiate<GameObject>(gameObject,v3Pos,new Quaternion(0,0,0,0));
        DropObjectAtNode(node, go);
    }

    public void DropCoin()
    {
        int n1 = Random.Range(0, m_nGridWidth);
        int n2 = Random.Range(0, m_nGridHeight);

        bool bValid = false;

        GameObject[] chests = new GameObject[5];
        chests = GameObject.FindGameObjectsWithTag("Chest");



        while (!bValid)
        {

            for (int i = 0; i < chests.Length; i++)
            {

                Node pos = GetNodeByPosition(chests[i].transform.position);


                if (m_Nodes[n1, n2].GetPosition() == pos.GetPosition() || m_Nodes[n1, n2].GetState() == StateMachine.ESTATE.HOLE ||  m_Nodes[n1, n2].GetState() == StateMachine.ESTATE.TENTACLE || m_Nodes[n1, n2].GetHasTentacle())
                {
                    n1 = Random.Range(0, m_nGridWidth);
                    n2 = Random.Range(0, m_nGridHeight);
                }

            }

            posChest0 = GetNodeByPosition(chests[0].transform.position);
            posChest1 = GetNodeByPosition(chests[1].transform.position);

            if (chests.Length == 3)
            {
                posChest2 = GetNodeByPosition(chests[2].transform.position);
            }

            if (chests.Length == 4)
            {
                posChest3 = GetNodeByPosition(chests[3].transform.position);
            }

            if (m_Nodes[n1, n2].GetPosition() != posChest0.GetPosition())
            {
                if (m_Nodes[n1, n2].GetPosition() != posChest1.GetPosition())
                {
                    if (posChest2 != null && m_Nodes[n1, n2].GetPosition() != posChest2.GetPosition())
                    {
                        if (posChest3 != null && m_Nodes[n1, n2].GetPosition() != posChest3.GetPosition())
                        {
                            if(m_Nodes[n1, n2].GetState() != StateMachine.ESTATE.HOLE)
                            {
                                if(m_Nodes[n1, n2].GetState() != StateMachine.ESTATE.TENTACLE)
                                {
                                    if(!m_Nodes[n1, n2].GetHasTentacle())
                                    {
                                        bValid = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            bValid = true;
                        }
                    }
                    else
                    {
                        bValid = true;
                    }
                }
            }
        }
        m_nCoinCount++;
        DropNewObjectAtNode(m_Nodes[n1,n2],m_Coin);
    }

    //----------------------------------------------------------------------------------------------------
    //SpawnNewObjectAtNode
    //Object is cloned and spawned 1 unit above the plane of a node
    //----------------------------------------------------------------------------------------------------
    public static void SpawnObjectAtNode(Node node, GameObject gameObject)
    {
        Vector3 v3Pos = node.GetPlane().transform.position;
        v3Pos.y += 1;
        GameObject go = Instantiate<GameObject>(gameObject, v3Pos, new Quaternion(0, 0, 0, 0)); 
    }

    // Update is called once per frame
    void Update()
    {
        m_fTentacleTimer += Time.deltaTime;
        m_fCoinTimer += Time.deltaTime;


        bool bReadyToSpawn = true;
        //Update All nodes
        for (int x = 0; x < m_Nodes.GetLength(0); x++)
        {
            for (int y = 0; y < m_Nodes.GetLength(1); y++)
            {
                m_Nodes[x, y].Update();
                StateMachine.ESTATE e = m_Nodes[x, y].GetState();
                if (e == StateMachine.ESTATE.TENTACLE || e == StateMachine.ESTATE.EXITING || e == StateMachine.ESTATE.ATTACKING)
                {
                    
                    //If the state is a tentacle or exiting
                    bReadyToSpawn = false;
                }
            }
        }




        //Tentacle Switching
        if (m_fTentacleTimer > m_fTentacleSwitchTime)
        {
            //if all tiles that are not floors, are holes
            if (bReadyToSpawn)
            {
                SpawnTentacles();
            }
            else //Time to switch but there are already tentacles or exiting tentacle
            {
                //change exiting state
                if (m_TentaclePositions.Count > 0)
                {
                    for (int i = 0; i < m_TentaclePositions.Count; i++)
                    {
                        StateMachine.ESTATE e = m_Nodes[Mathf.FloorToInt(m_TentaclePositions[i].x), Mathf.FloorToInt(m_TentaclePositions[i].y)].GetState();
                        if (e == StateMachine.ESTATE.TENTACLE && e != StateMachine.ESTATE.ATTACKING)
                        {
                            m_Nodes[Mathf.FloorToInt(m_TentaclePositions[i].x), Mathf.FloorToInt(m_TentaclePositions[i].y)].ChangeState(StateMachine.ESTATE.EXITING);
                        }
                       
                    }
                    
                    
                   // m_bSwitch = true;
                }
                
               
            }
        }

        if(m_fCoinTimer > m_fCoinSpawnTime)
        {
            m_fCoinTimer = 0.0f;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            if (!m_bLimitCoinCount)
            {
                DropCoin();
            }
            else if(m_nCoinCount < players.Length - 1)
            {
                DropCoin();
            }
        }

     

       
    }

    

    void SpawnTentacles()
    {
        m_fTentacleTimer = 0;

        //Reset all tentacles
        for (int i = 0; i < m_Nodes.GetLength(0); ++i)
        {
            for (int j = 0; j < m_Nodes.GetLength(1); ++j)
            {
                TentacleState tentacle = m_Nodes[i, j].GetStateMachine().GetTentacleState();
                tentacle.Reset();
            }
        }

        for (int i = 0; i < Mathf.FloorToInt((m_HolePositions.Count / 2)); i++) //Number of tentacles
        {
            int index = Random.Range(0, m_HolePositions.Count);

            //Switch to tentacle
            if (!m_TentaclePositions.Contains(m_HolePositions[index]))
            {
                m_TentaclePositions.Add(m_HolePositions[index]);
                m_Nodes[Mathf.FloorToInt(m_HolePositions[index].x), Mathf.FloorToInt(m_HolePositions[index].y)].SetHasTentacle(true);
                m_Nodes[Mathf.FloorToInt(m_HolePositions[index].x), Mathf.FloorToInt(m_HolePositions[index].y)].ChangeState(StateMachine.ESTATE.TENTACLE);
            }
        }
    }
}

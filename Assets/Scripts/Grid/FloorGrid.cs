using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGrid : MonoBehaviour
{

    [SerializeField] int m_nGridWidth = 10;
    [SerializeField] int m_nGridHeight = 10;
    [SerializeField] GameObject m_Plane;

    [SerializeField] float m_fDropHeight;
 
    [SerializeField] GameObject m_coin;
    Node[,] m_Nodes;

    //------------------------------------------------------------------------------------
    //Node Class
    //
    //------------------------------------------------------------------------------------

    //Node class. Node represent a tile
    public class Node
    {

        //-----------------------------------------------------------
        //ESTATE
        //
        //-----------------------------------------------------------
        public enum ESTATE
        {
            CHEST,
            FLOOR,
            HOLE,
            TENTACLE
        };




        //-----------------------------------------------------------
        //STATE CLASSES
        //-----------------------------------------------------------

        private abstract class State
        {
            public abstract void OnEnter();

            public abstract void OnExit();


            public abstract void Update();
           
        }


        private class ChestState: State 
        {
            public override void OnEnter()
            {

            }

            public override void OnExit()
            {

            }

            public override void Update()
            {
                
            }
        }

        private class FloorState : State
        {
            [SerializeField] GameObject m_Plane;
            public FloorState()
            {

            }
            public override void OnEnter()
            {
                m_Plane.SetActive(true);
            }

            public override void OnExit()
            {
               // m_Plane.SetActive(false);
            }
            public override void Update()
            {

            }
        }

        private class HoleState : State
        {
            public override void OnEnter()
            {

            }

            public override void OnExit()
            {

            }

            public override void Update()
            {

            }
        }

        private class TentacleState : State
        {
            public override void OnEnter()
            {

            }

            public override void OnExit()
            {

            }

            public override void Update()
            {

            }
        }

        public class StateMachine
        {
            private ChestState m_ChestState = new ChestState();
            private FloorState m_FloorState = new FloorState();
            private HoleState m_HoleState = new HoleState();
            private TentacleState m_TentacleState = new TentacleState();
            ESTATE m_eState = ESTATE.FLOOR;


            private Vector3 m_v3Position;
            private GameObject m_BasePlane;
            
            public void ChangeState(ESTATE eState)
            {

                //Exit the state you are currently in
                switch (m_eState)
                {
                    case ESTATE.FLOOR:
                        m_FloorState.OnExit();
                        break;
                    case ESTATE.CHEST:
                        m_ChestState.OnExit();
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
                    case ESTATE.CHEST:
                        m_ChestState.OnEnter();
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
                switch(m_eState)
                {
                    case ESTATE.FLOOR:
                        m_FloorState.Update();
                        break;
                    case ESTATE.CHEST:
                        m_ChestState.Update();
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

      

        //-----------------------------------------------------------
        //Variables
        //-----------------------------------------------------------


        public Vector3 m_v3Position;
        public StateMachine m_StateMachine;
        GameObject m_BasePlane;
        GameObject m_Plane;


        public GameObject GetPlane()
        {
            return m_Plane;
        }

        public Node(float fX, float fY, float fZ, GameObject Plane)
        {
            
            m_v3Position.x = fX;
            m_v3Position.y = fY;
            m_v3Position.z = fZ;
            m_BasePlane = Plane;
            m_StateMachine = new StateMachine();
            m_Plane = Instantiate(m_BasePlane, m_v3Position, new Quaternion(0, 0, 0, 0));
        }

        public Node(Vector3 v3Position, GameObject Plane)
        {
            m_v3Position = v3Position;
            m_StateMachine = new StateMachine();
            m_BasePlane = Plane;
            m_Plane = Instantiate(m_BasePlane, m_v3Position, new Quaternion(0, 0, 0, 0));

        }
        public StateMachine GetStateMachine()
        {
            return m_StateMachine;
        }
        private void Start()
        {

        }


        private void Update()
        {
            m_StateMachine.Update();
        }
    }
    //---------------------------------------------------------------------------------------------------------
    //End of Node Class
    //
    //---------------------------------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        m_Nodes = new Node[m_nGridWidth,m_nGridHeight];

        for(int x = 0; x < m_nGridWidth; x++)
        {
            for(int y = 0;  y < m_nGridHeight; y++)
            {

                
                Vector3 size = m_Plane.GetComponent<Collider>().bounds.size;

                
                m_Nodes[x, y] = new Node(new Vector3(x * size.x, 2.25f, y * size.z) ,m_Plane);
            
                
            }
        }

        
        
    }

    public Node GetNodeByPosition(Vector3 v3Pos)
    {

        Collider col = m_Plane.GetComponent<Collider>();
        float x = col.bounds.size.x;

        //Vector3 toPos =  v3Pos - transform.position;
        //toPos.z += (x / 2);
        //toPos.x += (x / 2);
        //float fW = toPos.x / col.bounds.size.x;



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
        


        return m_Nodes[nWidth, nHeight]; 
    }

    //----------------------------------------------------------------------------------------------------
    //DropObjectAtNode
    //Will move gameobject above the node to be dropped. Object must have physics for gravity so that it falls
    //----------------------------------------------------------------------------------------------------
    public void DropObjectAtNode(Node node, GameObject gameObject)
    {
        Vector3 pos = node.GetPlane().transform.position;
        pos.y += m_fDropHeight;
        gameObject.transform.Translate(pos,Space.World);
        
    }


    //----------------------------------------------------------------------------------------------------
    //DropNewObjectAtNode
    //Object is cloned and dropped at the node. Object must have physics for gravity so that it falls
    //----------------------------------------------------------------------------------------------------
    public void DropNewObjectAtNode(Node node, GameObject gameObject)
    {
        Vector3 pos = node.GetPlane().transform.position;
        pos.y += m_fDropHeight;
        GameObject go = Instantiate<GameObject>(gameObject,pos,new Quaternion(0,0,0,0));
    }

    bool done = false;

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            done = true;
            Node n = GetNodeByPosition(m_coin.transform.position);
            DropNewObjectAtNode(n, m_coin);
        }
    }
}

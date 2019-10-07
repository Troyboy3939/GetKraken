using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGrid : MonoBehaviour
{

    [SerializeField] int m_nGridWidth = 10;
    [SerializeField] int m_nGridHeight = 10;
    [SerializeField] GameObject m_Plane;
    [SerializeField] Vector3 m_GridPosition;
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

        private class ChestState 
        {
            public  void OnEnter()
            {

            }

            public  void OnExit()
            {

            }

            public void Update()
            {
                
            }
        }

        private class FloorState 
        {
            
            public void OnEnter(Vector3 v3Position, ref GameObject plane)
            {
                plane = Instantiate(plane, v3Position, new Quaternion(0, 0, 0, 0));
            }

            public void OnExit()
            {

            }
            public void Update()
            {

            }
        }

        private class HoleState
        {
            public  void OnEnter()
            {

            }

            public void OnExit()
            {

            }

            public void Update()
            {

            }
        }

        private class TentacleState
        {
            public  void OnEnter()
            {

            }

            public  void OnExit()
            {

            }

            public void Update()
            {

            }
        }

        public class StateMachine
        {
            private ChestState m_ChestState = new ChestState();
            private FloorState m_FloorState;
            private HoleState m_HoleState = new HoleState();
            private TentacleState m_TentacleState = new TentacleState();
            ESTATE m_eState = ESTATE.FLOOR;


            private Vector3 m_v3Position;
            private GameObject m_Plane;
            public StateMachine(Vector3 v3Position,GameObject plane)
            {
                m_FloorState = new FloorState();
                m_v3Position = v3Position;
                m_Plane = plane;
            }
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
                        m_FloorState.OnEnter(m_v3Position, ref m_Plane);
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
        GameObject m_Plane;

        public Node(float fX, float fY, float fZ, GameObject Plane)
        {
            
            m_v3Position.x = fX;
            m_v3Position.y = fY;
            m_v3Position.z = fZ;
            m_Plane = Plane;
            m_StateMachine = new StateMachine(m_v3Position,Plane);
        }

        public Node(Vector3 v3Position, GameObject Plane)
        {
            m_v3Position = v3Position;
            m_StateMachine = new StateMachine(m_v3Position, Plane);
            

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
                m_Nodes[x, y].GetStateMachine().ChangeState(Node.ESTATE.FLOOR);
            }
        }

        
        
    }

    public Node GetNodeByPosition(Vector3 pos)
    {
        return new Node(pos,m_Plane); //remove later, just to clear errors
    }

    //----------------------------------------------------------------------------------------------------
    //DropObjectAtNode
    //Will move gameobject above the node to be dropped. Object must have physics for gravity so that it falls
    //----------------------------------------------------------------------------------------------------
    public void DropObjectAtNode(Node node, GameObject gameObject)
    {

    }


    //----------------------------------------------------------------------------------------------------
    //DropNewObjectAtNode
    //Object is cloned and dropped at the node. Object must have physics for gravity so that it falls
    //----------------------------------------------------------------------------------------------------
    public void DropNewObjectAtNode(Node node, GameObject gameObject)
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

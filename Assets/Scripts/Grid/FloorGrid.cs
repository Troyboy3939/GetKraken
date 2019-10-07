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
    private class Node
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
        //Variables
        //-----------------------------------------------------------


        public Vector3 m_v3Position;
        public ESTATE m_eState;
        GameObject m_Plane;

        public Node(float fX, float fY, float fZ, GameObject Plane)
        {
            
            m_v3Position.x = fX;
            m_v3Position.y = fY;
            m_v3Position.z = fZ;
            m_Plane = Plane;
            m_Plane = Instantiate<GameObject>(Plane, m_v3Position, new Quaternion(0, 0, 0, 0));
        }

        public Node(Vector3 v3Position, GameObject Plane)
        {
            m_v3Position = v3Position;
            
            m_Plane = Instantiate<GameObject>(Plane,v3Position, new Quaternion(0,0,0,0));
            m_Plane.GetComponent<MeshCollider>().isTrigger = false;

        }

        private void Start()
        {

        }


        private void Update()
        {
            
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

    // Update is called once per frame
    void Update()
    {
  
    }
}

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
    [SerializeField] GameObject m_Plane;
    [SerializeField] GameObject m_Tentacle;
    [SerializeField] float m_fDropHeight;
    Node[,] m_Nodes;







    //-----------------------------------------------------------
    //Functions
    //-----------------------------------------------------------



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
        Vector3 v3Pos = node.GetPlane().transform.position;
        v3Pos.y += m_fDropHeight;
        gameObject.transform.Translate(v3Pos,Space.World);
        
    }


    //----------------------------------------------------------------------------------------------------
    //DropNewObjectAtNode
    //Object is cloned and dropped at the node. Object must have physics for gravity so that it falls
    //----------------------------------------------------------------------------------------------------
    public void DropNewObjectAtNode(Node node, GameObject gameObject)
    {
        Vector3 v3Pos = node.GetPlane().transform.position;
        v3Pos.y += m_fDropHeight;
        GameObject go = Instantiate<GameObject>(gameObject,v3Pos,new Quaternion(0,0,0,0));
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
        
      
    }
}
